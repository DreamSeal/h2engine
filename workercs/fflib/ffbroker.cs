using System;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ff
{
    class FFBroker
    {
        private static FFBroker gInstance = null;
        public static FFBroker Instance() {
            if (gInstance == null){
                gInstance = new FFBroker();
            }
            return gInstance;
        }
        public string   m_strListenHost;
        public RegisterToBrokerRet m_brokerData;
        public Dictionary<Int64/* node id*/, IFFSocket>     m_dictSockets;//!各个节点对应的连接信息
        public FFAcceptor m_acceptor;
        public Int64 m_nForAllocID;
        public FFBroker(){
            m_strListenHost = "tcp://127.0.0.1:43210";
            m_brokerData = new RegisterToBrokerRet() { NodeId = 0, RegisterFlag = 0, Service2nodeId = new Dictionary<string, long>() };
            m_dictSockets = new Dictionary<Int64/* node id*/, IFFSocket>();
            m_nForAllocID = 0;
        }
        public bool Init(string strBrokerCfg) {
            if (strBrokerCfg.Length > 0)
            {
                m_strListenHost = strBrokerCfg;
            }

            m_acceptor = FFNet.Listen(m_strListenHost, new SocketMsgHandler(HandleMsg), new SocketBrokenHandler(HandleBroken));
            if (m_acceptor != null)
            {
                FFLog.Trace(string.Format("FFBroker open....{0} ok", m_strListenHost));
            }
            else
            {
                FFLog.Trace(string.Format("FFBroker open....{0} failed", m_strListenHost));
            }
            return true;
        }
        public bool Cleanup(){
            if (m_acceptor != null)
                m_acceptor.Close();
            return true;
        }
        public void HandleMsg(IFFSocket ffsocket, UInt16 cmd, byte[] strMsg) {
            //FFLog.Trace(string.Format("FFBroker handleMsg....{0}, {1} [{2}]", cmd, strMsg.Length, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()));
            try
            {
                switch ((FFRPC_CMD)cmd)
                {
                    case FFRPC_CMD.REGISTER_TO_BROKER_REQ:
                        {
                            RegisterToBrokerReq reqMsg = new RegisterToBrokerReq();
                            FFNet.DecodeMsg(reqMsg, strMsg);
                            FFLog.Trace(string.Format("FFBroker handleMsg.REGISTER_TO_BROKER_REQ....{0}, {1}", reqMsg.NodeType, reqMsg.StrServiceName));
                            if (FFRPC_NODE_TYPE.RPC_NODE == (FFRPC_NODE_TYPE)reqMsg.NodeType)
                            {
                                if (m_brokerData.Service2nodeId.ContainsKey(reqMsg.StrServiceName))
                                {
                                    FFLog.Error(string.Format("FFBroker handleMsg servicename exist....{0}, {1}", reqMsg.NodeType, reqMsg.StrServiceName));
                                    ffsocket.Close();
                                    return;
                                }
                                Int64 nNodeID = allocNodeId();
                                m_dictSockets[nNodeID] = ffsocket;
                                m_brokerData.Service2nodeId[reqMsg.StrServiceName] = nNodeID;
                                m_brokerData.NodeId = nNodeID;
                                SyncNodeInfo(m_brokerData, ffsocket);//!广播给所有的子节点
                            }
                        } break;
                    case FFRPC_CMD.BROKER_ROUTE_MSG:
                        {
                            BrokerRouteMsgReq reqMsg = new BrokerRouteMsgReq();
                            FFNet.DecodeMsg(reqMsg, strMsg);
                            //FFLog.Trace(string.Format("FFBroker.BROKER_ROUTE_MSG service={0},func={1} Callback={2}",
                            //            reqMsg.DestServiceName, reqMsg.DestMsgName, reqMsg.CallbackId));
                            if (!m_dictSockets.ContainsKey(reqMsg.DestNodeId))
                            {
                                return;
                            }
                            IFFSocket destSocket = m_dictSockets[reqMsg.DestNodeId];
                            FFNet.SendMsg(destSocket, (UInt16)FFRPC_CMD.BROKER_TO_CLIENT_MSG, reqMsg);
                        } break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                FFLog.Error("FFBroker.Error:" + ex.Message);
            }
        }
        public void HandleBroken(IFFSocket ffsocket) {
            Int64 nNodeID = 0;
            string strServiceName = "";
            foreach (KeyValuePair<Int64, IFFSocket> kvp in m_dictSockets)
            {
                if (kvp.Value == ffsocket)
                {
                    nNodeID = kvp.Key;
                    break;
                }
            }
            if (nNodeID != 0)
            {
                m_dictSockets.Remove(nNodeID);
                foreach (KeyValuePair<string, long> kvp in m_brokerData.Service2nodeId)
                {
                    if (kvp.Value == nNodeID)
                    {
                        strServiceName = kvp.Key;
                        break;
                    }
                }
            }
            if (strServiceName.Length > 0)
            {
                m_brokerData.Service2nodeId.Remove(strServiceName);
            }
            FFLog.Trace(string.Format("FFBroker HandleBroken....{0}, {1}", nNodeID, strServiceName));
            SyncNodeInfo(m_brokerData, null);
        }
        private Int64 allocNodeId()
        {
            return ++m_nForAllocID;
        }
        private void SyncNodeInfo(RegisterToBrokerRet retMsg, IFFSocket ffsocket)//! 同步给所有的节点，当前的各个节点的信息
        {
            //!广播给所有的子节点
            foreach (IFFSocket s in m_dictSockets.Values)
            {
                if (s == ffsocket)
                {
                    retMsg.RegisterFlag = 1;
                }
                else
                {
                    retMsg.RegisterFlag = 0;
                }
                FFNet.SendMsg(s, (UInt16)FFRPC_CMD.REGISTER_TO_BROKER_RET, retMsg);
            }
        }
    }
}

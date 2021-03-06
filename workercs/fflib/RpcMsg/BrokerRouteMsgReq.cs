/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;


#if !SILVERLIGHT
[Serializable]
#endif
public partial class BrokerRouteMsgReq : TBase
{
  private string _destServiceName;
  private string _destMsgName;
  private long _destNodeId;
  private long _fromNodeId;
  private long _callbackId;
  private byte[] _body;
  private string _errinfo;

  public string DestServiceName
  {
    get
    {
      return _destServiceName;
    }
    set
    {
      __isset.destServiceName = true;
      this._destServiceName = value;
    }
  }

  public string DestMsgName
  {
    get
    {
      return _destMsgName;
    }
    set
    {
      __isset.destMsgName = true;
      this._destMsgName = value;
    }
  }

  public long DestNodeId
  {
    get
    {
      return _destNodeId;
    }
    set
    {
      __isset.destNodeId = true;
      this._destNodeId = value;
    }
  }

  public long FromNodeId
  {
    get
    {
      return _fromNodeId;
    }
    set
    {
      __isset.fromNodeId = true;
      this._fromNodeId = value;
    }
  }

  public long CallbackId
  {
    get
    {
      return _callbackId;
    }
    set
    {
      __isset.callbackId = true;
      this._callbackId = value;
    }
  }

  public byte[] Body
  {
    get
    {
      return _body;
    }
    set
    {
      __isset.body = true;
      this._body = value;
    }
  }

  public string Errinfo
  {
    get
    {
      return _errinfo;
    }
    set
    {
      __isset.errinfo = true;
      this._errinfo = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool destServiceName;
    public bool destMsgName;
    public bool destNodeId;
    public bool fromNodeId;
    public bool callbackId;
    public bool body;
    public bool errinfo;
  }

  public BrokerRouteMsgReq() {
  }

  public void Read (TProtocol iprot)
  {
    TField field;
    iprot.ReadStructBegin();
    while (true)
    {
      field = iprot.ReadFieldBegin();
      if (field.Type == TType.Stop) { 
        break;
      }
      switch (field.ID)
      {
        case 2:
          if (field.Type == TType.String) {
            DestServiceName = iprot.ReadString();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 3:
          if (field.Type == TType.String) {
            DestMsgName = iprot.ReadString();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 4:
          if (field.Type == TType.I64) {
            DestNodeId = iprot.ReadI64();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 6:
          if (field.Type == TType.I64) {
            FromNodeId = iprot.ReadI64();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 7:
          if (field.Type == TType.I64) {
            CallbackId = iprot.ReadI64();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 8:
          if (field.Type == TType.String) {
            Body = iprot.ReadBinary();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 9:
          if (field.Type == TType.String) {
            Errinfo = iprot.ReadString();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        default: 
          TProtocolUtil.Skip(iprot, field.Type);
          break;
      }
      iprot.ReadFieldEnd();
    }
    iprot.ReadStructEnd();
  }

  public void Write(TProtocol oprot) {
    TStruct struc = new TStruct("BrokerRouteMsgReq");
    oprot.WriteStructBegin(struc);
    TField field = new TField();
    if (DestServiceName != null && __isset.destServiceName) {
      field.Name = "destServiceName";
      field.Type = TType.String;
      field.ID = 2;
      oprot.WriteFieldBegin(field);
      oprot.WriteString(DestServiceName);
      oprot.WriteFieldEnd();
    }
    if (DestMsgName != null && __isset.destMsgName) {
      field.Name = "destMsgName";
      field.Type = TType.String;
      field.ID = 3;
      oprot.WriteFieldBegin(field);
      oprot.WriteString(DestMsgName);
      oprot.WriteFieldEnd();
    }
    if (__isset.destNodeId) {
      field.Name = "destNodeId";
      field.Type = TType.I64;
      field.ID = 4;
      oprot.WriteFieldBegin(field);
      oprot.WriteI64(DestNodeId);
      oprot.WriteFieldEnd();
    }
    if (__isset.fromNodeId) {
      field.Name = "fromNodeId";
      field.Type = TType.I64;
      field.ID = 6;
      oprot.WriteFieldBegin(field);
      oprot.WriteI64(FromNodeId);
      oprot.WriteFieldEnd();
    }
    if (__isset.callbackId) {
      field.Name = "callbackId";
      field.Type = TType.I64;
      field.ID = 7;
      oprot.WriteFieldBegin(field);
      oprot.WriteI64(CallbackId);
      oprot.WriteFieldEnd();
    }
    if (Body != null && __isset.body) {
      field.Name = "body";
      field.Type = TType.String;
      field.ID = 8;
      oprot.WriteFieldBegin(field);
      oprot.WriteBinary(Body);
      oprot.WriteFieldEnd();
    }
    if (Errinfo != null && __isset.errinfo) {
      field.Name = "errinfo";
      field.Type = TType.String;
      field.ID = 9;
      oprot.WriteFieldBegin(field);
      oprot.WriteString(Errinfo);
      oprot.WriteFieldEnd();
    }
    oprot.WriteFieldStop();
    oprot.WriteStructEnd();
  }

  public override string ToString() {
    StringBuilder sb = new StringBuilder("BrokerRouteMsgReq(");
    sb.Append("DestServiceName: ");
    sb.Append(DestServiceName);
    sb.Append(",DestMsgName: ");
    sb.Append(DestMsgName);
    sb.Append(",DestNodeId: ");
    sb.Append(DestNodeId);
    sb.Append(",FromNodeId: ");
    sb.Append(FromNodeId);
    sb.Append(",CallbackId: ");
    sb.Append(CallbackId);
    sb.Append(",Body: ");
    sb.Append(Body);
    sb.Append(",Errinfo: ");
    sb.Append(Errinfo);
    sb.Append(")");
    return sb.ToString();
  }

}


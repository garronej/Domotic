//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     .NET Micro Framework MFSvcUtil.Exe
//     Runtime Version:2.0.00001.0001
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Xml;
using System.Ext;
using System.Ext.Xml;
using Ws.ServiceModel;
using Ws.Services.Mtom;
using Ws.Services.Serialization;
using XmlElement = Ws.Services.Xml.WsXmlNode;
using XmlAttribute = Ws.Services.Xml.WsXmlAttribute;
using XmlConvert = Ws.Services.Serialization.WsXmlConvert;

namespace tempuri.org
{
    
    
    [DataContract(Namespace="http://tempuri.org/")]
    public class insertValue
    {
        
        [DataMember(Order=0, IsNillable=true, IsRequired=false)]
        public string dataType;
        
        [DataMember(Order=1, IsRequired=false)]
        public double value;
    }
    
    public class insertValueDataContractSerializer : DataContractSerializer
    {
        
        public insertValueDataContractSerializer(string rootName, string rootNameSpace) : 
                base(rootName, rootNameSpace)
        {
        }
        
        public insertValueDataContractSerializer(string rootName, string rootNameSpace, string localNameSpace) : 
                base(rootName, rootNameSpace, localNameSpace)
        {
        }
        
        public override object ReadObject(XmlReader reader)
        {
            insertValue insertValueField = null;
            if (IsParentStartElement(reader, false, true))
            {
                insertValueField = new insertValue();
                reader.Read();
                if (IsChildStartElement(reader, "dataType", true, false))
                {
                    reader.Read();
                    insertValueField.dataType = reader.ReadString();
                    reader.ReadEndElement();
                }
                if (IsChildStartElement(reader, "value", false, false))
                {
                    reader.Read();
                    insertValueField.value = XmlConvert.ToDouble(reader.ReadString());
                    reader.ReadEndElement();
                }
                reader.ReadEndElement();
            }
            return insertValueField;
        }
        
        public override void WriteObject(XmlWriter writer, object graph)
        {
            insertValue insertValueField = ((insertValue)(graph));
            if (WriteParentElement(writer, true, true, graph))
            {
                if (WriteChildElement(writer, "dataType", true, false, insertValueField.dataType))
                {
                    writer.WriteString(insertValueField.dataType);
                    writer.WriteEndElement();
                }
                if (WriteChildElement(writer, "value", false, false, insertValueField.value))
                {
                    writer.WriteString(XmlConvert.ToString(insertValueField.value));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            return;
        }
    }
    
    [DataContract(Namespace="http://tempuri.org/")]
    public class insertValueResponse
    {
    }
    
    public class insertValueResponseDataContractSerializer : DataContractSerializer
    {
        
        public insertValueResponseDataContractSerializer(string rootName, string rootNameSpace) : 
                base(rootName, rootNameSpace)
        {
        }
        
        public insertValueResponseDataContractSerializer(string rootName, string rootNameSpace, string localNameSpace) : 
                base(rootName, rootNameSpace, localNameSpace)
        {
        }
        
        public override object ReadObject(XmlReader reader)
        {
            insertValueResponse insertValueResponseField = null;
            if (IsParentStartElement(reader, false, true))
            {
                insertValueResponseField = new insertValueResponse();
                reader.Read();
                reader.ReadEndElement();
            }
            return insertValueResponseField;
        }
        
        public override void WriteObject(XmlWriter writer, object graph)
        {
            insertValueResponse insertValueResponseField = ((insertValueResponse)(graph));
            if (WriteParentElement(writer, true, true, graph))
            {
                writer.WriteEndElement();
            }
            return;
        }
    }
    
    [ServiceContract(Namespace="http://tempuri.org/")]
    public interface IIDomoticService
    {
        
        [OperationContract(Action="http://tempuri.org/IDomoticService/insertValue")]
        insertValueResponse insertValue(insertValue req);
    }
}

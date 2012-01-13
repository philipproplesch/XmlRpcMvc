using System;
using System.Runtime.Serialization;

namespace XmlRpcMvc
{
    public class XmlRpcFaultException : Exception
    {
        public XmlRpcFaultException(int code, string message)
            : base(message)
        {
            Code = code;
            String = message;
        }

        [DataMember(Name = "faultCode")]
        public int Code { get; set; }

        [DataMember(Name = "faultString")]
        public string String { get; set; }
    }
}
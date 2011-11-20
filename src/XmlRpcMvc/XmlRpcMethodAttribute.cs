using System;

namespace XmlRpcMvc
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class XmlRpcMethodAttribute 
        : Attribute
    {
        public XmlRpcResponseType ResponseType { get; private set; }

        public string MethodName { get; private set; }

        public XmlRpcMethodAttribute(string methodName)
            : this(methodName, XmlRpcResponseType.Wrapped)
        {
        }

        public XmlRpcMethodAttribute(
            string methodName, 
            XmlRpcResponseType responseType)
        {
            MethodName = methodName;
            ResponseType = responseType;
        }

        public override string ToString()
        {
            return MethodName;
        }
    }
}

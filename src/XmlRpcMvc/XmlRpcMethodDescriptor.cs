using System.Reflection;

namespace XmlRpcMvc
{
    public class XmlRpcMethodDescriptor
    {
        public XmlRpcResponseType ResponseType { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public string Name { get; set; }

        public XmlRpcMethodDescriptor(
            string name,
            XmlRpcResponseType responseType,
            MethodInfo methodInfo)
        {
            Name = name;
            ResponseType = responseType;
            MethodInfo = methodInfo;
        }
    }
}
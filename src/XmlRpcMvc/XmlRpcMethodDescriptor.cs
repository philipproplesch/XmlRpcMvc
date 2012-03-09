using System.Reflection;

namespace XmlRpcMvc
{
    public class XmlRpcMethodDescriptor
    {
        public XmlRpcResponseType ResponseType { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public XmlRpcMethodDescriptor(
            string name,
            string description,
            XmlRpcResponseType responseType,
            MethodInfo methodInfo)
        {
            Name = name;
            Description = description;
            ResponseType = responseType;
            MethodInfo = methodInfo;
        }
    }
}
using System.Collections.Generic;

namespace XmlRpcMvc
{
    internal class XmlRpcRequest
    {
        public string MethodName { get; set; }
        public List<object> Parameters { get; set; }
    }
}
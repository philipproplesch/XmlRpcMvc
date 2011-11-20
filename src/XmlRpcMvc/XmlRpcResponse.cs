namespace XmlRpcMvc
{
    internal class XmlRpcResponse
    {
        public XmlRpcResponseType ResponseType { get; set; }
        public object Payload { get; set; }
    }
}
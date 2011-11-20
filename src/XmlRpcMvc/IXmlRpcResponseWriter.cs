using System.Xml;

namespace XmlRpcMvc
{
    public interface IXmlRpcResponseWriter
    {
        void WriteResponse(XmlWriter output, dynamic result);
    }
}

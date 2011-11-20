using System.Xml;

namespace XmlRpcMvc.Extensions
{
    public static class XmlWriterExtensions
    {
       

        public static void WrapOutgoingType(
            this XmlWriter xmlWriter, 
            object value)
        {
            var dataType = "string";

            var type = value.GetType();
            if (type == TypeDef.Int)
            {
                dataType = "i4";
            }
            else if (type == TypeDef.Double)
            {
                dataType = "double";
            }
            else if (type == TypeDef.Bool)
            {
                dataType = "boolean";
            }
            else if (type == TypeDef.DateTime)
            {
                dataType = "dateTime.iso8601";
            }
            else if (type == TypeDef.ByteArray)
            {
                dataType = "base64";
            }

            xmlWriter.WriteStartElement(dataType);
            xmlWriter.WriteString(value.ToString());
            xmlWriter.WriteEndElement();
        }
    }
}
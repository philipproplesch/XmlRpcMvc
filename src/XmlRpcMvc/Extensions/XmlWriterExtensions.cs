using System;
using System.Xml;

namespace XmlRpcMvc.Extensions
{
    public static class XmlWriterExtensions
    {
        public static void WrapOutgoingType(
            this XmlWriter xmlWriter, 
            object value)
        {
            var stringValue = value.ToString();

            var dataType = "string";

            var type = value.GetType();
            if (type == TypeDef.Int)
            {
                //dataType = "i4";
                dataType = "int";
            }
            else if (type == TypeDef.Double)
            {
                dataType = "double";
            }
            else if (type == TypeDef.Bool)
            {
                dataType = "boolean";
                stringValue = (bool) value ? "1" : "0";
            }
            else if (type == TypeDef.DateTime)
            {
                dataType = "dateTime.iso8601";
                stringValue = ((DateTime)value).ToString("yyyyMMddTHH:mm:ssZ");
            }
            else if (type == TypeDef.ByteArray)
            {
                dataType = "base64";
            }

            xmlWriter.WriteStartElement(dataType);
            xmlWriter.WriteString(stringValue);
            xmlWriter.WriteEndElement();
        }
    }
}
using System;
using System.Web.Mvc;
using System.Xml;
using XmlRpcMvc.Extensions;

namespace XmlRpcMvc
{
    public class XmlRpcResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var request = context.HttpContext.Request;
            var requestInfo =
                XmlRpcRequestParser.GetRequestInformation2(
                    request.InputStream);

            if (string.IsNullOrWhiteSpace(requestInfo.MethodName))
            {
                throw new InvalidOperationException(
                    "XmlRpc call does not contain a method.");
            }

            var methodInfo = 
                XmlRpcRequestParser.GetRequestedMethod(requestInfo);

            if (methodInfo == null)
            {
                throw new InvalidOperationException(
                    string.Concat(
                        "There was no implementation of IXmlRpcService ",
                        "found, that containins a method decorated with ",
                        " the XmlRpcMethodAttribute value'",
                        requestInfo.MethodName,
                        "'."));
            }

            var result = 
                XmlRpcRequestParser.ExecuteRequestedMethod(
                    requestInfo, methodInfo, context.Controller);

            var response = context.RequestContext.HttpContext.Response;
            response.ContentType = "text/xml";
            
            var settings =
                new XmlWriterSettings
                {
                    OmitXmlDeclaration = true
                };

            using (var writer = 
                XmlWriter.Create(response.OutputStream, settings))
            {
                if (methodInfo.ResponseType == XmlRpcResponseType.Wrapped)
                {
                    WriteWrappedResponse(writer, result);
                    return;
                }
                WriteRawResponse(writer, result);
            }
        }

        private static void WriteRawResponse(
            XmlWriter output, 
            dynamic result)
        {
            output.WriteStartDocument();
            {
                output.WriteStartElement("response");
                {            
                    WriteObject(output, result);   
                }
                output.WriteEndElement();
            }
            output.WriteEndDocument();
        }

        private static void WriteWrappedResponse(
            XmlWriter output, 
            dynamic result)
        {
            output.WriteStartDocument();
            {
                output.WriteStartElement("methodResponse");
                {
                    output.WriteStartElement("params");
                    {
                        output.WriteStartElement("param");
                        {
                            output.WriteStartElement("value");
                            {
                                WriteObject(output, result);
                            }
                            output.WriteEndElement();
                        }
                        output.WriteEndElement();
                    }
                    output.WriteEndElement();
                }
                output.WriteEndElement();
            }
            output.WriteEndDocument();
        }

        private static void WriteObject(
            XmlWriter xmlWriter,
            dynamic result)
        {
            Type type = result.GetType();
            if (type.IsPrimitive())
            {
                xmlWriter.WrapOutgoingType((object)result);
            }
            else if (type.IsArray)
            {
                WriteArray(xmlWriter, result);
            }
            else if (!type.IsPrimitive && type.IsClass)
            {
                WriteClass(xmlWriter, type, result);
            }
        }

        private static void WriteClass(
            XmlWriter xmlWriter,
            Type type,
            object obj)
        {
            xmlWriter.WriteStartElement("struct");

            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(obj, null);
                if (value == null)
                    continue;

                xmlWriter.WriteStartElement("member");
                {
                    xmlWriter.WriteStartElement("name");
                    {
                        xmlWriter.WriteString(property.GetSerializationName());
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("value");
                    {
                        WriteObject(xmlWriter, value);
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }

            // struct
            xmlWriter.WriteEndElement();
        }

        private static void WriteArray(XmlWriter xmlWriter, dynamic obj)
        {
            xmlWriter.WriteStartElement("array");
            {
                xmlWriter.WriteStartElement("data");
                {
                    foreach (var resultEntry in obj)
                    {
                        xmlWriter.WriteStartElement("value");
                        {
                            WriteObject(xmlWriter, resultEntry);
                        }
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }

    }
}
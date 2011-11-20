using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using System.Linq;
using System.Reflection;
using XmlRpcMvc.Common;
using XmlRpcMvc.Extensions;

namespace XmlRpcMvc
{
    internal static class XmlRpcRequestParser
    {
        private static readonly Type _s_rpca = typeof(XmlRpcMethodAttribute);
        private static readonly Type _s_rpcs = typeof(IXmlRpcService);

        private static readonly Func<Type, bool> _s_isRpcService =
            type => type.IsClass && _s_rpcs.IsAssignableFrom(type);

        private static readonly Func<MethodInfo, XmlRpcMethodAttribute>
            _s_getRpcAttribute =
                method =>
                    method.GetCustomAttributes(_s_rpca, false).
                        Cast<XmlRpcMethodAttribute>().FirstOrDefault();

        private static readonly Func<MethodInfo[], IEnumerable<XmlRpcMethodDescriptor>>
            _s_getXmlRpcMethods = 
                methods => 
                    from method in methods
                    let attribute = _s_getRpcAttribute(method)
                    where attribute != null
                    select
                        new XmlRpcMethodDescriptor(
                        attribute.MethodName,
                        attribute.ResponseType,
                        method);

        private static readonly Dictionary<string, XmlRpcMethodDescriptor> 
            _s_rpcMethods =
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(
                    assembly => 
                        assembly.GetTypes().Where(_s_isRpcService).Select(
                            type =>  type.GetMethods()
                        ).SelectMany(_s_getXmlRpcMethods)
                ).ToDictionary(desc=>desc.Name, desc => desc);

        public static XmlRpcMethodDescriptor GetRequestedMethod(XmlRpcRequest request)
        {
            XmlRpcMethodDescriptor descriptor;
            _s_rpcMethods.TryGetValue(request.MethodName, out descriptor);
            return descriptor;
        }

        public static object ExecuteRequestedMethod(
            XmlRpcRequest request,
            XmlRpcMethodDescriptor methodDescriptor, 
            ControllerBase controller)
        {
            var parameters = new List<object>();
            var method = methodDescriptor.MethodInfo;
            var requiredParameters = method.GetParameters();

            for (var i = 0; i < requiredParameters.Length; i++)
            {
                var parameter = requiredParameters[i];
                if (parameter.ParameterType.IsPrimitive())
                {
                    parameters.Add(request.Parameters[i]);
                }
                else
                {
                    var complexInstanceParameters =
                        (Dictionary<string, object>)request.Parameters[i];

                    var complexInstance =
                        Activator.CreateInstance(parameter.ParameterType);

                    foreach (var property in
                        complexInstance.GetType().GetProperties())
                    {
                        var nameKey = property.GetSerializationName();
                        object value;
                        if (complexInstanceParameters.TryGetValue(
                            nameKey, 
                            out value))
                        {
                            property.SetValue(complexInstance, value, null);
                        }
                    }

                    parameters.Add(complexInstance);
                }
            }


            var instance = 
                controller.GetType() == method.DeclaringType
                    ? controller
                    : Activator.CreateInstance(method.DeclaringType);
            try
            {
                return method.Invoke(instance, parameters.ToArray());
            }
            catch (XmlRpcFaultException exception)
            {
                return exception;
            }
        }

        public static XmlRpcRequest GetRequestInformation2(Stream xml)
        {
            var request = new XmlRpcRequest();

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xml);

            var methodName = 
                xmlDocument.SelectSingleNode("methodCall/methodName");
            if (methodName != null)
            {
                request.MethodName = methodName.InnerText;
            }

            var parameters = 
                xmlDocument.SelectNodes("methodCall/params/param/value/*");
            if (parameters != null)
            {
                request.Parameters = new List<object>();
                foreach (XmlNode node in parameters)
                {
                    request.Parameters.Add(GetMethodMember(request, node));
                }
            }

            return request;
        }

        private static object GetMethodMember(
            XmlRpcRequest request, 
            XmlNode node)
        {
            switch (node.Name)
            {
                case "array":
                    return GetMethodArrayMember(node);

                case "struct":
                    return GetMethodStructMember(request, node);

                default:
                    return node.InnerText.ConvertTo(node.Name);
            }
        }

        private static Dictionary<string, object> GetMethodStructMember(
            XmlRpcRequest request, 
            XmlNode node)
        {
            var xpath = node.GetXPath();
            var values = node.SelectNodes(string.Concat(xpath, "/member"));

            if (values != null)
            {
                var dictionary = new Dictionary<string, object>();

                foreach (XmlNode value in values)
                {
                    var memberNameNode = value["name"];
                    var memberValueNode = value["value"];

                    if (memberNameNode == null || memberValueNode == null)
                    {
                        continue;
                    }

                    var memberName = memberNameNode.InnerText;

                    dictionary.Add(
                        memberName,
                        GetMethodMember(
                            request,
                            memberValueNode.FirstChild));
                }

                return dictionary;
            }

            return null;
        }

        private static object GetMethodArrayMember(XmlNode node)
        {
            var xpath = node.GetXPath();
            var values = node.SelectNodes(string.Concat(xpath, "/data/value"));

            if (values != null)
            {
                return values.Cast<XmlNode>()
                    .Select(
                        value =>
                        (string)value.InnerText.ConvertTo(value.FirstChild.Name))
                    .ToArray();
            }

            return null;
        }
    }
}
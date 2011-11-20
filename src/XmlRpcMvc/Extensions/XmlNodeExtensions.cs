using System;
using System.Text;
using System.Xml;

namespace XmlRpcMvc.Extensions
{
    public static class XmlNodeExtensions
    {
        // http://stackoverflow.com/questions/241238/how-to-get-xpath-from-an-xmlnode-instance-c

        public static string GetXPath(this XmlNode instance)
        {
            var builder = new StringBuilder();
            while (instance != null)
            {
                switch (instance.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + instance.Name);
                        instance = ((XmlAttribute)instance).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        int index = FindElementIndex((XmlElement)instance);
                        builder.Insert(0, "/" + instance.Name + "[" + index + "]");
                        instance = instance.ParentNode;
                        break;
                    case XmlNodeType.Document:
                        return builder.ToString();
                    default:
                        throw new ArgumentException("Only elements and attributes are supported");
                }
            }
            throw new ArgumentException("Node was not in a document");
        }

        private static int FindElementIndex(XmlElement element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }

            var parent = (XmlElement)parentNode;
            int index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlElement && candidate.Name == element.Name)
                {
                    if (candidate == element)
                    {
                        return index;
                    }
                    index++;
                }
            }
            throw new ArgumentException("Couldn't find element within parent");
        }
    }
}

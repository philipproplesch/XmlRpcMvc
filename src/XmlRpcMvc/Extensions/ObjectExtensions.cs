using System;

namespace XmlRpcMvc.Extensions
{
    internal static class ObjectExtensions
    {
        public static object ConvertTo(this object value, string typeName)
        {
            switch (typeName)
            {
                case "int":
                case "i4":
                    value = Convert.ToInt32(value);
                    break;
                case "double":
                    value = Convert.ToDouble(value);
                    break;
                case "boolean":
                    try
                    {
                        value = Convert.ToBoolean(value);
                    }
                    catch (Exception)
                    {
                        value = (string) value == "1";
                    }
                    break;
                case "dateTime.iso8601":
                    value = Convert.ToDateTime(value);
                    break;
                case "base64":
                    value = Convert.FromBase64String((string) value);
                    break;
                default:
                    value = Convert.ToString(value);
                    break;
            }

            return value;
        }

    }
}

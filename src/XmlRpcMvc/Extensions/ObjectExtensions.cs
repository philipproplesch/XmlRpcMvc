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
                    try
                    {
                        value = Convert.ToInt32(value);
                    }
                    catch (Exception)
                    {
                        value = default(int);
                    }
                    break;
                case "double":
                    try
                    {
                        value = Convert.ToDouble(value);
                    }
                    catch (Exception)
                    {
                        value = default(double);
                    }
                    break;
                case "boolean":
                    try
                    {
                        value = bool.Parse(value.ToString());
                    }
                    catch (FormatException)
                    {
                        value = (string)value == "1";
                    }
                    break;
                case "dateTime.iso8601":
                    value = ((string) value).ConvertToDateTime();
                    break;
                case "base64":
                    value = Convert.FromBase64String((string)value);
                    break;
                default:
                    value = Convert.ToString(value);
                    break;
            }

            return value;
        }

    }
}

using System;
using System.Globalization;

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
                        value = (string)value == "1";
                    }
                    break;
                case "dateTime.iso8601":
                    DateTime dateTime;
                    if (DateTime.TryParse(value.ToString(), out dateTime))
                    {
                        value = dateTime;
                    }
                    // HACK: Invalid according to the specs, but submitted by the WLW.
                    else if (DateTime.TryParseExact(
                                value.ToString(),
                                "yyyyMMddTHH:mm:ss",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out dateTime))
                    {
                        value = dateTime;
                    }
                    else
                    {
                        throw new FormatException(
                            "Unable to parse the passed date.");
                    }
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

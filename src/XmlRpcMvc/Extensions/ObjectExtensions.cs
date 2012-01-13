using System;
using System.Collections.Generic;

namespace XmlRpcMvc.Extensions
{
    using System.Globalization;

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
                    var formats =
                        new List<string>
                            {
                                "yyyyMMddTHH:mm:ss",
                                "yyyyMMddTHH:mm:ssZ"
                            };

                    DateTime dateTime;
                    if (DateTime.TryParse(value.ToString(), out dateTime))
                    {
                        value = dateTime;
                    }
                    else
                    {
                        foreach (var format in formats)
                        {
                            if (DateTime.TryParseExact(
                                    value.ToString(),
                                    format,
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out dateTime))
                            {
                                value = dateTime;
                                break;
                            }
                        }
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

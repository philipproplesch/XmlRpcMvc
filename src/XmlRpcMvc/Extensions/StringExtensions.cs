using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace XmlRpcMvc.Extensions
{
    public static class StringExtensions
    {
        public static DateTime ConvertToDateTime(this string instance)
        {
            DateTime dateTime;

            if (DateTime.TryParse(instance, out dateTime))
                return dateTime;

            return
                new List<string>
                    {
                        "yyyyMMddTHH:mm:ss",
                        "yyyyMMddTHH:mm:ssZ"
                    }
                    .Any(
                        format =>
                        DateTime.TryParseExact(
                            instance,
                            format,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out dateTime))
                    ? dateTime
                    : DateTime.MinValue;
        }
    }
}
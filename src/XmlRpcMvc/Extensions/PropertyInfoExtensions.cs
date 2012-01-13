using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace XmlRpcMvc.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static string GetSerializationName(
            this PropertyInfo instance)
        {
            return
                instance.GetCustomAttributes(
                    typeof(DataMemberAttribute),
                    true).
                        Cast<DataMemberAttribute>().
                        Where(
                            dma =>
                            !string.IsNullOrWhiteSpace(
                                dma.Name)).
                        Select(dma => dma.Name).
                        FirstOrDefault()
                            ?? instance.Name;
        }

    }
}

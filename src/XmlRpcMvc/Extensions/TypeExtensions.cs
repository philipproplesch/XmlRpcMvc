using System;

namespace XmlRpcMvc.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsPrimitive(this Type instance)
        {
            return instance.IsPrimitive ||
                   instance == TypeDef.Int ||
                   instance == TypeDef.UInt ||
                   instance == TypeDef.Long ||
                   instance == TypeDef.ULong ||
                   instance == TypeDef.Single ||
                   instance == TypeDef.Double ||
                   instance == TypeDef.Decimal ||
                   instance == TypeDef.Bool ||
                   instance == TypeDef.String ||
                   instance == TypeDef.Guid ||
                   instance == TypeDef.DateTime;
        }
    }
}

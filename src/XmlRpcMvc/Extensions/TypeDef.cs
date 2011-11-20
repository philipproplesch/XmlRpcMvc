using System;

namespace XmlRpcMvc.Extensions
{
    internal static class TypeDef
    {
        public static readonly Type Short = typeof(short);
        public static readonly Type UShort = typeof(ushort);
        public static readonly Type Int = typeof(int);
        public static readonly Type UInt = typeof(uint);
        public static readonly Type Long = typeof(long);
        public static readonly Type ULong = typeof(ulong);
        public static readonly Type Single = typeof(float);
        public static readonly Type Double = typeof(double);
        public static readonly Type Decimal = typeof(decimal);
        public static readonly Type Bool = typeof(bool);
        public static readonly Type String = typeof(string);
        public static readonly Type Guid = typeof(Guid);
        public static readonly Type DateTime = typeof(DateTime);
        public static readonly Type Byte = typeof(byte);
        public static readonly Type ByteArray = typeof(byte[]);
    }
}
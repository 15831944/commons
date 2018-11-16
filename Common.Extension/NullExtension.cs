namespace System
{
    public static class NullExtension
    {
        #region Built-In Types

        #region sbyte

        public static bool IsNull(this sbyte? s)
        {
            return s == null ? true : false;
        }

        public static bool IsNotNull(this sbyte? s)
        {
            return s == null ? false : true;
        }

        public static sbyte ToSByte(this sbyte? s)
        {
            return s ?? 0;
        }

        #endregion sbyte

        #region byte

        public static bool IsNull(this byte? b)
        {
            return b == null ? true : false;
        }

        public static bool IsNotNull(this byte? b)
        {
            return b == null ? false : true;
        }

        public static byte ToByte(this byte? b)
        {
            return b ?? 0;
        }

        #endregion byte

        #region char

        public static bool IsNull(this char? c)
        {
            return c == null ? true : false;
        }

        public static bool IsNotNull(this char? c)
        {
            return c == null ? false : true;
        }

        public static char ToChar(this char? c)
        {
            return c ?? '\0';
        }

        #endregion char

        #region short

        public static bool IsNull(this short? s)
        {
            return s == null ? true : false;
        }

        public static bool IsNotNull(this short? s)
        {
            return s == null ? false : true;
        }

        public static short ToShort(this short? s)
        {
            return s ?? 0;
        }

        public static short ToInt16(this short? s)

        {
            return s ?? 0;
        }

        #endregion short

        #region ushort

        public static bool IsNull(this ushort? u)
        {
            return u == null ? true : false;
        }

        public static bool IsNotNull(this ushort? u)
        {
            return u == null ? false : true;
        }

        public static ushort ToUShort(this ushort? u)
        {
            return u ?? 0;
        }

        public static ushort ToUInt16(this ushort? u)
        {
            return u ?? 0;
        }

        #endregion ushort

        #region int

        public static bool IsNull(this int? s)
        {
            return s == null ? true : false;
        }

        public static bool IsNotNull(this int? s)
        {
            return s == null ? false : true;
        }

        public static int ToInt(this int? s)
        {
            return s ?? 0;
        }

        public static int ToInt32(this int? s)
        {
            return s ?? 0;
        }

        #endregion int

        #region uint

        public static bool IsNull(this uint? u)
        {
            return u == null ? true : false;
        }

        public static bool IsNotNull(this uint? u)
        {
            return u == null ? true : false;
        }

        public static uint ToUInt(this uint? u)
        {
            return u ?? 0U;
        }

        public static uint ToUInt32(this uint? u)
        {
            return u ?? 0U;
        }

        #endregion uint

        #region long

        public static bool IsNull(this long? l)
        {
            return l == null ? true : false;
        }

        public static bool IsNotNull(this long? l)
        {
            return l == null ? false : true;
        }

        public static long ToLong(this long? l)
        {
            return l ?? 0L;
        }

        public static long ToInt64(this long? l)
        {
            return l ?? 0L;
        }

        #endregion long

        #region ulong

        public static bool IsNull(this ulong? u)
        {
            return u == null ? true : false;
        }

        public static bool IsNotNull(this ulong? u)
        {
            return u == null ? false : true;
        }

        public static ulong ToULong(this ulong? u)
        {
            return u ?? 0UL;
        }

        public static ulong ToUInt64(this ulong? u)
        {
            return u ?? 0UL;
        }

        #endregion ulong

        #region float

        public static bool IsNull(this float? f)
        {
            return f == null ? true : false;
        }

        public static bool IsNotNull(this float? f)
        {
            return f == null ? false : true;
        }

        public static float ToFloat(this float? f)
        {
            return f ?? 0.0F;
        }

        public static float ToSingle(this float? f)
        {
            return f ?? 0.0F;
        }

        #endregion float

        #region double

        public static bool IsNull(this double? d)
        {
            return d == null ? true : false;
        }

        public static bool IsNotNull(this double? d)
        {
            return d == null ? false : true;
        }

        public static double ToDouble(this double? d)
        {
            return d ?? 0.0D;
        }

        #endregion double

        #region bool

        public static bool IsNull(this bool? b)
        {
            return b == null ? true : false;
        }

        public static bool IsNotNull(this bool? b)
        {
            return b == null ? true : false;
        }

        public static bool ToBool(this bool? b)
        {
            return b ?? false;
        }

        public static bool ToBoolean(this bool? b)
        {
            return b ?? false;
        }

        #endregion bool

        #region decimal

        public static bool IsNull(this decimal? d)
        {
            return d == null ? true : false;
        }

        public static bool IsNotNull(this decimal? d)
        {
            return d == null ? false : true;
        }

        public static decimal ToDecimal(this decimal? d)
        {
            return d ?? 0.0M;
        }

        #endregion decimal

        #endregion Built-In Types

        public static bool IsNull(this DateTime? d)
        {
            return d == null ? true : false;
        }

        public static bool IsNotNull(this DateTime? d)
        {
            return d == null ? false : true;
        }

        public static DateTime ToDateTime(this DateTime? d)
        {
            return d ?? new DateTime(1970, 1, 1);
        }

        public static bool IsNull(this TimeSpan? t)
        {
            return t == null ? true : false;
        }

        public static bool IsNotNull(this TimeSpan? t)
        {
            return t == null ? false : true;
        }

        public static TimeSpan ToTimeSpan(this TimeSpan? t)
        {
            return t ?? new TimeSpan(0L);
        }

        public static bool IsNull(this ValueType v)
        {
            return v == null;
        }

        public static bool IsNotNull(this ValueType v)
        {
            return v != null;
        }
    }
}
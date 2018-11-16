namespace System
{
    public static class DoubleExtension
    {
        public static DateTime ToJavaScriptDateTimeUTC(this double l)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(l);
        }

        public static DateTime ToUnixDateTimeUTC(this double l)
        {
            return new DateTime(1970, 1, 1).AddSeconds(l);
        }
    }
}
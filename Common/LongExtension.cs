using System.Drawing;
using System.Globalization;

namespace System
{
    public static class LongExtension
    {
        public static DateTime ToJavaScriptDateTimeUTC(this long l)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0,
                new GregorianCalendar(), DateTimeKind.Utc).AddMilliseconds(l);
        }

        public static DateTime ToJavaScriptDateTime(this long l)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0,
                new GregorianCalendar(), DateTimeKind.Utc).AddMilliseconds(l).ToLocalTime();
        }

        public static DateTime ToUnixDateTimeUTC(this long l)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0,
                new GregorianCalendar(), DateTimeKind.Utc).AddSeconds(l);
        }

        public static DateTime ToUnixDateTime(this long l)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0,
                new GregorianCalendar(), DateTimeKind.Utc).AddSeconds(l).ToLocalTime();
        }

        /// <summary>
        ///     Converts the color of to heat.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="low">The low.</param>
        /// <param name="high">The high.</param>
        /// <returns></returns>
        public static string ConvertToHeatColor(this long value, long low, long high)
        {
            decimal range = high - low;
            var alpha = range == 0 ? 0 : (value / range * 255).RoundUp();
            var color = Color.FromArgb(alpha, 255, 165, 0);
            return color.ToRGBString();
        }
    }
}
using System.Globalization;

namespace System
{
    public static class DateTimeExtension
    {
        public static double ToJavaScriptTimeStampUTC(this DateTime d)
        {
            return (d - new DateTime(1970, 1, 1, 0, 0, 0, 0,
                new GregorianCalendar(), DateTimeKind.Utc)).TotalMilliseconds;
        }

        public static double ToJavaScriptTimeStamp(this DateTime d)
        {
            return (d - new DateTime(1970, 1, 1, 0, 0, 0, 0,
                new GregorianCalendar(), DateTimeKind.Local)).TotalMilliseconds;
        }

        public static double ToUnixTimeStampUTC(this DateTime d)
        {
            return (d - new DateTime(1970, 1, 1, 0, 0, 0, 0,
                new GregorianCalendar(), DateTimeKind.Utc)).TotalSeconds;
        }

        public static double ToUnixTimeStamp(this DateTime d)
        {
            return (d - new DateTime(1970, 1, 1, 0, 0, 0, 0,
                new GregorianCalendar(), DateTimeKind.Local)).TotalSeconds;
        }

        public static double ToUnixTimeStamp(this DateTime? d)
        {
            var e = d ?? new DateTime(1970, 1, 1, 0, 0, 0);
            return (e - new DateTime(1970, 1, 1, 0, 0, 0, 0,
                new GregorianCalendar(), DateTimeKind.Local)).TotalSeconds;
        }

        public static DateTime ToHourAndMinute(this DateTime d)
        {
            return new DateTime(1970, 1, 1, d.Hour, d.Minute, 0);
        }

        /// <summary>
        ///     Adds the business days.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        public static DateTime AddBusinessDays(this DateTime date, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    date = date.AddDays(sign);
                } while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || date.IsHoliday());
            }

            return date;
        }

        /// <summary>
        ///     Get the first day of the month
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime BeginOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        }

        /// <summary>
        ///     Get the first day of the month
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime? BeginOfMonth(this DateTime? date)
        {
            return date?.BeginOfMonth();
        }

        /// <summary>
        ///     Get the last day of the month
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
        }

        /// <summary>
        ///     Get the last day of the month
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime? EndOfMonth(this DateTime? date)
        {
            return date?.EndOfMonth();
        }

        /// <summary>
        ///     Determines whether [is business day].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///     <c>true</c> if [is business day] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBusinessDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !date.IsHoliday();
        }

        /// <summary>
        ///     Determines whether this instance is holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///     <c>true</c> if the specified date is holiday; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsHoliday(this DateTime date)
        {
            // TODO
            return false;
        }

        /// <summary>
        ///     Weeks the of year.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }

        /// <summary>
        ///     Weeks the of year.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static int? WeekOfYear(this DateTime? time)
        {
            return time?.WeekOfYear();
        }
    }
}
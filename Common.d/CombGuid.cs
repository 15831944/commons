namespace System
{
    /// <summary>
    /// Defines the <see cref="CombGuid" />
    /// </summary>
    public static class CombGuid
    {
        #region Fields

        /// <summary>
        /// Defines the nextGuid
        /// </summary>
        [ThreadStatic]
        public static Guid? nextGuid;

        #endregion Fields

        #region Methods

        /// <summary>
        /// The Bind
        /// </summary>
        /// <param name="dyn">The <see cref="dynamic"/></param>
        /// <returns>The <see cref="Guid"/></returns>
        public static Guid Bind(dynamic dyn)
        {
            var thisIsStupid = Guid.Parse(dyn.ToString());
            return thisIsStupid;
        }

        /// <summary>
        /// The NewGuid
        /// </summary>
        /// <returns>The <see cref="Guid"/></returns>
        public static Guid NewGuid()
        {
            if (nextGuid != null)
            {
                return nextGuid.Value;
            }

            var guidArray = Guid.NewGuid().ToByteArray();

            var baseDate = new DateTime(1900, 1, 1);
            var now = DateTime.Now;

            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            var msecs = now.TimeOfDay;

            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

        /// <summary>
        /// The SetNextGuid
        /// </summary>
        /// <param name="guid">The <see cref="Guid"/></param>
        public static void SetNextGuid(Guid guid)
        {
            nextGuid = guid;
        }

        #endregion Methods
    }
}
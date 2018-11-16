using System;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class EnumExtension
    {
        public static string GetName(this Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }

        public static int ToInt32(this Enum e)
        {
            return Convert.ToInt32(e);
        }

        public static int ToInt(this Enum e)
        {
            return Convert.ToInt32(e);
        }

        /// <summary>
        ///     Descriptions the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Description(this Enum value)
        {
            // get attributes
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
                displayAttribute = attributes.ElementAt(0);

            // return description
            return displayAttribute?.Description ?? value.ToString();
        }
    }
}
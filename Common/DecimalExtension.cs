using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DecimalExtension
    {
        /// <summary>
        ///     Rounds up.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="places">The places.</param>
        /// <returns></returns>
        public static decimal RoundUp(decimal input, int places)
        {
            var multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Convert.ToDecimal(Math.Ceiling(Convert.ToDouble(input) * multiplier) / multiplier);
        }

        /// <summary>
        ///     Rounds up.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static int RoundUp(this decimal input)
        {
            return Convert.ToInt32(RoundUp(input, 0));
        }

        /// <summary>
        ///     Forecasts the specified y.
        ///     based on http://bit.ly/29k08nO
        /// </summary>
        /// <param name="y">The y.</param>
        /// <param name="ys">The ys.</param>
        /// <param name="xs">The xs.</param>
        /// <returns></returns>
        public static decimal Forecast(this decimal y, IEnumerable<decimal> ys, IEnumerable<decimal> xs)
        {
            var yAvg = ys.Sum() / ys.Count();
            var xAvg = xs.Sum() / xs.Count();

            var yxs = ys.CombineWith(xs);

            var b = yxs.Sum(yx => (yx.Item1 - yAvg) * (yx.Item2 - xAvg)) / yxs.Sum(yx => (yx.Item2 - xAvg) * (yx.Item2 - xAvg));
            var a = yAvg - b * xAvg;

            return a + y * b;
        }
    }
}
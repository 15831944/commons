using System.Security.Cryptography;
using System.Threading;

namespace System.Helper
{
    /// <summary>
    /// 使用Random类生成伪随机数
    /// </summary>
    public class RandomHelper
    {
        #region Fields

        //随机数对象
        //随机数对象        /// <summary>
        /// Defines the _random
        /// </summary>
        private Random _random;

        /// <summary>
        /// Defines the rep
        /// </summary>
        private int rep = 0;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public RandomHelper()
        {
            //为随机数对象赋值
            //long num = DateTime.Now.Ticks + rep;
            this.rep++;
            //_random = new Random(((int)(((ulong)num) & 0xffffffffL)) | ((int)(num >> rep)));
            this._random = new Random();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 方法二：随机生成字符串（数字和字母混和）
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            //long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            //Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = this._random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 一：随机生成不重复数字字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public string GenerateCheckCodeNum(int codeCount)
        {
            string str = string.Empty;
            //long num2 = DateTime.Now.Ticks + rep;
            this.rep++;
            //Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = this._random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }

        /// <summary>
        /// 对一个数组进行随机排序
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public void GetRandomArray<T>(T[] arr)
        {
            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换

            //交换的次数,这里使用数组的长度作为交换次数
            int count = arr.Length;
            this.rep++;
            //开始交换
            for (int i = 0; i < count; i++)
            {
                //生成两个随机数位置
                int randomNum1 = this.GetRandomInt(0, arr.Length);
                int randomNum2 = this.GetRandomInt(0, arr.Length);

                //定义临时变量
                T temp;

                //交换两个随机数位置的值
                temp = arr[randomNum1];
                arr[randomNum1] = arr[randomNum2];
                arr[randomNum2] = temp;
            }
        }

        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public double GetRandomDouble()
        {
            this.rep++;
            return this._random.NextDouble();
        }

        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public int GetRandomInt(int minNum, int maxNum)
        {
            this.rep++;
            return this._random.Next(minNum, maxNum);
        }

        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        public string GetRandomCode(string allChar, int CodeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            this.rep++;
            var allCharArray = allChar.Split(',');
            var RandomCode = "";
            var temp = -1;
            //Random rand = new Random();
            for (var i = 0; i < CodeCount; i++)
            {
                //if (temp != -1)
                //{
                //rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                //}

                var t = this._random.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = this._random.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }

        #endregion Methods
    }

    /// <summary>
    ///     Getting random data with the Well512 algorithm.
    /// </summary>
    /// <remarks>This one is not cryptographically secure.</remarks>
    /// <see cref="http://stackoverflow.com/a/1227137/1837988" />
    public static class Well512RandomProvider
    {
        private static readonly uint[] State = new uint[16];

        private static uint _index;

        static Well512RandomProvider()
        {
            var random = new Random((int)DateTime.Now.Ticks);

            for (var i = 0; i < 16; i++)
            {
                State[i] = (uint)random.Next();
            }
        }

        /// <summary>
        ///     Returns a non-negative random integer.
        /// </summary>
        /// <returns>A non-negative random integer.</returns>
        public static uint Next()
        {
            var a = State[_index];
            var c = State[(_index + 13) & 15];
            var b = a ^ c ^ (a << 16) ^ (c << 15);
            c = State[(_index + 9) & 15];
            c ^= (c >> 11);
            a = State[_index] = b ^ c;
            var d = a ^ ((a << 5) & 0xda442d24U);
            //d = a ^ ((a << 5) & 0xDA442D24UL);
            _index = (_index + 15) & 15;
            a = State[_index];
            State[_index] = a ^ b ^ d ^ (a << 2) ^ (b << 18) ^ (c << 28);

            return State[_index];
        }

        /// <summary>
        ///     Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number to be generated. maxValue must be greater than or
        ///     equal to 0.
        /// </param>
        /// <returns>A non-negative random integer.</returns>
        public static uint Next(uint maxValue)
        {
            return Next() % maxValue;
        }

        /// <summary>
        ///     Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number returned. maxValue must be greater than or equal
        ///     to minValue.
        /// </param>
        /// <returns>A random integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static uint Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            return (uint)((Next() % (maxValue - minValue)) + minValue);
        }
    }

    /// <summary>
    ///     Getting random data in a thread-safe way.
    /// </summary>
    /// <remarks>This one is not cryptographically secure.</remarks>
    public static class RandomProvider
    {
        private static int _seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> Provider = new ThreadLocal<Random>(() =>
            new Random(Interlocked.Increment(ref _seed))
            );

        /// <summary>
        ///     Returns a non-negative random integer.
        /// </summary>
        /// <returns>A non-negative random integer.</returns>
        public static int Next()
        {
            return Provider.Value.Next();
        }

        /// <summary>
        ///     Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number to be generated. maxValue must be greater than or
        ///     equal to 0.
        /// </param>
        /// <returns>A non-negative random integer.</returns>
        public static int Next(int maxValue)
        {
            return Next() % maxValue;
        }

        /// <summary>
        ///     Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number returned. maxValue must be greater than or equal
        ///     to minValue.
        /// </param>
        /// <returns>A random integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            return ((Next() % (maxValue - minValue)) + minValue);
        }

        /// <summary>
        ///     Fills an array of bytes with a random sequence of values.
        /// </summary>
        /// <param name="data">The array to fill with random bytes.</param>
        public static void GetBytes(byte[] data)
        {
            Provider.Value.NextBytes(data);
        }
    }

    /// <summary>
    ///     Getting cryptographically strong random data in a thread-safe way.
    /// </summary>
    /// <remarks>This one is cryptographically secure.</remarks>
    public static class SecureRandomProvider
    {
        private static readonly ThreadLocal<RNGCryptoServiceProvider> Provider =
            new ThreadLocal<RNGCryptoServiceProvider>(() =>
                new RNGCryptoServiceProvider()
                );

        /// <summary>
        ///     Returns a non-negative random integer.
        /// </summary>
        /// <returns>A non-negative random integer.</returns>
        /// <exception cref="CryptographicException">The cryptographic service provider (CSP) cannot be acquired. </exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static int Next()
        {
            var buffer = new byte[4];
            Provider.Value.GetBytes(buffer);
            return new Random(BitConverter.ToInt32(buffer, 0)).Next();
        }

        /// <summary>
        ///     Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number to be generated. maxValue must be greater than or
        ///     equal to 0.
        /// </param>
        /// <returns>A non-negative random integer.</returns>
        /// <exception cref="CryptographicException">The cryptographic service provider (CSP) cannot be acquired. </exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static int Next(int maxValue)
        {
            return Next() % maxValue;
        }

        /// <summary>
        ///     Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number returned. maxValue must be greater than or equal
        ///     to minValue.
        /// </param>
        /// <returns>A random integer.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="CryptographicException">The cryptographic service provider (CSP) cannot be acquired. </exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            return ((Next() % (maxValue - minValue)) + minValue);
        }

        /// <summary>
        ///     Fills an array of bytes with a cryptographically strong random sequence of values.
        /// </summary>
        /// <param name="data">The array to fill with cryptographically strong random bytes.</param>
        /// <exception cref="CryptographicException">The cryptographic service provider (CSP) cannot be acquired. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is null.</exception>
        public static void GetBytes(byte[] data)
        {
            Provider.Value.GetBytes(data);
        }

        /// <summary>
        ///     Fills an array of bytes with a cryptographically strong sequence of random nonzero values.
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="CryptographicException">The cryptographic service provider (CSP) cannot be acquired. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is null.</exception>
        public static void GetNonZeroBytes(byte[] data)
        {
            Provider.Value.GetNonZeroBytes(data);
        }
    }
}
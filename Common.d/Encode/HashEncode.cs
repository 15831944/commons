namespace System.Encode
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// �õ������ȫ�루��ϣ���ܣ���
    /// </summary>
    public class HashEncode
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HashEncode"/> class.
        /// </summary>
        public HashEncode()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// �õ�һ�������ֵ
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            Random Seed = new Random();
            string RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }

        /// <summary>
        /// �õ������ϣ�����ַ���
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            string Security = HashEncoding(GetRandomValue());
            return Security;
        }

        /// <summary>
        /// ��ϣ����һ���ַ���
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        public static string HashEncoding(string Security)
        {
            byte[] Value;
            UnicodeEncoding Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(Security);
            SHA512Managed Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Security = "";
            foreach (byte o in Value)
            {
                Security += (int)o + "O";
            }
            return Security;
        }

        #endregion Methods
    }
}
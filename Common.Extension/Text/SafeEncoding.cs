namespace System.Text
{
    public class SafeEncoding
    {
        private static volatile Encoding utf8Encoding;
        private static volatile Encoding utf32Encoding;
        private static volatile Encoding bigEndianUTF32;
        private static volatile Encoding unicodeEncoding;
        private static volatile Encoding bigEndianUnicode;

        public static Encoding UTF8
        {
            get
            {
                if (utf8Encoding == null)
                {
                    utf8Encoding = new UTF8Encoding(false, true);
                }
                return utf8Encoding;
            }
        }

        public static Encoding UTF32
        {
            get
            {
                if (utf32Encoding == null)
                {
                    utf32Encoding = new UTF32Encoding(false, false, true);
                }
                return utf32Encoding;
            }
        }

        public static Encoding BigEndianUTF32
        {
            get
            {
                if (bigEndianUTF32 == null)
                {
                    bigEndianUTF32 = new UTF32Encoding(true, false, true);
                }
                return bigEndianUTF32;
            }
        }

        public static Encoding Unicode
        {
            get
            {
                if (unicodeEncoding == null)
                {
                    unicodeEncoding = new UnicodeEncoding(false, false, true);
                }
                return unicodeEncoding;
            }
        }

        public static Encoding BigEndianUnicode
        {
            get
            {
                if (bigEndianUnicode == null)
                {
                    bigEndianUnicode = new UnicodeEncoding(true, false, true);
                }
                return bigEndianUnicode;
            }
        }
    }
}
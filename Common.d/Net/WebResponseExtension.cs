using System.IO;
using System.Text;

namespace System.Net
{
    public static class WebResponseExtension
    {
        public static string GetResponseString(this HttpWebResponse response)
        {
            string result = string.Empty;
            StreamReader streamReader;
            if (!string.IsNullOrEmpty(response.CharacterSet))
            {
                Encoding encoding = Encoding.GetEncoding(response.CharacterSet);
                streamReader = new StreamReader(response.GetResponseStream(), encoding);
            }
            else
            {
                streamReader = new StreamReader(response.GetResponseStream());
            }
            using (streamReader)
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public static string GetResponseString(this HttpWebResponse response, Encoding encoding)
        {
            string result = string.Empty;
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), encoding);
            using (streamReader)
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }
}
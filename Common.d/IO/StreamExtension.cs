using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class StreamExtension
    {
        public static void WriteString(this Stream stream, string s)
        {
            stream.WriteString(s, Encoding.Default);
        }

        public static void WriteString(this Stream stream, string s, Encoding encoding)
        {
            byte[] array = s.ToBytes(encoding);
            stream.Write(array, 0, array.Length);
        }

        public static async Task SaveAsAsync(this Stream stream, string fullpath)
        {
            byte[] buffer = new byte[stream.Length];
            FileStream fs = new FileStream(fullpath, FileMode.CreateNew, FileAccess.Write);
            await stream.ReadAsync(buffer, 0, buffer.Length);
            await fs.WriteAsync(buffer, 0, buffer.Length);
            await fs.FlushAsync();
            fs.Close();
        }

        public static void SaveAs(this Stream stream, string fullpath)
        {
            byte[] buffer = new byte[stream.Length];
            stream.ReadAsync(buffer, 0, buffer.Length);
            FileStream fs = new FileStream(fullpath, FileMode.CreateNew, FileAccess.Write);
            fs.WriteAsync(buffer, 0, buffer.Length);
            fs.FlushAsync();
            fs.Close();
        }
    }
}
using System.IO;

namespace System.Data
{
    public static class DBLog
    {
        private static readonly object locker = new object();
        public static string path = AppDomain.CurrentDomain.BaseDirectory + "/Logs/DB";

        private enum LogLevel
        {
            Debug,
            Info,
            Warn,
            Error,
        }

        public static void Debug(string content)
        {
            if (DBInfo.LogLevel >= 4)
            {
                Write(content);
            }
        }

        public static void Info(string content)
        {
            if (DBInfo.LogLevel >= 3)
            {
                Write(content);
            }
        }

        public static void Warn(string content)
        {
            if (DBInfo.LogLevel >= 2)
            {
                Write(content);
            }
        }

        public static void Error(string content)
        {
            if (DBInfo.LogLevel >= 1)
            {
                Write(content);
            }
        }

        private static void Write(string content)
        {
            lock (locker)
            {
                var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                var filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                var dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    dir.Create();
                }
                var sw = File.AppendText(filename);
                sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                sw.WriteLine();
                sw.Write(content);
                sw.WriteLine();
                sw.Write("-----------------------------------------------------------------------------");
                sw.WriteLine();
                sw.Flush();
                sw.Close();
            }
        }
    }
}
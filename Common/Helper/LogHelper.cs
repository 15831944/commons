namespace System.Helper
{
    using System;
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// 日志帮助类。AppSettings节点可以配置LogHelper.Debug=0或LogHelper.Error=0来关闭日志记录。
    /// 如果不传入path参数，默认是在~/Log/下生成日志文件，也可以在AppSettings节点配置LogHelper.Path来设置默认日志文件路径，格式：D:\\File\\Log\\。
    /// </summary>
    public static class LogHelper
    {
        #region Fields

        /// <summary>
        /// Defines the olock
        /// </summary>
        private static readonly object olock = new object();

        #endregion Fields

        #region Enums

        /// <summary>
        /// Defines the LogHelperType
        /// </summary>
        private enum LogHelperType
        {
            /// <summary>
            /// Defines the Debug
            /// </summary>
            Debug,

            /// <summary>
            /// Defines the Error
            /// </summary>
            Error
        }

        #endregion Enums

        #region Methods

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <Param name="content">内容。如需换行可使用：\r\n</Param>
        /// <Param name="filePrefixName"></Param>
        /// <Param name="path">格式：D:\\File\\Logs\\</Param>
        public static void Debug(string content, string filePrefixName = null, string path = null)
        {
            Write(LogHelperType.Debug, content, filePrefixName, path);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <Param name="content">内容。如需换行可使用：\r\n</Param>
        /// <Param name="filePrefixName"></Param>
        /// <Param name="path">格式：D:\\File\\Logs\\</Param>
        public static void Error(string content, string filePrefixName = null, string path = null)
        {
            Write(LogHelperType.Error, content, filePrefixName, path);
        }

        /// <summary>
        /// filePrefixName是文件名前缀，最好用中文，方便在程序Logs文件下查看。
        /// </summary>
        /// <Param name="content">内容。如需换行可使用：\r\n</Param>
        /// <Param name="filePrefixName"></Param>
        /// <Param name="path"></Param>
        /// <Param name="logtype"></Param>
        private static void Write(LogHelperType logtype, string content, string filePrefixName = null, string path = null)
        {
            lock (olock)
            {
                try
                {
                    if (logtype == LogHelperType.Debug)
                    {
                        var logDebug = ConfigurationManager.AppSettings["LogHelper.Debug"];
                        if (logDebug != null && logDebug != "1")
                        {
                            return;
                        }
                    }
                    else
                    {
                        var logError = ConfigurationManager.AppSettings["LogHelper.Error"];
                        if (logError != null && logError != "1")
                        {
                            return;
                        }
                    }

                    var fileName = filePrefixName + DateTime.Now.ToString("yyyyMMdd") + logtype.ToString() + ".txt";
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        var logPath = ConfigurationManager.AppSettings["LogHelper.Path"];
                        if (string.IsNullOrWhiteSpace(logPath))
                        {
                            path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + fileName;
                        }
                        else
                        {
                            path = logPath + fileName;
                        }
                    }
                    else
                    {
                        path += fileName;
                    }
                    var di = new DirectoryInfo(path.Replace(fileName, ""));
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    //判断文件大小，需要新开文件
                    using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                    {
                        var sw = new StreamWriter(fs);
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
                catch
                {
                }
            }
        }

        #endregion Methods
    }
}
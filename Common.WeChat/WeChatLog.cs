using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace System.WeChat
{
  public class WeChatLog
  {
    //在网站根目录下创建日志目录
    public static string path = HttpContext.Current.Request.PhysicalApplicationPath + "/Logs/WeChat";

    /**
     * 向日志文件写入调试信息
     * @param className 类名
     * @param content 写入内容
     */

    public static void Debug(string className, string content)
    {
      if (WeChatInfo.LogLevel >= 3)
      {
        WriteLog("DEBUG", className, content);
      }
    }

    /**
    * 向日志文件写入运行时信息
    * @param className 类名
    * @param content 写入内容
    */

    public static void Info(string className, string content)
    {
      if (WeChatInfo.LogLevel >= 2)
      {
        WriteLog("INFO", className, content);
      }
    }

    /**
    * 向日志文件写入出错信息
    * @param className 类名
    * @param content 写入内容
    */

    public static void Error(string className, string content)
    {
      if (WeChatInfo.LogLevel >= 1)
      {
        WriteLog("ERROR", className, content);
      }
    }

    /**
    * 实际的写日志操作
    * @param type 日志记录类型
    * @param className 类名
    * @param content 写入内容
    */

    protected static void WriteLog(string type, string className, string content)
    {
      if (!Directory.Exists(path))//如果日志目录不存在就创建
      {
        Directory.CreateDirectory(path);
      }

      var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
      var filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名
      var dir = new DirectoryInfo(path);
      if (!dir.Exists)
      {
        dir.Create();
      }
      //创建或打开日志文件，向日志文件末尾追加记录
      var mySw = File.AppendText(filename);

      //向日志文件写入内容
      var write_content = time + " " + type + " " + className + ": " + content;
      mySw.WriteLine(write_content);

      //关闭日志文件
      mySw.Close();
    }
  }
}
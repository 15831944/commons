using System.Web;

namespace SufeiUtil.IP
{
    /// <summary>
    /// IP操作帮助类
    /// 编码人：宋文祺
    /// 编码时间：2015-05-18
    /// </summary>
    public class IPHelper
    {
        /// <summary>
        /// 提取开启代理/cdn服务后的客户端真实IP
        /// </summary>
        /// <returns></returns>
        public static string GetTrueIP()
        {
            string ip = string.Empty;
            string X_Forwarded_For = HttpContext.Current.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrWhiteSpace(X_Forwarded_For))
            {
                ip = X_Forwarded_For;
            }
            else
            {
                string CF_Connecting_IP = HttpContext.Current.Request.Headers["CF-Connecting-IP"];
                if (!string.IsNullOrWhiteSpace(CF_Connecting_IP))
                {
                    ip = CF_Connecting_IP;
                }
                else
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
            }
            return ip;
        }

        #region 获得用户IP

        /// <summary>
        /// 获得用户IP
        /// </summary>
        public static string GetUserIp()
        {
            string ip;
            string[] temp;
            bool isErr = false;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            else
                ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
            if (ip.Length > 15)
                isErr = true;
            else
            {
                temp = ip.Split('.');
                if (temp.Length == 4)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3) isErr = true;
                    }
                }
                else
                    isErr = true;
            }

            if (isErr)
                return "1.1.1.1";
            else
                return ip;
        }

        #endregion 获得用户IP
    }
}
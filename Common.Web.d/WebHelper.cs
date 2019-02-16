namespace System.Web
{
    /// <summary>
    /// 处理web端请求的一些处理方法
    /// </summary>
    public static class WebHelper
    {
        public static string RequestQueryString()
        {
            var queryString = string.Empty;
            var request = HttpContext.Current.Request;

            for (var i = 0; i < request.QueryString.Keys.Count; i++)
            {
                if (string.IsNullOrEmpty(queryString))
                {
                    queryString += "?" + request.QueryString.Keys[i] + "=" + request.QueryString[i];
                }
                else
                {
                    queryString += "&" + request.QueryString.Keys[i] + "=" + request.QueryString[i];
                }
            }
            return queryString;
        }

        public static string GetQueryString(string paramKey)
        {
            var request = HttpContext.Current.Request;

            return request.QueryString[paramKey] != null ? request.QueryString[paramKey].ToString().Replace("'", "''") : "";
        }

        public static int GetIntQueryString(string paramKey)
        {
            var request = HttpContext.Current.Request;
            var iOut = 0;
            if (request.QueryString[paramKey] != null)
            {
                var sOut = request[paramKey].ToString();
                if (!string.IsNullOrEmpty(sOut))
                {
                    int.TryParse(sOut, out iOut);
                }
            }
            return iOut;
        }

        public static short GetShortQueryString(string paramKey)
        {
            var request = HttpContext.Current.Request;
            short iOut = 0;
            if (request.QueryString[paramKey] != null)
            {
                var sOut = request[paramKey].ToString();
                if (!string.IsNullOrEmpty(sOut))
                {
                    short.TryParse(sOut, out iOut);
                }
            }
            return iOut;
        }

        public static DateTime GetDateTimeQueryString(string paramKey)
        {
            var request = HttpContext.Current.Request;
            var iOut = DateTime.MinValue;
            if (request.QueryString[paramKey] != null)
            {
                var sOut = request[paramKey].ToString();
                if (!string.IsNullOrEmpty(sOut))
                {
                    DateTime.TryParse(sOut, out iOut);
                }
            }
            return iOut;
        }

        public static string GetForm(string paramKey)
        {
            var request = HttpContext.Current.Request;
            return request.Form[paramKey] != null ? request.Form[paramKey].ToString().Replace("'", "''") : "";
        }

        public static string WebApplicationRootPath
        {
            get
            {
                var path = HttpContext.Current.Request.ApplicationPath;
                if (path.Length > 1)
                {
                    path = path + "/";
                }

                return path;
            }
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="strObject">可转为string类型的变量</param>
        /// <param name="strLen">截取字符位数，程序自动判断全半角，全角占2位，半角占1位</param>
        /// <returns>截取后字符串</returns>
        public static string TrimLen(object strObject, int strLen)
        {
            if (strObject == null || strObject == DBNull.Value)
            {
                return string.Empty;
            }

            var str = strObject.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            var arrChar = str.ToCharArray();
            var returnStr = str;
            var charCount = 0;

            for (var i = 0; i < arrChar.Length; i++)
            {
                if (arrChar[i] > 255)
                {
                    charCount += 2;
                }
                else
                {
                    charCount++;
                }

                if (charCount >= strLen)
                {
                    returnStr = str.Substring(0, i + 1);
                    if (str.Length > returnStr.Length)
                    {
                        returnStr += "...";
                    }

                    break;
                }
            }

            return returnStr;
        }

        // 控制时间格式
        public static string TrimDate(object dateObject, string dateType)
        {
            if (dateObject == null || dateObject == DBNull.Value)
            {
                return string.Empty;
            }

            try
            {
                Convert.ToDateTime(dateObject);
            }
            catch (Exception)
            {
                return dateObject.ToString();
            }

            return Convert.ToDateTime(dateObject).ToString(dateType);
        }

        // 将文本彩色显示
        public static string ShowColorText(object textObject, string color)
        {
            return textObject == null || textObject == DBNull.Value
                ? string.Empty
                : string.Format("<font color={0}>{1}</font>", color, textObject.ToString());
        }

        // 时间到期加亮显示
        public static string HighlightDate(object dateObject)
        {
            if (dateObject == null || dateObject == DBNull.Value)
            {
                return string.Empty;
            }

            DateTime srcDate;
            try
            {
                srcDate = Convert.ToDateTime(dateObject);
            }
            catch (Exception)
            {
                return dateObject.ToString();
            }

            return srcDate < DateTime.Today ? ShowColorText(TrimDate(srcDate, "yyyy-MM-dd"), "red") : TrimDate(srcDate, "yyyy-MM-dd");
        }

        public static string ShowImage(object boolObject)
        {
            if (boolObject == null || boolObject == DBNull.Value)
            {
                return string.Empty;
            }

            var visible = false;

            try
            {
                visible = Convert.ToBoolean(boolObject);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return visible == false ? "" : "display:none";
        }

        public static string FormatQuoteString(object objStr)
        {
            if (objStr == null)
            {
                return string.Empty;
            }

            var str = objStr.ToString();
            str = str.Replace("'", "\\'");
            str = str.Replace("\"", "\\'");
            return str;
        }

        public static void NoCacheThisPage()
        {
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Today.AddDays(-1);
            HttpContext.Current.Response.CacheControl = "no-cache";
        }

        public static string GetClientIp()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ip == null)
            {
                ip = HttpContext.Current.Request["Remote_Addr"];
            }
            if (ip == null)
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            return ip;
        }

        public static void RedirectError(string url)
        {
            HttpContext.Current.Response.Redirect(url);
        }

        /// <summary>
        /// 判断请求来源是否允许，允许则返回true
        /// </summary>
        public static bool IsAllowDomain
        {
            get
            {
                if (HttpContext.Current.Request.UrlReferrer == null)
                {
                    return false;
                }

                var reqDomain = HttpContext.Current.Request.UrlReferrer.Host.ToLower();
                return isAllowUrl(reqDomain);
            }
        }

        //TODO finish
        /// <summary>
        /// 判断请求来源是否允许
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool isAllowUrl(string url)
        {
            var AllowDomains = new string[] {
                " ",
                " "
            };
            foreach (var str in AllowDomains)
            {
                if (url.EndsWith(str))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断请求来源是否在排除之列(总站)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool isDenyUrl(string url)
        {
            var denyArr = new string[] {
                " ",
                " "
            };
            foreach (var str in denyArr)
            {
                if (url.StartsWith(str))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isAllowOpenUrl(string url)
        {
            return true;
        }

        /// <summary>
        /// 判断请求来源是否允许，允许则返回true
        /// </summary>
        public static bool IsAllowOpenDomain
        {
            get
            {
                if (HttpContext.Current.Request.UrlReferrer == null)
                {
                    return false;
                }

                var reqDomain = HttpContext.Current.Request.UrlReferrer.Host.ToLower();
                return isAllowUrl(reqDomain);
            }
        }
    }
}
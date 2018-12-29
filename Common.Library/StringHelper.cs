using System.Text.RegularExpressions;

namespace System
{
    public class StringHelper
    {
        #region 将字符串样式转换为纯字符串

        /// <summary>
        ///  将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            var RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                var NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }

        #endregion 将字符串样式转换为纯字符串

        #region 将字符串转换为新样式

        /// <summary>
        /// 将字符串转换为新样式
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="NewStyle"></param>
        /// <param name="SplitString"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            var ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                var strListLength = StrList.Length;
                var NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    var Lengstr = "";
                    for (var i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    var str = Lengstr.Split(',');
                    foreach (var bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }

        #endregion 将字符串转换为新样式

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitstr"></param>
        /// <returns></returns>
        public static string[] SplitMulti(string str, string splitstr)
        {
            string[] strArray = null;
            if ((str != null) && (str != ""))
            {
                strArray = new Regex(splitstr).Split(str);
            }
            return strArray;
        }

        public static string SqlSafeString(string String, bool IsDel)
        {
            if (IsDel)
            {
                String = String.Replace("'", "");
                String = String.Replace("\"", "");
                return String;
            }
            String = String.Replace("'", "&#39;");
            String = String.Replace("\"", "&#34;");
            return String;
        }

        #region 获取正确的Id，如果不是正整数，返回0

        /// <summary>
        /// 获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="_value"></param>
        /// <returns>返回正确的整数ID，失败返回0</returns>
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }

        #endregion 获取正确的Id，如果不是正整数，返回0

        #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。

        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。(0除外)
        /// </summary>
        /// <param name="_value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }

        #endregion 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。

        #region 快速验证一个字符串是否符合指定的正则表达式。

        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="_express">正则表达式的内容。</param>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            Regex myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }

        #endregion 快速验证一个字符串是否符合指定的正则表达式。
    }
}
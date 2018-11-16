using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace System.WeChat
{
  public class TenPayV3Data
  {
    //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
    private SortedDictionary<string, object> MainValues { get; set; } = new SortedDictionary<string, object>();

    /**
    * 设置某个字段的值
    * @param key 字段名
     * @param value 字段值
    */

    public void SetValue(string key, object value)
    {
      this.MainValues[key] = value;
    }

    /**
    * 根据字段名获取某个字段的值
    * @param key 字段名
     * @return key对应的字段值
    */

    public object GetValue(string key)
    {
      this.MainValues.TryGetValue(key, out var o);
      return o;
    }

    /**
     * 判断某个字段是否已设置
     * @param key 字段名
     * @return 若字段key已被设置，则返回true，否则返回false
     */

    public bool IsSet(string key)
    {
      this.MainValues.TryGetValue(key, out var o);
      if (null != o)
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    /**
    * @将Dictionary转成xml
    * @return 经转换得到的xml串
    * @throws WxPayException
    **/

    public string ToXml()
    {
      //数据为空时不能转化为xml格式
      if (0 == this.MainValues.Count)
      {
        WeChatLog.Error(this.GetType().ToString(), "WxPayData数据为空!");
        throw new TenPayV3Exception("WxPayData数据为空!");
      }

      var xml = "<xml>";
      foreach (var pair in this.MainValues)
      {
        //字段值不能为null，会影响后续流程
        if (pair.Value == null)
        {
          WeChatLog.Error(this.GetType().ToString(), "WxPayData内部含有值为null的字段!");
          throw new TenPayV3Exception("WxPayData内部含有值为null的字段!");
        }

        if (pair.Value.GetType() == typeof(int))
        {
          xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
        }
        else if (pair.Value.GetType() == typeof(string))
        {
          xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
        }
        else//除了string和int类型不能含有其他数据类型
        {
          WeChatLog.Error(this.GetType().ToString(), "WxPayData字段数据类型错误!");
          throw new TenPayV3Exception("WxPayData字段数据类型错误!");
        }
      }
      xml += "</xml>";
      return xml;
    }

    /**
    * @将xml转为WxPayData对象并返回对象内部的数据
    * @param string 待转换的xml串
    * @return 经转换得到的Dictionary
    * @throws WxPayException
    */

    public SortedDictionary<string, object> FromXml(string xml)
    {
      if (string.IsNullOrEmpty(xml))
      {
        WeChatLog.Error(this.GetType().ToString(), "将空的xml串转换为WxPayData不合法!");
        throw new TenPayV3Exception("将空的xml串转换为WxPayData不合法!");
      }

      var xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(xml);
      var xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
      var nodes = xmlNode.ChildNodes;
      foreach (XmlNode xn in nodes)
      {
        var xe = (XmlElement)xn;
        this.MainValues[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
      }

      try
      {
        //2015-06-29 错误是没有签名
        if (this.MainValues["return_code"].ToString() != "SUCCESS")
        {
          return this.MainValues;
        }
        CheckSign();//验证签名,不通过会抛异常
      }
      catch (TenPayV3Exception ex)
      {
        throw new TenPayV3Exception(ex.Message);
      }

      return this.MainValues;
    }

    /**
    * @Dictionary格式转化成url参数格式
    * @ return url格式串, 该串不包含sign字段值
    */

    public string ToUrl()
    {
      var buff = "";
      foreach (var pair in this.MainValues)
      {
        if (pair.Value == null)
        {
          WeChatLog.Error(this.GetType().ToString(), "WxPayData内部含有值为null的字段!");
          throw new TenPayV3Exception("WxPayData内部含有值为null的字段!");
        }

        if (pair.Key != "sign" && pair.Value.ToString() != "")
        {
          buff += pair.Key + "=" + pair.Value + "&";
        }
      }
      buff = buff.Trim('&');
      return buff;
    }

    /**
    * @Dictionary格式化成Json
     * @return json串数据
    */

    public string ToJson()
    {
      var jsonStr = JsonMapper.ToJson(this.MainValues);
      return jsonStr;
    }

    /**
    * @values格式化成能在Web页面上显示的结果（因为web页面上不能直接输出xml格式的字符串）
    */

    public string ToPrintStr()
    {
      var str = "";
      foreach (var pair in this.MainValues)
      {
        if (pair.Value == null)
        {
          WeChatLog.Error(this.GetType().ToString(), "WxPayData内部含有值为null的字段!");
          throw new TenPayV3Exception("WxPayData内部含有值为null的字段!");
        }

        str += string.Format("{0}={1}<br>", pair.Key, pair.Value.ToString());
      }
      WeChatLog.Debug(this.GetType().ToString(), "Print in Web Page : " + str);
      return str;
    }

    /**
    * @生成签名，详见签名生成算法
    * @return 签名, sign字段不参加签名
    */

    public string MakeSign()
    {
      //转url格式
      var str = ToUrl();
      //在string后加入API KEY
      str += "&key=" + WeChatInfo.Key;
      //MD5加密
      var md5 = MD5.Create();
      var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
      var sb = new StringBuilder();
      foreach (var b in bs)
      {
        sb.Append(b.ToString("x2"));
      }
      //所有字符转为大写
      return sb.ToString().ToUpper();
    }

    /**
    *
    * 检测签名是否正确
    * 正确返回true，错误抛异常
    */

    public bool CheckSign()
    {
      //如果没有设置签名，则跳过检测
      if (!IsSet("sign"))
      {
        WeChatLog.Error(this.GetType().ToString(), "WxPayData签名存在但不合法!");
        throw new TenPayV3Exception("WxPayData签名存在但不合法!");
      }
      //如果设置了签名但是签名为空，则抛异常
      else if (GetValue("sign") == null || GetValue("sign").ToString() == "")
      {
        WeChatLog.Error(this.GetType().ToString(), "WxPayData签名存在但不合法!");
        throw new TenPayV3Exception("WxPayData签名存在但不合法!");
      }

      //获取接收到的签名
      var return_sign = GetValue("sign").ToString();

      //在本地计算新的签名
      var cal_sign = MakeSign();

      if (cal_sign == return_sign)
      {
        return true;
      }

      WeChatLog.Error(this.GetType().ToString(), "WxPayData签名验证错误!");
      throw new TenPayV3Exception("WxPayData签名验证错误!");
    }

    /**
    * @获取Dictionary
    */

    public SortedDictionary<string, object> GetValues()
    {
      return this.MainValues;
    }
  }
}
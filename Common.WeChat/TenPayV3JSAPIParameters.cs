using Newtonsoft.Json;

using Senparc.Weixin.TenPay.V3;

namespace System.WeChat
{
    public class TenPayV3JSAPIParameters
    {
        public string AppId { get; set; }

        public string TimeStamp { get; set; }

        public string NonceStr { get; set; }

        public string Package { get; set; }

        public string SignType { get; set; }

        public string PaySign { get; set; }

        public TenPayV3JSAPIParameters(string prepay_id)
        {
            this.AppId = WeChatInfo.AppID;
            this.TimeStamp = TenPayV3Util.GetTimestamp();
            this.NonceStr = TenPayV3Util.GetNoncestr();
            this.Package = string.Format("prepay_id={0}", prepay_id);
            this.SignType = "MD5";
            this.PaySign = TenPayV3.GetJsPaySign(this.AppId, this.TimeStamp, this.NonceStr, this.Package, WeChatInfo.Key);
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;

namespace System.Cloud
{
    public static class SMSHelper
    {
        public static bool SendCaptcha(string phone, string captcha)
        {
            var product = "Dysmsapi";//短信API产品名称
            var domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            var accessKeyId = " ";//你的accessKeyId
            var accessKeySecret = " ";//你的accessKeySecret
            var profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            var acsClient = new DefaultAcsClient(profile);
            var request = new SendSmsRequest
            {
                PhoneNumbers = phone,
                SignName = "脚印温馨提醒",
                TemplateCode = "SMS_134250397",
                TemplateParam = "{\"code\":\"" + captcha + "\"}"
            };
            var response = acsClient.GetAcsResponse(request);
            return true;
        }

        public static bool SendNotice(string phone, string username)
        {
            return false;
        }
    }
}
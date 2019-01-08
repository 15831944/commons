using Newtonsoft.Json;

using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace System.WeChat
{
    public class WeChatHelper
    {
        public static string GetCodeUri(Uri redirectURI, string state)
        {
            return OAuthApi.GetAuthorizeUrl(WeChatInfo.AppID, redirectURI.ToString(), state, OAuthScope.snsapi_userinfo);
        }

        public static AccessTokenResult GetAccessToken(string code)
        {
            var result = OAuthApi.GetAccessToken(WeChatInfo.AppID, WeChatInfo.AppSecret, code);
            if (string.IsNullOrWhiteSpace(result.access_token))
            {
                return null;
            }
            else
            {
                var token = new AccessTokenResult
                {
                    AccessToken = result.access_token,
                    ExpiresIn = result.expires_in,
                    CurrentTime = DateTime.Now,
                    RefreshTime = DateTime.Now,
                    OpenID = result.openid,
                    RefreshToken = result.refresh_token,
                    Scope = result.scope,
                };
                return token;
            }
        }

        public static AccessTokenResult RefreshAccessToken(AccessTokenResult token)
        {
            var appId = WeChatInfo.AppID;
            if (DateTime.Now < token.CurrentTime.AddSeconds(token.ExpiresIn - 100))
            {
                return token;
            }
            var result = OAuthApi.RefreshToken(appId, token.RefreshToken);
            if (string.IsNullOrWhiteSpace(result.access_token))
            {
                return null;
            }
            else
            {
                token.RefreshTime = DateTime.Now;
                token.AccessToken = result.access_token;
                token.ExpiresIn = result.expires_in;
                token.RefreshToken = result.refresh_token;
                token.OpenID = result.openid;
                token.Scope = result.scope;
                return token;
            }
        }

        public static WeChatUserResult GetWeChatUser(AccessTokenResult token)
        {
            var user = OAuthApi.GetUserInfo(token.AccessToken, token.OpenID);
            var result = new WeChatUserResult
            {
                City = user.city,
                Country = user.country,
                HeadImgUrl = user.headimgurl,
                NickName = user.nickname,
                OpenID = user.openid,
                Privilege = JsonConvert.SerializeObject(user.privilege),
                Province = user.province,
                Sex = user.sex,
                UnionID = user.unionid,
            };
            return result;
        }
    }
}
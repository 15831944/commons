using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;

namespace System.WeChat
{
    public class WeChatSession : ClaimInfo<string>
    {
        public string OpenID { get; set; }
        public string NickName { get; set; }
        public int Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string HeadImgUrl { get; set; }
        public string Privilege { get; set; }
        public string UnionID { get; set; }

        public List<Claim> ToClaims()
        {
            var claims = new List<Claim> {
                new Claim("OpenID",this.OpenID??""),
                new Claim("NickName",this.NickName??""),
                new Claim("Sex",this.Sex.ToString()),
                new Claim("Province",this.Province??""),
                new Claim("City",this.City??""),
                new Claim("Country",this.Country??""),
                new Claim("HeadImgUrl",this.HeadImgUrl??""),
                new Claim("Privilege",this.Privilege??""),
                new Claim("UnionID",this.UnionID??""),
            };
            return claims;
        }

        public static WeChatSession Parse(IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                var session = new WeChatSession
                {
                };
                return session;
            }
            else
            {
                return null;
            }
        }
    }
}
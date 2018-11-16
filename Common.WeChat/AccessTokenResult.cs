using System.Runtime.Serialization;

namespace System.WeChat
{
    [Serializable]
    [DataContract]
    public class AccessTokenResult
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime CurrentTime { get; set; }
        public DateTime RefreshTime { get; set; }
        public string RefreshToken { get; set; }
        public string OpenID { get; set; }
        public string Scope { get; set; }
    }
}
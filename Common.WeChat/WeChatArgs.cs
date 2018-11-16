namespace System.WeChat
{
    public class WeChatArgs
    {
        public string AppID { get; set; }
        public string AppSecret { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
    }
}
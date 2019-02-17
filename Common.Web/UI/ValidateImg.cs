using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace System.Web.UI
{
    /// <summary>
    /// 验证码 继承 System.Web.UI.Page ，Session["xk_validate_code"]
    /// </summary>
    public class ValidateImg : System.Web.UI.Page
    {
        private void Page_Load(object sender, EventArgs e)
        {
            var chars = "023456789".ToCharArray();
            var random = new Random();

            var validateCode = string.Empty;
            for (var i = 0; i < 4; i++)
            {
                var rc = chars[random.Next(0, chars.Length)];
                if (validateCode.IndexOf(rc) > -1)
                {
                    i--;
                    continue;
                }
                validateCode += rc;
            }
            this.Session["xk_validate_code"] = validateCode;
            this.CreateImage(validateCode);
        }

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="checkCode"></param>
        private void CreateImage(string checkCode)
        {
            var iwidth = checkCode.Length * 11;
            var image = new Bitmap(iwidth, 19);
            var g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Chocolate, Color.Brown, Color.DarkCyan, Color.Purple };
            var rand = new Random();

            //输出不同字体和颜色的验证码字符
            for (var i = 0; i < checkCode.Length; i++)
            {
                var cindex = rand.Next(7);
                var f = new Font("Microsoft Sans Serif", 11);
                var b = new SolidBrush(c[cindex]);
                g.DrawString(checkCode.Substring(i, 1), f, b, (i * 10) + 1, 0, StringFormat.GenericDefault);
            }
            //画一个边框
            g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);

            //输出到浏览器
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            this.Response.ClearContent();
            this.Response.ContentType = "image/Jpeg";
            this.Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }
    }
}
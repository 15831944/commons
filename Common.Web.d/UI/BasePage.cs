using System.IO;
using System.Web.UI.WebControls;

namespace System.Web.UI
{
    public class BasePage : Page
    {
        public BasePage()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public static new string Title = "标题";

        public static string keywords = "关键字";

        public static string description = "网站描述";

        protected override void OnInit(EventArgs e)
        {
            if (this.Session["admin"] == null || this.Session["admin"].ToString().Trim() == "")
            {
                this.Response.Redirect("login.aspx");
            }
            base.OnInit(e);
        }

        protected void ExportData(string strContent, string FileName)
        {
            FileName = FileName + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();

            this.Response.Clear();
            this.Response.Charset = "gb2312";
            this.Response.ContentType = "application/ms-excel";
            this.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //this.Page.EnableViewState = false;
            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名
            this.Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls");
            // 把文件流发送到客户端
            this.Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            this.Response.Write(strContent);
            this.Response.Write("</body></html>");
            // 停止页面的执行
            //Response.End();
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="obj"></param>
        public void ExportData(GridView obj)
        {
            try
            {
                var style = obj.Rows.Count > 0 ? @"<style> .text { mso-number-format:\@; } </script> " : "no data.";
                this.Response.ClearContent();
                var dt = DateTime.Now;
                var filename = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString();
                this.Response.AddHeader("content-disposition", "attachment; filename=ExportData" + filename + ".xls");
                this.Response.ContentType = "application/ms-excel";
                this.Response.Charset = "GB2312";
                this.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                obj.RenderControl(htw);
                this.Response.Write(style);
                this.Response.Write(sw.ToString());
                this.Response.End();
            }
            catch
            {
            }
        }
    }
}
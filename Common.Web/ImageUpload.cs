using System.Drawing;
using System.IO;
using System.Web.UI.HtmlControls;

namespace System.Web
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileExtension
    {
        JPG = 255216,

        GIF = 7173,

        BMP = 6677,

        PNG = 13780,

        RAR = 8297,

        jpg = 255216,

        exe = 7790,

        xml = 6063,

        html = 6033,

        aspx = 239187,

        cs = 117115,

        js = 119105,

        txt = 210187,

        sql = 255254
    }

    /// <summary>
    /// 图片检测类
    /// </summary>
    public static class FileValidation
    {
        #region 上传图片检测类

        /// <summary>
        /// 是否允许
        /// </summary>
        public static bool IsAllowedExtension(HttpPostedFile oFile, FileExtension[] fileEx)
        {
            var fileLen = oFile.ContentLength;
            var imgArray = new byte[fileLen];
            oFile.InputStream.Read(imgArray, 0, fileLen);
            var ms = new MemoryStream(imgArray);
            var br = new BinaryReader(ms);
            var fileclass = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileclass = buffer.ToString();
                buffer = br.ReadByte();
                fileclass += buffer.ToString();
            }
            catch { }
            br.Close();
            ms.Close();
            foreach (var fe in fileEx)
            {
                if (int.Parse(fileclass) == (int)fe)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 上传前的图片是否可靠
        /// </summary>
        public static bool IsSecureUploadPhoto(HttpPostedFile oFile)
        {
            var isPhoto = false;
            var fileExtension = Path.GetExtension(oFile.FileName).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
            for (var i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    isPhoto = true;
                    break;
                }
            }
            if (!isPhoto)
            {
                return true;
            }
            FileExtension[] fe = { FileExtension.BMP, FileExtension.GIF, FileExtension.JPG, FileExtension.PNG };

            return IsAllowedExtension(oFile, fe) ? true : false;
        }

        /// <summary>
        /// 上传后的图片是否安全
        /// </summary>
        /// <param name="photoFile">物理地址</param>
        public static bool IsSecureUpfilePhoto(string photoFile)
        {
            var isPhoto = false;
            var Img = "Yes";
            var fileExtension = Path.GetExtension(photoFile).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
            for (var i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    isPhoto = true;
                    break;
                }
            }

            if (!isPhoto)
            {
                return true;
            }
            var sr = new StreamReader(photoFile, System.Text.Encoding.Default);
            var strContent = sr.ReadToEnd();
            sr.Close();
            var str = "request|<script|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
            foreach (var s in str.Split('|'))
            {
                if (strContent.ToLower().IndexOf(s) != -1)
                {
                    File.Delete(photoFile);
                    Img = "No";
                    break;
                }
            }
            return (Img == "Yes");
        }

        #endregion 上传图片检测类
    }

    /// <summary>
    /// 图片上传类
    /// </summary>
    //----------------调用-------------------
    //imageUpload iu = new imageUpload();
    //iu.AddText = "";
    //iu.CopyIamgePath = "";
    //iu.DrawString_x = ;
    //iu.DrawString_y = ;
    //iu.DrawStyle = ;
    //iu.Font = "";
    //iu.FontSize = ;
    //iu.FormFile = File1;
    //iu.IsCreateImg =;
    //iu.IsDraw = ;
    //iu.OutFileName = "";
    //iu.OutThumbFileName = "";
    //iu.SavePath = @"~/image/";
    //iu.SaveType = ;
    //iu.sHeight  = ;
    //iu.sWidth   = ;
    //iu.Upload();
    //--------------------------------------
    public class ImageUpload
    {
        #region 私有成员

        private int _MaxSize = 1024 * 1024;//最大单个上传文件 (默认)

        private string _FileType = "jpg;gif;bmp;png";//所支持的上传类型用"/"隔开

        private string _SavePath = HttpContext.Current.Server.MapPath(".") + "\\";//保存文件的实际路径

        private int _SaveType = 0;//上传文件的类型，0代表自动生成文件名

        private HtmlInputFile _FormFile;//上传控件。

        private string _InFileName = "";//非自动生成文件名设置。

        private string _CopyIamgePath = HttpContext.Current.Server.MapPath(".") + "/images/5dm_new.jpg";//图片水印模式下的覆盖图片的实际地址

        #endregion 私有成员

        #region 公有属性

        /// <summary>
        /// Error返回值
        /// 1、没有上传的文件
        /// 2、类型不允许
        /// 3、大小超限
        /// 4、未知错误
        /// 0、上传成功。
        /// </summary>
        public int Error { get; private set; } = 0;

        /// <summary>
        /// 最大单个上传文件
        /// </summary>
        public int MaxSize
        {
            set => this._MaxSize = value;
        }

        /// <summary>
        /// 所支持的上传类型用";"隔开
        /// </summary>
        public string FileType
        {
            set => this._FileType = value;
        }

        /// <summary>
        /// 保存文件的实际路径
        /// </summary>
        public string SavePath
        {
            set => this._SavePath = HttpContext.Current.Server.MapPath(value);
            get => this._SavePath;
        }

        /// <summary>
        /// 上传文件的类型，0代表自动生成文件名
        /// </summary>
        public int SaveType
        {
            set => this._SaveType = value;
        }

        /// <summary>
        /// 上传控件
        /// </summary>
        public HtmlInputFile FormFile
        {
            set => this._FormFile = value;
        }

        /// <summary>
        /// 非自动生成文件名设置。
        /// </summary>
        public string InFileName
        {
            set => this._InFileName = value;
        }

        /// <summary>
        /// 输出文件名
        /// </summary>
        public string OutFileName { get; set; } = "";

        /// <summary>
        /// 输出的缩略图文件名
        /// </summary>
        public string OutThumbFileName { get; set; }

        /// <summary>
        /// 是否有缩略图生成.
        /// </summary>
        public bool Iss { get; private set; } = false;

        /// <summary>
        /// 获取上传图片的宽度
        /// </summary>
        public int Width { get; private set; } = 0;

        /// <summary>
        /// 获取上传图片的高度
        /// </summary>
        public int Height { get; private set; } = 0;

        /// <summary>
        /// 设置缩略图的宽度
        /// </summary>
        public int sWidth { get; set; } = 120;

        /// <summary>
        /// 设置缩略图的高度
        /// </summary>
        public int sHeight { get; set; } = 120;

        /// <summary>
        /// 是否生成缩略图
        /// </summary>
        public bool IsCreateImg { get; set; } = true;

        /// <summary>
        /// 是否加水印
        /// </summary>
        public bool IsDraw { get; set; } = false;

        /// <summary>
        /// 设置加水印的方式
        /// 0:文字水印模式
        /// 1:图片水印模式
        /// 2:不加
        /// </summary>
        public int DrawStyle { get; set; } = 0;

        /// <summary>
        /// 绘制文本的Ｘ坐标（左上角）
        /// </summary>
        public int DrawString_x { get; set; } = 10;

        /// <summary>
        /// 绘制文本的Ｙ坐标（左上角）
        /// </summary>
        public int DrawString_y { get; set; } = 10;

        /// <summary>
        /// 设置文字水印内容
        /// </summary>
        public string AddText { get; set; } = "GlobalNatureCrafts";

        /// <summary>
        /// 设置文字水印字体
        /// </summary>
        public string Font { get; set; } = "宋体";

        /// <summary>
        /// 设置文字水印字的大小
        /// </summary>
        public int FontSize { get; set; } = 12;

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize { get; set; } = 0;

        /// <summary>
        /// 图片水印模式下的覆盖图片的实际地址
        /// </summary>
        public string CopyIamgePath
        {
            set => this._CopyIamgePath = HttpContext.Current.Server.MapPath(value);
        }

        #endregion 公有属性

        #region 私有方法

        /// <summary>
        /// 获取文件的后缀名
        /// </summary>
        private string GetExt(string path)
        {
            return Path.GetExtension(path);
        }

        /// <summary>
        /// 获取输出文件的文件名
        /// </summary>
        private string FileName(string Ext)
        {
            return this._SaveType == 0 || this._InFileName.Trim() == "" ? DateTime.Now.ToString("yyyyMMddHHmmssfff") + Ext : this._InFileName;
        }

        /// <summary>
        /// 检查上传的文件的类型，是否允许上传。
        /// </summary>
        private bool IsUpload(string Ext)
        {
            Ext = Ext.Replace(".", "");
            var b = false;
            var arrFileType = this._FileType.Split(';');
            foreach (var str in arrFileType)
            {
                if (str.ToLower() == Ext.ToLower())
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        #endregion 私有方法

        #region 上传图片

        public void Upload()
        {
            var hpFile = this._FormFile.PostedFile;
            if (hpFile == null || hpFile.FileName.Trim() == "")
            {
                this.Error = 1;
                return;
            }
            var Ext = this.GetExt(hpFile.FileName);
            if (!this.IsUpload(Ext))
            {
                this.Error = 2;
                return;
            }
            var iLen = hpFile.ContentLength;
            if (iLen > this._MaxSize)
            {
                this.Error = 3;
                return;
            }
            try
            {
                if (!Directory.Exists(this._SavePath))
                {
                    Directory.CreateDirectory(this._SavePath);
                }

                var bData = new byte[iLen];
                hpFile.InputStream.Read(bData, 0, iLen);
                string fName;
                fName = this.FileName(Ext);
                var TempFile = this.IsDraw ? fName.Split('.').GetValue(0).ToString() + "_temp." + fName.Split('.').GetValue(1).ToString() : fName;
                var newFile = new FileStream(this._SavePath + TempFile, FileMode.Create);
                newFile.Write(bData, 0, bData.Length);
                newFile.Flush();
                var _FileSizeTemp = hpFile.ContentLength;

                var ImageFilePath = this._SavePath + fName;
                if (this.IsDraw)
                {
                    if (this.DrawStyle == 0)
                    {
                        var Img1 = Image.FromStream(newFile);
                        var g = Graphics.FromImage(Img1);
                        g.DrawImage(Img1, 100, 100, Img1.Width, Img1.Height);
                        var f = new Font(this.Font, this.FontSize);
                        Brush b = new SolidBrush(Color.Red);
                        var addtext = this.AddText;
                        g.DrawString(addtext, f, b, this.DrawString_x, this.DrawString_y);
                        g.Dispose();
                        Img1.Save(ImageFilePath);
                        Img1.Dispose();
                    }
                    else
                    {
                        var image = Image.FromStream(newFile);
                        var copyImage = Image.FromFile(this._CopyIamgePath);
                        var g = Graphics.FromImage(image);
                        g.DrawImage(copyImage, new Rectangle(image.Width - copyImage.Width - 5, image.Height - copyImage.Height - 5, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                        g.Dispose();
                        image.Save(ImageFilePath);
                        image.Dispose();
                    }
                }

                //获取图片的高度和宽度
                var Img = Image.FromStream(newFile);
                this.Width = Img.Width;
                this.Height = Img.Height;

                //生成缩略图部分
                if (this.IsCreateImg)
                {
                    #region 缩略图大小只设置了最大范围，并不是实际大小

                    var realbili = this.Width / (float)this.Height;
                    var wishbili = this.sWidth / (float)this.sHeight;

                    //实际图比缩略图最大尺寸更宽矮，以宽为准
                    if (realbili > wishbili)
                    {
                        this.sHeight = (int)(this.sWidth / realbili);
                    }
                    //实际图比缩略图最大尺寸更高长，以高为准
                    else
                    {
                        this.sWidth = (int)(this.sHeight * realbili);
                    }

                    #endregion 缩略图大小只设置了最大范围，并不是实际大小

                    this.OutThumbFileName = fName.Split('.').GetValue(0).ToString() + "_s." + fName.Split('.').GetValue(1).ToString();
                    var ImgFilePath = this._SavePath + this.OutThumbFileName;

                    var newImg = Img.GetThumbnailImage(this.sWidth, this.sHeight, null, IntPtr.Zero);
                    newImg.Save(ImgFilePath);
                    newImg.Dispose();
                    this.Iss = true;
                }

                if (this.IsDraw)
                {
                    if (File.Exists(this._SavePath + fName.Split('.').GetValue(0).ToString() + "_temp." + fName.Split('.').GetValue(1).ToString()))
                    {
                        newFile.Dispose();
                        File.Delete(this._SavePath + fName.Split('.').GetValue(0).ToString() + "_temp." + fName.Split('.').GetValue(1).ToString());
                    }
                }
                newFile.Close();
                newFile.Dispose();
                this.OutFileName = fName;
                this.FileSize = _FileSizeTemp;
                this.Error = 0;
                return;
            }
            catch (Exception)
            {
                this.Error = 4;
                return;
            }
        }

        #endregion 上传图片
    }
}
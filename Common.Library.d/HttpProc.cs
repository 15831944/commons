using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace System
{
    /// <summary>
    /// �ϴ����ݲ���
    /// </summary>
    public class UploadEventArgs : EventArgs
    {
        private int bytesSent;

        private int totalBytes;

        /// <summary>
        /// �ѷ��͵��ֽ���
        /// </summary>
        public int BytesSent
        {
            get { return this.bytesSent; }
            set { this.bytesSent = value; }
        }

        /// <summary>
        /// ���ֽ���
        /// </summary>
        public int TotalBytes
        {
            get { return this.totalBytes; }
            set { this.totalBytes = value; }
        }
    }

    /// <summary>
    /// �������ݲ���
    /// </summary>
    public class DownloadEventArgs : EventArgs
    {
        private int bytesReceived;

        private int totalBytes;

        private byte[] receivedData;

        /// <summary>
        /// �ѽ��յ��ֽ���
        /// </summary>
        public int BytesReceived
        {
            get { return this.bytesReceived; }
            set { this.bytesReceived = value; }
        }

        /// <summary>
        /// ���ֽ���
        /// </summary>
        public int TotalBytes
        {
            get { return this.totalBytes; }
            set { this.totalBytes = value; }
        }

        /// <summary>
        /// ��ǰ���������յ�����
        /// </summary>
        public byte[] ReceivedData
        {
            get { return this.receivedData; }
            set { this.receivedData = value; }
        }
    }

    public class WebClient
    {
        private Encoding encoding = Encoding.Default;

        private string respHtml = "";

        private WebProxy proxy;

        private static CookieContainer cc;

        private WebHeaderCollection requestHeaders;

        private WebHeaderCollection responseHeaders;

        private int bufferSize = 15240;

        public event EventHandler<UploadEventArgs> UploadProgressChanged;

        public event EventHandler<DownloadEventArgs> DownloadProgressChanged;

        static WebClient()
        {
            LoadCookiesFromDisk();
        }

        /// <summary>
        /// ����WebClient��ʵ��
        /// </summary>
        public WebClient()
        {
            this.requestHeaders = new WebHeaderCollection();
            this.responseHeaders = new WebHeaderCollection();
        }

        /// <summary>
        /// ���÷��ͺͽ��յ����ݻ����С
        /// </summary>
        public int BufferSize
        {
            get { return this.bufferSize; }
            set { this.bufferSize = value; }
        }

        /// <summary>
        /// ��ȡ��Ӧͷ����
        /// </summary>
        public WebHeaderCollection ResponseHeaders
        {
            get { return this.responseHeaders; }
        }

        /// <summary>
        /// ��ȡ����ͷ����
        /// </summary>
        public WebHeaderCollection RequestHeaders
        {
            get { return this.requestHeaders; }
        }

        /// <summary>
        /// ��ȡ�����ô���
        /// </summary>
        public WebProxy Proxy
        {
            get { return this.proxy; }
            set { this.proxy = value; }
        }

        /// <summary>
        /// ��ȡ��������������Ӧ���ı����뷽ʽ
        /// </summary>
        public Encoding Encoding
        {
            get { return this.encoding; }
            set { this.encoding = value; }
        }

        /// <summary>
        /// ��ȡ��������Ӧ��html����
        /// </summary>
        public string RespHtml
        {
            get { return this.respHtml; }
            set { this.respHtml = value; }
        }

        /// <summary>
        /// ��ȡ�����������������Cookie����
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return cc; }
            set { cc = value; }
        }

        /// <summary>
        ///  ��ȡ��ҳԴ����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns></returns>
        public string GetHtml(string url)
        {
            HttpWebRequest request = this.CreateRequest(url, "GET");
            this.respHtml = this.encoding.GetString(this.GetData(request));
            return this.respHtml;
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="url">�ļ�URL��ַ</param>
        /// <param name="filename">�ļ���������·��</param>
        public void DownloadFile(string url, string filename)
        {
            FileStream fs = null;
            try
            {
                HttpWebRequest request = this.CreateRequest(url, "GET");
                byte[] data = this.GetData(request);
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                fs.Write(data, 0, data.Length);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// ��ָ��URL��������
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns></returns>
        public byte[] GetData(string url)
        {
            HttpWebRequest request = this.CreateRequest(url, "GET");
            return this.GetData(request);
        }

        /// <summary>
        /// ��ָ��URL�����ı�����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <param name="postData">urlencode������ı�����</param>
        /// <returns></returns>
        public string Post(string url, string postData)
        {
            byte[] data = this.encoding.GetBytes(postData);
            return this.Post(url, data);
        }

        /// <summary>
        /// ��ָ��URL�����ֽ�����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <param name="postData">���͵��ֽ�����</param>
        /// <returns></returns>
        public string Post(string url, byte[] postData)
        {
            HttpWebRequest request = this.CreateRequest(url, "POST");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.KeepAlive = true;
            this.PostData(request, postData);
            this.respHtml = this.encoding.GetString(this.GetData(request));
            return this.respHtml;
        }

        /// <summary>
        /// ��ָ����ַ����mulitpart���������
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <param name="mulitpartForm">mulitpart form data</param>
        /// <returns></returns>
        public string Post(string url, MultipartForm mulitpartForm)
        {
            HttpWebRequest request = this.CreateRequest(url, "POST");
            request.ContentType = mulitpartForm.ContentType;
            request.ContentLength = mulitpartForm.FormData.Length;
            request.KeepAlive = true;
            this.PostData(request, mulitpartForm.FormData);
            this.respHtml = this.encoding.GetString(this.GetData(request));
            return this.respHtml;
        }

        /// <summary>
        /// ��ȡ���󷵻ص�����
        /// </summary>
        /// <param name="request">�������</param>
        /// <returns></returns>
        private byte[] GetData(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            this.responseHeaders = response.Headers;
            //SaveCookiesToDisk();

            DownloadEventArgs args = new DownloadEventArgs();
            if (this.responseHeaders[HttpResponseHeader.ContentLength] != null)
            {
                args.TotalBytes = Convert.ToInt32(this.responseHeaders[HttpResponseHeader.ContentLength]);
            }

            MemoryStream ms = new MemoryStream();
            int count = 0;
            byte[] buf = new byte[this.bufferSize];
            while ((count = stream.Read(buf, 0, buf.Length)) > 0)
            {
                ms.Write(buf, 0, count);
                if (this.DownloadProgressChanged != null)
                {
                    args.BytesReceived += count;
                    args.ReceivedData = new byte[count];
                    Array.Copy(buf, args.ReceivedData, count);
                    this.DownloadProgressChanged(this, args);
                }
            }
            stream.Close();
            //��ѹ
            if (this.ResponseHeaders[HttpResponseHeader.ContentEncoding] != null)
            {
                MemoryStream msTemp = new MemoryStream();
                count = 0;
                buf = new byte[100];
                switch (this.ResponseHeaders[HttpResponseHeader.ContentEncoding].ToLower())
                {
                    case "gzip":
                        GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress);
                        while ((count = gzip.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }
                        return msTemp.ToArray();

                    case "deflate":
                        DeflateStream deflate = new DeflateStream(ms, CompressionMode.Decompress);
                        while ((count = deflate.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }
                        return msTemp.ToArray();

                    default:
                        break;
                }
            }
            return ms.ToArray();
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="request">�������</param>
        /// <param name="postData">�����͵��ֽ�����</param>
        private void PostData(HttpWebRequest request, byte[] postData)
        {
            int offset = 0;
            int sendBufferSize = this.bufferSize;
            int remainBytes = 0;
            Stream stream = request.GetRequestStream();
            UploadEventArgs args = new UploadEventArgs();
            args.TotalBytes = postData.Length;
            while ((remainBytes = postData.Length - offset) > 0)
            {
                if (sendBufferSize > remainBytes)
                {
                    sendBufferSize = remainBytes;
                }

                stream.Write(postData, offset, sendBufferSize);
                offset += sendBufferSize;
                if (this.UploadProgressChanged != null)
                {
                    args.BytesSent = offset;
                    this.UploadProgressChanged(this, args);
                }
            }
            stream.Close();
        }

        /// <summary>
        /// ����HTTP����
        /// </summary>
        /// <param name="url">URL��ַ</param>
        /// <returns></returns>
        private HttpWebRequest CreateRequest(string url, string method)
        {
            Uri uri = new Uri(url);

            if (uri.Scheme == "https")
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
            }

            // Set a default policy level for the "http:" and "https" schemes.
            HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
            HttpWebRequest.DefaultCachePolicy = policy;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = false;
            request.AllowWriteStreamBuffering = false;
            request.Method = method;
            if (this.proxy != null)
            {
                request.Proxy = this.proxy;
            }

            request.CookieContainer = cc;
            foreach (string key in this.requestHeaders.Keys)
            {
                request.Headers.Add(key, this.requestHeaders[key]);
            }
            this.requestHeaders.Clear();
            return request;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// ��Cookie���浽����
        /// </summary>
        private static void SaveCookiesToDisk()
        {
            string cookieFile = System.Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
            FileStream fs = null;
            try
            {
                fs = new FileStream(cookieFile, FileMode.Create);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formater.Serialize(fs, cc);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// �Ӵ��̼���Cookie
        /// </summary>
        private static void LoadCookiesFromDisk()
        {
            cc = new CookieContainer();
            string cookieFile = System.Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
            if (!File.Exists(cookieFile))
            {
                return;
            }

            FileStream fs = null;
            try
            {
                fs = new FileStream(cookieFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                cc = (CookieContainer)formater.Deserialize(fs);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }

    /// <summary>
    /// ���ļ����ı����ݽ���Multipart��ʽ�ı���
    /// </summary>
    public class MultipartForm
    {
        private Encoding encoding;

        private MemoryStream ms;

        private string boundary;

        private byte[] formData;

        /// <summary>
        /// ��ȡ�������ֽ�����
        /// </summary>
        public byte[] FormData
        {
            get
            {
                if (this.formData == null)
                {
                    byte[] buffer = this.encoding.GetBytes("--" + this.boundary + "--\r\n");
                    this.ms.Write(buffer, 0, buffer.Length);
                    this.formData = this.ms.ToArray();
                }
                return this.formData;
            }
        }

        /// <summary>
        /// ��ȡ�˱������ݵ�����
        /// </summary>
        public string ContentType
        {
            get { return string.Format("multipart/form-data; boundary={0}", this.boundary); }
        }

        /// <summary>
        /// ��ȡ�����ö��ַ������õı�������
        /// </summary>
        public Encoding StringEncoding
        {
            set { this.encoding = value; }
            get { return this.encoding; }
        }

        /// <summary>
        /// ʵ����
        /// </summary>
        public MultipartForm()
        {
            this.boundary = string.Format("--{0}--", Guid.NewGuid());
            this.ms = new MemoryStream();
            this.encoding = Encoding.Default;
        }

        /// <summary>
        /// ���һ���ļ�
        /// </summary>
        /// <param name="name">�ļ�������</param>
        /// <param name="filename">�ļ�������·��</param>
        public void AddFlie(string name, string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("������Ӳ����ڵ��ļ���", filename);
            }

            FileStream fs = null;
            byte[] fileData = { };
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                fileData = new byte[fs.Length];
                fs.Read(fileData, 0, fileData.Length);
                this.AddFlie(name, Path.GetFileName(filename), fileData, fileData.Length);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// ���һ���ļ�
        /// </summary>
        /// <param name="name">�ļ�������</param>
        /// <param name="filename">�ļ���</param>
        /// <param name="fileData">�ļ�����������</param>
        /// <param name="dataLength">���������ݴ�С</param>
        public void AddFlie(string name, string filename, byte[] fileData, int dataLength)
        {
            if (dataLength <= 0 || dataLength > fileData.Length)
            {
                dataLength = fileData.Length;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("--{0}\r\n", this.boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\n", name, filename);
            sb.AppendFormat("Content-Type: {0}\r\n", this.GetContentType(filename));
            sb.Append("\r\n");
            byte[] buf = this.encoding.GetBytes(sb.ToString());
            this.ms.Write(buf, 0, buf.Length);
            this.ms.Write(fileData, 0, dataLength);
            byte[] crlf = this.encoding.GetBytes("\r\n");
            this.ms.Write(crlf, 0, crlf.Length);
        }

        /// <summary>
        /// ����ַ���
        /// </summary>
        /// <param name="name">�ı�������</param>
        /// <param name="value">�ı�ֵ</param>
        public void AddString(string name, string value)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("--{0}\r\n", this.boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n", name);
            sb.Append("\r\n");
            sb.AppendFormat("{0}\r\n", value);
            byte[] buf = this.encoding.GetBytes(sb.ToString());
            this.ms.Write(buf, 0, buf.Length);
        }

        /// <summary>
        /// ��ע����ȡ�ļ�����
        /// </summary>
        /// <param name="filename">������չ�����ļ���</param>
        /// <returns>�磺application/stream</returns>
        private string GetContentType(string filename)
        {
            Microsoft.Win32.RegistryKey fileExtKey = null; ;
            string contentType = "application/stream";
            try
            {
                fileExtKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Path.GetExtension(filename));
                contentType = fileExtKey.GetValue("Content Type", contentType).ToString();
            }
            finally
            {
                if (fileExtKey != null)
                {
                    fileExtKey.Close();
                }
            }
            return contentType;
        }
    }
}
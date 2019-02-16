using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace System.FTP
{
    /// <summary>
    /// FTP 操作类客户端
    /// </summary>
    public class FTPClient
    {
        public static object obj = new object();

        #region 构造函数

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public FTPClient()
        {
            this.strRemoteHost = "";
            this.strRemotePath = "";
            this.strRemoteUser = "";
            this.strRemotePass = "";
            this.strRemotePort = 21;
            this.bConnected = false;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FTPClient(string remoteHost, string remotePath, string remoteUser, string remotePass, int remotePort)
        {
            this.strRemoteHost = remoteHost;
            this.strRemotePath = remotePath;
            this.strRemoteUser = remoteUser;
            this.strRemotePass = remotePass;
            this.strRemotePort = remotePort;
            this.Connect();
        }

        #endregion 构造函数

        #region 字段

        private int strRemotePort;

        private Boolean bConnected;

        private string strRemoteHost;

        private string strRemotePass;

        private string strRemoteUser;

        private string strRemotePath;

        /// <summary>
        /// 服务器返回的应答信息(包含应答码)
        /// </summary>
        private string strMsg;

        /// <summary>
        /// 服务器返回的应答信息(包含应答码)
        /// </summary>
        private string strReply;

        /// <summary>
        /// 服务器返回的应答码
        /// </summary>
        private int iReplyCode;

        /// <summary>
        /// 进行控制连接的socket
        /// </summary>
        private Socket socketControl;

        /// <summary>
        /// 传输模式
        /// </summary>
        private TransferType trType;

        /// <summary>
        /// 接收和发送数据的缓冲区
        /// </summary>
        private static int BLOCK_SIZE = 512;

        /// <summary>
        /// 编码方式
        /// </summary>
        private Encoding ASCII = Encoding.ASCII;

        /// <summary>
        /// 字节数组
        /// </summary>
        private Byte[] buffer = new Byte[BLOCK_SIZE];

        #endregion 字段

        #region 属性

        /// <summary>
        /// FTP服务器IP地址
        /// </summary>
        public string RemoteHost
        {
            get
            {
                return this.strRemoteHost;
            }
            set
            {
                this.strRemoteHost = value;
            }
        }

        /// <summary>
        /// FTP服务器端口
        /// </summary>
        public int RemotePort
        {
            get
            {
                return this.strRemotePort;
            }
            set
            {
                this.strRemotePort = value;
            }
        }

        /// <summary>
        /// 当前服务器目录
        /// </summary>
        public string RemotePath
        {
            get
            {
                return this.strRemotePath;
            }
            set
            {
                this.strRemotePath = value;
            }
        }

        /// <summary>
        /// 登录用户账号
        /// </summary>
        public string RemoteUser
        {
            set
            {
                this.strRemoteUser = value;
            }
        }

        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string RemotePass
        {
            set
            {
                this.strRemotePass = value;
            }
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool Connected
        {
            get
            {
                return this.bConnected;
            }
        }

        #endregion 属性

        #region 链接

        /// <summary>
        /// 建立连接
        /// </summary>
        public void Connect()
        {
            lock (obj)
            {
                this.socketControl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.RemoteHost), this.strRemotePort);
                try
                {
                    this.socketControl.Connect(ep);
                }
                catch (Exception)
                {
                    throw new IOException("不能连接ftp服务器");
                }
            }
            this.ReadReply();
            if (this.iReplyCode != 220)
            {
                this.DisConnect();
                throw new IOException(this.strReply.Substring(4));
            }
            this.SendCommand("USER " + this.strRemoteUser);
            if (!(this.iReplyCode == 331 || this.iReplyCode == 230))
            {
                this.CloseSocketConnect();
                throw new IOException(this.strReply.Substring(4));
            }
            if (this.iReplyCode != 230)
            {
                this.SendCommand("PASS " + this.strRemotePass);
                if (!(this.iReplyCode == 230 || this.iReplyCode == 202))
                {
                    this.CloseSocketConnect();
                    throw new IOException(this.strReply.Substring(4));
                }
            }
            this.bConnected = true;
            this.ChDir(this.strRemotePath);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void DisConnect()
        {
            if (this.socketControl != null)
            {
                this.SendCommand("QUIT");
            }
            this.CloseSocketConnect();
        }

        #endregion 链接

        #region 传输模式

        /// <summary>
        /// 传输模式:二进制类型、ASCII类型
        /// </summary>
        public enum TransferType { Binary, ASCII };

        /// <summary>
        /// 设置传输模式
        /// </summary>
        /// <param name="ttType">传输模式</param>
        public void SetTransferType(TransferType ttType)
        {
            if (ttType == TransferType.Binary)
            {
                this.SendCommand("TYPE I");//binary类型传输
            }
            else
            {
                this.SendCommand("TYPE A");//ASCII类型传输
            }
            if (this.iReplyCode != 200)
            {
                throw new IOException(this.strReply.Substring(4));
            }
            else
            {
                this.trType = ttType;
            }
        }

        /// <summary>
        /// 获得传输模式
        /// </summary>
        /// <returns>传输模式</returns>
        public TransferType GetTransferType()
        {
            return this.trType;
        }

        #endregion 传输模式

        #region 文件操作

        /// <summary>
        /// 获得文件列表
        /// </summary>
        /// <param name="strMask">文件名的匹配字符串</param>
        public string[] Dir(string strMask)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            Socket socketData = this.CreateDataSocket();
            this.SendCommand("NLST " + strMask);
            if (!(this.iReplyCode == 150 || this.iReplyCode == 125 || this.iReplyCode == 226))
            {
                throw new IOException(this.strReply.Substring(4));
            }
            this.strMsg = "";
            Thread.Sleep(2000);
            while (true)
            {
                int iBytes = socketData.Receive(this.buffer, this.buffer.Length, 0);
                this.strMsg += this.ASCII.GetString(this.buffer, 0, iBytes);
                if (iBytes < this.buffer.Length)
                {
                    break;
                }
            }
            char[] seperator = { '\n' };
            string[] strsFileList = this.strMsg.Split(seperator);
            socketData.Close(); //数据socket关闭时也会有返回码
            if (this.iReplyCode != 226)
            {
                this.ReadReply();
                if (this.iReplyCode != 226)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
            }
            return strsFileList;
        }

        public void newPutByGuid(string strFileName, string strGuid)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            string str = strFileName.Substring(0, strFileName.LastIndexOf("\\"));
            string strTypeName = strFileName.Substring(strFileName.LastIndexOf("."));
            strGuid = str + "\\" + strGuid;
            Socket socketData = this.CreateDataSocket();
            this.SendCommand("STOR " + Path.GetFileName(strGuid));
            if (!(this.iReplyCode == 125 || this.iReplyCode == 150))
            {
                throw new IOException(this.strReply.Substring(4));
            }
            FileStream input = new FileStream(strGuid, FileMode.Open);
            input.Flush();
            int iBytes = 0;
            while ((iBytes = input.Read(this.buffer, 0, this.buffer.Length)) > 0)
            {
                socketData.Send(this.buffer, iBytes, 0);
            }
            input.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
            {
                this.ReadReply();
                if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
                {
                    throw new IOException(this.strReply.Substring(4));
                }
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <returns>文件大小</returns>
        public long GetFileSize(string strFileName)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            this.SendCommand("SIZE " + Path.GetFileName(strFileName));
            long lSize = 0;
            if (this.iReplyCode == 213)
            {
                lSize = Int64.Parse(this.strReply.Substring(4));
            }
            else
            {
                throw new IOException(this.strReply.Substring(4));
            }
            return lSize;
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <returns>文件大小</returns>
        public string GetFileInfo(string strFileName)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            Socket socketData = this.CreateDataSocket();
            this.SendCommand("LIST " + strFileName);
            string strResult = "";
            if (!(this.iReplyCode == 150 || this.iReplyCode == 125
                || this.iReplyCode == 226 || this.iReplyCode == 250))
            {
                throw new IOException(this.strReply.Substring(4));
            }
            byte[] b = new byte[512];
            MemoryStream ms = new MemoryStream();

            while (true)
            {
                int iBytes = socketData.Receive(b, b.Length, 0);
                ms.Write(b, 0, iBytes);
                if (iBytes <= 0)
                {
                    break;
                }
            }
            byte[] bt = ms.GetBuffer();
            strResult = System.Text.Encoding.ASCII.GetString(bt);
            ms.Close();
            return strResult;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strFileName">待删除文件名</param>
        public void Delete(string strFileName)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            this.SendCommand("DELE " + strFileName);
            if (this.iReplyCode != 250)
            {
                throw new IOException(this.strReply.Substring(4));
            }
        }

        /// <summary>
        /// 重命名(如果新文件名与已有文件重名,将覆盖已有文件)
        /// </summary>
        /// <param name="strOldFileName">旧文件名</param>
        /// <param name="strNewFileName">新文件名</param>
        public void Rename(string strOldFileName, string strNewFileName)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            this.SendCommand("RNFR " + strOldFileName);
            if (this.iReplyCode != 350)
            {
                throw new IOException(this.strReply.Substring(4));
            }
            //  如果新文件名与原有文件重名,将覆盖原有文件
            this.SendCommand("RNTO " + strNewFileName);
            if (this.iReplyCode != 250)
            {
                throw new IOException(this.strReply.Substring(4));
            }
        }

        #endregion 文件操作

        #region 上传和下载

        /// <summary>
        /// 下载一批文件
        /// </summary>
        /// <param name="strFileNameMask">文件名的匹配字符串</param>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        public void Get(string strFileNameMask, string strFolder)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            string[] strFiles = this.Dir(strFileNameMask);
            foreach (string strFile in strFiles)
            {
                if (!strFile.Equals(""))//一般来说strFiles的最后一个元素可能是空字符串
                {
                    this.Get(strFile, strFolder, strFile);
                }
            }
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="strRemoteFileName">要下载的文件名</param>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        /// <param name="strLocalFileName">保存在本地时的文件名</param>
        public void Get(string strRemoteFileName, string strFolder, string strLocalFileName)
        {
            Socket socketData = this.CreateDataSocket();
            try
            {
                if (!this.bConnected)
                {
                    this.Connect();
                }
                this.SetTransferType(TransferType.Binary);
                if (strLocalFileName.Equals(""))
                {
                    strLocalFileName = strRemoteFileName;
                }
                this.SendCommand("RETR " + strRemoteFileName);
                if (!(this.iReplyCode == 150 || this.iReplyCode == 125 || this.iReplyCode == 226 || this.iReplyCode == 250))
                {
                    throw new IOException(this.strReply.Substring(4));
                }
                FileStream output = new FileStream(strFolder + "\\" + strLocalFileName, FileMode.Create);
                while (true)
                {
                    int iBytes = socketData.Receive(this.buffer, this.buffer.Length, 0);
                    output.Write(this.buffer, 0, iBytes);
                    if (iBytes <= 0)
                    {
                        break;
                    }
                }
                output.Close();
                if (socketData.Connected)
                {
                    socketData.Close();
                }
                if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
                {
                    this.ReadReply();
                    if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
                    {
                        throw new IOException(this.strReply.Substring(4));
                    }
                }
            }
            catch
            {
                socketData.Close();
                socketData = null;
                this.socketControl.Close();
                this.bConnected = false;
                this.socketControl = null;
            }
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="strRemoteFileName">要下载的文件名</param>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        /// <param name="strLocalFileName">保存在本地时的文件名</param>
        public void GetNoBinary(string strRemoteFileName, string strFolder, string strLocalFileName)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }

            if (strLocalFileName.Equals(""))
            {
                strLocalFileName = strRemoteFileName;
            }
            Socket socketData = this.CreateDataSocket();
            this.SendCommand("RETR " + strRemoteFileName);
            if (!(this.iReplyCode == 150 || this.iReplyCode == 125 || this.iReplyCode == 226 || this.iReplyCode == 250))
            {
                throw new IOException(this.strReply.Substring(4));
            }
            FileStream output = new FileStream(strFolder + "\\" + strLocalFileName, FileMode.Create);
            while (true)
            {
                int iBytes = socketData.Receive(this.buffer, this.buffer.Length, 0);
                output.Write(this.buffer, 0, iBytes);
                if (iBytes <= 0)
                {
                    break;
                }
            }
            output.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
            {
                this.ReadReply();
                if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
                {
                    throw new IOException(this.strReply.Substring(4));
                }
            }
        }

        /// <summary>
        /// 上传一批文件
        /// </summary>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        /// <param name="strFileNameMask">文件名匹配字符(可以包含*和?)</param>
        public void Put(string strFolder, string strFileNameMask)
        {
            string[] strFiles = Directory.GetFiles(strFolder, strFileNameMask);
            foreach (string strFile in strFiles)
            {
                this.Put(strFile);
            }
        }

        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="strFileName">本地文件名</param>
        public void Put(string strFileName)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            Socket socketData = this.CreateDataSocket();
            if (Path.GetExtension(strFileName) == "")
            {
                this.SendCommand("STOR " + Path.GetFileNameWithoutExtension(strFileName));
            }
            else
            {
                this.SendCommand("STOR " + Path.GetFileName(strFileName));
            }

            if (!(this.iReplyCode == 125 || this.iReplyCode == 150))
            {
                throw new IOException(this.strReply.Substring(4));
            }

            FileStream input = new FileStream(strFileName, FileMode.Open);
            int iBytes = 0;
            while ((iBytes = input.Read(this.buffer, 0, this.buffer.Length)) > 0)
            {
                socketData.Send(this.buffer, iBytes, 0);
            }
            input.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
            {
                this.ReadReply();
                if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
                {
                    throw new IOException(this.strReply.Substring(4));
                }
            }
        }

        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="strFileName">本地文件名</param>
        public void PutByGuid(string strFileName, string strGuid)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            string str = strFileName.Substring(0, strFileName.LastIndexOf("\\"));
            string strTypeName = strFileName.Substring(strFileName.LastIndexOf("."));
            strGuid = str + "\\" + strGuid;
            System.IO.File.Copy(strFileName, strGuid);
            System.IO.File.SetAttributes(strGuid, System.IO.FileAttributes.Normal);
            Socket socketData = this.CreateDataSocket();
            this.SendCommand("STOR " + Path.GetFileName(strGuid));
            if (!(this.iReplyCode == 125 || this.iReplyCode == 150))
            {
                throw new IOException(this.strReply.Substring(4));
            }
            FileStream input = new FileStream(strGuid, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            int iBytes = 0;
            while ((iBytes = input.Read(this.buffer, 0, this.buffer.Length)) > 0)
            {
                socketData.Send(this.buffer, iBytes, 0);
            }
            input.Close();
            File.Delete(strGuid);
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
            {
                this.ReadReply();
                if (!(this.iReplyCode == 226 || this.iReplyCode == 250))
                {
                    throw new IOException(this.strReply.Substring(4));
                }
            }
        }

        #endregion 上传和下载

        #region 目录操作

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="strDirName">目录名</param>
        public void MkDir(string strDirName)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            this.SendCommand("MKD " + strDirName);
            if (this.iReplyCode != 257)
            {
                throw new IOException(this.strReply.Substring(4));
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="strDirName">目录名</param>
        public void RmDir(string strDirName)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            this.SendCommand("RMD " + strDirName);
            if (this.iReplyCode != 250)
            {
                throw new IOException(this.strReply.Substring(4));
            }
        }

        /// <summary>
        /// 改变目录
        /// </summary>
        /// <param name="strDirName">新的工作目录名</param>
        public void ChDir(string strDirName)
        {
            if (strDirName.Equals(".") || strDirName.Equals(""))
            {
                return;
            }
            if (!this.bConnected)
            {
                this.Connect();
            }
            this.SendCommand("CWD " + strDirName);
            if (this.iReplyCode != 250)
            {
                throw new IOException(this.strReply.Substring(4));
            }
            this.strRemotePath = strDirName;
        }

        #endregion 目录操作

        #region 内部函数

        /// <summary>
        /// 将一行应答字符串记录在strReply和strMsg,应答码记录在iReplyCode
        /// </summary>
        private void ReadReply()
        {
            this.strMsg = "";
            this.strReply = this.ReadLine();
            this.iReplyCode = Int32.Parse(this.strReply.Substring(0, 3));
        }

        /// <summary>
        /// 建立进行数据连接的socket
        /// </summary>
        /// <returns>数据连接socket</returns>
        private Socket CreateDataSocket()
        {
            this.SendCommand("PASV");
            if (this.iReplyCode != 227)
            {
                throw new IOException(this.strReply.Substring(4));
            }
            int index1 = this.strReply.IndexOf('(');
            int index2 = this.strReply.IndexOf(')');
            string ipData = this.strReply.Substring(index1 + 1, index2 - index1 - 1);
            int[] parts = new int[6];
            int len = ipData.Length;
            int partCount = 0;
            string buf = "";
            for (int i = 0; i < len && partCount <= 6; i++)
            {
                char ch = Char.Parse(ipData.Substring(i, 1));
                if (Char.IsDigit(ch))
                {
                    buf += ch;
                }
                else if (ch != ',')
                {
                    throw new IOException("Malformed PASV strReply: " + this.strReply);
                }
                if (ch == ',' || i + 1 == len)
                {
                    try
                    {
                        parts[partCount++] = Int32.Parse(buf);
                        buf = "";
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV strReply: " + this.strReply);
                    }
                }
            }
            string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];
            int port = (parts[4] << 8) + parts[5];
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            try
            {
                s.Connect(ep);
            }
            catch (Exception)
            {
                throw new IOException("无法连接ftp服务器");
            }
            return s;
        }

        /// <summary>
        /// 关闭socket连接(用于登录以前)
        /// </summary>
        private void CloseSocketConnect()
        {
            lock (obj)
            {
                if (this.socketControl != null)
                {
                    this.socketControl.Close();
                    this.socketControl = null;
                }
                this.bConnected = false;
            }
        }

        /// <summary>
        /// 读取Socket返回的所有字符串
        /// </summary>
        /// <returns>包含应答码的字符串行</returns>
        private string ReadLine()
        {
            lock (obj)
            {
                while (true)
                {
                    int iBytes = this.socketControl.Receive(this.buffer, this.buffer.Length, 0);
                    this.strMsg += this.ASCII.GetString(this.buffer, 0, iBytes);
                    if (iBytes < this.buffer.Length)
                    {
                        break;
                    }
                }
            }
            char[] seperator = { '\n' };
            string[] mess = this.strMsg.Split(seperator);
            if (this.strMsg.Length > 2)
            {
                this.strMsg = mess[mess.Length - 2];
            }
            else
            {
                this.strMsg = mess[0];
            }
            if (!this.strMsg.Substring(3, 1).Equals(" ")) //返回字符串正确的是以应答码(如220开头,后面接一空格,再接问候字符串)
            {
                return this.ReadLine();
            }
            return this.strMsg;
        }

        /// <summary>
        /// 发送命令并获取应答码和最后一行应答字符串
        /// </summary>
        /// <param name="strCommand">命令</param>
        public void SendCommand(String strCommand)
        {
            lock (obj)
            {
                Byte[] cmdBytes = Encoding.ASCII.GetBytes((strCommand + "\r\n").ToCharArray());
                this.socketControl.Send(cmdBytes, cmdBytes.Length, 0);
                Thread.Sleep(500);
                this.ReadReply();
            }
        }

        #endregion 内部函数
    }
}
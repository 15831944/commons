using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace System.FTP
{
    /// <summary>
    /// FTP ������ͻ���
    /// </summary>
    public class FTPClient
    {
        public static object obj = new object();

        #region ���캯��

        /// <summary>
        /// ȱʡ���캯��
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
        /// ���캯��
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

        #endregion ���캯��

        #region �ֶ�

        private int strRemotePort;

        private Boolean bConnected;

        private string strRemoteHost;

        private string strRemotePass;

        private string strRemoteUser;

        private string strRemotePath;

        /// <summary>
        /// ���������ص�Ӧ����Ϣ(����Ӧ����)
        /// </summary>
        private string strMsg;

        /// <summary>
        /// ���������ص�Ӧ����Ϣ(����Ӧ����)
        /// </summary>
        private string strReply;

        /// <summary>
        /// ���������ص�Ӧ����
        /// </summary>
        private int iReplyCode;

        /// <summary>
        /// ���п������ӵ�socket
        /// </summary>
        private Socket socketControl;

        /// <summary>
        /// ����ģʽ
        /// </summary>
        private TransferType trType;

        /// <summary>
        /// ���պͷ������ݵĻ�����
        /// </summary>
        private static int BLOCK_SIZE = 512;

        /// <summary>
        /// ���뷽ʽ
        /// </summary>
        private Encoding ASCII = Encoding.ASCII;

        /// <summary>
        /// �ֽ�����
        /// </summary>
        private Byte[] buffer = new Byte[BLOCK_SIZE];

        #endregion �ֶ�

        #region ����

        /// <summary>
        /// FTP������IP��ַ
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
        /// FTP�������˿�
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
        /// ��ǰ������Ŀ¼
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
        /// ��¼�û��˺�
        /// </summary>
        public string RemoteUser
        {
            set
            {
                this.strRemoteUser = value;
            }
        }

        /// <summary>
        /// �û���¼����
        /// </summary>
        public string RemotePass
        {
            set
            {
                this.strRemotePass = value;
            }
        }

        /// <summary>
        /// �Ƿ��¼
        /// </summary>
        public bool Connected
        {
            get
            {
                return this.bConnected;
            }
        }

        #endregion ����

        #region ����

        /// <summary>
        /// ��������
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
                    throw new IOException("��������ftp������");
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
        /// �ر�����
        /// </summary>
        public void DisConnect()
        {
            if (this.socketControl != null)
            {
                this.SendCommand("QUIT");
            }
            this.CloseSocketConnect();
        }

        #endregion ����

        #region ����ģʽ

        /// <summary>
        /// ����ģʽ:���������͡�ASCII����
        /// </summary>
        public enum TransferType { Binary, ASCII };

        /// <summary>
        /// ���ô���ģʽ
        /// </summary>
        /// <param name="ttType">����ģʽ</param>
        public void SetTransferType(TransferType ttType)
        {
            if (ttType == TransferType.Binary)
            {
                this.SendCommand("TYPE I");//binary���ʹ���
            }
            else
            {
                this.SendCommand("TYPE A");//ASCII���ʹ���
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
        /// ��ô���ģʽ
        /// </summary>
        /// <returns>����ģʽ</returns>
        public TransferType GetTransferType()
        {
            return this.trType;
        }

        #endregion ����ģʽ

        #region �ļ�����

        /// <summary>
        /// ����ļ��б�
        /// </summary>
        /// <param name="strMask">�ļ�����ƥ���ַ���</param>
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
            socketData.Close(); //����socket�ر�ʱҲ���з�����
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
        /// ��ȡ�ļ���С
        /// </summary>
        /// <param name="strFileName">�ļ���</param>
        /// <returns>�ļ���С</returns>
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
        /// ��ȡ�ļ���Ϣ
        /// </summary>
        /// <param name="strFileName">�ļ���</param>
        /// <returns>�ļ���С</returns>
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
        /// ɾ��
        /// </summary>
        /// <param name="strFileName">��ɾ���ļ���</param>
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
        /// ������(������ļ����������ļ�����,�����������ļ�)
        /// </summary>
        /// <param name="strOldFileName">���ļ���</param>
        /// <param name="strNewFileName">���ļ���</param>
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
            //  ������ļ�����ԭ���ļ�����,������ԭ���ļ�
            this.SendCommand("RNTO " + strNewFileName);
            if (this.iReplyCode != 250)
            {
                throw new IOException(this.strReply.Substring(4));
            }
        }

        #endregion �ļ�����

        #region �ϴ�������

        /// <summary>
        /// ����һ���ļ�
        /// </summary>
        /// <param name="strFileNameMask">�ļ�����ƥ���ַ���</param>
        /// <param name="strFolder">����Ŀ¼(������\����)</param>
        public void Get(string strFileNameMask, string strFolder)
        {
            if (!this.bConnected)
            {
                this.Connect();
            }
            string[] strFiles = this.Dir(strFileNameMask);
            foreach (string strFile in strFiles)
            {
                if (!strFile.Equals(""))//һ����˵strFiles�����һ��Ԫ�ؿ����ǿ��ַ���
                {
                    this.Get(strFile, strFolder, strFile);
                }
            }
        }

        /// <summary>
        /// ����һ���ļ�
        /// </summary>
        /// <param name="strRemoteFileName">Ҫ���ص��ļ���</param>
        /// <param name="strFolder">����Ŀ¼(������\����)</param>
        /// <param name="strLocalFileName">�����ڱ���ʱ���ļ���</param>
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
        /// ����һ���ļ�
        /// </summary>
        /// <param name="strRemoteFileName">Ҫ���ص��ļ���</param>
        /// <param name="strFolder">����Ŀ¼(������\����)</param>
        /// <param name="strLocalFileName">�����ڱ���ʱ���ļ���</param>
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
        /// �ϴ�һ���ļ�
        /// </summary>
        /// <param name="strFolder">����Ŀ¼(������\����)</param>
        /// <param name="strFileNameMask">�ļ���ƥ���ַ�(���԰���*��?)</param>
        public void Put(string strFolder, string strFileNameMask)
        {
            string[] strFiles = Directory.GetFiles(strFolder, strFileNameMask);
            foreach (string strFile in strFiles)
            {
                this.Put(strFile);
            }
        }

        /// <summary>
        /// �ϴ�һ���ļ�
        /// </summary>
        /// <param name="strFileName">�����ļ���</param>
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
        /// �ϴ�һ���ļ�
        /// </summary>
        /// <param name="strFileName">�����ļ���</param>
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

        #endregion �ϴ�������

        #region Ŀ¼����

        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        /// <param name="strDirName">Ŀ¼��</param>
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
        /// ɾ��Ŀ¼
        /// </summary>
        /// <param name="strDirName">Ŀ¼��</param>
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
        /// �ı�Ŀ¼
        /// </summary>
        /// <param name="strDirName">�µĹ���Ŀ¼��</param>
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

        #endregion Ŀ¼����

        #region �ڲ�����

        /// <summary>
        /// ��һ��Ӧ���ַ�����¼��strReply��strMsg,Ӧ�����¼��iReplyCode
        /// </summary>
        private void ReadReply()
        {
            this.strMsg = "";
            this.strReply = this.ReadLine();
            this.iReplyCode = Int32.Parse(this.strReply.Substring(0, 3));
        }

        /// <summary>
        /// ���������������ӵ�socket
        /// </summary>
        /// <returns>��������socket</returns>
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
                throw new IOException("�޷�����ftp������");
            }
            return s;
        }

        /// <summary>
        /// �ر�socket����(���ڵ�¼��ǰ)
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
        /// ��ȡSocket���ص������ַ���
        /// </summary>
        /// <returns>����Ӧ������ַ�����</returns>
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
            if (!this.strMsg.Substring(3, 1).Equals(" ")) //�����ַ�����ȷ������Ӧ����(��220��ͷ,�����һ�ո�,�ٽ��ʺ��ַ���)
            {
                return this.ReadLine();
            }
            return this.strMsg;
        }

        /// <summary>
        /// ���������ȡӦ��������һ��Ӧ���ַ���
        /// </summary>
        /// <param name="strCommand">����</param>
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

        #endregion �ڲ�����
    }
}
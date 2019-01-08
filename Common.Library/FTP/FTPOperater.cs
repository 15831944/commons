using System.IO;
using System.Text;

namespace System.FTP
{
    /// <summary>
    /// FTP������
    /// </summary>
    public class FTPOperater
    {
        #region ����

        private FTPClient ftp;

        /// <summary>
        /// ȫ��FTP���ʱ���
        /// </summary>
        public FTPClient Ftp
        {
            get { return this.ftp; }
            set { this.ftp = value; }
        }

        private string _server;

        /// <summary>
        /// Ftp������
        /// </summary>
        public string Server
        {
            get { return this._server; }
            set { this._server = value; }
        }

        private string _User;

        /// <summary>
        /// Ftp�û�
        /// </summary>
        public string User
        {
            get { return this._User; }
            set { this._User = value; }
        }

        private string _Pass;

        /// <summary>
        /// Ftp����
        /// </summary>
        public string Pass
        {
            get { return this._Pass; }
            set { this._Pass = value; }
        }

        private string _FolderZJ;

        /// <summary>
        /// Ftp����
        /// </summary>
        public string FolderZJ
        {
            get { return this._FolderZJ; }
            set { this._FolderZJ = value; }
        }

        private string _FolderWX;

        /// <summary>
        /// Ftp����
        /// </summary>
        public string FolderWX
        {
            get { return this._FolderWX; }
            set { this._FolderWX = value; }
        }

        #endregion ����

        /// <summary>
        /// �õ��ļ��б�
        /// </summary>
        /// <returns></returns>
        public string[] GetList(string strPath)
        {
            if (this.ftp == null)
            {
                this.ftp = this.getFtpClient();
            }

            this.ftp.Connect();
            this.ftp.ChDir(strPath);
            return this.ftp.Dir("*");
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="ftpFolder">ftpĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
        /// <param name="localFolder">����Ŀ¼</param>
        /// <param name="localFileName">�����ļ���</param>
        public bool GetFile(string ftpFolder, string ftpFileName, string localFolder, string localFileName)
        {
            try
            {
                if (this.ftp == null)
                {
                    this.ftp = this.getFtpClient();
                }

                if (!this.ftp.Connected)
                {
                    this.ftp.Connect();
                    this.ftp.ChDir(ftpFolder);
                }
                this.ftp.Get(ftpFileName, localFolder, localFileName);

                return true;
            }
            catch
            {
                try
                {
                    this.ftp.DisConnect();
                    this.ftp = null;
                }
                catch { this.ftp = null; }
                return false;
            }
        }

        /// <summary>
        /// �޸��ļ�
        /// </summary>
        /// <param name="ftpFolder">����Ŀ¼</param>
        /// <param name="ftpFileName">�����ļ���temp</param>
        /// <param name="localFolder">����Ŀ¼</param>
        /// <param name="localFileName">�����ļ���</param>
        public bool AddMSCFile(string ftpFolder, string ftpFileName, string localFolder, string localFileName, string BscInfo)
        {
            string sLine = "";
            string sResult = "";
            string path = "���Ӧ�ó������ڵ�������·��";
            path = path.Substring(0, path.LastIndexOf("\\"));
            try
            {
                FileStream fsFile = new FileStream(ftpFolder + "\\" + ftpFileName, FileMode.Open);
                FileStream fsFileWrite = new FileStream(localFolder + "\\" + localFileName, FileMode.Create);
                StreamReader sr = new StreamReader(fsFile);
                StreamWriter sw = new StreamWriter(fsFileWrite);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                while (sr.Peek() > -1)
                {
                    sLine = sr.ReadToEnd();
                }
                string[] arStr = sLine.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < arStr.Length - 1; i++)
                {
                    sResult += BscInfo + "," + arStr[i].Trim() + "\n";
                }
                sr.Close();
                byte[] connect = new UTF8Encoding(true).GetBytes(sResult);
                fsFileWrite.Write(connect, 0, connect.Length);
                fsFileWrite.Flush();
                sw.Close();
                fsFile.Close();
                fsFileWrite.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="ftpFolder">ftpĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
        public bool DelFile(string ftpFolder, string ftpFileName)
        {
            try
            {
                if (this.ftp == null)
                {
                    this.ftp = this.getFtpClient();
                }

                if (!this.ftp.Connected)
                {
                    this.ftp.Connect();
                    this.ftp.ChDir(ftpFolder);
                }
                this.ftp.Delete(ftpFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        /// <param name="ftpFolder">ftpĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
        public bool PutFile(string ftpFolder, string ftpFileName)
        {
            try
            {
                if (this.ftp == null)
                {
                    this.ftp = this.getFtpClient();
                }

                if (!this.ftp.Connected)
                {
                    this.ftp.Connect();
                    this.ftp.ChDir(ftpFolder);
                }
                this.ftp.Put(ftpFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="ftpFolder">ftpĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
        /// <param name="localFolder">����Ŀ¼</param>
        /// <param name="localFileName">�����ļ���</param>
        public bool GetFileNoBinary(string ftpFolder, string ftpFileName, string localFolder, string localFileName)
        {
            try
            {
                if (this.ftp == null)
                {
                    this.ftp = this.getFtpClient();
                }

                if (!this.ftp.Connected)
                {
                    this.ftp.Connect();
                    this.ftp.ChDir(ftpFolder);
                }
                this.ftp.GetNoBinary(ftpFileName, localFolder, localFileName);
                return true;
            }
            catch
            {
                try
                {
                    this.ftp.DisConnect();
                    this.ftp = null;
                }
                catch
                {
                    this.ftp = null;
                }
                return false;
            }
        }

        /// <summary>
        /// �õ�FTP���ļ���Ϣ
        /// </summary>
        /// <param name="ftpFolder">FTPĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
        public string GetFileInfo(string ftpFolder, string ftpFileName)
        {
            string strResult = "";
            try
            {
                if (this.ftp == null)
                {
                    this.ftp = this.getFtpClient();
                }

                if (this.ftp.Connected)
                {
                    this.ftp.DisConnect();
                }

                this.ftp.Connect();
                this.ftp.ChDir(ftpFolder);
                strResult = this.ftp.GetFileInfo(ftpFileName);
                return strResult;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// ����FTP�������Ƿ�ɵ�½
        /// </summary>
        public bool CanConnect()
        {
            if (this.ftp == null)
            {
                this.ftp = this.getFtpClient();
            }

            try
            {
                this.ftp.Connect();
                this.ftp.DisConnect();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// �õ�FTP���ļ���Ϣ
        /// </summary>
        /// <param name="ftpFolder">FTPĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
        public string GetFileInfoConnected(string ftpFolder, string ftpFileName)
        {
            string strResult = "";
            try
            {
                if (this.ftp == null)
                {
                    this.ftp = this.getFtpClient();
                }

                if (!this.ftp.Connected)
                {
                    this.ftp.Connect();
                    this.ftp.ChDir(ftpFolder);
                }
                strResult = this.ftp.GetFileInfo(ftpFileName);
                return strResult;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// �õ��ļ��б�
        /// </summary>
        /// <param name="ftpFolder">FTPĿ¼</param>
        /// <returns>FTPͨ�����</returns>
        public string[] GetFileList(string ftpFolder, string strMask)
        {
            string[] strResult;
            try
            {
                if (this.ftp == null)
                {
                    this.ftp = this.getFtpClient();
                }

                if (!this.ftp.Connected)
                {
                    this.ftp.Connect();
                    this.ftp.ChDir(ftpFolder);
                }
                strResult = this.ftp.Dir(strMask);
                return strResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///�õ�FTP�������
        /// </summary>
        public FTPClient getFtpClient()
        {
            FTPClient ft = new FTPClient();
            ft.RemoteHost = this.Server;
            ft.RemoteUser = this.User;
            ft.RemotePass = this.Pass;
            return ft;
        }
    }
}
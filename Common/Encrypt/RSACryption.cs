namespace System.Encrypt
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// RSA���ܽ��ܼ�RSAǩ������֤
    /// </summary>
    public class RSACryption
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RSACryption"/> class.
        /// </summary>
        public RSACryption()
        {
        }

        #endregion Constructors

        #region Methods

        //��ȡHash������
        /// <summary>
        /// The GetHash
        /// </summary>
        /// <param name="m_strSource">The <see cref="string"/></param>
        /// <param name="HashData">The <see cref="byte[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool GetHash(string m_strSource, ref byte[] HashData)
        {
            //���ַ�����ȡ��Hash����
            byte[] Buffer;
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            Buffer = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            HashData = MD5.ComputeHash(Buffer);

            return true;
        }

        //��ȡHash������
        /// <summary>
        /// The GetHash
        /// </summary>
        /// <param name="m_strSource">The <see cref="string"/></param>
        /// <param name="strHashData">The <see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool GetHash(string m_strSource, ref string strHashData)
        {
            //���ַ�����ȡ��Hash����
            byte[] Buffer;
            byte[] HashData;
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            Buffer = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            HashData = MD5.ComputeHash(Buffer);

            strHashData = Convert.ToBase64String(HashData);
            return true;
        }

        //��ȡHash������
        /// <summary>
        /// The GetHash
        /// </summary>
        /// <param name="objFile">The <see cref="System.IO.FileStream"/></param>
        /// <param name="HashData">The <see cref="byte[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool GetHash(System.IO.FileStream objFile, ref byte[] HashData)
        {
            //���ļ���ȡ��Hash����
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            HashData = MD5.ComputeHash(objFile);
            objFile.Close();

            return true;
        }

        //��ȡHash������
        /// <summary>
        /// The GetHash
        /// </summary>
        /// <param name="objFile">The <see cref="System.IO.FileStream"/></param>
        /// <param name="strHashData">The <see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool GetHash(System.IO.FileStream objFile, ref string strHashData)
        {
            //���ļ���ȡ��Hash����
            byte[] HashData;
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            HashData = MD5.ComputeHash(objFile);
            objFile.Close();

            strHashData = Convert.ToBase64String(HashData);

            return true;
        }

        //RSA�Ľ��ܺ���  byte
        /// <summary>
        /// The RSADecrypt
        /// </summary>
        /// <param name="xmlPrivateKey">The <see cref="string"/></param>
        /// <param name="DecryptString">The <see cref="byte[]"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
        {
            byte[] DypherTextBArray;
            string Result;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            DypherTextBArray = rsa.Decrypt(DecryptString, false);
            Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
            return Result;
        }

        //RSA�Ľ��ܺ���  string
        /// <summary>
        /// The RSADecrypt
        /// </summary>
        /// <param name="xmlPrivateKey">The <see cref="string"/></param>
        /// <param name="m_strDecryptString">The <see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
        {
            byte[] PlainTextBArray;
            byte[] DypherTextBArray;
            string Result;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            PlainTextBArray = Convert.FromBase64String(m_strDecryptString);
            DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
            Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
            return Result;
        }

        //RSA�ļ��ܺ��� byte[]
        /// <summary>
        /// The RSAEncrypt
        /// </summary>
        /// <param name="xmlPublicKey">The <see cref="string"/></param>
        /// <param name="EncryptString">The <see cref="byte[]"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {
            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            CypherTextBArray = rsa.Encrypt(EncryptString, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;
        }

        //##############################################################################
        //RSA ��ʽ����
        //˵��KEY������XML����ʽ,���ص����ַ���
        //����һ����Ҫ˵�������ü��ܷ�ʽ�� ���� ���Ƶģ���
        //##############################################################################

        //RSA�ļ��ܺ���  string
        /// <summary>
        /// The RSAEncrypt
        /// </summary>
        /// <param name="xmlPublicKey">The <see cref="string"/></param>
        /// <param name="m_strEncryptString">The <see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
        {
            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            PlainTextBArray = (new UnicodeEncoding()).GetBytes(m_strEncryptString);
            CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;
        }

        /// <summary>
        /// RSA ����Կ���� ����˽Կ �͹�Կ
        /// </summary>
        /// <param name="xmlKeys"></param>
        /// <param name="xmlPublicKey"></param>
        public void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlKeys = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }

        /// <summary>
        /// The SignatureDeformatter
        /// </summary>
        /// <param name="p_strKeyPublic">The <see cref="string"/></param>
        /// <param name="HashbyteDeformatter">The <see cref="byte[]"/></param>
        /// <param name="DeformatterData">The <see cref="byte[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //ָ�����ܵ�ʱ��HASH�㷨ΪMD5
            RSADeformatter.SetHashAlgorithm("MD5");

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The SignatureDeformatter
        /// </summary>
        /// <param name="p_strKeyPublic">The <see cref="string"/></param>
        /// <param name="HashbyteDeformatter">The <see cref="byte[]"/></param>
        /// <param name="p_strDeformatterData">The <see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
        {
            byte[] DeformatterData;

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //ָ�����ܵ�ʱ��HASH�㷨ΪMD5
            RSADeformatter.SetHashAlgorithm("MD5");

            DeformatterData = Convert.FromBase64String(p_strDeformatterData);

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The SignatureDeformatter
        /// </summary>
        /// <param name="p_strKeyPublic">The <see cref="string"/></param>
        /// <param name="p_strHashbyteDeformatter">The <see cref="string"/></param>
        /// <param name="DeformatterData">The <see cref="byte[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
        {
            byte[] HashbyteDeformatter;

            HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //ָ�����ܵ�ʱ��HASH�㷨ΪMD5
            RSADeformatter.SetHashAlgorithm("MD5");

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The SignatureDeformatter
        /// </summary>
        /// <param name="p_strKeyPublic">The <see cref="string"/></param>
        /// <param name="p_strHashbyteDeformatter">The <see cref="string"/></param>
        /// <param name="p_strDeformatterData">The <see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
        {
            byte[] DeformatterData;
            byte[] HashbyteDeformatter;

            HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //ָ�����ܵ�ʱ��HASH�㷨ΪMD5
            RSADeformatter.SetHashAlgorithm("MD5");

            DeformatterData = Convert.FromBase64String(p_strDeformatterData);

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //RSAǩ��
        /// <summary>
        /// The SignatureFormatter
        /// </summary>
        /// <param name="p_strKeyPrivate">The <see cref="string"/></param>
        /// <param name="HashbyteSignature">The <see cref="byte[]"/></param>
        /// <param name="EncryptedSignatureData">The <see cref="byte[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //����ǩ�����㷨ΪMD5
            RSAFormatter.SetHashAlgorithm("MD5");
            //ִ��ǩ��
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            return true;
        }

        //RSAǩ��
        /// <summary>
        /// The SignatureFormatter
        /// </summary>
        /// <param name="p_strKeyPrivate">The <see cref="string"/></param>
        /// <param name="HashbyteSignature">The <see cref="byte[]"/></param>
        /// <param name="m_strEncryptedSignatureData">The <see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref string m_strEncryptedSignatureData)
        {
            byte[] EncryptedSignatureData;

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //����ǩ�����㷨ΪMD5
            RSAFormatter.SetHashAlgorithm("MD5");
            //ִ��ǩ��
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

            return true;
        }

        //RSAǩ��
        /// <summary>
        /// The SignatureFormatter
        /// </summary>
        /// <param name="p_strKeyPrivate">The <see cref="string"/></param>
        /// <param name="m_strHashbyteSignature">The <see cref="string"/></param>
        /// <param name="EncryptedSignatureData">The <see cref="byte[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            byte[] HashbyteSignature;

            HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //����ǩ�����㷨ΪMD5
            RSAFormatter.SetHashAlgorithm("MD5");
            //ִ��ǩ��
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            return true;
        }

        //RSAǩ��
        /// <summary>
        /// The SignatureFormatter
        /// </summary>
        /// <param name="p_strKeyPrivate">The <see cref="string"/></param>
        /// <param name="m_strHashbyteSignature">The <see cref="string"/></param>
        /// <param name="m_strEncryptedSignatureData">The <see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref string m_strEncryptedSignatureData)
        {
            byte[] HashbyteSignature;
            byte[] EncryptedSignatureData;

            HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //����ǩ�����㷨ΪMD5
            RSAFormatter.SetHashAlgorithm("MD5");
            //ִ��ǩ��
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

            return true;
        }

        #endregion Methods
    }
}
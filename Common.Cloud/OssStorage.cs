using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Common.Authentication;
using Aliyun.OSS.Util;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Cloud
{
    public class OssStorage : IStorage
    {
        private const string accessKeyId = " ";
        private const string accessKeySecret = " ";
        public static string endpoint = "oss-cn-qingdao.aliyuncs.com";
        public static string endpointInternal = "oss-cn-qingdao-internal.aliyuncs.com";
        public static string bucketName = "think-cultural";

        #region 构造函数

        private Aliyun.OSS.OssClient client;

        public OssStorage()
        {
            this.client = new Aliyun.OSS.OssClient(endpoint, accessKeyId, accessKeySecret);
        }

        public OssStorage(ClientConfiguration configuration)
        {
            this.client = new Aliyun.OSS.OssClient(endpoint, accessKeyId, accessKeySecret, configuration);
        }

        public OssStorage(ICredentialsProvider credsProvider)
        {
            this.client = new Aliyun.OSS.OssClient(endpoint, credsProvider);
        }

        public OssStorage(ICredentialsProvider credsProvider, ClientConfiguration configuration)
        {
            this.client = new Aliyun.OSS.OssClient(endpoint, credsProvider, configuration);
        }

        #endregion 构造函数

        #region 上传

        public string UploadFile(string key, string file)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key", "is null");
            }

            if (Encoding.UTF8.GetByteCount(key) > 1023)
            {
                throw new ArgumentOutOfRangeException("key", key, "too long");
            }

            return this.client.PutObject(bucketName, key, file).ETag;
        }

        public string UploadFile(string key, Stream file)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key", "is null");
            }

            if (Encoding.UTF8.GetByteCount(key) > 1023)
            {
                throw new ArgumentOutOfRangeException("key", key, "too long");
            }

            return this.client.PutObject(bucketName, key, file).ETag;
        }

        public string UploadFile(Stream file)
        {
            var key = OssUtils.ComputeContentMd5(file, file.Length);
            file.Seek(0, SeekOrigin.Begin);
            return this.client.PutObject(bucketName, key, file).ETag;
        }

        #endregion 上传

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="key"></param>
        public void DeleteFile(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key", "is null");
            }

            if (Encoding.UTF8.GetByteCount(key) > 1023)
            {
                throw new ArgumentOutOfRangeException("key", key, "too long");
            }

            this.client.DeleteObject(bucketName, key);
        }

        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="keys"></param>
        public void DeleteFiles(List<string> keys)
        {
            if (keys == null || keys.Count < 1)
            {
                throw new ArgumentNullException("keys", "keys count is too less");
            }

            for (var i = 0; i < keys.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(keys[i]))
                {
                    throw new ArgumentNullException("key", "is null");
                }

                if (Encoding.UTF8.GetByteCount(keys[i]) > 1023)
                {
                    throw new ArgumentOutOfRangeException("key", keys[i], "too long");
                }
            }
            var resuest = new DeleteObjectsRequest(bucketName, keys);
            this.client.DeleteObjects(resuest);
        }

        /// <summary>
        /// key拼接成域名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetUriString(string key)
        {
            return GetUri(key).ToString();
        }

        /// <summary>
        /// key拼接成域名
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static List<string> GetUrisString(List<string> keys)
        {
            var uris = new List<string>();
            foreach (var item in keys)
            {
                uris.Add(GetUri(item).ToString());
            }
            return uris;
        }

        /// <summary>
        /// key拼接成域名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Uri GetUri(string key)
        {
            return new Uri("http://" + bucketName + "." + endpoint + "/" + key + "?x-oss-process=style/s1");
        }

        /// <summary>
        /// key拼接成域名
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static List<Uri> GetUris(List<string> keys)
        {
            var uris = new List<Uri>();
            foreach (var item in keys)
            {
                uris.Add(GetUri(item));
            }
            return uris;
        }

        /// <summary>
        /// 图片url转img标签
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetImgTag(Uri uri)
        {
            var key = GetKeyFromUri(uri);
            return @"<img src='" + uri.ToString() +
                @"' class='file-preview-image' alt='" + key +
                @"' title='" + key + @"'style='width: auto; height: 60px; '>";
        }

        public static string GetImgTag(string key)
        {
            return @"<img src='" + GetUri(key).ToString() +
                @"' class='file-preview-image' alt='" + key +
                @"' title='" + key + @"'style='width: auto; height: 60px; '>";
        }

        /// <summary>
        /// 去掉域名保留key
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetKeyFromUri(Uri uri)
        {
            return Regex.Replace(uri.ToString(), "http://" + bucketName + "." + endpoint + "/", "");
        }

        /// <summary>
        /// 去掉域名保留key
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string GetKeyFromUri(string uri)
        {
            return Regex.Replace(uri, "http://" + bucketName + "." + endpoint + "/", "");
        }

        public static List<string> GetKeysFromUris(List<string> uris)
        {
            var keys = new List<string>();
            foreach (var item in uris)
            {
                keys.Add(Regex.Replace(item, "http://" + bucketName + "." + endpoint + "/", ""));
            }
            return keys;
        }
    }
}
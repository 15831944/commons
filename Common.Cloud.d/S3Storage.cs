using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Cloud
{
    public class S3Storage : IStorage
    {
        private const string accessKeyId = " ";

        private const string accessKeySecret = " ";

        public static RegionEndpoint endpoint = RegionEndpoint.CNNorth1;

        public static string bucketName = "school";

        private IAmazonS3 client;

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

            using (this.client = new AmazonS3Client(accessKeyId, accessKeySecret, endpoint))
            {
                var request = new PutObjectRequest()
                {
                    InputStream = file,
                    CannedACL = S3CannedACL.PublicRead,
                    BucketName = bucketName,
                    Key = key,
                };
                var response = this.client.PutObject(request);
                return response.ETag;
            }
        }

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

            using (this.client = new AmazonS3Client(accessKeyId, accessKeySecret, endpoint))
            {
                var request = new DeleteObjectRequest()
                {
                    BucketName = bucketName,
                    Key = key,
                };
                var response = this.client.DeleteObject(request);
            }
        }

        public string GetUriString(string key)
        {
            return new Uri("https://s3.cn-north-1.amazonaws.com.cn/" + bucketName + "/" + key).ToString();
        }

        public string GetKeyFromUri(string uri)
        {
            return Regex.Replace(uri, "https://s3.cn-north-1.amazonaws.com.cn/" + bucketName + "/", "");
        }
    }
}
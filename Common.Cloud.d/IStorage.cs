using System.IO;

namespace System.Cloud
{
    public interface IStorage
    {
        void DeleteFile(string key);

        string UploadFile(string key, Stream file);

        string GetUriString(string key);

        string GetKeyFromUri(string uri);
    }
}
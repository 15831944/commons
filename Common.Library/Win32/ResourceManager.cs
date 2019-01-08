using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace System.Win32
{
    /// <summary>
    /// ResourceManager
    /// </author>
    /// </summary>
    public class ResourceManager
    {
        private static volatile ResourceManager instance = null;

        private static object locker = new Object();

        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ResourceManager();
                        }
                    }
                }
                return instance;
            }
        }

        private string FolderPath = string.Empty;

        public SortedList<String, Resources> LanguageResources = new SortedList<String, Resources>();

        public void Serialize(Resources resources, string filePath)
        {
            ResourcesSerializer.Serialize(filePath, resources);
        }

        public void Init(string filePath)
        {
            this.FolderPath = filePath;
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            this.LanguageResources.Clear();
            if (!directoryInfo.Exists)
            {
                return;
            }
            FileInfo[] FileInfo = directoryInfo.GetFiles();
            for (int i = 0; i < FileInfo.Length; i++)
            {
                Resources resources = ResourcesSerializer.DeSerialize(FileInfo[i].FullName);
                resources.createIndex();
                this.LanguageResources.Add(resources.language, resources);
            }
        }

        public Hashtable GetLanguages()
        {
            Hashtable hashtable = new Hashtable();
            IEnumerator<KeyValuePair<String, Resources>> iEnumerator = this.LanguageResources.GetEnumerator();
            while (iEnumerator.MoveNext())
            {
                hashtable.Add(iEnumerator.Current.Key, iEnumerator.Current.Value.displayName);
            }
            return hashtable;
        }

        public Hashtable GetLanguages(string path)
        {
            Hashtable hashtable = new Hashtable();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                return hashtable;
            }
            FileInfo[] fileInfo = directoryInfo.GetFiles();
            for (int i = 0; i < fileInfo.Length; i++)
            {
                Resources resources = ResourcesSerializer.DeSerialize(fileInfo[i].FullName);
                hashtable.Add(resources.language, resources.displayName);
            }
            return hashtable;
        }

        public Resources GetResources(string filePath)
        {
            Resources resources = new Resources();
            if (File.Exists(filePath))
            {
                resources = ResourcesSerializer.DeSerialize(filePath);
                resources.createIndex();
            }
            return resources;
        }

        public string Get(string language, string key)
        {
            if (!this.LanguageResources.ContainsKey(language))
            {
                return string.Empty;
            }
            return this.LanguageResources[language].Get(key);
        }
    }
}
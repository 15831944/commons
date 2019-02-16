using System.Collections;

namespace System.Win32
{
    /// <summary>
    /// ResourceManagerWrapper
    /// </author>
    /// </summary>
    public class ResourceManagerWrapper
    {
        private static volatile ResourceManagerWrapper instance = null;

        private static object locker = new Object();

        private static string CurrentLanguage = "en-us";

        public static ResourceManagerWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ResourceManagerWrapper();
                        }
                    }
                }
                return instance;
            }
        }

        private ResourceManager ResourceManager;

        public ResourceManagerWrapper()
        {
        }

        public void LoadResources(string path)
        {
            this.ResourceManager = ResourceManager.Instance;
            this.ResourceManager.Init(path);
        }

        public string Get(string key)
        {
            return this.ResourceManager.Get(CurrentLanguage, key);
        }

        public string Get(string lanauage, string key)
        {
            return this.ResourceManager.Get(lanauage, key);
        }

        public Hashtable GetLanguages()
        {
            return this.ResourceManager.GetLanguages();
        }

        public Hashtable GetLanguages(string path)
        {
            return this.ResourceManager.GetLanguages(path);
        }

        public void Serialize(string path, string language, string key, string value)
        {
            Resources Resources = this.GetResources(path, language);
            Resources.Set(key, value);
            string filePath = path + "\\" + language + ".xml";
            this.ResourceManager.Serialize(Resources, filePath);
        }

        public Resources GetResources(string path, string language)
        {
            string filePath = path + "\\" + language + ".xml";
            return this.ResourceManager.GetResources(filePath);
        }

        public Resources GetResources(string language)
        {
            return this.ResourceManager.LanguageResources[language];
        }
    }
}
using EPiServer;
using EPiServer.Data.Dynamic;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using ManifestJsonHandler.Models.Data;
using System;
using System.Linq;

namespace ManifestJsonHandler.Services
{
    [ServiceConfiguration(typeof(IManifestJsonService), Lifecycle = ServiceInstanceScope.Singleton)]
    internal class ManifestJsonService : IManifestJsonService
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(ManifestJsonService));

        public string Get(string key)
        {
            // Always try to retrieve from cache first
            var cache = CacheManager.Get(GetCacheKey(key));
            if (cache != null)
                return cache as string;

            // Get from DDS
            var result = Find(key)?.Data ?? "{}";

            // Save cache
            CacheManager.Insert(GetCacheKey(key), result);

            return result;
        }

        public void Save(string key, string data)
        {
            using (var dataStore = GetStore())
            {
                var result = Find(key);

                if (result != null)
                {
                    result.Data = data;
                }
                else
                {
                    result = new ManifestJsonData()
                    {
                        Key = key,
                        Data = data
                    };
                }

                dataStore.Save(result);

                if (CacheManager.Get(GetCacheKey(key)) != null)
                    CacheManager.Remove(GetCacheKey(key));
            }
        }

        public string GetSiteKey(Guid siteId, string hostName)
        {
            return $"{siteId}_{hostName}";
        }

        private string GetCacheKey(string key)
        {
            return "ManifestJsonHandler_{0}" + key;
        }

        private DynamicDataStore GetStore()
        {
            return typeof(ManifestJsonData).GetStore();
        }

        private ManifestJsonData Find(string key)
        {
            using (var dataStore = GetStore())
            {
                // Look up the content in the DDS
                return dataStore.Find<ManifestJsonData>(nameof(ManifestJsonData.Key), key).FirstOrDefault();
            }
        }
    }
}

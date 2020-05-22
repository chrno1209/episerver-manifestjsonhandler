using System;
using ManifestJsonHandler.Models.Data;

namespace ManifestJsonHandler.Services
{
    public interface IManifestJsonService
    {
        string Get(string key);
        void Save(string key, string data);
        string GetSiteKey(Guid siteId, string hostName);
    }
}

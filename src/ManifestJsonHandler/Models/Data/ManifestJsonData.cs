using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace ManifestJsonHandler.Models.Data
{
    [EPiServerDataStore(AutomaticallyCreateStore = true)]
    public class ManifestJsonData : IDynamicData
    {
        public Identity Id { get; set; }

        [EPiServerDataIndex]
        public string Key { get; set; }

        public string Data { get; set; }
    }
}

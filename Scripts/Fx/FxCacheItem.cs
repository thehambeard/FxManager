using Kingmaker.Blueprints;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Kingmaker.ResourceLinks;

namespace FxManager.Fx
{
    public class FxCacheItem
    {
        [JsonProperty]
        public string AssetId {get; set;}

        [JsonProperty]
        public string Name { get; set;}

        [JsonProperty]
        public ConcurrentDictionary<string, byte> BlueprintReferences {get; set;}

        [JsonProperty]
        public PrefabLink PrefabLink {get; set;}

        public FxCacheItem() 
        { 
        }
        

        public FxCacheItem(string assetId, string name, PrefabLink prefabLink, BlueprintScriptableObject blueprint)
        {
            AssetId = assetId;
            Name = name;
            PrefabLink = prefabLink;

            //Fake Concurrent HashSet. I think better performance than using lock.
            BlueprintReferences = new ConcurrentDictionary<string, byte>
            {
                [blueprint.AssetGuidThreadSafe] = 0x00
            };
        }
    }
}

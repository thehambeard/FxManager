using Kingmaker.Blueprints;
using System.Collections.Concurrent;
using Newtonsoft.Json;

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

        public FxCacheItem() 
        { 
        }
        

        public FxCacheItem(string assetId, string name, BlueprintScriptableObject blueprint)
        {
            AssetId = assetId;
            Name = name;

            //Fake Concurrent HashSet. I think better performance than using lock.
            BlueprintReferences = new ConcurrentDictionary<string, byte>
            {
                [blueprint.AssetGuidThreadSafe] = 0x00
            };
        }
    }
}

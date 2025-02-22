using Kingmaker.Blueprints;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Kingmaker.ResourceLinks;
using System;

namespace FxManager.Cache
{
    [Serializable]
    public class BPCacheBase
    {
        [JsonProperty]
        public string AssetId { get; set; }

        [JsonProperty]
        public string name { get; set; }

        public BPCacheBase() { }

        public BPCacheBase(BlueprintScriptableObject blueprint)
        {
            AssetId = blueprint.AssetGuidThreadSafe;
            name = blueprint.name;
        }
    }
}

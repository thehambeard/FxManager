using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FxManager.Cache
{
    [Serializable]
    public class FxAssetsCache
    {
        [JsonProperty]
        public Dictionary<string, string> AssetByName { get; set; } = new Dictionary<string, string>();

        [JsonProperty]
        public Dictionary<string, string> NameByAsset { get; set; } = new Dictionary<string, string>();

        public FxAssetsCache() { }

        private static FxAssetsCache _instance;

        [JsonIgnore]
        public static FxAssetsCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FxAssetsCache();
                    _instance.Load();
                }
                return _instance;
            }
        }

        public bool TryGetAssetName(string name, out string asset)
        {
            return AssetByName.TryGetValue(name, out asset);
        }

        public bool TryGetNameByAsset(string asset, out string name)
        {
            return NameByAsset.TryGetValue(asset, out name);
        }

        public void Load()
        {
            var json = File.ReadAllText(Path.Combine(Main.ModDetails.Path, CacheConstants.FxAssetsCachePath));
            _instance = JsonConvert.DeserializeObject<FxAssetsCache>(json);
        }
    }
}

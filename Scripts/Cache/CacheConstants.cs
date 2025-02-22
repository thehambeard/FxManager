using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxManager.Cache
{
    internal static class CacheConstants
    {
        public const string CacheFolder = "Resource";
        public const string BlueprintCacheName = "BlueprintsCache.json";
        public const string BlueprintCachePath = CacheFolder + "/" + BlueprintCacheName;
        public const string FxAssetsCacheName = "FxAssetsCache.json";
        public const string FxAssetsCachePath = CacheFolder + "/" + FxAssetsCacheName;
    }
}

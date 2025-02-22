using Kingmaker.Blueprints;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxManager.Cache
{
    [Serializable]
    public class BPCacheUnit : BPCacheBase
    {
        [JsonIgnore]
        public BlueprintUnit Unit { get; private set; }

        [JsonProperty]
        public string CharacterName { get; set; }

        [JsonProperty]
        public string SpawnFxOnStart { get; set; }

        [JsonProperty]
        public string SpawnFxOnDeath { get; set; }

        public BPCacheUnit()
        {
        }

        public BPCacheUnit(BlueprintUnit unit) : base(unit)
        {
            Unit = unit;
            CharacterName = unit.CharacterName;
            
            var prefab = unit.Prefab.Load();
            SpawnFxOnStart = prefab.m_SpawnFxOnStart.FxOnStart.AssetId;
            SpawnFxOnDeath = prefab.m_SpawnFxOnStart.FxOnDeath.AssetId;
        }
    }
}

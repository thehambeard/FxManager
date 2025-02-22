using Kingmaker.UnitLogic.Buffs.Blueprints;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxManager.Cache
{
    [Serializable]
    internal class BPCacheBuff : BPCacheBase
    {
        [JsonIgnore]
        public BlueprintBuff Buff { get; private set; }

        [JsonProperty]
        public string FxOnStart { get; set; }

        [JsonProperty]
        public string FxOnRemove { get; set; }


        public BPCacheBuff()
        {
        }

        public BPCacheBuff(BlueprintBuff buff) : base(buff)
        {
            Buff = buff;
            FxOnStart = Buff.FxOnStart.AssetId;
            FxOnRemove = Buff.FxOnRemove.AssetId;
        }
    }
}

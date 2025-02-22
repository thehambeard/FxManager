using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Newtonsoft.Json;
using System;
using System.Reflection.Emit;

namespace FxManager.Cache
{
    [Serializable]
    internal class BPCacheProjectile : BPCacheBase
    {
        [JsonIgnore]
        public BlueprintProjectile Projectile { get; private set; }

        [JsonProperty]
        public string CastFx { get; set; }

        [JsonProperty]
        public string HitFx { get; set; }

        [JsonProperty]
        public string MissFx { get; set; }

        [JsonProperty]
        public string HitSnapFx { get; set; }

        [JsonProperty]
        public string MissDecalFx { get; set; }

        public BPCacheProjectile()
        {
        }

        public BPCacheProjectile(BlueprintProjectile projectile) : base(projectile)
        {
            Projectile = projectile;
            CastFx = projectile.CastFx.AssetId;
            HitFx = projectile.ProjectileHit.HitFx.AssetId;
            MissFx = projectile.ProjectileHit.MissFx.AssetId;
            HitSnapFx = projectile.ProjectileHit.HitSnapFx.AssetId;
            MissDecalFx = projectile.ProjectileHit.MissDecalFx.AssetId;
        }
    }
}
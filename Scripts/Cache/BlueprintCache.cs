using FxManager.Cache;
using FxManager.UI.MVVM.Views;
using FxManager.UI.MVVM.VMs;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace FxManager.Cache
{
    [HarmonyPatch]
    internal static class AfterLoadPatch
    {
        [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Awake))]
        [HarmonyPostfix]
        static void AfterLaunch(MainMenu __instance)
        {
            var prefab = ResourcesLibrary.TryGetResource<GameObject>("d187ac7feef9828419f6a49ac812ee5d");

            if (prefab == null)
                throw new NullReferenceException("Unable to load LoadStatusBox prefab");

            var obj = GameObject.Instantiate(prefab, __instance.transform, false);
            var view = obj.GetComponent<LoadStatusBoxPCView>();
            view.Bind(new LoadStatusBoxVM());
            BlueprintCache.Instance.Load();
        }
    }

    public enum BlueprintLoadingStatus
    {
        None,
        BlueprintsLoading,
        Complete,
        Error
    }



    internal class BlueprintCache
    {
        private static BlueprintCache _instance;

        public static BlueprintCache Instance
        {
            get => _instance ??= new BlueprintCache();
        }

        [JsonProperty]
        public ConcurrentDictionary<string, BPCacheBase> Blueprints = new ConcurrentDictionary<string, BPCacheBase>();

        public ReactiveProperty<BlueprintLoadingStatus> Status = new ReactiveProperty<BlueprintLoadingStatus>(BlueprintLoadingStatus.None);
        public ReactiveProperty<float> Progress = new ReactiveProperty<float>();

        public void Load()
        {
            _ = LoadBlueprintsAsync();
        }

        public async Task<List<SimpleBlueprint>> LoadBlueprintsAsync(Action onComplete = null)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            var path = Path.Combine(Main.ModDetails.Path, CacheConstants.BlueprintCachePath);

            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    var dummy = JsonConvert.DeserializeObject<ConcurrentDictionary<string, BPCacheBase>>(json, settings);
                    Blueprints = new ConcurrentDictionary<string, BPCacheBase>(dummy);
                    Status.Value = BlueprintLoadingStatus.Complete;
                    return null;
                }
                catch (Exception e)
                {
                    Main.Logger.Error("Loading blueprints from cache failed!");
                    Main.Logger.Error(e);
                }
            }

            List<SimpleBlueprint> bps;

            Status.Value = BlueprintLoadingStatus.BlueprintsLoading;

            while ((bps = BlueprintLoader.Shared.GetBlueprints()) == null && BlueprintLoader.Shared.IsRunning)
            {
                Progress.Value = BlueprintLoader.Shared.progress;
                await Task.Delay(1000);
            }

            Progress.Value = 1f;

            foreach (var bp in bps)
            {
                switch (bp)
                {
                    case BlueprintBuff buff:
                        Blueprints.TryAdd(buff.AssetGuidThreadSafe, new BPCacheBuff(buff));
                        break;
                    case BlueprintUnit unit:
                        Blueprints.TryAdd(unit.AssetGuidThreadSafe, new BPCacheUnit(unit));
                        break;
                    case BlueprintProjectile projectile:
                        Blueprints.TryAdd(projectile.AssetGuidThreadSafe, new BPCacheProjectile(projectile));
                        break;
                    default:
                        continue;
                }
            }

            Status.Value = BlueprintLoadingStatus.Complete;

            var json2write = JsonConvert.SerializeObject(Blueprints, Formatting.Indented, settings);
            File.WriteAllText(path, json2write);

            return bps;
        }
    }
}

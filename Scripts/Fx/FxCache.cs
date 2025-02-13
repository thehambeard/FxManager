using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxManager.Utility;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using FxManager.Utility.Extensions;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Blueprints.Root;
using UnityEngine;
using UniRx;
using FxManager.UI.MVVM.Views;
using FxManager.UI.MVVM.VMs;
using Kingmaker.ResourceLinks;
using System.Collections.Concurrent;
using System.Threading;
using Newtonsoft.Json;
using System.IO;

namespace FxManager.Fx
{
    [HarmonyPatch]
    internal static class AfterLoadPatch
    {
        [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Awake))]
        [HarmonyPostfix]
        static void AfterLaunch(MainMenu __instance)
        { 
            var prefab = ResourcesLibrary.TryGetResource<GameObject>("ae891d86221dbf541a88a10ce3a9d149");

            if (prefab == null)
                throw new NullReferenceException("Unable to load LoadStatusBox prefab");

            var obj = GameObject.Instantiate(prefab, __instance.transform, false);
            var view = obj.GetComponent<LoadStatusBoxPCView>();
            view.Bind(new LoadStatusBoxVM());
            

            FxCache.Instance.Load(); 
        }
    }

    public enum FxCollectionStatus
    {
        None,
        BlueprintsLoading,
        GettingFXs,
        Complete,
        Error
    }

    internal class FxCache
    {
        private static FxCache _instance;

        private int _total;
        private int _completed;
        private const string _cacheName = "fxCache.json";

        private readonly HashSet<string> _ignore = new HashSet<string>()
        {
            "a4f0eb3b30dbbfa45a612bd4ed804349"
        };

        public static FxCache Instance
        {
            get => _instance ??= new FxCache();
        }

        [JsonProperty] 
        public ConcurrentDictionary<string, FxCacheItem> FXs = new ConcurrentDictionary<string, FxCacheItem>();

        public ReactiveProperty<FxCollectionStatus> Status = new ReactiveProperty<FxCollectionStatus>(FxCollectionStatus.None);
        public ReactiveProperty<float> Progress = new ReactiveProperty<float>();

        public void Load() => _ = LoadAsync();

        public async Task LoadAsync(Action onComplete = null)
        {
            var path = Path.Combine(Main.ModDetails.Path, _cacheName);

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var dummy = JsonConvert.DeserializeObject<Dictionary<string, FxCacheItem>>(json);
                FXs = new ConcurrentDictionary<string, FxCacheItem>(dummy);
                Status.Value = FxCollectionStatus.Complete;
                return;
            }


            List<SimpleBlueprint> bps;

            Status.Value = FxCollectionStatus.BlueprintsLoading;

            while ((bps = BlueprintLoader.Shared.GetBlueprints()) == null && BlueprintLoader.Shared.IsRunning)
            {
                Progress.Value = BlueprintLoader.Shared.progress;
                await Task.Delay(200); 
            }

            Status.SetValueAndForceNotify(FxCollectionStatus.GettingFXs);
            Progress.Value = 0f;

            _total = bps.Count;
            _completed = 0;

            _ = Task.Run(Progressor);

            await Task.Run(() =>
            {
                Parallel.ForEach(bps, bp =>
                {
                    try
                    {
                        if (bp is BlueprintBuff buff)
                        {
                            if (!string.IsNullOrEmpty(buff.FxOnStart?.AssetId))
                                AddFxThreadSafe(buff.FxOnStart.AssetId, buff.FxOnStart, buff);
                            if (!string.IsNullOrEmpty(buff.FxOnRemove?.AssetId))
                                AddFxThreadSafe(buff.FxOnRemove.AssetId, buff.FxOnRemove, buff);
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.Logger.Error(ex);
                    }

                    _completed++;
                });
            });

            Status.Value = FxCollectionStatus.Complete;

            Progress.Value = 1f;

            try
            {
                string json = JsonConvert.SerializeObject(FXs, Formatting.Indented);
                File.WriteAllText(Path.Combine(Main.ModDetails.Path, _cacheName), json);
            }
            catch 
            {
                Main.Logger.Error("Saving cache failed.");
            }
            onComplete?.Invoke();
        }

        public void Progressor()
        {
            while (Status.Value != FxCollectionStatus.Complete)
            {
                Progress.Value = _completed / (float)_total;
                Progress.SetValueAndForceNotify(Math.Min(Math.Max(Progress.Value, 0), 1));
                Thread.Sleep(200);
            }
            Progress.Value = 1f;
        }

        private void AddFxThreadSafe(string assetId, PrefabLink link, BlueprintScriptableObject blueprint)
        {
            try
            {
                if (_ignore.Contains(assetId))
                    return;

                Observable.NextFrame().Subscribe(_ =>
                {
                    if (FXs.ContainsKey(assetId))
                        FXs[assetId].BlueprintReferences[blueprint.AssetGuid.ToString()] = 0x00;
                    else
                        FXs[assetId] = new FxCacheItem(assetId, link.Load().name, blueprint);
                });
            }
            catch
            {
                Main.Logger.Warning($"{link.AssetId} failed to load from bluprint {blueprint.name}: {blueprint.AssetGuid}");
            }
        }
    }
}

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
using Kingmaker.UnitLogic.Buffs;

namespace FxManager.Fx
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
            "a4f0eb3b30dbbfa45a612bd4ed804349",
            "a001e1b222dd2c2439445a1db0b77948",
            "7efcf49ee795e944199fd1891f69df63",
            "7f0b130061e53494ea06f654771aa5d0",
            "94a5a956678d8c947a4c34da92c0a3ed",
            "cfcddb3309117e2459957336cc6654bf",
            "e82d2f0a230564943ad63acf5cad8474",
            "a001e1b222dd2c2439445a1db0b77948",
            "ff2d5ffa001627f4d934002fd7c8d491",
            "f8dcfbcd0d6de5b4d8689479419ac930",
            "83e3242f47212ae419adeee8d8d94210",
            "8836539fbc8c99d42bf5ba18ec6f7b47",
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
            Progress.Value = -1f;

            _total = bps.Count;
            _completed = 0;

            _ = Task.Run(Progressor);

            await Task.Run(() =>
            {
                Parallel.ForEach(bps, bp =>
                {
                    try
                    {
                        switch (bp)
                        {
                            case BlueprintBuff buff:
                                AddFxThreadSafe(buff.FxOnStart, buff);
                                AddFxThreadSafe(buff.FxOnRemove, buff);
                                break;
                            case BlueprintUnit unit:
                                Observable.NextFrame().Subscribe(_ =>
                                {
                                    if (_ignore.Contains(unit.Prefab.AssetId))
                                        return;

                                    var prefab = unit.Prefab.Load();

                                    if (prefab == null || prefab.m_SpawnFxOnStart == null)
                                        return;

                                    AddFxThreadSafe(
                                        prefab.m_SpawnFxOnStart.FxOnStart,
                                        unit);

                                    AddFxThreadSafe(
                                        prefab.m_SpawnFxOnStart.FxOnDeath,
                                        unit);
                                });
                                break;
                        }
                        _completed++;
                    }
                    catch (Exception ex)
                    {
                        Main.Logger.Error(ex);
                    }
                });
            });

            Progress.Value = 1f;
            Status.Value = FxCollectionStatus.Complete;

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
                Thread.Sleep(500);
            }
            Progress.Value = 1f;
        }

        private void AddFxThreadSafe(PrefabLink link, BlueprintScriptableObject blueprint)
        {
            try
            {
                if (link == null || string.IsNullOrEmpty(link.AssetId))
                    return;

                if (_ignore.Contains(link.AssetId))
                    return;

                Observable.NextFrame().Subscribe(_ =>
                {
                    var prefabName = link.Load().name;

                    if (FXs.ContainsKey(prefabName) && FXs[prefabName].AssetId != link.AssetId)
                        Main.Logger.Warning($"PREFAB NAME COLLISON! {prefabName} between {link.AssetId} and {FXs[prefabName].AssetId}");
                    else if (FXs.ContainsKey(link.AssetId))
                        FXs[link.AssetId].BlueprintReferences[blueprint.AssetGuid.ToString()] = 0x00;
                    else
                        FXs[link.AssetId] = new FxCacheItem(link.AssetId, prefabName, link, blueprint);
                });
            }
            catch
            {
                Main.Logger.Warning($"{link.AssetId} failed to load from blueprint {blueprint.name}: {blueprint.AssetGuid}");
            }
        }
    }
}

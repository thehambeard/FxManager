using HarmonyLib;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.View;
using Kingmaker.Visual.Particles;
using Kingmaker.Visual.Particles.FxSpawnSystem;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using FxManager.Utility.Extensions;
using UniRx;

namespace FxManager.Fx
{
    internal class ActiveFxs
    {
        public static IReadOnlyCollection<ActiveFxModelBase> ActiveFXs => (IReadOnlyCollection<ActiveFxModelBase>) _activeFXs.Keys;
        private static readonly ConcurrentDictionary<ActiveFxModelBase, byte> _activeFXs = new ConcurrentDictionary<ActiveFxModelBase, byte>();

        private static readonly Subject<ActiveFxModelBase> _fxAddedSubject = new Subject<ActiveFxModelBase>();
        private static readonly Subject<ActiveFxModelBase> _fxRemovedSubject = new Subject<ActiveFxModelBase>();

        public static IObservable<ActiveFxModelBase> FxAddedObservable => _fxAddedSubject.AsObservable();
        public static IObservable<ActiveFxModelBase> FxRemovedObservable => _fxRemovedSubject.AsObservable();

        public static void Add(ActiveFxModelBase fx)
        {
            if (!_activeFXs.TryAdd(fx, 0))
                throw new InvalidOperationException($"The FX model already exists in the collection: {fx.Name}");

            _fxAddedSubject.OnNext(fx);
        }

        public static void Remove(ActiveFxModelBase fx)
        {
            if (!_activeFXs.TryRemove(fx, out _))
                throw new InvalidOperationException($"Removing FX model failed: {fx.Name}");

            _fxRemovedSubject.OnNext(fx);
        }

        static class FxPatches
        {
            [HarmonyPatch(typeof(FxHelper))]
            static class FxHelper_SpawnFxOnUnit_Patch
            {
                [HarmonyPatch(nameof(FxHelper.SpawnFxOnUnit))]
                [HarmonyPostfix]
                public static void SpawnFxOnUnit(
                    IFxHandle __result, 
                    GameObject prefab, 
                    UnitEntityView unit, 
                    bool? partyRelated = null, 
                    string defaultLocator = null, 
                    Vector3 offsetDirection = default(Vector3), 
                    FxPriority priority = FxPriority.EventuallyImportant)
                {
                    if (__result != null)
                        Add(new ActiveFxModelUnit(__result, unit));
                }
            }
        }
    }
}
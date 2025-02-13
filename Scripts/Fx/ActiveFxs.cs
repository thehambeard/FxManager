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
using UnityEngine;

namespace FxManager.Fx
{
    internal class ActiveFxs
    {
        public static ConcurrentDictionary<PrefabLink, ActiveFxModel> Active = new ConcurrentDictionary<PrefabLink, ActiveFxModel>();

        static class FxPatches
        {
            [HarmonyPatch(typeof(SpawnFxOnStart), nameof(SpawnFxOnStart.SpawnFx))]
            static class SpawnFxOnStartSpawnFxPatch
            {
                static PrefabLink _prefabLink;

                [HarmonyTranspiler]
                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
                {
                    /*
                    var matcher = new CodeMatcher()
                        .MatchStartForward(CodeMatch.IsLdarg(0));

                    matcher.DeclareLocal(typeof(SpawnFxOnStart), out var instance);
                    */
                    return instructions;

                }
            }
        }
    }
}
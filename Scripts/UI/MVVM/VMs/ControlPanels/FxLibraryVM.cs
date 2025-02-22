using FxManager.Cache;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Visual.Particles;
using Kingmaker.Visual;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace FxManager.UI.MVVM.VMs.ControlPanels
{
    internal class FxLibraryVM : BaseDisposable, IViewModel
    {
        public readonly ReactiveCollection<SettingHeaderElementVM> LibraryFxElements = new ReactiveCollection<SettingHeaderElementVM>();

        public readonly Dictionary<string, Type> ItemTypeMap = new Dictionary<string, Type>()
        {
            { "All", null },
            { "Buff", typeof(BPCacheBuff) },
            { "Unit", typeof(BPCacheUnit) }
        };

        public ReactiveProperty<Type> CurrentType { get; private set; } = new ReactiveProperty<Type>();

        public FxLibraryVM()
        {
        }

        public override void DisposeImplementation()
        {
        }

        internal void OnItemTypeChanged(string type)
        {
            if (ItemTypeMap.ContainsKey(type))
                CurrentType.Value = ItemTypeMap[type];

            var filteredBlueprints = BlueprintCache.Instance.Blueprints.Values
                .Where(bp => CurrentType.Value == null || bp.GetType() == CurrentType.Value)
                .ToList();

            LibraryFxElements.Clear();
            foreach (var blueprint in filteredBlueprints)
            {
                var element = new SettingHeaderElementVM(blueprint.Name);
                LibraryFxElements.Add(element);
            }
        }
    }
}

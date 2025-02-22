using FxManager.Cache;
using FxManager.UI.Events;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.PubSubSystem;
using Kingmaker.Visual;
using Kingmaker.Visual.Particles;
using Kingmaker.Visual.Particles.FxSpawnSystem;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace FxManager.UI.MVVM.VMs
{
    internal class CurrentFxPreviewVM : BaseDisposable, IGlobalSubscriber, IActiveFXSelected, IViewModel
    {
        public ReactiveProperty<string> FxNameText = new ReactiveProperty<string>("No FX Selected");
        public ReactiveProperty<string> AssetIdText = new ReactiveProperty<string>("");
        public ReactiveProperty<string> TypeText = new ReactiveProperty<string>("");
        public ReactiveProperty<string> TargetText = new ReactiveProperty<string>("");
        public ReactiveProperty<string> _delayInputField = new ReactiveProperty<string>("50");

        private UnitFxVisibilityManager _unitFxManager;
        private PooledFx _pooledFx;

        public CurrentFxPreviewVM()
        {
            base.AddDisposable(EventBus.Subscribe(this));
        }

        public override void DisposeImplementation()
        {
        }

        public void OnCurrentFXSelected(UnitFxVisibilityManager activeFx)
        {
            _unitFxManager = activeFx;

            if (!activeFx.TryGetComponent<PooledFx>(out _pooledFx))
                Main.Logger.Warning($"Failed to get PooledFx component from {activeFx.name}");

            SetProperties();
        }

        private void SetProperties()
        {
            if (_unitFxManager == null)
            {
                FxNameText.Value = "No FX Selected";
                AssetIdText.Value = "";
                TypeText.Value = "";
                TargetText.Value = "";
                return;
            }

            var name = _pooledFx != null ? _pooledFx.Prefab.name : _unitFxManager.name;

            FxNameText.Value = name;
            AssetIdText.Value = FxAssetsCache.Instance.TryGetAssetName(name, out var assetId)
                ? assetId
                : "No AssetId Found";
        }
    }
}

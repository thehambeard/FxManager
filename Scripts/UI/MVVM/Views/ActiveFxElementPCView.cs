using FxManager.UI.MVVM.VMs;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using Owlcat.Runtime.UI.VirtualListSystem.ElementSettings;
using UnityEngine.UI;
using Kingmaker.PubSubSystem;
using FxManager.UI.Events;
using Kingmaker.Visual.Particles;
using Kingmaker.ResourceLinks;
using Kingmaker;
using Kingmaker.Visual.Particles.FxSpawnSystem;
using System.Diagnostics;
using Kingmaker.Blueprints.Root;
using Kingmaker.Utility;

namespace FxManager.UI.MVVM.Views
{
    internal class ActiveFxElementPCView : VirtualListElementViewBase<ActiveFxElementVM>
    {
        public override VirtualListLayoutElementSettings LayoutSettings => _layoutSettings;

        [SerializeField]
        private VirtualListLayoutElementSettings _layoutSettings;

        [SerializeField]
        private TextMeshProUGUI _fxName;

        [SerializeField]
        private TextMeshProUGUI _fxDescription;

        [SerializeField]
        private Button _selectFxButton;

        //private PrefabLink _fxPrefabLink;
        //private bool toggle = false;
        //private bool force = false;
        //private FxHandle _handle;
        //private Stopwatch _repeat;
        //private GameObject _prefab;
        //private float _timeToLive;

        public override void BindViewImplementation()
        {
            base.AddDisposable(ViewModel.FxName.Subscribe(x => _fxName.text = x));
            base.AddDisposable(ViewModel.Description.Subscribe(x => _fxDescription.text = x));
            base.AddDisposable(_selectFxButton
                .OnClickAsObservable()
                .Subscribe(x =>
                    EventBus.RaiseEvent<IActiveFXSelected>((h) =>
                        h.OnCurrentFXSelected(ViewModel.UnitFxVisibilityManager))));

            //_fxPrefabLink = new PrefabLink() { AssetId = "85c681ca38c69834a96c3255c99a152c" };
            //_prefab = _fxPrefabLink.Load();

            //base.AddDisposable(_selectFxButton
            //    .OnClickAsObservable()
            //    .Subscribe(x =>
            //    {
            //        toggle = !toggle;
            //        force = true;
            //    }));
        }

        //public void LateUpdate()
        //{
        //    if (!force && (!toggle || _repeat?.ElapsedMilliseconds < _timeToLive))
        //        return;

        //    if (_handle?.SpawnedObject != null)
        //        GameObject.DestroyImmediate(_handle.SpawnedObject);

        //    if (_handle != null && !_handle.IsDestroyed)
        //    {
        //        _handle.HandleDestroy();
        //        return;
        //    }

        //    force = false;
        //    _handle = (FxHandle)FxHelper.SpawnFxOnUnit(_prefab, Game.Instance.Player.MainCharacter.Value.View, defaultLocator: "Locator_PalmFX_Left");
        //    _timeToLive = _handle.TimeToLive * 1000f;
        //    _repeat = Stopwatch.StartNew();
        //}

        public override void DestroyViewImplementation()
        {
        }
    }
}

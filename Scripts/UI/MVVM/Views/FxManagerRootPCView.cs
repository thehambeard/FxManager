using FxManager.UI.MVVM.VMs;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UI.Log;
using Kingmaker.UI.MVVM._PCView.IngameMenu;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FxManager.Utility.Helpers;

namespace FxManager.UI.MVVM.Views
{
    internal class FxManagerRootPCView : ViewBase<FxManagerRootVM>, IInitializable
    {
        [SerializeField]
        private ActiveFxsWindowPCView _activeFxsWindowPCView;

        [SerializeField]
        private FxOpenCloseWindowPCView _openCloseWindowPCView;

        [SerializeField]
        private CurrentFxPreviewPCView _currentFxPreviewPCView;

#if UNITY_EDITOR
        private void OnEnable()
        {
            Initialize();
            Bind(new FxManagerRootVM());
        }
#endif
        public void Initialize()
        {
            _currentFxPreviewPCView.Initialize();
            _activeFxsWindowPCView.Initialize();
        }

        public override void BindViewImplementation()
        {
            _activeFxsWindowPCView.Bind(new ActiveFxsWindowVM());
            _openCloseWindowPCView.Bind(new FxOpenCloseVM());
            _currentFxPreviewPCView.Bind(new CurrentFxPreviewVM());
        }

        public override void DestroyViewImplementation()
        {
        }

        [HarmonyPatch(typeof(IngameMenuPCView))]
        static class AttachToMenu
        {
            static FxManagerRootPCView _rootPCView;
            static GameObject _prefab;

            [HarmonyPatch(nameof(IngameMenuPCView.BindViewImplementation))]
            [HarmonyPostfix]
            public static void Attach(IngameMenuPCView __instance)
            {
                if (_prefab == null)
                    _prefab = ResourcesLibrary.TryGetResource<GameObject>("5d637199b62dd4e46bb69cd9e21f8b1f");

                if (_rootPCView != null)
                    _rootPCView.DestroyView();

                var go = GameObject.Instantiate(_prefab, WrathHelpers.GetStaticCanvas().transform, false);
                var rootPCView = go.GetComponent<FxManagerRootPCView>();
                rootPCView.Initialize();
                rootPCView.Bind(new FxManagerRootVM());
            }
        }
    }
}

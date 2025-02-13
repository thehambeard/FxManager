using FxManager.UI.MVVM.VMs;
using Kingmaker.UI.Log;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            _activeFxsWindowPCView.Initialize();
            _currentFxPreviewPCView.Initialize();
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
    }
}

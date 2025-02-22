using FxManager.UI.MVVM.Views.ControlPanels;
using FxManager.UI.MVVM.VMs;
using FxManager.UI.MVVM.VMs.ControlPanels;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FxManager.UI.MVVM.Views
{
    internal interface INameable
    {
        string Name { get; }
    }

    internal class ControlWindowPCView : ViewBase<ControlWindowVM>, IInitializable
    {
        [SerializeField]
        private TMP_InputField _searchInput;

        [SerializeField]
        public ActiveFxsWindowPCView ActiveFxsWindow;

        [SerializeField]
        public FxLibraryPCView FxLibrary;

        [SerializeField]
        public CaptureFxsPCView CaptureFxs;

        [SerializeField]
        public CustomsFxsPCView CustomsFxs;

        [SerializeField]
        private Button _activeFxsButton;

        [SerializeField]
        private Button _fxLibraryButton;

        [SerializeField]
        private Button _captureFxsButton;

        [SerializeField]
        private Button _customsFxsButton;

#if UNITY_EDITOR
        public void Awake()
        {
            Initialize();
            this.Bind(new ControlWindowVM(ActiveFxsWindow.gameObject));
        }
#endif
        public void Initialize()
        {
            ActiveFxsWindow.Initialize();
            FxLibrary.Initialize();
            CaptureFxs.Initialize();
            CustomsFxs.Initialize();
        }

        public override void BindViewImplementation()
        {
            ActiveFxsWindow.Bind(new ActiveFxsWindowVM());
            FxLibrary.Bind(new FxLibraryVM());
            CaptureFxs.Bind(new CaptureFxsVM());
            CustomsFxs.Bind(new CustomsFxsVM());

            base.AddDisposable(_activeFxsButton.OnClickAsObservable().Subscribe(_ => OnViewButtonClicked(ActiveFxsWindow.gameObject)));
            base.AddDisposable(_fxLibraryButton.OnClickAsObservable().Subscribe(_ => OnViewButtonClicked(FxLibrary.gameObject)));
            base.AddDisposable(_captureFxsButton.OnClickAsObservable().Subscribe(_ => OnViewButtonClicked(CaptureFxs.gameObject)));
            base.AddDisposable(_customsFxsButton.OnClickAsObservable().Subscribe(_ => OnViewButtonClicked(CustomsFxs.gameObject)));
            ViewModel.CurrentView.Value.gameObject.SetActive(true);

            gameObject.SetActive(false);
            transform.localScale = new Vector3(.6f, .6f, .6f);
        }

        private void OnViewButtonClicked(GameObject newView)
        {
            if (ViewModel.CurrentView.Value != newView)
            {
                if (!ViewModel.CurrentView.Value.TryGetComponent<CanvasGroup>(out var canvasGroupFadeOut))
                    throw new InvalidOperationException("Current view has no CanvasGroup component");

                if (!newView.TryGetComponent<CanvasGroup>(out var canvasGroupFadeIn))
                    throw new InvalidOperationException($"${newView.name} has no CanvasGroup component");

                canvasGroupFadeOut.DOFade(0, 0.15f).OnComplete(() =>
                {
                    ViewModel.CurrentView.Value.SetActive(false);
                    canvasGroupFadeIn.alpha = 0;
                    newView.gameObject.SetActive(true);
                    canvasGroupFadeIn.DOFade(1, 0.15f);
                });

                ViewModel.CurrentView.Value = newView;
            }
        }
        public override void DestroyViewImplementation()
        {
        }
    }
}

using FxManager.Cache;
using FxManager.UI.Events;
using FxManager.UI.MVVM.VMs;
using FxManager.UI.WindowControl;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace FxManager.UI.MVVM.Views
{
    internal class CurrentFxPreviewPCView : ViewBase<CurrentFxPreviewVM>, IInitializable
    {
        [SerializeField]
        private SettingsPanelPCView _settingsPanelPCView;

        [SerializeField]
        private TextMeshProUGUI _fxNameText;

        [SerializeField]
        private TextMeshProUGUI _assetIdText;

        [SerializeField] 
        private TextMeshProUGUI _typeText;

        [SerializeField] 
        private TextMeshProUGUI _targetText;

        [SerializeField]
        private Button _toggleFx;

        [SerializeField]
        private TMP_InputField _delayInputField;

        public void Initialize()
        {
            _settingsPanelPCView.Initialize();
        }

        public override void BindViewImplementation()
        {
            _settingsPanelPCView.Bind(new SettingsPanelVM());
            base.AddDisposable(ViewModel.FxNameText.Subscribe(x => _fxNameText.text = x));
            base.AddDisposable(ViewModel.AssetIdText.Subscribe(x => _assetIdText.text = x));
            base.AddDisposable(ViewModel.TypeText.Subscribe(x => _typeText.text = x));
            base.AddDisposable(ViewModel.TargetText.Subscribe(x => _targetText.text = x));
            base.AddDisposable(ViewModel._delayInputField.Subscribe(x => _delayInputField.text = x));
            base.AddDisposable(_delayInputField.onValueChanged
                .AsObservable()
                .Subscribe(input =>
                {
                    if (input.All(char.IsDigit))
                        ViewModel._delayInputField.Value = input;
                    else
                        _delayInputField.text = ViewModel._delayInputField.Value;
                }));

            gameObject.SetActive(false);
            transform.localScale = new Vector3(.6f, .6f, .6f);
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

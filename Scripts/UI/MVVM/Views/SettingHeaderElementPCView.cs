using FxManager.UI.MVVM.VMs;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using Owlcat.Runtime.UI.VirtualListSystem.ElementSettings;
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
    internal class SettingHeaderElementPCView : VirtualListElementViewBase<SettingHeaderElementVM>
    {
        [SerializeField]
        private TextMeshProUGUI _headerText;

        [SerializeField]
        private Button _expandButton;

        [SerializeField]
        private VirtualListLayoutElementSettings _layoutSettings;

        public override VirtualListLayoutElementSettings LayoutSettings => _layoutSettings;

        public override void BindViewImplementation()
        {
            base.AddDisposable(ViewModel.HeaderText.Subscribe(x => _headerText.text = x));
            base.AddDisposable(_expandButton.OnClickAsObservable().Subscribe(_ => base.ViewModel.IsOn.Value = !base.ViewModel.IsOn.Value));
            base.AddDisposable(ViewModel.IsOn.Subscribe(OnChange));
        }

        public override void DestroyViewImplementation()
        {
        }

        private void OnChange(bool isOn)
        {
        }
    }
}

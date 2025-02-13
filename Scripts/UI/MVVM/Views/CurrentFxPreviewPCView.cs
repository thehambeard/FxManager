using FxManager.UI.MVVM.VMs;
using FxManager.UI.WindowControl;
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
    internal class CurrentFxPreviewPCView : ViewBase<CurrentFxPreviewVM>, IInitializable
    {
        [SerializeField]
        private SettingsPanelPCView _settingsPanelPCView;

        public void Initialize()
        {
            _settingsPanelPCView.Initialize();
        }

        public override void BindViewImplementation()
        {
            _settingsPanelPCView.Bind(new SettingsPanelVM());
            gameObject.SetActive(false);
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

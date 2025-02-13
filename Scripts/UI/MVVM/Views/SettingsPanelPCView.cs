using FxManager.UI.MVVM.Views.Settings;
using FxManager.UI.MVVM.VMs;
using FxManager.UI.MVVM.VMs.Settings;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using Owlcat.Runtime.UI.VirtualListSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace FxManager.UI.MVVM.Views
{
    internal class SettingsPanelPCView : ViewBase<SettingsPanelVM>, IInitializable
    {
        [SerializeField]
        private TextMeshProUGUI _fxName;

        [SerializeField]
        private VirtualListVertical _virtualListVertical;

        [SerializeField]
        SettingViews _settingViews;

        public void Initialize()
        {
            _settingViews.InitializeVirtualList(_virtualListVertical);
        }

        public override void BindViewImplementation()
        {
            base.AddDisposable(_virtualListVertical.Subscribe<VirtualListElementVMBase>(base.ViewModel.SettingEntities));
        }

        public override void DestroyViewImplementation()
        {
        }

        [Serializable]
        public class SettingViews
        {
            public void InitializeVirtualList(VirtualListComponent virtualListComponent)
            {
                virtualListComponent.Initialize(new IVirtualListElementTemplate[]
                { 
                    new VirtualListElementTemplate<SettingHeaderElementVM>(_settingHeaderElementPCView),
                    new VirtualListElementTemplate<ColorPickerElementVM>(_colorPickerPCView),
                });
            }

            [SerializeField]
            private SettingHeaderElementPCView _settingHeaderElementPCView;

            [SerializeField]
            private ColorPickerElementPCView _colorPickerPCView;

        }
    }
}

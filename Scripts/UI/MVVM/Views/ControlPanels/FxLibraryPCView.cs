using FxManager.UI.MVVM.VMs;
using FxManager.UI.MVVM.VMs.ControlPanels;
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

namespace FxManager.UI.MVVM.Views.ControlPanels
{
    internal class FxLibraryPCView : ViewBase<FxLibraryVM>, IInitializable, INameable
    {
        [SerializeField]
        private TMP_Dropdown _fxLibraryDropdown;

        [SerializeField]
        private VirtualListVertical _virtualListVertical;

        [SerializeField]
        private SettingHeaderElementPCView _virtualListElementPrefab;

        public string Name => "Library";
        public void Initialize()
        {
            _virtualListVertical.Initialize(new VirtualListElementTemplate<SettingHeaderElementVM>(_virtualListElementPrefab));
        }

        public override void BindViewImplementation()
        {
            gameObject.SetActive(false);
            _fxLibraryDropdown.ClearOptions();
            _fxLibraryDropdown.options.AddRange(ViewModel.ItemTypeMap.Keys.Select(x => new TMP_Dropdown.OptionData(x)));
            _fxLibraryDropdown.RefreshShownValue();
            _fxLibraryDropdown.onValueChanged.AddListener(OnItemTypeChanged);

            if (_fxLibraryDropdown.options.Count > 0)
            {
                _fxLibraryDropdown.value = 0;
                OnItemTypeChanged(0);
            }

            base.AddDisposable(_virtualListVertical.Subscribe(ViewModel.LibraryFxElements));
        }

        private void OnItemTypeChanged(int index) => ViewModel.OnItemTypeChanged(_fxLibraryDropdown.options[index].text);

        public override void DestroyViewImplementation()
        {
        }
    }
}

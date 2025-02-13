using FxManager.UI.MVVM.VMs.Settings;
using Kingmaker.UI.SettingsUI;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace FxManager.UI.MVVM.VMs
{
    internal class SettingsPanelVM : BaseDisposable, IViewModel
    {
        public IReadOnlyReactiveCollection<VirtualListElementVMBase> SettingEntities => _settingEntities;

        private readonly ReactiveCollection<VirtualListElementVMBase> _settingEntities = new ReactiveCollection<VirtualListElementVMBase>();

        public SettingsPanelVM()
        {
            MakeSetting("Test", new List<VirtualListElementVMBase>
            {
                new ColorPickerElementVM("Color 1", Color.cyan),
                new ColorPickerElementVM("Color 2", Color.red),
                new ColorPickerElementVM("Color 3", Color.green)
            });

            MakeSetting("Test2", new List<VirtualListElementVMBase>
            {
                new ColorPickerElementVM("Color 1", Color.cyan),
                new ColorPickerElementVM("Color 2", Color.red),
                new ColorPickerElementVM("Color 3", Color.green)
            });
        }

        public void MakeSetting(string headerText, List<VirtualListElementVMBase> children)
        {
            var header = new SettingHeaderElementVM(headerText, children);

            _settingEntities.Add(header);

            if (header.IsOn.Value)
                children.ForEach(vmbase => _settingEntities.Add(vmbase));

            base.AddDisposable(header.IsOn.Subscribe(isOn =>
            {
                if (isOn)
                {
                    var index = _settingEntities.IndexOf(header);

                    foreach (var child in header.Children.Reverse())
                    {
                        if (_settingEntities.Contains(child))
                            continue;

                        if (index >= _settingEntities.Count)
                            _settingEntities.Add(child);
                        else
                            _settingEntities.Insert(index + 1, child);
                    }
                }
                else
                {
                    foreach (var child in header.Children)
                    {
                        _settingEntities.Remove(child);
                    }
                }
            }));
        }

        public override void DisposeImplementation()
        {
        }
    }
}

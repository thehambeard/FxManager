using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace FxManager.UI.MVVM.VMs.Settings
{
    internal class ColorPickerElementVM : VirtualListElementVMBase
    {
        public readonly StringReactiveProperty SettingName = new StringReactiveProperty("");
        public readonly ReactiveProperty<Color> Color = new ReactiveProperty<Color>();

        public ColorPickerElementVM(string settingName, Color color = default)
        {
            Color.Value = color;
            SettingName.Value = settingName;
        }
        
        public override void DisposeImplementation()
        {
        }
    }
}

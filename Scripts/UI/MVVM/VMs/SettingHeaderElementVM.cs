using JetBrains.Annotations;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace FxManager.UI.MVVM.VMs
{
    internal class SettingHeaderElementVM : VirtualListElementVMBase
    {
        public readonly StringReactiveProperty HeaderText = new StringReactiveProperty("");
        public readonly BoolReactiveProperty IsOn = new BoolReactiveProperty(true);
        public IReadOnlyCollection<VirtualListElementVMBase> Children => _children;

        private readonly ReactiveCollection<VirtualListElementVMBase> _children = new ReactiveCollection<VirtualListElementVMBase>();

        public SettingHeaderElementVM(string headerText, List<VirtualListElementVMBase> children = null)
        {
            HeaderText.Value = headerText;
        
            if (children != null) 
                children.ForEach(x => _children.Add(x));
        }

        public override void DisposeImplementation()
        {
            _children.Clear();
        }
    }
}

using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace FxManager.UI.MVVM.VMs
{
    public class ActiveFxElementVM : VirtualListElementVMBase
    {
        public ReactiveProperty<string> FxName = new ReactiveProperty<string>("");

        public ActiveFxElementVM(string fxName)
        {
            FxName.Value = fxName;
        }

        public override void DisposeImplementation()
        {
        }
    }
}

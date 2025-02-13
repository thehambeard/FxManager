using FxManager.UI.MVVM.Views;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace FxManager.UI.MVVM.VMs
{
    public class ActiveFxsWindowVM : BaseDisposable, IViewModel
    {
        public IReadOnlyReactiveCollection<VirtualListElementVMBase> ActiveFxElements => _activeFxElements;

        private readonly ReactiveCollection<VirtualListElementVMBase> _activeFxElements = new ReactiveCollection<VirtualListElementVMBase>();

        public ActiveFxsWindowVM()
        {
            for (int i = 0; i < 50; i++)
            {
                _activeFxElements.Add(new ActiveFxElementVM($"FX TEST{i}"));
            }
        }

        public override void DisposeImplementation()
        {
        }
    }
}

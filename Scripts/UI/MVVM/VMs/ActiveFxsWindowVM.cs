using FxManager.UI.MVVM.Views;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxManager.Fx;
using UniRx;

namespace FxManager.UI.MVVM.VMs
{
    public class ActiveFxsWindowVM : BaseDisposable, IViewModel
    {
        public IReadOnlyReactiveCollection<VirtualListElementVMBase> ActiveFxElements => _activeFxElements;

        private readonly ReactiveCollection<VirtualListElementVMBase> _activeFxElements = new ReactiveCollection<VirtualListElementVMBase>();
        private readonly Dictionary<ActiveFxModelBase, VirtualListElementVMBase> _kvps = new Dictionary<ActiveFxModelBase, VirtualListElementVMBase>();

        public ActiveFxsWindowVM()
        {
            base.AddDisposable(ActiveFxs.FxAddedObservable.Subscribe(fx => AddFx(fx)));
            base.AddDisposable(ActiveFxs.FxRemovedObservable.Subscribe(fx => RemoveFx(fx)));
        }

        private void AddFx(ActiveFxModelBase fx)
        {
            var vm = new ActiveFxElementVM(fx);

            _activeFxElements.Add(vm);
            _kvps.Add(fx, vm);
        }

        private void RemoveFx(ActiveFxModelBase fx)
        {
            if (!_kvps.TryGetValue(fx, out var vm))
                throw new KeyNotFoundException();

            _activeFxElements.Remove(vm);
            _kvps.Remove(fx);
        }

        public override void DisposeImplementation()
        {
        }
    }
}

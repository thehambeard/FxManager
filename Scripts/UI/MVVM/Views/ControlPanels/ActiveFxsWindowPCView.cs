using FxManager.UI.MVVM.VMs.ControlPanels;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using Owlcat.Runtime.UI.VirtualListSystem;
using UnityEngine;
using UniRx;
using FxManager.UI.MVVM.VMs;

namespace FxManager.UI.MVVM.Views.ControlPanels
{
    internal class ActiveFxsWindowPCView : ViewBase<ActiveFxsWindowVM>, IInitializable, INameable
    {
        [SerializeField]
        private VirtualListVertical _virtualListVertical;

        [SerializeField]
        private ActiveFxElementPCView _virtualListElementPrefab;

        public string Name => "Active";

        public void Initialize()
        {
            _virtualListVertical.Initialize(new VirtualListElementTemplate<ActiveFxElementVM>(_virtualListElementPrefab));
        }

        public override void BindViewImplementation()
        {
            base.AddDisposable(_virtualListVertical.Subscribe(ViewModel.ActiveFxElements));
            //base.AddDisposable(this.ObserveEveryValueChanged(_ => ((RectTransform)transform).sizeDelta)
            //    .DistinctUntilChanged()
            //    .Subscribe(newSize => 
            //    {
            //        _virtualListVertical.m_LayoutSettings.Width = newSize.x - _virtualListVertical.m_LayoutSettings.Padding.Right;
            //        _virtualListVertical.m_VirtualList.m_Scroll.UpdateScrollValue();
            //        _virtualListVertical.m_VirtualList.m_DistancesCalculator.UpdateViewportData();
            //        _virtualListVertical.m_VirtualList.m_ElementsWalker.Tick(true);
            //    }));
        }

        public override void DestroyViewImplementation()
        {
        }

        
    }
}

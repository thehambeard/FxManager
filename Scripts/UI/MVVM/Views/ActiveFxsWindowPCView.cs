using FxManager.UI.MVVM.VMs;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using Owlcat.Runtime.UI.VirtualListSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using Owlcat.Runtime.UI.VirtualListSystem.Vertical;

namespace FxManager.UI.MVVM.Views
{
    internal class ActiveFxsWindowPCView : ViewBase<ActiveFxsWindowVM>, IInitializable
    {
        [SerializeField]
        private VirtualListVertical _virtualListVertical;

        [SerializeField]
        private ActiveFxElementPCView _virtualListElementPrefab;

        public void Initialize()
        {
            _virtualListVertical.Initialize(new VirtualListElementTemplate<ActiveFxElementVM>(_virtualListElementPrefab));
        }

        public override void BindViewImplementation()
        {
            base.AddDisposable(_virtualListVertical.Subscribe(ViewModel.ActiveFxElements));
            base.AddDisposable(this.ObserveEveryValueChanged(_ => ((RectTransform)transform).sizeDelta)
                .DistinctUntilChanged()
                .Subscribe(newSize => 
                {
                    _virtualListVertical.m_LayoutSettings.Width = newSize.x - _virtualListVertical.m_LayoutSettings.Padding.Right;
                    _virtualListVertical.m_VirtualList.m_Scroll.UpdateScrollValue();
                    _virtualListVertical.m_VirtualList.m_DistancesCalculator.UpdateViewportData();
                    _virtualListVertical.m_VirtualList.m_ElementsWalker.Tick(true);
                }));
                
            gameObject.SetActive(false);
        }

        public override void DestroyViewImplementation()
        {
        }

        
    }
}

using FxManager.UI.MVVM.Views;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxManager.Cache;
using UniRx;
using UnityEngine;
using Kingmaker.Visual.Particles;
using Owlcat.Runtime.Core.Utils;
using Kingmaker.Visual;

namespace FxManager.UI.MVVM.VMs.ControlPanels
{
    public class ActiveFxsWindowVM : BaseDisposable, IViewModel
    {
        public readonly ReactiveCollection<ActiveFxElementVM> ActiveFxElements = new ReactiveCollection<ActiveFxElementVM>();
        private readonly Dictionary<Transform, UnitFxVisibilityManager> _visibilityManagerCache = new Dictionary<Transform, UnitFxVisibilityManager>();
        private readonly ChildChangeObserver _childChangeObserver;

        public ActiveFxsWindowVM()
        {
            _childChangeObserver = FxHelper.FxRoot.EnsureComponent<ChildChangeObserver>();
            _childChangeObserver.activeFxsWindowVM = this;
        }

        private void OnChildChange()
        {
            int transformChildCount = FxHelper.FxRoot.transform.childCount;

            for (int i = 0; i < transformChildCount; i++)
            {
                Transform child = FxHelper.FxRoot.transform.GetChild(i);
                if (_visibilityManagerCache.TryGetValue(child, out var visibilityManager))
                {
                    if (i < ActiveFxElements.Count)
                        ActiveFxElements[i].SetManager(visibilityManager);
                    else
                        ActiveFxElements.Add(new ActiveFxElementVM(visibilityManager));
                }
                else
                {
                    if (child.TryGetComponent<UnitFxVisibilityManager>(out var newVisibilityManager))
                    {
                        _visibilityManagerCache.Add(child, newVisibilityManager);
                        if (i < ActiveFxElements.Count)
                            ActiveFxElements[i].SetManager(visibilityManager);
                        else
                            ActiveFxElements.Add(new ActiveFxElementVM(visibilityManager));
                    }
                }
            }

            if (ActiveFxElements.Count > transformChildCount)
            {
                for (int i = transformChildCount; i < ActiveFxElements.Count; i++)
                {
                    ActiveFxElements.RemoveAt(i);
                }
            }
        }   
        
        public override void DisposeImplementation()
        {
        }
        public class ChildChangeObserver : MonoBehaviour
        {
            public ActiveFxsWindowVM activeFxsWindowVM;

            public void OnTransformChildrenChanged() => activeFxsWindowVM.OnChildChange();
        }
    }
}

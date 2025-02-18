using FxManager.UI.MVVM.VMs;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using Owlcat.Runtime.UI.VirtualListSystem.ElementSettings;
using UnityEngine.UI;
using Kingmaker.PubSubSystem;
using FxManager.UI.Events;

namespace FxManager.UI.MVVM.Views
{
    internal class ActiveFxElementPCView : VirtualListElementViewBase<ActiveFxElementVM>
    {
        public override VirtualListLayoutElementSettings LayoutSettings => _layoutSettings;

        [SerializeField]
        private VirtualListLayoutElementSettings _layoutSettings;

        [SerializeField]
        private TextMeshProUGUI _fxName;

        [SerializeField]
        private TextMeshProUGUI _fxDescription;

        [SerializeField]
        private Button SelectFxButton;

        
        public override void BindViewImplementation()
        {
            base.AddDisposable(ViewModel.FxName.Subscribe(x => _fxName.text = x));
            base.AddDisposable(ViewModel.Description.Subscribe(x => _fxDescription.text = x));
            base.AddDisposable(SelectFxButton
                .OnClickAsObservable()
                .Subscribe(x => 
                    EventBus.RaiseEvent<IActiveFXSelected>((h) => 
                        h.OnCurrentFXSelected(ViewModel.FxModel))));
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

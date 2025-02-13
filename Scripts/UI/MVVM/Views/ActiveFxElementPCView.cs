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

namespace FxManager.UI.MVVM.Views
{
    internal class ActiveFxElementPCView : VirtualListElementViewBase<ActiveFxElementVM>
    {
        [SerializeField]
        private TextMeshProUGUI _fxName;

        
        public override void BindViewImplementation()
        {
            base.AddDisposable(ViewModel.FxName.Subscribe(x => _fxName.text = x));
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

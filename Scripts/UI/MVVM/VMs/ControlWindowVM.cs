using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace FxManager.UI.MVVM.VMs
{
    internal class ControlWindowVM : BaseDisposable, IViewModel
    {
        public ReactiveProperty<GameObject> CurrentView = new ReactiveProperty<GameObject>();

        public ControlWindowVM(GameObject initialActive)
        {
            CurrentView.Value = initialActive;
        }

        public override void DisposeImplementation()
        {
        }
    }
}

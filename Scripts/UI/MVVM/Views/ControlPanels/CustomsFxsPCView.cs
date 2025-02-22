using FxManager.UI.MVVM.VMs.ControlPanels;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxManager.UI.MVVM.Views.ControlPanels
{
    internal class CustomsFxsPCView : ViewBase<CustomsFxsVM>, IInitializable, INameable
    {
        public string Name => "Customs";

        public void Initialize()
        {
        }

        public override void BindViewImplementation()
        {
            gameObject.SetActive(false);
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

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
    internal class CaptureFxsPCView : ViewBase<CaptureFxsVM>, IInitializable, INameable
    {
        public string Name => "Capture";
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

using FxManager.Cache;
using Kingmaker.PubSubSystem;
using Kingmaker.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxManager.UI.Events
{
    internal interface IActiveFXSelected : IGlobalSubscriber
    {
        public void OnCurrentFXSelected(UnitFxVisibilityManager activeFx);
    }
}

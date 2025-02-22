using FxManager.Cache;
using Kingmaker.Visual;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace FxManager.UI.MVVM.VMs
{
    public class ActiveFxElementVM : VirtualListElementVMBase
    {
        public ReactiveProperty<string> FxName = new ReactiveProperty<string>("");
        public ReactiveProperty<string> Description = new ReactiveProperty<string>("");
        public UnitFxVisibilityManager UnitFxVisibilityManager { get; private set; }
        public ActiveFxElementVM(UnitFxVisibilityManager visibilityManager)
        {
            SetManager(visibilityManager);
        }

        public void SetManager(UnitFxVisibilityManager visibilityManager)
        {
            UnitFxVisibilityManager = visibilityManager;
            SetProperties();
        }

        private void SetProperties()
        {
            if (UnitFxVisibilityManager == null)
            {
                FxName.Value = "null";
                Description.Value = "null";
                return;
            }

            FxName.Value = UnitFxVisibilityManager.gameObject.name;
            StringBuilder description = new StringBuilder();
            description.Append("Unit: ");
            description.Append(UnitFxVisibilityManager.m_Unit == null ? "null" : UnitFxVisibilityManager.m_Unit.Data.CharacterName);
            description.Append(" Renderers: ");
            description.Append(UnitFxVisibilityManager.m_Renderers.Count);
            description.Append(" SnapControllers: ");
            description.Append(UnitFxVisibilityManager.m_SnapControllers.Count);
            Description.Value = description.ToString();
        }


        public override void DisposeImplementation()
        {
        }
    }
}

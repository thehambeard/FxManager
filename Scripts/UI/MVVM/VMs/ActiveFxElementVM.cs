using FxManager.Fx;
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
        public readonly ActiveFxModelBase FxModel;

        public ActiveFxElementVM(ActiveFxModelBase activeFxModelBase)
        {
            FxModel = activeFxModelBase;
            SetProperties();
        }

        private void SetProperties()
        {
            FxName.Value = FxModel.Name;
            string description = FxModel.Handle.Request.Target.name;

            switch (FxModel)
            {
                case ActiveFxModelUnit unitModel:
                    description = $"Type: Unit {unitModel.Unit.Data.CharacterName} | Target: {FxModel.Handle.Request.Target.name}";
                    break;
            }

            Description.Value = description;
        }


        public override void DisposeImplementation()
        {
        }
    }
}

using Kingmaker.ResourceLinks;
using Kingmaker.View;
using Kingmaker.Visual.Particles.FxSpawnSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxManager.Fx
{
    internal class ActiveFxModelUnit : ActiveFxModelBase
    {
        public UnitEntityView Unit { get; private set; }

        public ActiveFxModelUnit(IFxHandle handle, UnitEntityView unit) : base(handle)
        {
            Unit = unit;
        }
    }
}

using Kingmaker.ResourceLinks;
using Kingmaker.Visual.Particles.FxSpawnSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FxManager.Fx
{
    public enum FxType
    {
        Unit,
        Weapon,
        Point
    }

    internal class ActiveFxModel
    {
        IFxHandle Handle { get; set; }
        FxType FxType { get; set; }

        public ActiveFxModel(IFxHandle handle, FxType fxType)
        {
            Handle = handle;
            FxType = fxType;
        }
    }
}

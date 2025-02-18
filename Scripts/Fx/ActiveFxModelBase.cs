using Kingmaker.ResourceLinks;
using Kingmaker.Visual.Particles.FxSpawnSystem;
using System;
using System.Linq;

using UniRx;

namespace FxManager.Fx
{
    public abstract class ActiveFxModelBase : IDisposable
    {
        public IFxHandle Handle { get; private set; }
        public string Name { get; }

        private readonly IDisposable _removeOnDestroy;

        public ActiveFxModelBase(IFxHandle handle)
        {
            Handle = handle;
            Name = handle.Request.Prefab.name;

            _removeOnDestroy = Observable.EveryUpdate()
                .Select(_ => handle.IsDestroyed)
                .DistinctUntilChanged()
                .Where(isDestroyed => isDestroyed)
                .Subscribe(_ => RemoveFromDictionary());
        }

        private void RemoveFromDictionary()
        {
            ActiveFxs.Remove(this);
            Dispose();
        }

        public virtual void Dispose()
        {
            _removeOnDestroy?.Dispose();
            Handle = null;
        }
    }
}

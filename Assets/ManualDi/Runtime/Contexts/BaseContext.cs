using ManualDi.Main;
using UnityEngine;

namespace ManualDi.Unity3d
{
    public abstract class BaseContext<TFacade, TData> : MonoBehaviour, IContext<TFacade, TData>, IInstaller
        where TFacade : MonoBehaviour
    {
        private bool disposedValue;

        public IDiContainer Container { get; private set; }

        public TFacade Facade { get; private set; }
        public TData Data { get; private set; }

        public GameObject GameObject => gameObject;

        public TFacade Initiate(IDiContainer parentDiContainer, TData data)
        {
            disposedValue = false;

            Data = data;

            Container = new DiContainerBuilder()
                .WithParentContainer(parentDiContainer)
                .WithInstaller(this)
                .Build();

            Facade = Container.Resolve<TFacade>();

            return Facade;
        }

        public virtual void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposedValue)
            {
                return;
            }
            disposedValue = true;

            if (Container == null)
            {
                return;
            }

            Container.Dispose();
            Container = null;

            Data = default;
            Facade = default;
        }

        public abstract void Install(IDiContainerBindings bindings);
    }
}

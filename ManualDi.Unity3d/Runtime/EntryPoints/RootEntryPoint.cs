using System;
using ManualDi.Main;
using UnityEngine;

namespace ManualDi.Unity3d
{
    public abstract class MonoBehaviourRootEntryPoint : MonoBehaviour, IInstaller, IDisposable
    {
        [field: SerializeField] public bool InitializeOnStart { get; set; }
        
        public bool IsInitialized => Container is not null;
        public IDiContainer? Container { get; private set; }
        
        public void Start()
        {
            if (!InitializeOnStart)
            {
                return;
            }
            
            Initiate();
        }

        public void Initiate()
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("Context is already initialized");
            }
            
            Container = new DiContainerBindings()
                .Install(this)
                .Build();
        }

        public virtual void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (Container is null)
            {
                return;
            }

            Container.Dispose();
            Container = null;
        }

        public abstract void Install(DiContainerBindings b);
    }
}
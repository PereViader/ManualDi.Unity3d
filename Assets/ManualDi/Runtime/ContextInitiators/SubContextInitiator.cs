using UnityEngine;

namespace ManualDi.Unity3d
{
    public class SubContextInitiator : IContextInitiator
    {
        private readonly IContextEntryPoint parentContext;

        public SubContextInitiator(IContextEntryPoint parentContext)
        {
            this.parentContext = parentContext;
        }

        public TFacade Initiate<TData, TFacade>(IContextEntryPoint<TData, TFacade> contextEntryPoint, TData data) where TFacade : MonoBehaviour
        {
            return contextEntryPoint.Initiate(parentContext.Container, data);
        }
    }
}

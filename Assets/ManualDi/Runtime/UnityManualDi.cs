using ManualDi.Main;
using UnityEngine;

namespace ManualDi.Unity3d
{
    public static class UnityManualDi
    {
        public static TFacade Instantiate<TFacade, TData>(IContext<TFacade, TData> context, TData data, Transform parent = null, IDiContainer parentDiContainer = null)
            where TFacade : MonoBehaviour
        {
            var gameObjectInstance = GameObject.Instantiate(context.GameObject, parent);
            var contextInstance = gameObjectInstance.GetComponent<IContext<TFacade, TData>>();
            return contextInstance.Initiate(parentDiContainer, data);
        }
    }
}

using ManualDi.Main;
using System;
using UnityEngine;

namespace ManualDi.Unity3d
{
    public interface IContext : IDisposable
    {
        IDiContainer Container { get; }
        GameObject GameObject { get; }
    }

    public interface IContext<TFacade, TData> : IContext
        where TFacade : MonoBehaviour
    {
        TFacade Facade { get; }

        TFacade Initiate(IDiContainer parentDiContainer, TData data);
    }
}

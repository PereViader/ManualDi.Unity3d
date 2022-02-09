using ManualDi.Main;
using System;
using UnityEngine;

namespace ManualDi.Unity3d
{
    public interface IContextEntryPoint : IDisposable
    {
        IDiContainer Container { get; }
        GameObject GameObject { get; }
    }

    public interface IContextEntryPoint<TData, TContext> : IContextEntryPoint
        where TContext : MonoBehaviour
    {
        TContext Context { get; }

        TContext Initiate(IDiContainer parentDiContainer, TData data);
    }
}

#if UNITY_EDITOR

using ManualDi.Main;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ManualDi.Unity3d.Tests
{
    public class TestMonoBehaviourInstallerStore : ScriptableObject
    {
        public List<Action<IDiContainer>> installActions = new List<Action<IDiContainer>>();
    }
}

#endif

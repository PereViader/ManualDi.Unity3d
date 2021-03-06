#if UNITY_EDITOR

using ManualDi.Main;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ManualDi.Unity3d.Tests
{
    [AddComponentMenu("")]
    public class TestContextEntryPoint : BaseContextEntryPoint<TestData, TestContext>
    {
        public InstallDelegate InstallDelegate { get; set; } = _ => { };

        public override void Install(IDiContainerBindings bindings)
        {
            InstallDelegate.Invoke(bindings);
        }
    }
}
#endif

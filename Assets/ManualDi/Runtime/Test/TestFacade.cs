#if UNITY_EDITOR

using ManualDi.Main;
using UnityEngine;

namespace ManualDi.Unity3d.Tests
{
    [AddComponentMenu("")]
    public class TestFacade : MonoBehaviour
    {
        public IDiContainer DiContainer { get; set; }
        public TestData TestData { get; set; }
    }
}

#endif

using UnityEngine;

namespace ManualDi.Unity3d.Tests.PlayMode
{
    public static class TestGameObjectUtils
    {
        public static TestMonoBehaviourInstaller CreateTestGameObjectInstaller()
        {
            var gameObjectPrefab = new GameObject();
            var component = gameObjectPrefab.AddComponent<TestMonoBehaviourInstaller>();
            component.SetAsRoot();
            return component;
        }
    }
}

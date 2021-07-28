using ManualDi.Main;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace ManualDi.Unity3d.Tests.PlayMode
{
    public class TestContainerStarterGameObjectPrefabExtensions
    {
        [UnityTest]
        public IEnumerator TestGameObjectPrefabInstallation()
        {
            var prefabInstaller = TestGameObjectUtils.CreateTestGameObjectInstaller();

            var instance = new object();
            prefabInstaller.Add(c => c.Bind<object>(b => b.FromInstance(instance)));

            Transform parent = new GameObject().transform;

            Assert.That(parent.transform.childCount, Is.EqualTo(0));

            IDiContainer container = new ContainerStarter().WithGameObjectPrefabInstaller(prefabInstaller, parentTransform: parent).Start();

            Assert.That(parent.childCount, Is.EqualTo(1));
            Assert.That(container.Resolve<object>(), Is.EqualTo(instance));

            container.Dispose();

            yield return null;

            Assert.That(parent.childCount, Is.EqualTo(0));
        }
    }
}

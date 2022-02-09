using ManualDi.Main;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace ManualDi.Unity3d.Tests.PlayMode
{
    public class TestContainerStarterGameObjectPrefabExtensions
    {
        [Test]
        public void InstantiateAndBind_PrefabContext_CreatesFacadeWithTheBindingsAndData()
        {
            var instance = new object();

            var data = new TestData();
            var context = UnityManualDiTest.Instantiate(data, b =>
            {
                b.Bind<object>().FromInstance(instance);
            });

            Assert.That(context.TestData, Is.EqualTo(data));
            Assert.That(context.DiContainer.Resolve<object>(), Is.EqualTo(instance));
        }

        [UnityTest]
        public IEnumerator Destroy_ContextInstance_DisposesContainer()
        {
            bool disposed = false;

            var context = UnityManualDiTest.Instantiate(installDelegate: b =>
            {
                b.QueueDispose(() => disposed = true);
            });

            GameObject.Destroy(context.gameObject);

            yield return null;

            Assert.That(disposed, Is.True);
        }
    }
}

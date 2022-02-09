using ManualDi.Main;
using UnityEngine;

namespace ManualDi.Unity3d.Tests.PlayMode
{
    public static class UnityManualDiTest
    {
        public static TestContext Instantiate(TestData data = null, InstallDelegate installDelegate = null, IDiContainer parentDiContainer = null)
        {
            var gameObjectPrefab = new GameObject();
            var contextEntryPoint = gameObjectPrefab.AddComponent<TestContextEntryPoint>();

            contextEntryPoint.InstallDelegate = b =>
            {
                b.Bind<TestContext>()
                    .FromNewComponent(gameObjectPrefab)
                    .Initialize((o, c) =>
                    {
                        o.DiContainer = c;
                        o.TestData = data;
                    });

                installDelegate?.Invoke(b);
            };

            return contextEntryPoint.Initiate(parentDiContainer, data);
        }
    }
}

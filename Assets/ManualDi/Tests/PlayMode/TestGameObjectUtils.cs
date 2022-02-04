using ManualDi.Main;
using UnityEngine;

namespace ManualDi.Unity3d.Tests.PlayMode
{
    public static class UnityManualDiTest
    {
        public static TestFacade Instantiate(TestData data = null, InstallDelegate installDelegate = null, IDiContainer parentDiContainer = null)
        {
            var gameObjectPrefab = new GameObject();
            var context = gameObjectPrefab.AddComponent<TestContext>();

            context.InstallDelegate = b =>
            {
                b.Bind<TestFacade>()
                    .FromNewComponent(gameObjectPrefab)
                    .Initialize((o, c) =>
                    {
                        o.DiContainer = c;
                        o.TestData = data;
                    });

                installDelegate?.Invoke(b);
            };

            return context.Initiate(parentDiContainer, data);
        }
    }
}

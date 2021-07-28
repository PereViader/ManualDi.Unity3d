using ManualDi.Main;

using UnityEngine;

namespace ManualDi.Unity3d
{
    public static class ContainerStarterGameObjectPrefabExtensions
    {
        public static IContainerStarter WithGameObjectPrefabInstaller<T>(this IContainerStarter containerStarter, T monoBehaviourInstallerPrefab, Transform parentTransform) where T : MonoBehaviourInstaller
        {
            containerStarter.WithDelegatedInstaller((IDiContainer container) =>
            {
                T monoBehaviourInstaller = GameObject.Instantiate(
                    monoBehaviourInstallerPrefab,
                    parentTransform
                    );

                container.QueueDispose(() => GameObject.Destroy(monoBehaviourInstaller.gameObject));

                return monoBehaviourInstaller;
            });
            return containerStarter;
        }
    }
}

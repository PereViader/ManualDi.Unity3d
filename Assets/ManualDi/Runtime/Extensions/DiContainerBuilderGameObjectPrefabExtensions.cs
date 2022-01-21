using ManualDi.Main;

using UnityEngine;

namespace ManualDi.Unity3d
{
    public static class DiContainerBuilderGameObjectPrefabExtensions
    {
        public static IDiContainerBuilder WithGameObjectPrefabInstaller<T>(
            this IDiContainerBuilder containerBuilder,
            T monoBehaviourInstallerPrefab,
            Transform parentTransform
            ) where T : MonoBehaviourInstaller
        {
            containerBuilder.WithInstallDelegate(b =>
            {
                T monoBehaviourInstaller = GameObject.Instantiate(
                    monoBehaviourInstallerPrefab,
                    parentTransform
                    );

                b.QueueDispose(() => GameObject.Destroy(monoBehaviourInstaller.gameObject));

                monoBehaviourInstaller.Install(b);
            });
            return containerBuilder;
        }
    }
}

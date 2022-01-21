#if UNITY_EDITOR

using ManualDi.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ManualDi.Unity3d.Tests
{
    [AddComponentMenu("")]
    public class TestMonoBehaviourInstaller : MonoBehaviourInstaller
    {
        [SerializeField] TestMonoBehaviourInstallerStore testMonoBehaviourInstallerStore = default;

        private bool isRootTest = false;

        public void SetAsRoot()
        {
            testMonoBehaviourInstallerStore = ScriptableObject.CreateInstance<TestMonoBehaviourInstallerStore>();
            isRootTest = true;
        }

        private void OnDestroy()
        {
            if (isRootTest)
            {
                ScriptableObject.Destroy(testMonoBehaviourInstallerStore);
            }
        }

        public override void Install(IDiContainerBindings bindings)
        {
            foreach (var installAction in testMonoBehaviourInstallerStore.installActions)
            {
                installAction.Invoke(bindings);
            }
        }

        public void Add(InstallDelegate installDelegate)
        {
            testMonoBehaviourInstallerStore.installActions.Add(installDelegate);
        }

        public void Add(IEnumerable<InstallDelegate> installDelegates)
        {
            testMonoBehaviourInstallerStore.installActions.AddRange(installDelegates);
        }

        public void Add(IInstaller installer)
        {
            Add(installer.Install);
        }

        public void Add(IEnumerable<IInstaller> installers)
        {
            var installDelegates = installers.Select<IInstaller, InstallDelegate>(x => x.Install);
            Add(installDelegates);
        }
    }
}

#endif

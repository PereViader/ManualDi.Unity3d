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

        public override void Install(IDiContainer container)
        {
            foreach (var installAction in testMonoBehaviourInstallerStore.installActions)
            {
                installAction.Invoke(container);
            }
        }

        public void Add(Action<IDiContainer> action)
        {
            testMonoBehaviourInstallerStore.installActions.Add(action);
        }

        public void Add(IEnumerable<Action<IDiContainer>> actions)
        {
            testMonoBehaviourInstallerStore.installActions.AddRange(actions);
        }

        public void Add(IInstaller installer)
        {
            Add(installer.Install);
        }

        public void Add(IEnumerable<IInstaller> installers)
        {
            Add(installers.Select<IInstaller, Action<IDiContainer>>(x => x.Install));
        }
    }
}

#endif

using ManualDi.Main;

using UnityEngine;


namespace Odyssey.Game.Simulation
{
    public abstract class MonoBehaviourInstaller : MonoBehaviour, IInstaller
    {
        public abstract void Install(IDiContainer container);
    }
}

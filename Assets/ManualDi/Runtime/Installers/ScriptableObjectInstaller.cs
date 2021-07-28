
using ManualDi.Main;

using UnityEngine;


namespace Odyssey.Game.Simulation
{
    public abstract class ScriptableObjectInstaller : ScriptableObject, IInstaller
    {
        public abstract void Install(IDiContainer container);
    }
}

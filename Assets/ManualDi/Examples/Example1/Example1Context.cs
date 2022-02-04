using ManualDi.Main;

namespace ManualDi.Unity3d.Examples.Example1
{
    public class Example1Context : BaseContext<Example1Facade, int>
    {
        public Example1Configuration configuration;
        public Example1Facade facade;

        public override void Install(IDiContainerBindings bindings)
        {
            bindings.Bind<Example1Facade>()
                .FromInstance(facade)
                .Initialize((o, c) => o.Init(Data, configuration));

            bindings.QueueDispose(() => UnityEngine.Debug.Log("Dispose " + Data));
        }
    }
}

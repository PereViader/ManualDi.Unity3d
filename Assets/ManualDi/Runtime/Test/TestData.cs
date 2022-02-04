#if UNITY_EDITOR

using System.Collections.Generic;

namespace ManualDi.Unity3d.Tests
{
    public class TestData
    {
        private readonly Dictionary<object, object> valueStore = new Dictionary<object, object>();

        public Y Get<T, Y>(T key)
        {
            return (Y)valueStore[key];
        }

        public void Set<T, Y>(T key, T value)
        {
            valueStore[key] = value;
        }
    }
}

#endif

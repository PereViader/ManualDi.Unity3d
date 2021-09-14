
using ManualDi.Main;

using UnityEngine;

namespace ManualDi.Unity3d
{
    public static class TypeBindingGameObjectExtensions
    {
        public static ITypeBinding<TInterface, TConcrete> FromComponentInChildren<TInterface, TConcrete>(
            this ITypeBinding<TInterface, TConcrete> typeBinding,
            GameObject gameObject
            )
            where TConcrete : Component, TInterface
        {
            typeBinding.FromMethod(c => gameObject.GetComponentInChildren<TConcrete>());

            return typeBinding;
        }

        public static ITypeBinding<TInterface[], TConcrete[]> FromComponentsInChildren<TInterface, TConcrete>(
            this ITypeBinding<TInterface[], TConcrete[]> typeBinding,
            GameObject gameObject
            )
            where TConcrete : Component, TInterface
        {
            typeBinding.FromMethod(c => gameObject.GetComponentsInChildren<TConcrete>());

            return typeBinding;
        }

        public static ITypeBinding<TInterface, TConcrete> FromComponent<TInterface, TConcrete>(
            this ITypeBinding<TInterface, TConcrete> typeBinding,
            GameObject gameObject
            )
            where TConcrete : Component, TInterface
        {
            typeBinding.FromMethod(b => gameObject.GetComponent<TConcrete>());

            return typeBinding;
        }

        public static ITypeBinding<TInterface[], TConcrete[]> FromComponents<TInterface, TConcrete>(
            this ITypeBinding<TInterface[],
                TConcrete[]> typeBinding, GameObject gameObject
            )
            where TConcrete : Component, TInterface
        {
            typeBinding.FromMethod(b => gameObject.GetComponents<TConcrete>());

            return typeBinding;
        }
    }
}

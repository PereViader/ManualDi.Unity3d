using ManualDi.Main;

using UnityEngine;

namespace ManualDi.Unity3d
{
    public static class TypeBindingGameObjectExtensions
    {
        public static ITypeBinding<T> FromComponentInChildren<T>(this ITypeBinding<T> typeBinding, GameObject gameObject)
            where T : Component
        {
            typeBinding.FromMethod(b => gameObject.GetComponentInChildren<T>());

            return typeBinding;
        }

        public static ITypeBinding<T[]> FromComponentsInChildren<T>(this ITypeBinding<T[]> typeBinding, GameObject gameObject)
            where T : Component
        {
            typeBinding.FromMethod(b => gameObject.GetComponentsInChildren<T>());

            return typeBinding;
        }

        public static ITypeBinding<T> FromComponent<T>(this ITypeBinding<T> typeBinding, GameObject gameObject)
            where T : Component
        {
            typeBinding.FromMethod(b => gameObject.GetComponent<T>());

            return typeBinding;
        }

        public static ITypeBinding<T[]> FromComponents<T>(this ITypeBinding<T[]> typeBinding, GameObject gameObject)
            where T : Component
        {
            typeBinding.FromMethod(b => gameObject.GetComponents<T>());

            return typeBinding;
        }
    }
}

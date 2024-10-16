﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ManualDi.Main
{
    public sealed class DiContainer : IDiContainer
    {
        private readonly Dictionary<IntPtr, TypeBinding> allTypeBindings;
        private readonly IDiContainer? parentDiContainer;
        private readonly BindingContext bindingContext = new();
        
        private DiContainerInitializer diContainerInitializer;
        private DiContainerDisposer diContainerDisposer;
        private TypeBinding? injectedTypeBinding;

        public DiContainer(
            Dictionary<IntPtr, TypeBinding> allTypeBindings, 
            IDiContainer? parentDiContainer,
            int? initializationsCount = null, 
            int? disposablesCount = null)
        {
            diContainerInitializer = new(initializationsCount);
            diContainerDisposer = new(disposablesCount);
            
            this.allTypeBindings = allTypeBindings;
            this.parentDiContainer = parentDiContainer;
        }

        public void Initialize()
        {
            foreach (var firstTypeBinding in allTypeBindings)
            {
                TypeBinding? typeBinding = firstTypeBinding.Value;
                while (typeBinding is not null)
                {
                    if (!typeBinding.IsLazy)
                    {
                        typeBinding.ResolveBinding(this);
                    }

                    typeBinding = typeBinding.NextTypeBinding;
                }
            }
        }

        public object? ResolveContainer(Type type)
        {
            var typeBinding = GetTypeForConstraint(type);
            if (typeBinding is not null)
            {
                return typeBinding.ResolveBinding(this);
            }

            return parentDiContainer?.ResolveContainer(type);
        }
        
        public object? ResolveContainer(Type type, FilterBindingDelegate filterBindingDelegate)
        {
            var typeBinding = GetTypeForConstraint(type, filterBindingDelegate);
            if (typeBinding is not null)
            {
                return typeBinding.ResolveBinding(this);
            }

            return parentDiContainer?.ResolveContainer(type, filterBindingDelegate);
        }

        internal object ResolveBinding<TInterface, TConcrete>(TypeBinding<TInterface, TConcrete> typeBinding)
        {
            if (typeBinding.SingleInstance is not null) //Optimization: We don't check if Scope is Single
            {
                return typeBinding.SingleInstance;
            }
            
            var previousInjectedTypeBinding = injectedTypeBinding;
            injectedTypeBinding = typeBinding;

            var instance = typeBinding.CreateConcreteDelegate!.Invoke(this) ??
                throw new InvalidOperationException($"Could not create object for {typeof(TypeBinding<TInterface, TConcrete>).FullName}");
            object objectInstance = instance;
            
            if (typeBinding.TypeScope is TypeScope.Single)
            {
                typeBinding.SingleInstance = objectInstance;
            }
            
            typeBinding.InjectionDelegate?.Invoke(instance, this);
            if (typeBinding.InitializationDelegate is not null)
            {
                diContainerInitializer.QueueInitialize(typeBinding, objectInstance);
            }
            
            if (instance is IDisposable disposable && typeBinding.TryToDispose)
            {
                QueueDispose(disposable);
            }

            injectedTypeBinding = previousInjectedTypeBinding;
            if (injectedTypeBinding is null)
            {
                diContainerInitializer.InitializeCurrentLevelQueued(this);
            }

            return objectInstance;
        }
        
        internal object ResolveBinding(UnsafeTypeBinding typeBinding)
        {
            if (typeBinding.SingleInstance is not null) //Optimization: We don't check if Scope is Single
            {
                return typeBinding.SingleInstance;
            }
            
            var previousInjectedTypeBinding = injectedTypeBinding;
            injectedTypeBinding = typeBinding;

            var instance = typeBinding.CreateConcreteDelegate!.Invoke(this) ??
                throw new InvalidOperationException($"Could not create object for UnsafeTypeBinding of ApparentType {typeBinding.ApparentType.FullName} and ConcreteType {typeBinding.ConcreteType.FullName}");
            
            if (typeBinding.TypeScope is TypeScope.Single)
            {
                typeBinding.SingleInstance = instance;
            }
            
            typeBinding.InjectionDelegate?.Invoke(instance, this);
            if (typeBinding.InitializationDelegate is not null)
            {
                diContainerInitializer.QueueInitialize(typeBinding, instance);
            }
            
            if (typeBinding.TryToDispose && instance is IDisposable disposable)
            {
                QueueDispose(disposable);
            }

            injectedTypeBinding = previousInjectedTypeBinding;
            if (injectedTypeBinding is null)
            {
                diContainerInitializer.InitializeCurrentLevelQueued(this);
            }

            return instance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TypeBinding? GetTypeForConstraint(Type type)
        {
            if (!allTypeBindings.TryGetValue(type.TypeHandle.Value, out TypeBinding? typeBinding))
            {
                return null;
            }

            if (typeBinding.FilterBindingDelegate is null)
            {
                return typeBinding;
            }

            bindingContext.InjectedIntoTypeBinding = injectedTypeBinding;
            do
            {
                bindingContext.TypeBinding = typeBinding;

                if (typeBinding.FilterBindingDelegate?.Invoke(bindingContext) ?? true)
                {
                    return typeBinding;
                }

                typeBinding = typeBinding.NextTypeBinding;
            } while (typeBinding is not null);
            
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TypeBinding? GetTypeForConstraint(Type type, FilterBindingDelegate filterBindingDelegate)
        {
            if (!allTypeBindings.TryGetValue(type.TypeHandle.Value, out TypeBinding? typeBinding))
            {
                return null;
            }

            bindingContext.InjectedIntoTypeBinding = injectedTypeBinding;
            do
            {
                bindingContext.TypeBinding = typeBinding;

                if (filterBindingDelegate.Invoke(bindingContext) &&
                    (typeBinding.FilterBindingDelegate?.Invoke(bindingContext) ?? true))
                {
                    return typeBinding;
                }

                typeBinding = typeBinding.NextTypeBinding;
            } while (typeBinding is not null);
            
            return null;
        }

        public void ResolveAllContainer(Type type, FilterBindingDelegate? filterBindingDelegate, IList resolutions)
        {
            if (allTypeBindings.TryGetValue(type.TypeHandle.Value, out TypeBinding? typeBinding))
            {
                bindingContext.InjectedIntoTypeBinding = injectedTypeBinding;
                do
                {
                    bindingContext.TypeBinding = typeBinding;

                    if ((filterBindingDelegate?.Invoke(bindingContext) ?? true) &&
                        (typeBinding.FilterBindingDelegate?.Invoke(bindingContext) ?? true))
                    {
                        resolutions.Add(typeBinding.ResolveBinding(this));
                    }

                    typeBinding = typeBinding.NextTypeBinding;
                } while (typeBinding is not null);
            }

            parentDiContainer?.ResolveAllContainer(type, filterBindingDelegate, resolutions);
        }

        public bool WouldResolveContainer(
            Type type, 
            FilterBindingDelegate? filterBindingDelegate,
            Type? overrideInjectedIntoType, 
            FilterBindingDelegate? overrideFilterBindingDelegate)
        {
            var previousInjectedTypeBinding = injectedTypeBinding;
            if (overrideInjectedIntoType is not null)
            {
                injectedTypeBinding = null;
                injectedTypeBinding = overrideFilterBindingDelegate is null 
                    ? GetTypeForConstraint(overrideInjectedIntoType) 
                    : GetTypeForConstraint(overrideInjectedIntoType, overrideFilterBindingDelegate);
            }
            
            var typeBinding = filterBindingDelegate is null 
                ? GetTypeForConstraint(type)
                : GetTypeForConstraint(type, filterBindingDelegate);
            
            injectedTypeBinding = previousInjectedTypeBinding;
            if (typeBinding is not null)
            {
                return true;
            }

            if (parentDiContainer is null)
            {
                return false;
            }

            return parentDiContainer.WouldResolveContainer(type, filterBindingDelegate, overrideInjectedIntoType, 
                overrideFilterBindingDelegate);
        }
        
        public void QueueDispose(IDisposable disposable)
        {
            diContainerDisposer.QueueDispose(disposable);
        }
        
        public void QueueDispose(Action disposableAction)
        {
            diContainerDisposer.QueueDispose(disposableAction);
        }

        public void Dispose()
        {
            diContainerDisposer.Dispose();
        }
    }
}

# ManualDi.Unity3d

# Introduction

Welcome to the ManualDi.Unity3d package. This package contains all the functionality that the ManualDi project offers for the Unity3d game engine.
For information about the main the ManualDi functionality, please visit [here](https://github.com/PereViader/ManualDi.Main)

The unity 3d game engine is a great engine to create games with, but we believe it moves away developers from the land of code driven development and more into the land of mouse driven development.

With this project, we aim to decrease the complexity of your Unity3d project, by providing adapters and helpers for Unity3d functionality so they can be used with ease with the ManualDi.Main project.

The priciples this project is based on are:
 - Understandable: A new developer should be able to quickly understand how to use it
 - Familiar: Concepts used by the container should be the same as already as existing containers
 - Fast: The container should be able to resolve the object graph quickly and efficiently
 - Natural: As developers we should be creating value and not fighting Unity3d
 - Easy: Adding functionality should not require a high mental load
 - Pluggable:  Users of the container should be able to customize the container from the outside as they wish
 - Generic: The container should not assume the needs of the user and should allow for any strategy


# Examples

To get to the action, please visit the automated tests of the project found on https://github.com/PereViader/ManualDi.Unity3d/tree/main/Assets/ManualDi/Tests

# Installation

In order to install this package you will have to 

- Go to the ManualDi.Main git repo and clone/download it on your computer
- Compile ManualDi.Main project
- Copy the ManualDi.Main.dll file created by the compilation into your project inside the `Assets/Plugins/` folder
- Add this git repo as a package on the UnityPackage manager using the following Git Url: `https://github.com/PereViader/ManualDi.Unity3d.git#upm`


# API

# Container creation

The container is created using the same Fluent api as in the ManualDi.Main project. This project adds some extension methods to it for the Unity3d specific functionality.

Let's look at some use cases.

Creating a container from a GameObject prefab by instantiating it on the scene and calling it's installer

```
[SerializeField] private SomeMonoBehaviourInstaller installer;
    
private IDiContainer container;
    
private void Awake()
{
    container = new ContainerStarter()
        .WithGameObjectPrefabInstaller(installer, parentTransform: null)
        .Start();
}
```

Creating a container from an installer that is already instantiated/does not need to be instantiated/is a scriptable object is done using regular container functionality

```
[SerializeField] private SomeInstaller installer;
    
private IDiContainer container;
    
private void Awake()
{
    container = new ContainerStarter()
        .WithInstaller(installer)
        .Start();
}
```

Remember that you can always add as many installers as you want and they will be all created. The call order should not be relevant if the installers are properly implemented.

```
[SerializeField] private SomeMonoBehaviourInstaller installer1;
[SerializeField] private SomeInstaller installer2;
    
private IDiContainer container;
    
private void Awake()
{
    container = new ContainerStarter()
        .WithGameObjectPrefabInstaller(installer1, parentTransform: null)
        .WithInstaller(installer2)
        .Start();
}
```

## Container disposal

In order to get get rid of the container you will have to call the Dipose method on it's interface `container.Dispose()`
When this is done, all the objects that were queued for disposal on the container will also have their dispose method called.

If you installed the container using `WithGameObjectPrefabInstaller` the prefab will be destroyed. 

# Installers
Installers in Unity3d come with 2 flavours

## ScriptableObjectInstallers
ScriptableObjectInstallers are installers that live on the project outside of any concrete screen. They should be used over MonoBehaviourInstallers when possible, as they do not pollute scenes as much and promote healthier project habits. 

## MonoBehaviourInstallers
MonoBehaviour installers are installers that are attached to a GameObject on a scene or a prefab. They should only be used when references inside that scene or prefab are needed. Otherwise prefer ScriptableObjectInstallers.

# Bindings

This project offers several extension methods around unity functionality

- GameObject: All GetComponent methods can be used as part of the Binding From call
  - Example: `container.Bind<SomeType>(b => b.FromComponent(this.gameObject)`
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CodeController>().AsSingle().NonLazy();
        var interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
        foreach (var interactable in interactables)
        {
            Container.Inject(interactable);
        }

        Container.Bind<TVController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<StrongboxController>().FromComponentInHierarchy().AsSingle();
        var audioVolumeChanger = FindObjectOfType<AudioVolumeChanger>();
        Container.Bind<AudioVolumeChanger>().FromInstance(audioVolumeChanger).AsSingle();
    }
}
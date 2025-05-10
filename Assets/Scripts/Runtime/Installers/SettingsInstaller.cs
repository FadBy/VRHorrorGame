using UnityEngine;
using Zenject;

public class SettingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameSettings>().AsSingle();
    }
}
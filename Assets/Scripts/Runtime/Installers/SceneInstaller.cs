using UnityEngine;
using Zenject;

namespace Runtime.Bindings
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
            foreach (var interactable in interactables)
            {
                Container.Inject(interactable);
            }
        }
    }
}
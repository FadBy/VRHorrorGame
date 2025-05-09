using System;
using Runtime;
using UnityEngine;
using Zenject;

public class PlayerControlsInstaller : MonoInstaller
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector2InputSmoother _inputSmoother;
    [SerializeField] private Transform _playerSpawnPoint;
    
    public override void InstallBindings()
    {
        Container.Bind<PlayerMovementInputReader>().To<PlayerMovementNewInputReader>().AsSingle();
        Container.Bind<CursorState>().AsSingle();
        BindPlayerInputAction();
        var obj = Container.InstantiatePrefabForComponent<PlayerMovement>(_prefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation, null);
        Container.Bind<PlayerMovement>().FromInstance(obj).AsSingle();
        Container.Bind<Inventory>().AsSingle();
        var inventoryDisplay = FindObjectOfType<InventoryDisplay>();
        Container.Bind<InventoryDisplay>().FromInstance(inventoryDisplay).AsSingle();
    }

    private void BindPlayerInputAction()
    {
        Container.Bind<PlayerInputAction>().AsSingle();
        var playerInputAction = Container.Resolve<PlayerInputAction>();
        playerInputAction.Enable();
    }
}
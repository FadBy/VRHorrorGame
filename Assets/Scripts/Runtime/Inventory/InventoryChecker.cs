using UnityEngine;
using Zenject;

public class InventoryChecker : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Item _keyItem;
    
    private Inventory _inventory;
    

    [Inject]
    public void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    public void ProceedIfChecked()
    {
        if (_inventory.ContainsItem(_keyItem))
        {
            _animator.SetTrigger("Open");
            _inventory.LoseItem(_keyItem);
        }
    }
}

using UnityEngine;
using Zenject;

public class Pickupable : MonoBehaviour
{
    [SerializeField] private Item _item;
    private Inventory _inventory;
    
    [Inject]
    public void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    public void Pickup()
    {
        _inventory.AddItem(_item);
        Destroy(gameObject);
    }
}

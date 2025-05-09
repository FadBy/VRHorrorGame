using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private InventorySlot _inventorySlot;
    private Inventory _inventory;
    
    private List<Item> _displayedItems = new List<Item>();
    private List<InventorySlot> _slots = new List<InventorySlot>();
    
    [Inject]
    public void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    private void Update()
    {
        if (AreItemsDifferent())
        {
            DisplayItems(_inventory.Items);
        }
    }

    private void ClearSlots()
    {
        _displayedItems.Clear();
        _slots.Clear();
    }

    private void DisplayItems(List<Item> items)
    {
        ClearSlots();
        foreach (var item in items)
        {
            DisplayItem(item);
        }
    }

    private void DisplayItem(Item item)
    {
        var slotObj = Instantiate(_inventorySlot, _container);
        slotObj.Display(item);
        _displayedItems.Add(item);
        _slots.Add(slotObj);
    }

    private bool AreItemsDifferent()
    {
        return !_displayedItems.SequenceEqual(_inventory.Items);
    }
}
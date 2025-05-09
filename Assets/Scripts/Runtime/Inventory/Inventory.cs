using System.Collections.Generic;
public class Inventory
{
    private List<Item> _items = new List<Item>();
    
    public List<Item> Items => _items;

    public void AddItem(Item item)
    {
        if (Items.Contains(item)) return;
        _items.Add(item); 
    }

    public void LoseItem(Item item)
    {
        if (!_items.Contains(item)) return;
        _items.Remove(item);
    }

    public bool ContainsItem(Item item)
    {
        return _items.Contains(item);
    }
}
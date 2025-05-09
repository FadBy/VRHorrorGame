using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void Display(Item item)
    {
        _image.sprite = item.Sprite;
    }
}
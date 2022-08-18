using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Item item;
    public int itemCount;
    public Image itemImage;
    public EquipmentType equipmentType;
    Rect rc;

    void Start()
    {
        rc.x = itemImage.rectTransform.position.x - itemImage.rectTransform.rect.width / 2;
        rc.y = itemImage.rectTransform.position.y + itemImage.rectTransform.rect.height / 2;

        rc.xMax = itemImage.rectTransform.rect.width;
        rc.yMax = itemImage.rectTransform.rect.height;

        rc.width = itemImage.rectTransform.rect.width;
        rc.height = itemImage.rectTransform.rect.height;
    }

    public bool IsInRect(Vector2 uiPos)
    {
        if (uiPos.x >= rc.x && uiPos.x <= rc.x + rc.width && uiPos.y >= rc.y - rc.height && uiPos.y <= rc.y) { return true; }

        return false;
    }

    public bool HasItem()
    {
        if (item != null) return true;
        return false;
    }
    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
    }

    public void AddItem(EquippableItem _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = _item.itemImage;

        SetColor(1);
    }
}

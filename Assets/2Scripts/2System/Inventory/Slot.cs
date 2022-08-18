using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour
{
    public Item item;
    public int itemCount;
    public Image itemImage;
    public TextMeshProUGUI textCount; 
    public GameObject go_Count;

    Rect rc;

    void Update()
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

        textCount.text = "0";
        go_Count.SetActive(false);
    }

    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = _item.itemImage;

        if (item.itemType != ItemType.Equipment)
        {
            if (itemCount > 1)
            {
                go_Count.SetActive(true);
                textCount.text = itemCount.ToString();
            }
            else
            {
                go_Count.SetActive(false);
                textCount.text = itemCount.ToString();
            }
        }
        else
        {
            textCount.text = "0";
            go_Count.SetActive(false);
        }

        SetColor(1);
    }

    public void SetSlotCount(int _count)
    {
        itemCount += _count;

        if (itemCount > 1)
        {
            go_Count.SetActive(true);
            textCount.text = itemCount.ToString();
        }
        else
        {
            go_Count.SetActive(false);
            textCount.text = itemCount.ToString();
        }

        if (itemCount <= 0) ClearSlot();
    }

    public void Use(UsableItem _item)
    {
        Debug.Log(_item.name + "À» »ç¿ë");
        SetSlotCount(-1);

        Player.instance.curhealth += _item.HealValue;

        if (Player.instance.curhealth > Player.instance.maxhealth)
        {
            Player.instance.curhealth = Player.instance.maxhealth;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    public Item item;
    public Image itemImage;

    [Header("Shop UI")]
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textCost;

    public bool isChoose;

    Rect rc;
    RectTransform rct;
    Image img;

    private void Start()
    {
        rct = this.GetComponent<RectTransform>();
        img = this.GetComponent<Image>();
    }

    void Update()
    {
        rc.x = rct.position.x - rct.rect.width / 2;
        rc.y = rct.position.y + rct.rect.height / 2;

        rc.xMax = rct.rect.width;
        rc.yMax = rct.rect.height;

        rc.width = rct.rect.width;
        rc.height = rct.rect.height;
    }

    public void UpdateSlotData(Item item)
    {
        itemImage.sprite = item.itemImage;
        textName.text = item.itemName;
        textDescription.text = item.itemDescription;
        textCost.text = item.itemCost.ToString();
    }

    public bool IsInRect(Vector2 uiPos)
    {
        if (uiPos.x >= rc.x && uiPos.x <= rc.x + rc.width && uiPos.y >= rc.y - rc.height && uiPos.y <= rc.y) { return true; }

        return false;
    }

    public void SetColor(float _alpha)
    {
        Color color = img.color;
        color.a = _alpha;
        img.color = color;
    }
}

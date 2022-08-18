using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;
    private void Awake()
    {
        if(Instance == null )
        {
            Instance = this;
        }
    }

    [SerializeField]
    private GameObject go_tooltip;

    [SerializeField]
    private Item item;
    [SerializeField]
    private TextMeshProUGUI itemNameText;
    [SerializeField]
    private TextMeshProUGUI itemDescriptionText;
    [SerializeField]
    private TextMeshProUGUI itemHowUseText;

    public GameObject Go_tooltop => go_tooltip;

    public void ShowItemTooltop( Item _item )
    {
        item = _item;

        if ( !go_tooltip.activeSelf )
            go_tooltip.SetActive(true);

        itemNameText.text = item.itemName;

        switch ( item.itemType )
        {
            case ItemType.Equipment:
                EquippableItem equippableItem = _item as EquippableItem;
                itemDescriptionText.text = $"+ {equippableItem.ATKBonus} ATK\n";
                itemDescriptionText.text += $"+ {equippableItem.DEFBonus} DEF\n";
                itemDescriptionText.text += $"+ {equippableItem.HPBonus} HP";

                itemHowUseText.text = "우클릭으로 장착";
                break;
            case ItemType.Used:
                UsableItem usableItem = _item as UsableItem;
                itemDescriptionText.text = $"+ {usableItem.HealValue} Heal";

                itemHowUseText.text = "우클릭으로 사용";
                break;
            case ItemType.Default:
                itemDescriptionText.text = $"{_item.itemDescription}";

                itemHowUseText.text = "";
                break;
            default:
                break;
        }
    }

    public void HideItemTooltip()
    {
        if ( go_tooltip.activeSelf )
            go_tooltip.SetActive(false);

        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemHowUseText.text = "";
    }
}

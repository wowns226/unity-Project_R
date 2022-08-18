using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Shop : MonoBehaviour,IPointerClickHandler
{
    public static bool shopActivated = false;

    #region Shop SingleTon
    public static Shop instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    [Header("Shop UI")]
    public GameObject go_shop;
    public GameObject shopBuyPanel;
    public TextMeshProUGUI itemCountText;
    public GameObject UpButton;
    public GameObject DownButton;

    [Space]
    public Item[] itemArray;
    public ShopSlot[] slots;
    [SerializeField]
    ShopSlot prevSlot;
    [SerializeField]
    ShopSlot nowSlot;
    public int buyItemCount = 1;

    private void Start()
    {
        int i = 0;
        foreach(var slot in slots)
        {
            slot.item = itemArray[i];
            slot.UpdateSlotData(slot.item);
            i++;
        }
    }

    public void OpenShop() 
    {
        go_shop.SetActive(true);
        shopActivated = true;
    }

    public void CloseShop() 
    {
        go_shop.SetActive(false);
        shopActivated = false;
    }

    private void Update()
    {
        UpdateCountUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ( eventData.button == PointerEventData.InputButton.Left )
        {
            ItemTooltip tooltip = ItemTooltip.Instance;

            for ( int i = 0 ; i < slots.Length ; i++ )
            {
                if ( slots[i].IsInRect(eventData.position) )
                {
                    tooltip.ShowItemTooltop(slots[i].item);
                }
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsInRect(eventData.position))
            {
                nowSlot = slot;
                if (prevSlot != null)
                {
                    prevSlot.SetColor(1f);
                }

                Debug.Log($"{slot.item.itemName}을 선택하였습니다.");
                slot.SetColor(0.5f);
                shopBuyPanel.SetActive(true);
                buyItemCount = 1;

                prevSlot = slot;

                if (slot.item.itemType == ItemType.Equipment)
                {
                    UpButton.SetActive(false);
                    DownButton.SetActive(false);
                }
                else
                {
                    UpButton.SetActive(true);
                    DownButton.SetActive(true);
                }

                
            }
        }
        
    }

    public void UpdateCountUI()
    {
        if (shopBuyPanel.activeSelf)
        {
            itemCountText.text = buyItemCount.ToString();
        }
    }

    public void BuyItem(ShopSlot slot, int count)
    {
        int price = slot.item.itemCost * count;
        if (Inventory.instance.playerCoin < price)
        {
            NotifyText.Instance.SetText($"<color=red>{price - Inventory.instance.playerCoin}</color> 코인이 부족합니다");

        }
        else
        {
            Inventory.instance.playerCoin -= price;
            Inventory.instance.AcquireItem(slot.item, count);
        }
    }

    public void OnClickedCountUp()
    {
        buyItemCount++;
    }

    public void OnClickedCountDown()
    {
        if(buyItemCount == 1)
        {
            return;
        }

        buyItemCount--;
    }

    public void OnClickedBuyButton()
    {
        BuyItem(nowSlot, buyItemCount);
        prevSlot.SetColor(1f);
        prevSlot = null;

        shopBuyPanel.SetActive(false);
        ItemTooltip.Instance.Go_tooltop.SetActive(false);
    }

    public void OnClickedCancelButton()
    {
        buyItemCount = 1;
        shopBuyPanel.SetActive(false);
        prevSlot.SetColor(1f);
        prevSlot = null;
    }
}
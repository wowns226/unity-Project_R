using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Inventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public static bool inventoryActivated = false;
    
    #region Inventory Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }
    #endregion

    

    [SerializeField] private GameObject go_inventory;
    public GameObject Go_Inventory
    {
        get { return go_inventory; }
    }
    [SerializeField] private GameObject go_slotsParent;
    [SerializeField] private GameObject go_ItemAcquireText;
    [SerializeField] private TextMeshProUGUI itemAcquireText;
    private int previousSlotNum;
    public Slot[] slots;

    [Header("플레이어의 코인")]
    [SerializeField] private TextMeshProUGUI playerCoinText;
    public int playerCoin;

    [SerializeField]
    private Slot previousSlot;
    [SerializeField]
    private Slot quickSlot;

    private void OnValidate()
    {
        slots = go_slotsParent.GetComponentsInChildren<Slot>();
    }

    private void Start()
    {
        playerCoin = 0;
    }

    private void Update()
    {
        TryOpenInventory();
        UpdateCoin();
    }

    private void UpdateCoin()
    {
        playerCoinText.text = $"{playerCoin}";
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !go_inventory.activeSelf;
            if (inventoryActivated) OpenInventory();
            else CloseInventory();
        }
    }
    public void OpenInventory() 
    {
        inventoryActivated = true;
        go_inventory.SetActive(true); 
    }

    public void CloseInventory() 
    {
        inventoryActivated = false;
        ItemTooltip.Instance.Go_tooltop.SetActive(false);
        go_inventory.SetActive(false); 
    }

    private void ChangeSlot(Slot _toSlot)
    {
        Item _tempItem = _toSlot.item;
        int _tempItemCount = _toSlot.itemCount;

        _toSlot.AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    private void ChangeSlot(Slot _toSlot, Slot _fromSlot )
    {
        Item tempSlotItem = _toSlot.item;
        int tempSlotItemCount = _toSlot.itemCount;

        _toSlot.ClearSlot();
        _toSlot.AddItem(_fromSlot.item, _fromSlot.itemCount);

        _fromSlot.ClearSlot();
        _fromSlot.AddItem(tempSlotItem, tempSlotItemCount);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if (ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }

            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }

    public int GetItemCount(Item _item)
    {
        int itemCount = 0;
        foreach(var slot in slots)
        {
            if(slot.item!=null && slot.item.name.Equals(_item.name))
            {
                itemCount += slot.itemCount;
            }
        }

        return itemCount;
    }

    public Slot GetSlot(Item _item )
    {
        foreach(var slot in slots )
        {
            if ( slot.item.itemName.Equals(_item.itemName) )
            {
                return slot;
            }
        }

        return null;
    }

    public int PlusCoin( int coin )
    {
        return playerCoin += coin;
    }

    public int MinusCoin( int coin )
    {
        return playerCoin -= coin;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsInRect(eventData.position) && slots[i].HasItem())
                {
                    Debug.Log(i);
                    Debug.Log(slots[i].item.itemName);

                    previousSlotNum = i;
                    previousSlot = slots[i];
                    slots[i].SetColor(0.3f);

                    DragSlot.instance.dragSlot = slots[i];
                    DragSlot.instance.SetColor(1f);
                    DragSlot.instance.SetDragImage(slots[i].itemImage);
                    DragSlot.instance.transform.position = eventData.position;
                }
            }
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DragSlot.instance.transform.position = eventData.position;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            slots[previousSlotNum].SetColor(1f);

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsInRect(eventData.position))
                {
                    Debug.Log(i);
                    if (i != previousSlotNum)
                    {
                        if (slots[i].HasItem())
                        {
                            if ( slots[i].item == slots[previousSlotNum].item )
                            {
                                if( slots[i].item.itemType != ItemType.Equipment )
                                {
                                    slots[i].SetSlotCount(slots[i].itemCount + slots[previousSlotNum].itemCount);

                                    slots[previousSlotNum].SetSlotCount(0);
                                    slots[previousSlotNum].ClearSlot();
                                }
                            }
                            else
                            {
                                ChangeSlot(slots[i]);
                            }
                        }
                        else
                        {
                            ChangeSlot(slots[i]);
                        }
                    }

                }
            }

            DragSlot.instance.dragSlot = null;
            DragSlot.instance.SetColor(0f);
        }
        ItemTooltip.Instance.Go_tooltop.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsInRect(eventData.position))
                {
                    if (slots[i].HasItem())
                    {
                        if (slots[i].item.itemType == ItemType.Equipment)
                        {
                            EquippableItem previousEquip;
                            EquippableItem tempItem = (EquippableItem)slots[i].item;


                            for (int j = 0; j < Equipment.instance.equipmentSlots.Length; j++)
                            {
                                if (Equipment.instance.equipmentSlots[j].equipmentType == tempItem.equipmentType)
                                {
                                    if (!Equipment.instance.equipmentSlots[j].HasItem())
                                    {
                                        Equipment.instance.equipmentSlots[j].AddItem(tempItem);
                                        slots[i].ClearSlot();
                                        Equipment.instance.Equip(tempItem);
                                    }
                                    else
                                    {
                                        previousEquip = (EquippableItem)Equipment.instance.equipmentSlots[j].item;

                                        Equipment.instance.equipmentSlots[j].AddItem(tempItem);
                                        slots[i].AddItem(previousEquip);
                                        Equipment.instance.Equip(tempItem);
                                        Equipment.instance.UnEquip(previousEquip);
                                    }
                                }
                            }
                        }
                        else if (slots[i].item.itemType == ItemType.Used)
                        {
                            slots[i].Use((UsableItem)slots[i].item);

                            if ( slots[i].item == quickSlot.item )
                            {
                                quickSlot.itemCount -= 1;
                            }
                        }
                    }
                }
            }

            ItemTooltip.Instance.Go_tooltop.SetActive(false);
        }

        else if( eventData.button == PointerEventData.InputButton.Left )
        {
            ItemTooltip tooltip = ItemTooltip.Instance;

            for ( int i = 0 ; i < slots.Length ; i++ )
            {
                if ( slots[i].IsInRect(eventData.position) )
                {
                    if ( slots[i].HasItem() )
                    {
                        previousSlot = slots[i];

                        tooltip.ShowItemTooltop(slots[i].item);                        
                    }
                }
            }
        }
    }

    public void SetQuickSlot()
    {
        if ( previousSlot.HasItem() )
        {
            if ( previousSlot.item.itemType == ItemType.Used )
            {
                if ( quickSlot.HasItem() )
                {
                    ChangeSlot(quickSlot, previousSlot);
                }
                else
                {
                    quickSlot.SetColor(1f);
                    quickSlot.AddItem(previousSlot.item, previousSlot.itemCount);
                    previousSlot.ClearSlot();
                }
            }


            else if ( previousSlot.item.itemType == ItemType.Equipment )
            {
                EquippableItem previousEquip;
                EquippableItem tempItem = (EquippableItem)previousSlot.item;

                for ( int j = 0 ; j < Equipment.instance.equipmentSlots.Length ; j++ )
                {
                    if ( Equipment.instance.equipmentSlots[j].equipmentType == tempItem.equipmentType )
                    {
                        if ( !Equipment.instance.equipmentSlots[j].HasItem() )
                        {
                            Equipment.instance.equipmentSlots[j].AddItem(tempItem);
                            previousSlot.ClearSlot();
                            Equipment.instance.Equip(tempItem);
                        }
                        else
                        {
                            previousEquip = (EquippableItem)Equipment.instance.equipmentSlots[j].item;

                            Equipment.instance.equipmentSlots[j].AddItem(tempItem);
                            previousSlot.AddItem(previousEquip);
                            Equipment.instance.Equip(tempItem);
                            Equipment.instance.UnEquip(previousEquip);
                        }
                    }
                }
            }
        }

        ItemTooltip.Instance.Go_tooltop.SetActive(false);
    }
}

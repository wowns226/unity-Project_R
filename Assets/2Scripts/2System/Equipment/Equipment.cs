using UnityEngine;
using UnityEngine.EventSystems;

public class Equipment : MonoBehaviour, IPointerClickHandler
{
    public static bool equipmentActivated = false;

    #region EquipmentSingleton
    public static Equipment instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    

    [SerializeField] private GameObject go_equipment;
    [SerializeField] private GameObject go_equipmentSlotsParent;
    public EquipmentSlot[] equipmentSlots;

    private void OnValidate()
    {
        equipmentSlots = go_equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }



    private void Update()
    {
        TryOpenEquipment();
    }

    private void TryOpenEquipment()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            equipmentActivated = !go_equipment.activeSelf;
            if (equipmentActivated) OpenEquipment();
            else CloseEquipment();
        }
    }

    public void OpenEquipment() 
    {
        equipmentActivated = true;
        go_equipment.SetActive(true); 
    }

    public void CloseEquipment() 
    {
        equipmentActivated = false;
        go_equipment.SetActive(false);
    }

    public void Equip(EquippableItem _item)
    {
        Weapon.instance.attackdamage += _item.ATKBonus;
        Player.instance.Defense += _item.DEFBonus;
        Player.instance.maxhealth += _item.HPBonus;
    }

    public void UnEquip(EquippableItem _item)
    {
        Weapon.instance.attackdamage -= _item.ATKBonus;
        Player.instance.Defense -= _item.DEFBonus;
        Player.instance.maxhealth -= _item.HPBonus;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].IsInRect(eventData.position) && equipmentSlots[i].HasItem())
                {
                    for (int j = 0; j < Inventory.instance.slots.Length; j++)
                    {
                        if (!Inventory.instance.slots[j].HasItem())
                        {
                            Inventory.instance.slots[j].AddItem(equipmentSlots[i].item);
                            UnEquip((EquippableItem)equipmentSlots[i].item);
                            equipmentSlots[i].ClearSlot();
                            return;
                        }
                    }
                }
            }
        }
    }
}

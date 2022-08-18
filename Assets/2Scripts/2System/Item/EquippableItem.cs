using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Head,
    Armor,
    Shoes,
    Weapon,
    Ring,
    NeckLace
}

[CreateAssetMenu(fileName = "EquippableItem", menuName = "Assets/Items/Equipment Item")]
public class EquippableItem : Item
{
    public int ATKBonus;
    public int DEFBonus;
    public int HPBonus;
    public int CriBouns;
    public EquipmentType equipmentType;
}

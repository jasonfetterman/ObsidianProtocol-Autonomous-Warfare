using UnityEngine;

public enum EquipSlot
{
    Weapon,
    Armor,
    Helmet,
    Boots,
    Accessory
}

[System.Serializable]
public class EquippedItem
{
    public EquipSlot slot;
    public EquipmentItem item;
}

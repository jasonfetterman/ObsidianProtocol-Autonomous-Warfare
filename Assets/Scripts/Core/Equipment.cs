using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour
{
    public List<EquippedItem> equipped = new();

    public void Equip(EquipmentItem item, EquipSlot slot)
    {
        for (int i = 0; i < equipped.Count; i++)
        {
            if (equipped[i].slot == slot)
            {
                equipped[i].item = item;
                return;
            }
        }

        equipped.Add(new EquippedItem { slot = slot, item = item });
    }

    public float GetBonusHealth()
    {
        float total = 0f;
        foreach (var e in equipped)
            if (e.item != null)
                total += e.item.bonusHealth;
        return total;
    }

    public float GetBonusDamage()
    {
        float total = 0f;
        foreach (var e in equipped)
            if (e.item != null)
                total += e.item.bonusDamage;
        return total;
    }

    public float GetBonusArmor()
    {
        float total = 0f;
        foreach (var e in equipped)
            if (e.item != null)
                total += e.item.bonusArmor;
        return total;
    }

    public float GetBonusRange()
    {
        float total = 0f;
        foreach (var e in equipped)
            if (e.item != null)
                total += e.item.bonusRange;
        return total;
    }
}

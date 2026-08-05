using UnityEngine;

public class EquipmentUpgrade : MonoBehaviour
{
    public void ApplyUpgrade(Equipment equip, UpgradeItem upgrade)
    {
        foreach (var e in equip.equipped)
        {
            if (e.item == null) continue;

            e.item.bonusHealth += upgrade.bonusHealth;
            e.item.bonusDamage += upgrade.bonusDamage;
            e.item.bonusArmor += upgrade.bonusArmor;
            e.item.bonusRange += upgrade.bonusRange;
        }
    }
}


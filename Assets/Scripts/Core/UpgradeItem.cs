using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "RTS/Upgrade Item")]
public class UpgradeItem : ScriptableObject
{
    public string upgradeName;

    public float bonusHealth;
    public float bonusDamage;
    public float bonusArmor;
    public float bonusRange;
}

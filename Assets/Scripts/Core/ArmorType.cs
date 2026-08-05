using UnityEngine;

public enum ArmorClass
{
    Light,
    Medium,
    Heavy
}

public class ArmorType : MonoBehaviour
{
    public ArmorClass armorClass = ArmorClass.Light;

    public float ApplyArmor(float damage)
    {
        switch (armorClass)
        {
            case ArmorClass.Light:
                return damage * 0.8f;   // 20% reduction

            case ArmorClass.Medium:
                return damage * 0.6f;   // 40% reduction

            case ArmorClass.Heavy:
                return damage * 0.4f;   // 60% reduction

            default:
                return damage;
        }
    }
}

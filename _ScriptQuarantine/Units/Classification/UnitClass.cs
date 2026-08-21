using UnityEngine;

public enum UnitClassType
{
    Infantry,
    Sniper,
    Heavy,
    Flamethrower
}

public class UnitClass : MonoBehaviour
{
    public UnitClassType classType = UnitClassType.Infantry;

    public float GetDamageMultiplier()
    {
        switch (classType)
        {
            case UnitClassType.Infantry:
                return 1f;     // baseline

            case UnitClassType.Sniper:
                return 2.5f;   // high damage

            case UnitClassType.Heavy:
                return 1.5f;   // moderate damage

            case UnitClassType.Flamethrower:
                return 0.6f;   // low direct damage, high DOT
        }

        return 1f;
    }

    public float GetRangeBonus()
    {
        switch (classType)
        {
            case UnitClassType.Sniper:
                return 20f;

            case UnitClassType.Heavy:
                return 5f;

            case UnitClassType.Flamethrower:
                return -5f;

            default:
                return 0f;
        }
    }
}

using UnityEngine;

public class DamageResolver : MonoBehaviour
{
    public float BulletResist = 0.1f;        // 10% reduction
    public float ExplosiveResist = 0.3f;     // 30% reduction
    public float FireResist = 0.0f;          // no reduction
    public float ArmorPierceResist = -0.2f;  // -20% = 20% extra damage

    public float ApplyType(DamageClass type, float dmg)
    {
        switch (type)
        {
            case DamageClass.Kinetic:
                return dmg - (dmg * BulletResist);

            case DamageClass.Explosive:
                return dmg - (dmg * ExplosiveResist);

            case DamageClass.Fire:
                return dmg - (dmg * FireResist);

            case DamageClass.ArmorPiercing:
                return dmg - (dmg * ArmorPierceResist);

            default:
                return dmg;
        }
    }
}

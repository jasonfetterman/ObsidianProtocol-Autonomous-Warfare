using UnityEngine;

public abstract class WeaponFramework
{
    public string WeaponName;
    public float BaseDamage;
    public float Range;
    public float Cooldown;

    protected float lastFireTime = -999f;

    public virtual void Fire(Vector3 origin, Vector3 target)
    {
        if (!CanFire())
        {
            Debug.Log($"{WeaponName} cannot fire yet — cooldown active.");
            return;
        }

        Debug.Log($"{WeaponName} fired from {origin} toward {target}.");
        lastFireTime = Time.time;
    }

    public bool CanFire()
    {
        return Time.time >= lastFireTime + Cooldown;
    }
}

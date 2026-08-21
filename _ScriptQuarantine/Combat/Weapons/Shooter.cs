using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject muzzleFlashPrefab;

    public float baseRange = 10f;
    public float fireRate = 1f;
    public float baseDamage = 10f;
    public DamageClass damageClass = DamageClass.Kinetic;

    UnitClass unitClass;
    UnitXP xp;
    Equipment equipment;
    BuffSystem buff;

    float nextFireTime;

    void Awake()
    {
        unitClass = GetComponent<UnitClass>();
        xp = GetComponent<UnitXP>();
        equipment = GetComponent<Equipment>();
        buff = GetComponent<BuffSystem>();
    }

    public void TryShoot(GameObject target)
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > baseRange) return;

        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;

        float dmg = baseDamage;

        if (unitClass != null)
            dmg *= unitClass.GetDamageMultiplier();

        if (xp != null)
            dmg *= xp.GetDamageMultiplier();

        if (equipment != null)
            dmg += equipment.GetBonusDamage();

        if (buff != null)
            dmg *= buff.currentDamageMultiplier;

        Vector3 targetPos = target.transform.position;

        if (muzzleFlashPrefab != null)
            Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);

        GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        p.GetComponent<Projectile>().Init(targetPos, dmg, damageClass, gameObject);
    }
}

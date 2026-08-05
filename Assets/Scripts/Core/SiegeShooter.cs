using UnityEngine;

public class SiegeShooter : MonoBehaviour
{
    public GameObject projectilePrefab;

    public float baseRange = 35f;
    public float fireRate = 3f;
    public float baseDamage = 80f;
    public float splashRadius = 6f;

    float nextFireTime;

    public void TryShoot(GameObject target)
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > baseRange) return;

        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        Vector3 targetPos = target.transform.position;

        GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        BallisticProjectile bp = p.GetComponent<BallisticProjectile>();
        bp.splashRadius = splashRadius;
        bp.Init(targetPos, baseDamage, DamageClass.Explosive, gameObject);
    }
}

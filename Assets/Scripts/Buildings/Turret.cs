using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range = 15f;
    public float fireRate = 1f;
    public float damage = 15f;

    float nextFireTime;

    void Update()
    {
        GameObject target = FindClosestEnemy();
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > range) return;

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Health h = target.GetComponent<Health>();
            if (h != null)
                h.TakeDamage(damage, DamageClass.Kinetic, gameObject);
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < closestDist)
            {
                closestDist = d;
                closest = e;
            }
        }

        return closest;
    }
}

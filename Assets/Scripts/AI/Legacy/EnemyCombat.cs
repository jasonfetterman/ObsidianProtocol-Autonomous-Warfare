using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float attackRange = 10f;
    public float fireRate = 1f;
    public float damage = 10f;

    float nextFireTime;

    void Update()
    {
        GameObject target = FindClosestPlayer();

        if (target == null)
            return;

        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist <= attackRange && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Health hp = target.GetComponent<Health>();
            if (hp != null)
                hp.TakeDamage(damage, DamageClass.Kinetic, gameObject);
        }
    }

    GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            float d = Vector3.Distance(transform.position, p.transform.position);

            if (d < closestDist)
            {
                closestDist = d;
                closest = p;
            }
        }

        return closest;
    }
}

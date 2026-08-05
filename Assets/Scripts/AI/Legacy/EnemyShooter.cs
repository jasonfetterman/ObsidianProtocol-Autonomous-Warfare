using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject muzzleFlashPrefab;

    public float attackRange = 10f;
    public float fireRate = 1f;
    public float damage = 10f;

    float nextFireTime;

    void Update()
    {
        GameObject target = FindClosestPlayer();
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > attackRange) return;

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Vector3 targetPos = target.transform.position;

            if (muzzleFlashPrefab != null)
                Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);

            GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            p.GetComponent<Projectile>().Init(
                targetPos,
                damage,
                DamageClass.Kinetic,
                gameObject
            );
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

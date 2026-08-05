using UnityEngine;

public class ShooterBallistic : MonoBehaviour
{
    public GameObject projectilePrefab;

    public float baseRange = 25f;
    public float fireRate = 1.5f;
    public float baseDamage = 40f;

    public float missChance = 0.1f;
    public float inaccuracyRadius = 2f;

    float nextFireTime;

    void Update() { }

    public void TryShoot(GameObject target)
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > baseRange) return;

        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        Vector3 targetPos = target.transform.position;

        if (Random.value < missChance)
        {
            targetPos += new Vector3(
                Random.Range(-inaccuracyRadius, inaccuracyRadius),
                0,
                Random.Range(-inaccuracyRadius, inaccuracyRadius)
            );
        }

        GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        BallisticProjectile bp = p.GetComponent<BallisticProjectile>();
        bp.Init(targetPos, baseDamage, DamageClass.Explosive, gameObject);
    }
}

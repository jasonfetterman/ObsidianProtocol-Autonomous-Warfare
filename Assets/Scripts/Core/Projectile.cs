using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 40f;
    public float damage = 10f;
    public float lifeTime = 3f;

    public DamageClass damageClass = DamageClass.Kinetic;

    public GameObject hitEffectPrefab;
    public GameObject bloodEffectPrefab;
    public GameObject bloodDecalPrefab;

    Vector3 targetPos;
    GameObject attacker;

    public void Init(Vector3 target, float dmg, DamageClass type, GameObject owner)
    {
        targetPos = target;
        damage = dmg;
        damageClass = type;
        attacker = owner;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            SpawnImpactFX();

            Health hp = GetComponentInParent<Health>();
            if (hp != null)
                hp.TakeDamage(damage, damageClass, attacker);

            Destroy(gameObject);
        }
    }

    void SpawnImpactFX()
    {
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        if (bloodEffectPrefab != null)
            Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);

        if (bloodDecalPrefab != null)
        {
            Vector3 decalPos = transform.position;
            decalPos.y = 0.01f;
            Instantiate(bloodDecalPrefab, decalPos, Quaternion.identity);
        }
    }
}

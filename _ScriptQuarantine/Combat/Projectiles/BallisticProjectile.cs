using UnityEngine;

public class BallisticProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float gravity = -9.81f;
    public float splashRadius = 3f;
    public float damage = 20f;
    public DamageClass damageClass = DamageClass.Explosive;

    public Vector3 targetPos;
    public GameObject attacker;

    Vector3 velocity;
    bool initialized = false;

    ExplosionAudio explosionAudio;

    void Start()
    {
        explosionAudio = GetComponent<ExplosionAudio>();

        if (!initialized)
            Destroy(gameObject);
    }

    public void Init(Vector3 target, float dmg, DamageClass type, GameObject atk)
    {
        attacker = atk;
        targetPos = target;
        damage = dmg;
        damageClass = type;

        Vector3 dir = (targetPos - transform.position);
        float dist = dir.magnitude;

        float time = dist / speed;

        velocity = dir.normalized * speed;
        velocity.y = (dir.y / time) - (0.5f * gravity * time);

        initialized = true;
    }

    void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            Explode();
        }
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, splashRadius);

        foreach (var h in hits)
        {
            Health health = h.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(damage, damageClass, attacker);
        }

        if (explosionAudio != null)
            explosionAudio.PlayExplosion();

        Destroy(gameObject);
    }
}

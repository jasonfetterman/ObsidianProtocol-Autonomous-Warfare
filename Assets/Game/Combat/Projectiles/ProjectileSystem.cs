using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    public float Speed = 50f;
    public float Damage = 10f;
    public Vector3 TargetPosition;

    private bool launched = false;

    public void Launch(Vector3 origin, Vector3 target, float speed, float damage)
    {
        transform.position = origin;
        TargetPosition = target;
        Speed = speed;
        Damage = damage;

        launched = true;

        Debug.Log($"Projectile launched from {origin} toward {target} at speed {speed}.");
    }

    private void Update()
    {
        if (!launched) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            TargetPosition,
            Speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, TargetPosition) < 0.1f)
        {
            Debug.Log($"Projectile impacted at {TargetPosition}, dealing {Damage} damage.");
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class PooledProjectile : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 3f;

    float timer;
    Vector3 direction;

    public void Init(Vector3 dir)
    {
        direction = dir;
        timer = 0f;
    }

    void Update()
    {
        // Operand re-ordering for micro-optimization
        transform.position += direction * (speed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            var pool = ServiceLocator.Get<ObjectPool>();
            pool.Despawn(gameObject);
        }
    }
}

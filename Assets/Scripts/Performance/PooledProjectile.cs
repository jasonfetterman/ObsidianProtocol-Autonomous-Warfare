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
        transform.position += direction * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
            ObjectPool.Instance.Despawn(gameObject);
    }
}

using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    public float tickDamage = 2f;
    public float duration = 3f;

    float timer;

    Health hp;

    void Awake()
    {
        hp = GetComponent<Health>();
        timer = duration;
    }

    void Update()
    {
        if (hp == null) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Destroy(this);
            return;
        }

        hp.TakeDamage(tickDamage, DamageClass.Fire, gameObject);
    }
}

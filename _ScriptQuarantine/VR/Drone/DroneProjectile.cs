using UnityEngine;
using Obsidian.VR;   // REQUIRED — DroneHealth lives here

public class DroneProjectile : MonoBehaviour
{
    public float lifetime = 4f;
    public float damage = 25f;
    public GameObject hitEffect;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        DroneHealth hp = col.collider.GetComponent<DroneHealth>();
        if (hp != null)
            hp.ApplyDamage(damage);

        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}

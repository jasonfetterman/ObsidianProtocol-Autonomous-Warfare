using UnityEngine;

public class ImpactFX : MonoBehaviour
{
    public float lifeTime = 0.3f;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
}

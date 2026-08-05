using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float lifeTime = 0.2f;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
}

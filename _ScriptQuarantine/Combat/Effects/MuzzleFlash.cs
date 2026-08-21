using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public float lifeTime = 0.05f;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
}

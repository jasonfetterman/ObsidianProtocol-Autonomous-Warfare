using UnityEngine;

public class BloodDecal : MonoBehaviour
{
    public float lifeTime = 10f;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
}

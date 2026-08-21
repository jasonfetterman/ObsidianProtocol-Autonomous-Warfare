using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public float currentDamageMultiplier = 1f;

    float buffEndTime = 0f;

    void Update()
    {
        if (Time.time > buffEndTime)
            currentDamageMultiplier = 1f;
    }

    public void ApplyBuff(float duration, float multiplier)
    {
        currentDamageMultiplier = multiplier;
        buffEndTime = Time.time + duration;
    }
}

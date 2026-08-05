using UnityEngine;

public class TickThrottler : MonoBehaviour
{
    public float interval = 0.5f;
    float nextTick;

    public System.Action onTick;

    void Update()
    {
        if (Time.time < nextTick) return;
        nextTick = Time.time + interval;

        onTick?.Invoke();
    }
}

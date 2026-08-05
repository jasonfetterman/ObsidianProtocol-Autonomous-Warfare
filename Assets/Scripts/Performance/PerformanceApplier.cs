using UnityEngine;

public class PerformanceApplier : MonoBehaviour
{
    [SerializeField] private AIBatchProcessor processor;
    [SerializeField] private float newTickRate = 0.1f;

    private void Start()
    {
        if (processor != null)
        {
            processor.TickRate = newTickRate;
        }
    }
}

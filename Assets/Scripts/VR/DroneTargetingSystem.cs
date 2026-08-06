using UnityEngine;

public class DroneTargetingSystem : MonoBehaviour
{
    public float lockRange = 800f;
    public float lockAngle = 25f;
    public float lockTime = 1.2f;

    public Transform CurrentTarget { get; private set; }

    private float lockTimer;

    void Update()
    {
        AcquireTarget();
    }

    private void AcquireTarget()
    {
        Transform best = null;
        float bestScore = Mathf.Infinity;

        Collider[] hits = Physics.OverlapSphere(transform.position, lockRange);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Drone"))
                continue;

            if (hit.transform == transform)
                continue;

            Vector3 dir = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dir);

            if (angle > lockAngle)
                continue;

            float dist = Vector3.Distance(transform.position, hit.transform.position);

            if (dist < bestScore)
            {
                bestScore = dist;
                best = hit.transform;
            }
        }

        if (best == null)
        {
            CurrentTarget = null;
            lockTimer = 0f;
            return;
        }

        if (CurrentTarget != best)
        {
            CurrentTarget = best;
            lockTimer = 0f;
        }

        lockTimer += Time.deltaTime;

        if (lockTimer >= lockTime)
        {
            // Target locked
        }
    }
}

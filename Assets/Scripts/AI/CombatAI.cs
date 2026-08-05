using UnityEngine;

public partial class CombatAI : MonoBehaviour
{
    public float visionRange = 20f;
    public float flankOffset = 3f;
    public float retreatThreshold = 0.25f; // retreat at 25% HP

    UnitMover mover;
    Shooter shooter;
    Health health;
    ThreatLevel threatEval;

    GameObject currentTarget;

    void Awake()
    {
        mover = GetComponent<UnitMover>();
        shooter = GetComponent<Shooter>();
        health = GetComponent<Health>();
        threatEval = GetComponent<ThreatLevel>();
    }

    void Update()
    {
        if (health == null || shooter == null || mover == null || threatEval == null)
            return;

        if (currentTarget != null && health.currentHealth <= health.baseHealth * retreatThreshold)
        {
            Retreat();
            return;
        }

        AcquireTarget();

        if (currentTarget != null)
            EngageTarget();
    }

    void AcquireTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float bestThreat = -Mathf.Infinity;
        GameObject bestTarget = null;

        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist > visionRange) continue;

            Health eh = e.GetComponent<Health>();
            Shooter es = e.GetComponent<Shooter>();

            float threat = threatEval.CalculateThreat(eh, es);

            if (threat > bestThreat)
            {
                bestThreat = threat;
                bestTarget = e;
            }
        }

        currentTarget = bestTarget;
    }

    void EngageTarget()
    {
        if (currentTarget == null) return;

        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (dist < shooter.baseRange * 0.5f)
        {
            Vector3 flankPos = currentTarget.transform.position + (transform.right * flankOffset);
            mover.MoveTo(flankPos);
        }
        else if (dist > shooter.baseRange)
        {
            mover.MoveTo(currentTarget.transform.position);
        }
        else
        {
            mover.MoveTo(transform.position);
        }

        shooter.TryShoot(currentTarget);
    }

    void Retreat()
    {
        if (currentTarget == null) return;

        Vector3 retreatDir = (transform.position - currentTarget.transform.position).normalized;
        Vector3 retreatPos = transform.position + retreatDir * 10f;

        mover.MoveTo(retreatPos);
    }
}

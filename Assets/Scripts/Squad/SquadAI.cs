using UnityEngine;
using System.Collections.Generic;

public class SquadAI : MonoBehaviour
{
    public List<CombatAI> members = new();
    public float regroupRadius = 5f;
    public float leaderSearchRadius = 20f;
    public float attackCommandRange = 30f;

    CombatAI leader;

    void Awake()
    {
        AssignLeader();
    }

    void Update()
    {
        if (leader == null)
        {
            AssignLeader();
            return;
        }

        RegroupMembers();
        SyncTargets();
    }

    void AssignLeader()
    {
        float bestHP = -Mathf.Infinity;
        CombatAI best = null;

        foreach (var m in members)
        {
            if (m == null) continue;

            Health h = m.GetComponent<Health>();
            if (h != null && h.currentHealth > bestHP)
            {
                bestHP = h.currentHealth;
                best = m;
            }
        }

        leader = best;
    }

    void RegroupMembers()
    {
        foreach (var m in members)
        {
            if (m == null || m == leader) continue;

            float dist = Vector3.Distance(m.transform.position, leader.transform.position);

            if (dist > regroupRadius)
            {
                UnitMover mover = m.GetComponent<UnitMover>();
                if (mover != null)
                    mover.MoveTo(leader.transform.position);
            }
        }
    }

    void SyncTargets()
    {
        CombatAI leaderAI = leader;
        if (leaderAI == null) return;

        GameObject target = leaderAI.GetCurrentTarget();
        if (target == null) return;

        foreach (var m in members)
        {
            if (m == null) continue;

            float dist = Vector3.Distance(m.transform.position, target.transform.position);
            if (dist > attackCommandRange) continue;

            m.SetForcedTarget(target);
        }
    }
}


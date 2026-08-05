using UnityEngine;
using System.Collections.Generic;

public class EnemyAttackManager : MonoBehaviour
{
    public float attackInterval = 20f;
    float nextAttackTime;

    public Transform playerBase;

    public void AttackTick(EnemyUnitManager unitManager)
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackInterval;

        SendAttackWave(unitManager.units);
    }

    void SendAttackWave(List<GameObject> units)
    {
        foreach (var u in units)
        {
            if (u == null) continue;

            UnitMover mover = u.GetComponent<UnitMover>();
            if (mover != null)
                mover.MoveTo(playerBase.position);

            CombatAI ai = u.GetComponent<CombatAI>();
            if (ai != null)
                ai.SetForcedTarget(null); // clear target so they engage enemies on the way
        }
    }
}

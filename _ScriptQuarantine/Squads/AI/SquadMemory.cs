using UnityEngine;

public class SquadMemory
{
    // Last enemy the squad attacked
    public GameObject LastAttackTarget { get; private set; }

    // Last known enemy position
    public Vector3? LastEnemyPosition { get; private set; }

    // Last move target issued by SquadController
    public Vector3? LastMoveTarget { get; private set; }

    // Last formation type used
    public SquadAI.FormationType LastFormation { get; private set; }

    // Time since last enemy interaction
    public float TimeSinceEnemySeen { get; private set; }

    public void Tick(float deltaTime)
    {
        TimeSinceEnemySeen += deltaTime;
    }

    public void SetAttackTarget(GameObject target)
    {
        LastAttackTarget = target;
        LastEnemyPosition = target != null ? target.transform.position : null;
        TimeSinceEnemySeen = 0f;
    }

    public void SetMoveTarget(Vector3 pos)
    {
        LastMoveTarget = pos;
    }

    public void SetFormation(SquadAI.FormationType type)
    {
        LastFormation = type;
    }
}


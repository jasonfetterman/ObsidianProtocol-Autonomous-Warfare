using UnityEngine;

public class SquadIntent
{
    public enum IntentType
    {
        None,
        Move,
        Attack,
        Formation
    }

    public IntentType CurrentIntent { get; private set; } = IntentType.None;

    public Vector3? MoveTarget { get; private set; }
    public GameObject AttackTarget { get; private set; }
    public SquadAI.FormationType Formation { get; private set; } = SquadAI.FormationType.None;

    // -----------------------------
    // MOVE INTENT
    // -----------------------------
    public void SetMoveIntent(Vector3 target)
    {
        CurrentIntent = IntentType.Move;
        MoveTarget = target;
        AttackTarget = null;
        Formation = SquadAI.FormationType.None;
    }

    // -----------------------------
    // ATTACK INTENT
    // -----------------------------
    public void SetAttackIntent(GameObject target)
    {
        CurrentIntent = IntentType.Attack;
        AttackTarget = target;
        MoveTarget = null;
        Formation = SquadAI.FormationType.None;
    }

    // -----------------------------
    // FORMATION INTENT
    // -----------------------------
    public void SetFormationIntent(SquadAI.FormationType type)
    {
        CurrentIntent = IntentType.Formation;
        Formation = type;
        MoveTarget = null;
        AttackTarget = null;
    }

    public void ClearFormationIntent()
    {
        if (CurrentIntent == IntentType.Formation)
            CurrentIntent = IntentType.None;

        Formation = SquadAI.FormationType.None;
    }

    // -----------------------------
    // CLEAR ALL
    // -----------------------------
    public void ClearAll()
    {
        CurrentIntent = IntentType.None;
        MoveTarget = null;
        AttackTarget = null;
        Formation = SquadAI.FormationType.None;
    }
}

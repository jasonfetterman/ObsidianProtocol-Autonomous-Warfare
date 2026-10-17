using UnityEngine;
using ObsidianProtocol.Game.Command;

[RequireComponent(typeof(UnitAutonomy))]
public sealed class WardenCommandReceiver : MonoBehaviour
{
    private UnitAutonomy autonomy;

    private void Awake()
    {
        autonomy = GetComponent<UnitAutonomy>();

        Debug.Log(
            "[WARDEN COMMAND] RECEIVER READY // " +
            gameObject.name);
    }

    public void ReceiveIntent(IntentType intent)
    {
        if (autonomy == null)
        {
            Debug.LogError(
                "[WARDEN COMMAND] UnitAutonomy missing on " +
                gameObject.name);

            return;
        }

        UnitAutonomy.AutonomousState state =
            UnitAutonomy.AutonomousState.Patrol;

        switch (intent)
        {
            case IntentType.Move:
                state = UnitAutonomy.AutonomousState.Pursue;
                break;

            case IntentType.Attack:
                state = UnitAutonomy.AutonomousState.Engage;
                break;

            case IntentType.Defend:
                state = UnitAutonomy.AutonomousState.Idle;
                break;

            case IntentType.Recon:
                state = UnitAutonomy.AutonomousState.Investigate;
                break;

            case IntentType.Pursue:
                state = UnitAutonomy.AutonomousState.Pursue;
                break;

            case IntentType.Retreat:
                state = UnitAutonomy.AutonomousState.Retreat;
                break;

            case IntentType.Flank:
                state = UnitAutonomy.AutonomousState.Pursue;
                break;

            case IntentType.Suppress:
                state = UnitAutonomy.AutonomousState.Engage;
                break;

            case IntentType.Breach:
                state = UnitAutonomy.AutonomousState.Pursue;
                break;

            case IntentType.Reinforce:
                state = UnitAutonomy.AutonomousState.Investigate;
                break;
        }

        autonomy.SetAutonomousIntent(
            state,
            transform.position);

        Debug.Log(
            "[WARDEN COMMAND] INTENT RECEIVED // " +
            gameObject.name +
            " // " +
            intent +
            " ? " +
            state);
    }
}

using UnityEngine;
using UnityEngine.AI;
using ObsidianProtocol.Game.Command;

public class AutonomousVehicle : MonoBehaviour
{
    [SerializeField]
    private SquadIntentController squadIntent;

    [SerializeField]
    private Transform reconTarget;

    [SerializeField]
    private Transform attackTarget;

    [SerializeField]
    private Transform defendPosition;

    private NavMeshAgent agent;
    private IntentType lastIntent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (squadIntent == null)
            squadIntent = GetComponent<SquadIntentController>();

        if (squadIntent == null)
            squadIntent = GetComponentInChildren<SquadIntentController>();
    }

    private void Start()
    {
        if (squadIntent != null && squadIntent.CurrentIntent != null)
        {
            lastIntent = squadIntent.CurrentIntent.Type;
        }
    }

    private void Update()
    {
        if (squadIntent == null)
            return;

        Intent currentIntent = squadIntent.CurrentIntent;

        if (currentIntent == null)
            return;

        if (lastIntent != currentIntent.Type)
        {
            lastIntent = currentIntent.Type;

            Debug.Log(
                $"{gameObject.name} received intent: {lastIntent}"
            );
        }

        switch (currentIntent.Type)
        {
            case IntentType.Move:
                Move(currentIntent);
                break;

            case IntentType.Attack:
                Attack(currentIntent);
                break;

            case IntentType.Defend:
                Defend(currentIntent);
                break;

            case IntentType.Recon:
                Recon(currentIntent);
                break;

            case IntentType.Flank:
                Flank(currentIntent);
                break;

            case IntentType.Suppress:
                Suppress(currentIntent);
                break;

            case IntentType.Breach:
                Breach(currentIntent);
                break;

            case IntentType.Pursue:
                Pursue(currentIntent);
                break;

            case IntentType.Retreat:
                Retreat(currentIntent);
                break;

            case IntentType.Reinforce:
                Reinforce(currentIntent);
                break;
        }
    }

    private void Move(Intent intent)
    {
        if (agent == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(intent.Position);
    }

    private void Attack(Intent intent)
    {
        if (agent == null || attackTarget == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(attackTarget.position);
    }

    private void Defend(Intent intent)
    {
        if (agent == null)
            return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    private void Recon(Intent intent)
    {
        if (agent == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(intent.Position);
    }

    private void Flank(Intent intent)
    {
        if (agent == null || attackTarget == null)
            return;

        Vector3 direction =
            transform.position - attackTarget.position;

        if (direction.sqrMagnitude < 0.01f)
            direction = transform.right;

        Vector3 flankPosition =
            attackTarget.position +
            direction.normalized * 10f +
            Vector3.Cross(
                Vector3.up,
                direction.normalized
            ) * 10f;

        agent.isStopped = false;
        agent.SetDestination(flankPosition);
    }

    private void Suppress(Intent intent)
    {
        if (agent == null || attackTarget == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(attackTarget.position);
    }

    private void Breach(Intent intent)
    {
        if (agent == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(intent.Position);
    }

    private void Pursue(Intent intent)
    {
        if (agent == null || attackTarget == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(attackTarget.position);
    }

    private void Retreat(Intent intent)
    {
        if (agent == null || attackTarget == null)
            return;

        Vector3 direction =
            (transform.position - attackTarget.position).normalized;

        Vector3 retreatPosition =
            transform.position + direction * 20f;

        agent.isStopped = false;
        agent.SetDestination(retreatPosition);
    }

    private void Reinforce(Intent intent)
    {
        if (agent == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(intent.Position);
    }
}

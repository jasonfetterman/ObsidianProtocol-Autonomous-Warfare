using UnityEngine;
using UnityEngine.AI;

public class AutonomousVehicle : MonoBehaviour
{
    [SerializeField]
    private SquadIntent squad;

    [SerializeField]
    private Transform reconTarget;

    private NavMeshAgent agent;
    private IntentType lastIntent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (squad != null)
        {
            lastIntent = squad.CurrentIntent;
        }
    }

    private void Update()
    {
        if (squad == null)
            return;

        if (lastIntent != squad.CurrentIntent)
        {
            lastIntent = squad.CurrentIntent;

            Debug.Log($"{gameObject.name} received intent: {lastIntent}");
        }

        switch (squad.CurrentIntent)
        {
            case IntentType.Hold:
                Hold();
                break;

            case IntentType.Recon:
                Recon();
                break;

            case IntentType.Attack:
                Attack();
                break;

            case IntentType.Defend:
                Defend();
                break;
        }
    }

    private void Hold()
    {
        agent.isStopped = true;
    }

    private void Recon()
    {
        if (reconTarget == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(reconTarget.position);
    }

    private void Attack()
    {
    }

    private void Defend()
    {
    }
}

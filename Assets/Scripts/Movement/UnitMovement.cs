
using UnityEngine;
using UnityEngine.AI;
using ObsidianProtocol.Game.Command.Objectives;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent == null || agent.pathPending)
            return;

        if (agent.hasPath &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            ObjectiveSystem.ActiveInstance.CheckMoveObjectives(this);
        }
    }

    public void MoveTo(Vector3 destination)
    {
        if (agent != null)
            agent.SetDestination(destination);
    }
}

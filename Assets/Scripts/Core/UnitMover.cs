using UnityEngine;
using UnityEngine.AI;

public class UnitMover : MonoBehaviour
{
    public NavMeshAgent agent;

    public float stoppingDistance = 1f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = true;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }

    public void MoveTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    public bool IsMoving()
    {
        return agent.remainingDistance > agent.stoppingDistance;
    }
}

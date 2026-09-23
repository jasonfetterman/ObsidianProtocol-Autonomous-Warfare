using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitAutonomy : MonoBehaviour
{
    public enum AutonomousState
    {
        Idle,
        Patrol,
        Investigate,
        Pursue,
        Engage,
        Retreat
    }

    [Header("Autonomous Behavior")]
    [SerializeField] private AutonomousState state = AutonomousState.Patrol;
    [SerializeField] private float decisionInterval = 0.5f;
    [SerializeField] private float detectionRadius = 30f;
    [SerializeField] private float patrolRadius = 20f;
    [SerializeField] private float stoppingDistance = 3f;

    [Header("Combat")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float engagementDistance = 12f;

    [Header("Runtime")]
    [SerializeField] private Transform currentTarget;
    [SerializeField] private Vector3 currentDestination;

    private NavMeshAgent agent;
    private Vector3 homePosition;
    private float nextDecisionTime;

    public AutonomousState State => state;
    public Transform CurrentTarget => currentTarget;
    public Vector3 CurrentDestination => currentDestination;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        homePosition = transform.position;

        agent.stoppingDistance = stoppingDistance;
    }

    private void Update()
    {
        if (Time.time < nextDecisionTime)
            return;

        nextDecisionTime = Time.time + decisionInterval;

        Think();
    }

    private void Think()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        FindThreat();

        if (currentTarget != null)
        {
            float distance =
                Vector3.Distance(
                    transform.position,
                    currentTarget.position);

            if (distance <= engagementDistance)
            {
                state = AutonomousState.Engage;
                agent.ResetPath();
                return;
            }

            state = AutonomousState.Pursue;
            agent.SetDestination(currentTarget.position);
            currentDestination = currentTarget.position;
            return;
        }

        switch (state)
        {
            case AutonomousState.Idle:
                agent.ResetPath();
                break;

            case AutonomousState.Patrol:
                Patrol();
                break;

            case AutonomousState.Investigate:
                MoveToObjective();
                break;

            case AutonomousState.Pursue:
                MoveToObjective();
                break;

            case AutonomousState.Engage:
                agent.ResetPath();
                break;

            case AutonomousState.Retreat:
                MoveToObjective();
                break;
        }
    }

    private void FindThreat()
    {
        currentTarget = null;

        Collider[] contacts =
            Physics.OverlapSphere(
                transform.position,
                detectionRadius);

        float closestDistance = float.MaxValue;

        foreach (Collider contact in contacts)
        {
            if (contact.gameObject == gameObject)
                continue;

            if (!contact.CompareTag(enemyTag))
                continue;

            float distance =
                Vector3.Distance(
                    transform.position,
                    contact.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = contact.transform;
            }
        }
    }

    private void Patrol()
    {
        if (agent.hasPath && !agent.isStopped)
        {
            if (agent.remainingDistance > stoppingDistance)
                return;
        }

        Vector3 randomPoint =
            homePosition +
            Random.insideUnitSphere * patrolRadius;

        randomPoint.y = homePosition.y;

        if (NavMesh.SamplePosition(
            randomPoint,
            out NavMeshHit hit,
            patrolRadius,
            NavMesh.AllAreas))
        {
            currentDestination = hit.position;
            agent.SetDestination(hit.position);
        }
    }

    private void MoveToObjective()
    {
        if (Vector3.Distance(
                transform.position,
                currentDestination) <= stoppingDistance)
        {
            return;
        }

        agent.SetDestination(currentDestination);
    }

    public void SetAutonomousIntent(
        AutonomousState newState,
        Vector3 destination)
    {
        state = newState;
        currentDestination = destination;
        currentTarget = null;

        if (agent != null && agent.isOnNavMesh)
            agent.SetDestination(destination);

        Debug.Log(
            "[AUTONOMY] " +
            gameObject.name +
            " received ARCHIVE intent: " +
            newState);
    }

    public void SetTarget(Transform target)
    {
        currentTarget = target;

        if (currentTarget != null)
            state = AutonomousState.Pursue;
    }

    public void ReleaseCommandControl()
    {
        state = AutonomousState.Patrol;
        currentTarget = null;

        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();
    }

    public void ClearIntent()
    {
        currentTarget = null;

        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();

        state = AutonomousState.Patrol;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRadius);

        Gizmos.DrawWireSphere(
            Application.isPlaying
                ? homePosition
                : transform.position,
            patrolRadius);
    }
}

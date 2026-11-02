using UnityEngine;
using UnityEngine.AI;

namespace ObsidianProtocol.Game.Units
{
    [DisallowMultipleComponent]
    public sealed class WaypointPatrolNPC : MonoBehaviour
    {
        [Header("Patrol Route")]
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private bool loopRoute = true;
        [SerializeField] private bool reverseAtEnd = true;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float turnSpeed = 180f;
        [SerializeField] private float arrivalDistance = 1.5f;
        [SerializeField] private float waitAtPoint = 2f;

        private NavMeshAgent agent;
        private int targetIndex;
        private int direction = 1;
        private float waitTimer;
        private bool waiting;
        private bool routeFinished;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            if (agent == null)
                agent = gameObject.AddComponent<NavMeshAgent>();

            agent.speed = moveSpeed;
            agent.angularSpeed = turnSpeed;
            agent.stoppingDistance = arrivalDistance;
            agent.autoBraking = true;
        }

        private void Start()
        {
            if (patrolPoints == null || patrolPoints.Length == 0)
            {
                Debug.LogWarning(
                    "[NPC PATROL] Assign patrol points to " + name, this);
                enabled = false;
                return;
            }

            if (!agent.isOnNavMesh)
            {
                Debug.LogError(
                    "[NPC PATROL] " + name +
                    " is not on a baked NavMesh. Bake the patrol area's NavMesh.",
                    this);
                enabled = false;
                return;
            }

            targetIndex = FindNextValidPoint(0);
            MoveToTarget();
        }

        private void Update()
        {
            if (routeFinished || agent == null || !agent.isOnNavMesh)
                return;

            if (waiting)
            {
                waitTimer -= Time.deltaTime;

                if (waitTimer <= 0f)
                {
                    waiting = false;
                    AdvanceTarget();
                    if (!routeFinished)
                        MoveToTarget();
                }

                return;
            }

            if (agent.pathPending)
                return;

            if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
                return;

            if (agent.remainingDistance <=
                agent.stoppingDistance + 0.1f)
            {
                agent.isStopped = true;
                waiting = true;
                waitTimer = waitAtPoint;
            }
        }

        private void MoveToTarget()
        {
            if (routeFinished || !agent.isOnNavMesh)
                return;

            Transform point = patrolPoints[targetIndex];

            if (point == null)
            {
                AdvanceTarget();
                if (!routeFinished)
                    MoveToTarget();
                return;
            }

            agent.isStopped = false;

            if (!agent.SetDestination(point.position))
            {
                Debug.LogWarning(
                    "[NPC PATROL] Could not set destination for " + name,
                    this);
            }
        }

        private void AdvanceTarget()
        {
            if (patrolPoints.Length <= 1)
            {
                if (loopRoute)
                    targetIndex = FindNextValidPoint(0);
                else
                    routeFinished = true;

                return;
            }

            int next = targetIndex + direction;

            if (next >= patrolPoints.Length || next < 0)
            {
                if (reverseAtEnd)
                {
                    direction *= -1;
                    next = targetIndex + direction;
                }
                else if (loopRoute)
                {
                    next = next >= patrolPoints.Length
                        ? 0
                        : patrolPoints.Length - 1;
                }
                else
                {
                    routeFinished = true;
                    agent.isStopped = true;
                    return;
                }
            }

            targetIndex = FindNextValidPoint(next);
        }

        private int FindNextValidPoint(int start)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                int index = (start + i) % patrolPoints.Length;

                if (patrolPoints[index] != null)
                    return index;
            }

            routeFinished = true;
            Debug.LogWarning(
                "[NPC PATROL] No valid patrol points assigned to " + name,
                this);
            return 0;
        }

        private void OnDrawGizmosSelected()
        {
            if (patrolPoints == null)
                return;

            Gizmos.color = Color.cyan;

            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] == null)
                    continue;

                Gizmos.DrawWireSphere(patrolPoints[i].position, 0.5f);

                int next = i + 1;

                if (next < patrolPoints.Length &&
                    patrolPoints[next] != null)
                {
                    Gizmos.DrawLine(
                        patrolPoints[i].position,
                        patrolPoints[next].position);
                }
                else if (loopRoute && patrolPoints.Length > 1 &&
                         patrolPoints[0] != null)
                {
                    Gizmos.DrawLine(
                        patrolPoints[i].position,
                        patrolPoints[0].position);
                }
            }
        }
    }
}

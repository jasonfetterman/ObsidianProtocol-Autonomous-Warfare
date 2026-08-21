using UnityEngine;
using UnityEngine.AI;

namespace Obsidian.VR
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitMover : MonoBehaviour
    {
        private NavMeshAgent agent;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Called when player right-clicks a point on the terrain
        public void MoveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
        }
    }
}

using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Ground
{
    public sealed class GroundNavigationAgent : MonoBehaviour
    {
        [SerializeField] private GroundNavigationDefinition definition;

        private Vector3 destination;
        private bool hasDestination;

        public GroundNavigationDefinition Definition => definition;
        public bool HasDestination => hasDestination;
        public Vector3 Destination => destination;

        public void SetDestination(Vector3 target)
        {
            destination = target;
            hasDestination = true;
        }

        public void ClearDestination()
        {
            hasDestination = false;
        }

        public bool HasReachedDestination()
        {
            if (!hasDestination)
            {
                return true;
            }

            float stoppingDistance =
                definition != null
                    ? definition.StoppingDistance
                    : 1f;

            return Vector3.Distance(
                transform.position,
                destination) <= stoppingDistance;
        }
    }
}

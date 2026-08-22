using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Air
{
    public sealed class AirNavigationAgent : MonoBehaviour
    {
        [SerializeField] private AirNavigationDefinition definition;

        private Vector3 destination;
        private bool hasDestination;

        public AirNavigationDefinition Definition => definition;
        public bool HasDestination => hasDestination;
        public Vector3 Destination => destination;

        public void SetDestination(Vector3 target)
        {
            destination = target;

            if (definition != null)
            {
                destination.y = Mathf.Clamp(
                    destination.y,
                    definition.MinimumAltitude,
                    definition.MaximumAltitude);
            }

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
                    : 2f;

            return Vector3.Distance(
                transform.position,
                destination) <= stoppingDistance;
        }
    }
}

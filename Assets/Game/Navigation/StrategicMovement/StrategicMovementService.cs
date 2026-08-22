using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.StrategicMovement
{
    public sealed class StrategicMovementService : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float destinationTolerance = 1f;

        public float MovementSpeed =>
            Mathf.Max(0f, movementSpeed);

        public float DestinationTolerance =>
            Mathf.Max(0.01f, destinationTolerance);

        public Vector3 GetMovementDirection(
            Vector3 currentPosition,
            Vector3 destination)
        {
            Vector3 direction =
                destination - currentPosition;

            direction.y = 0f;

            if (direction.sqrMagnitude <=
                DestinationTolerance * DestinationTolerance)
            {
                return Vector3.zero;
            }

            return direction.normalized;
        }

        public Vector3 GetNextPosition(
            Vector3 currentPosition,
            Vector3 destination,
            float deltaTime)
        {
            Vector3 direction =
                GetMovementDirection(
                    currentPosition,
                    destination);

            if (direction == Vector3.zero)
            {
                return currentPosition;
            }

            return Vector3.MoveTowards(
                currentPosition,
                destination,
                MovementSpeed *
                Mathf.Max(0f, deltaTime));
        }

        public bool HasReachedDestination(
            Vector3 currentPosition,
            Vector3 destination)
        {
            return Vector3.Distance(
                currentPosition,
                destination) <=
                DestinationTolerance;
        }
    }
}

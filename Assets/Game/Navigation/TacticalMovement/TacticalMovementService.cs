using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.TacticalMovement
{
    public sealed class TacticalMovementService : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 8f;
        [SerializeField] private float turnSpeed = 8f;
        [SerializeField] private float destinationTolerance = 1f;

        public float MovementSpeed =>
            Mathf.Max(0f, movementSpeed);

        public float TurnSpeed =>
            Mathf.Max(0f, turnSpeed);

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
                DestinationTolerance *
                DestinationTolerance)
            {
                return Vector3.zero;
            }

            return direction.normalized;
        }

        public Vector3 GetTacticalDirection(
            Vector3 currentPosition,
            Vector3 destination,
            Vector3 avoidanceDirection,
            float avoidanceWeight)
        {
            Vector3 movementDirection =
                GetMovementDirection(
                    currentPosition,
                    destination);

            avoidanceDirection.y = 0f;

            if (avoidanceDirection.sqrMagnitude <=
                0.0001f)
            {
                return movementDirection;
            }

            avoidanceDirection.Normalize();

            Vector3 result = Vector3.Lerp(
                movementDirection,
                avoidanceDirection,
                Mathf.Clamp01(avoidanceWeight));

            result.y = 0f;

            return result.sqrMagnitude > 0.0001f
                ? result.normalized
                : movementDirection;
        }

        public Quaternion GetTargetRotation(
            Transform unitTransform,
            Vector3 movementDirection,
            float deltaTime)
        {
            if (unitTransform == null ||
                movementDirection.sqrMagnitude <=
                0.0001f)
            {
                return unitTransform != null
                    ? unitTransform.rotation
                    : Quaternion.identity;
            }

            Quaternion targetRotation =
                Quaternion.LookRotation(
                    movementDirection.normalized,
                    Vector3.up);

            return Quaternion.RotateTowards(
                unitTransform.rotation,
                targetRotation,
                TurnSpeed *
                90f *
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

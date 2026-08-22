using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.NavigationRecovery
{
    public sealed class NavigationRecoveryService : MonoBehaviour
    {
        [SerializeField] private float recoveryDistance = 10f;
        [SerializeField] private float stuckTimeThreshold = 3f;
        [SerializeField] private float movementThreshold = 0.1f;

        private Vector3 lastPosition;
        private float stationaryTime;

        public float RecoveryDistance =>
            Mathf.Max(1f, recoveryDistance);

        public float StuckTimeThreshold =>
            Mathf.Max(0.1f, stuckTimeThreshold);

        private void Awake()
        {
            lastPosition = transform.position;
        }

        public bool UpdateRecoveryState(float deltaTime)
        {
            float movement =
                Vector3.Distance(
                    transform.position,
                    lastPosition);

            if (movement < movementThreshold)
            {
                stationaryTime +=
                    Mathf.Max(0f, deltaTime);
            }
            else
            {
                stationaryTime = 0f;
            }

            lastPosition = transform.position;

            return stationaryTime >=
                   StuckTimeThreshold;
        }

        public Vector3 GetRecoveryDestination(
            Vector3 currentPosition,
            Vector3 intendedDestination)
        {
            Vector3 direction =
                intendedDestination -
                currentPosition;

            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.0001f)
            {
                direction = Vector3.forward;
            }

            direction.Normalize();

            Vector3 recoveryDirection =
                Vector3.Cross(
                    Vector3.up,
                    direction);

            return currentPosition +
                   recoveryDirection *
                   RecoveryDistance;
        }

        public void ResetRecovery()
        {
            stationaryTime = 0f;
            lastPosition = transform.position;
        }
    }
}

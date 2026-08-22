using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Commands
{
    public sealed class MovementCommand : MonoBehaviour
    {
        [SerializeField] private MovementCommandDefinition definition;

        private Vector3 targetPosition;
        private bool isActive;

        public MovementCommandDefinition Definition => definition;
        public Vector3 TargetPosition => targetPosition;
        public bool IsActive => isActive;

        public void Issue(Vector3 target)
        {
            targetPosition = target;
            isActive = true;
        }

        public void Cancel()
        {
            isActive = false;
        }

        public bool HasReachedTarget(Vector3 currentPosition)
        {
            if (!isActive)
            {
                return true;
            }

            float acceptanceRadius =
                definition != null
                    ? definition.AcceptanceRadius
                    : 1f;

            return Vector3.Distance(
                currentPosition,
                targetPosition) <= acceptanceRadius;
        }
    }
}

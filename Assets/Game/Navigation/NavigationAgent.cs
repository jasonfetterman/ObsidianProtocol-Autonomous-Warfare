using UnityEngine;

namespace ObsidianProtocol.Game.Navigation
{
    public sealed class NavigationAgent : MonoBehaviour
    {
        private NavigationRequest? currentRequest;

        public bool HasDestination => currentRequest.HasValue;
        public Vector3 Destination =>
            currentRequest.HasValue
                ? currentRequest.Value.Destination
                : transform.position;

        public float AcceptanceRadius =>
            currentRequest.HasValue
                ? currentRequest.Value.AcceptanceRadius
                : 0f;

        public void SetDestination(NavigationRequest request)
        {
            currentRequest = request;
        }

        public void ClearDestination()
        {
            currentRequest = null;
        }

        public bool HasReachedDestination()
        {
            if (!currentRequest.HasValue)
            {
                return true;
            }

            return Vector3.Distance(
                transform.position,
                currentRequest.Value.Destination) <=
                currentRequest.Value.AcceptanceRadius;
        }
    }
}

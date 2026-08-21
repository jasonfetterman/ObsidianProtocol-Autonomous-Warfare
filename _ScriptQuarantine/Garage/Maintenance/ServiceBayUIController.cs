using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class ServiceBayUIController : MonoBehaviour
    {
        [Header("Provider")]
        [SerializeField]
        private ServiceBayUIProvider provider;

        [Header("Current Bay")]
        [SerializeField]
        private ServiceBayUIState currentState;

        public ServiceBayUIState CurrentState =>
            currentState;

        public ServiceBayUIState Refresh(
            string bayId)
        {
            if (provider == null)
            {
                Debug.LogWarning(
                    "ServiceBayUIController: Provider is not assigned.");

                return null;
            }

            currentState =
                provider.BuildState(bayId);

            return currentState;
        }

        public string GetStatus()
        {
            if (currentState == null)
                return "NO BAY";

            return currentState.Status;
        }

        public bool IsAvailable()
        {
            return currentState != null &&
                   currentState.available;
        }

        public bool IsOccupied()
        {
            return currentState != null &&
                   currentState.occupied;
        }

        public float GetServiceProgress()
        {
            return currentState != null
                ? currentState.serviceProgress
                : 0f;
        }

        public float GetInspectionProgress()
        {
            return currentState != null
                ? currentState.inspectionProgress
                : 0f;
        }

        public void Clear()
        {
            currentState = null;
        }
    }
}

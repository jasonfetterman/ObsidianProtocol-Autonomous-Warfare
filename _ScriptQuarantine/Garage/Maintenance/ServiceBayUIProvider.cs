using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class ServiceBayUIProvider : MonoBehaviour
    {
        [SerializeField]
        private ServiceBayManager serviceBayManager;

        public ServiceBayUIState BuildState(
            string bayId)
        {
            if (serviceBayManager == null)
                return null;

            ServiceBayState bay =
                serviceBayManager.GetBay(bayId);

            if (bay == null)
                return null;

            return new ServiceBayUIState
            {
                bayId = bay.bayId,
                bayName = bay.bayName,
                unitInstanceId = bay.activeUnitInstanceId,
                occupied = bay.occupied,
                servicing = bay.servicing,
                inspecting = bay.inspecting,
                available = bay.available,
                serviceProgress = bay.serviceProgress,
                inspectionProgress = bay.inspectionProgress
            };
        }
    }
}

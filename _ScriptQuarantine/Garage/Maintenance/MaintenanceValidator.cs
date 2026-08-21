using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceValidator : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private MaintenanceManager maintenanceManager;

        [SerializeField]
        private InspectionManager inspectionManager;

        [SerializeField]
        private DeploymentReadinessManager readinessManager;

        [SerializeField]
        private ServiceBayManager serviceBayManager;

        public bool Validate()
        {
            bool valid = true;

            if (maintenanceManager == null)
            {
                Debug.LogWarning(
                    "MaintenanceValidator: MaintenanceManager missing.");

                valid = false;
            }

            if (inspectionManager == null)
            {
                Debug.LogWarning(
                    "MaintenanceValidator: InspectionManager missing.");

                valid = false;
            }

            if (readinessManager == null)
            {
                Debug.LogWarning(
                    "MaintenanceValidator: DeploymentReadinessManager missing.");

                valid = false;
            }

            if (serviceBayManager == null)
            {
                Debug.LogWarning(
                    "MaintenanceValidator: ServiceBayManager missing.");

                valid = false;
            }

            if (valid)
            {
                Debug.Log(
                    "GARAGE MAINTENANCE SYSTEM VERIFIED.");
            }
            else
            {
                Debug.LogWarning(
                    "GARAGE MAINTENANCE SYSTEM NEEDS CHECKING.");
            }

            return valid;
        }
    }
}

using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceEventDispatcher : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private MaintenanceManager maintenanceManager;

        [SerializeField]
        private InspectionManager inspectionManager;

        public void CheckMaintenance(
            string unitInstanceId)
        {
            if (maintenanceManager == null)
                return;

            if (maintenanceManager.RequiresMaintenance(
                    unitInstanceId))
            {
                MaintenanceEvents.RaiseMaintenanceRequired(
                    unitInstanceId);
            }
        }

        public void CompleteMaintenance(
            string unitInstanceId)
        {
            MaintenanceEvents.RaiseMaintenanceCompleted(
                unitInstanceId);
        }

        public void CompleteInspection(
            string unitInstanceId)
        {
            if (inspectionManager == null)
                return;

            MaintenanceInspection inspection =
                inspectionManager.Get(unitInstanceId);

            if (inspection == null)
                return;

            MaintenanceEvents.RaiseInspectionCompleted(
                unitInstanceId);

            if (inspection.passed)
            {
                MaintenanceEvents.RaiseDeploymentReady(
                    unitInstanceId);
            }
        }
    }
}

using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceServiceController : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private MaintenanceManager maintenanceManager;

        [SerializeField]
        private InspectionManager inspectionManager;

        [SerializeField]
        private DeploymentReadinessManager readinessManager;

        public void ServiceUnit(
            string unitInstanceId,
            float repairAmount)
        {
            if (maintenanceManager == null)
                return;

            maintenanceManager.Repair(
                unitInstanceId,
                repairAmount);

            RefreshReadiness(unitInstanceId);
        }

        public void InspectUnit(
            string unitInstanceId)
        {
            if (inspectionManager == null)
                return;

            MaintenanceInspection inspection =
                inspectionManager.Inspect(unitInstanceId);

            if (inspection == null)
                return;

            if (readinessManager != null)
            {
                readinessManager.SetInspectionPassed(
                    unitInstanceId,
                    inspection.passed);

                readinessManager.SetMaintenanceReady(
                    unitInstanceId,
                    !inspection.requiresRepair);
            }
        }

        public void RefreshReadiness(
            string unitInstanceId)
        {
            if (maintenanceManager == null ||
                readinessManager == null)
                return;

            bool ready =
                !maintenanceManager.RequiresMaintenance(
                    unitInstanceId);

            readinessManager.SetMaintenanceReady(
                unitInstanceId,
                ready);
        }

        public bool CanDeploy(
            string unitInstanceId)
        {
            if (readinessManager == null)
                return false;

            return readinessManager.IsDeploymentReady(
                unitInstanceId);
        }
    }
}

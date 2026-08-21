using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceStation : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private MaintenanceServiceController serviceController;

        [SerializeField]
        private MaintenanceDashboardController dashboardController;

        [Header("Station")]
        [SerializeField]
        private string stationId = "MAINTENANCE_01";

        public string StationId =>
            stationId;

        public bool Service(
            string unitInstanceId,
            float repairAmount)
        {
            if (serviceController == null)
                return false;

            serviceController.ServiceUnit(
                unitInstanceId,
                repairAmount);

            return true;
        }

        public void Inspect(
            string unitInstanceId)
        {
            if (serviceController == null)
                return;

            serviceController.InspectUnit(
                unitInstanceId);

            Refresh(
                unitInstanceId,
                string.Empty,
                string.Empty);
        }

        public bool CanDeploy(
            string unitInstanceId)
        {
            if (serviceController == null)
                return false;

            return serviceController.CanDeploy(
                unitInstanceId);
        }

        public MaintenanceDashboardState Refresh(
            string unitInstanceId,
            string unitDefinitionId,
            string displayName)
        {
            if (dashboardController == null)
                return null;

            return dashboardController.Refresh(
                unitInstanceId,
                unitDefinitionId,
                displayName);
        }
    }
}

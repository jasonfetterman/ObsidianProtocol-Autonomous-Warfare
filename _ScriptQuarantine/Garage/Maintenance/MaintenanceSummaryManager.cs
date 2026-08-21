using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceSummaryManager : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private MaintenanceDashboardProvider dashboardProvider;

        [SerializeField]
        private MaintenanceNotificationController notificationController;

        [Header("Current Summary")]
        [SerializeField]
        private MaintenanceDashboardState currentSummary;

        public MaintenanceDashboardState CurrentSummary =>
            currentSummary;

        public MaintenanceDashboardState Refresh(
            string unitInstanceId,
            string unitDefinitionId,
            string displayName)
        {
            if (dashboardProvider == null)
            {
                Debug.LogWarning(
                    "MaintenanceSummaryManager: Dashboard provider is not assigned.");

                return null;
            }

            currentSummary =
                dashboardProvider.BuildDashboard(
                    unitInstanceId,
                    unitDefinitionId,
                    displayName);

            return currentSummary;
        }

        public bool IsReady()
        {
            return currentSummary != null &&
                   currentSummary.deploymentReady;
        }

        public bool NeedsMaintenance()
        {
            return currentSummary != null &&
                   currentSummary.maintenanceRequired;
        }

        public float Condition()
        {
            return currentSummary != null
                ? currentSummary.condition
                : 0f;
        }

        public float Readiness()
        {
            return currentSummary != null
                ? currentSummary.readinessScore
                : 0f;
        }

        public string Status()
        {
            return currentSummary != null
                ? currentSummary.Status
                : "NO UNIT";
        }

        public string LatestNotification()
        {
            if (notificationController == null)
                return string.Empty;

            return notificationController.GetLatest();
        }

        public void Clear()
        {
            currentSummary = null;
        }
    }
}

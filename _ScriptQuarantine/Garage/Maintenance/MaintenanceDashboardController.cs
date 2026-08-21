using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceDashboardController : MonoBehaviour
    {
        [Header("Provider")]
        [SerializeField]
        private MaintenanceDashboardProvider provider;

        [Header("Current Dashboard")]
        [SerializeField]
        private MaintenanceDashboardState currentDashboard;

        public MaintenanceDashboardState CurrentDashboard =>
            currentDashboard;

        public MaintenanceDashboardState Refresh(
            string unitInstanceId,
            string unitDefinitionId,
            string displayName)
        {
            if (provider == null)
            {
                Debug.LogWarning(
                    "MaintenanceDashboardController: Provider is not assigned.");

                return null;
            }

            currentDashboard =
                provider.BuildDashboard(
                    unitInstanceId,
                    unitDefinitionId,
                    displayName);

            return currentDashboard;
        }

        public bool IsReady()
        {
            return currentDashboard != null &&
                   currentDashboard.deploymentReady;
        }

        public string GetStatus()
        {
            if (currentDashboard == null)
                return "NO UNIT";

            return currentDashboard.Status;
        }

        public float GetCondition()
        {
            if (currentDashboard == null)
                return 0f;

            return currentDashboard.condition;
        }

        public float GetReadiness()
        {
            if (currentDashboard == null)
                return 0f;

            return currentDashboard.readinessScore;
        }

        public void Clear()
        {
            currentDashboard = null;
        }
    }
}

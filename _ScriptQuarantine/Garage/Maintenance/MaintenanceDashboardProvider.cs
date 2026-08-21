using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceDashboardProvider : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private MaintenanceUIProvider maintenanceUIProvider;

        [SerializeField]
        private GarageSessionManager sessionManager;

        [SerializeField]
        private GarageConfiguration configuration;

        public MaintenanceDashboardState BuildDashboard(
            string unitInstanceId,
            string unitDefinitionId,
            string displayName)
        {
            MaintenanceDashboardState dashboard =
                new MaintenanceDashboardState
                {
                    unitInstanceId = unitInstanceId,
                    unitDefinitionId = unitDefinitionId,
                    displayName = displayName
                };

            if (maintenanceUIProvider != null)
            {
                UnitReadinessSummary summary =
                    maintenanceUIProvider.BuildSummary(
                        unitInstanceId,
                        unitDefinitionId,
                        displayName);

                if (summary != null)
                {
                    dashboard.condition =
                        summary.condition;

                    dashboard.inspectionScore =
                        summary.inspectionScore;

                    dashboard.readinessScore =
                        summary.readinessScore;

                    dashboard.maintenanceRequired =
                        summary.requiresMaintenance;

                    dashboard.inspectionPassed =
                        summary.inspectionPassed;

                    dashboard.resourcesReady =
                        summary.resourcesReady;

                    dashboard.crewReady =
                        summary.crewReady;

                    dashboard.deploymentReady =
                        summary.deploymentReady;
                }
            }

            if (sessionManager != null)
            {
                GarageSessionState session =
                    sessionManager.Session;

                dashboard.deployed =
                    session.deployed;

                dashboard.online =
                    session.sessionMode ==
                    GarageSessionMode.Online;
            }

            if (configuration != null)
            {
                dashboard.vrEnabled =
                    configuration.enableVRControl;

                dashboard.freeRoamEnabled =
                    configuration.enableFreeRoam;
            }

            return dashboard;
        }
    }
}

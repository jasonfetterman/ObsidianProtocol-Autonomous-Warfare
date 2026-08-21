using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceUIProvider : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private MaintenanceManager maintenanceManager;

        [SerializeField]
        private InspectionManager inspectionManager;

        [SerializeField]
        private DeploymentReadinessManager readinessManager;

        public UnitReadinessSummary BuildSummary(
            string unitInstanceId,
            string unitDefinitionId,
            string displayName)
        {
            UnitReadinessSummary summary =
                new UnitReadinessSummary
                {
                    unitInstanceId = unitInstanceId,
                    unitDefinitionId = unitDefinitionId,
                    displayName = displayName
                };

            if (maintenanceManager != null)
            {
                MaintenanceState maintenance =
                    maintenanceManager.GetOrCreate(
                        unitInstanceId);

                if (maintenance != null)
                {
                    summary.condition =
                        maintenance.overallCondition;

                    summary.requiresMaintenance =
                        maintenance.requiresMaintenance;
                }
            }

            if (inspectionManager != null)
            {
                MaintenanceInspection inspection =
                    inspectionManager.Get(
                        unitInstanceId);

                if (inspection != null)
                {
                    summary.inspectionScore =
                        inspection.overallScore;

                    summary.inspectionPassed =
                        inspection.passed;
                }
            }

            if (readinessManager != null)
            {
                DeploymentReadiness readiness =
                    readinessManager.GetOrCreate(
                        unitInstanceId);

                if (readiness != null)
                {
                    summary.readinessScore =
                        readiness.readinessScore;

                    summary.resourcesReady =
                        readiness.resourcesReady;

                    summary.crewReady =
                        readiness.crewReady;

                    summary.deploymentReady =
                        readiness.deploymentReady;
                }
            }

            return summary;
        }
    }
}

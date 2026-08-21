using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class MaintenanceDashboardState
    {
        [Header("Selected Unit")]
        public string unitInstanceId;
        public string unitDefinitionId;
        public string displayName;

        [Header("Condition")]
        [Range(0f, 1f)]
        public float condition;

        [Range(0f, 1f)]
        public float inspectionScore;

        [Range(0f, 1f)]
        public float readinessScore;

        [Header("Status")]
        public bool maintenanceRequired;
        public bool inspectionPassed;
        public bool resourcesReady;
        public bool crewReady;
        public bool deploymentReady;

        [Header("Operation")]
        public bool deployed;
        public bool online;
        public bool vrEnabled;
        public bool freeRoamEnabled;

        public string Status
        {
            get
            {
                if (deploymentReady)
                    return "READY";

                if (maintenanceRequired)
                    return "MAINTENANCE";

                if (!inspectionPassed)
                    return "INSPECTION";

                if (!resourcesReady)
                    return "RESOURCES";

                if (!crewReady)
                    return "CREW";

                return "NOT READY";
            }
        }
    }
}

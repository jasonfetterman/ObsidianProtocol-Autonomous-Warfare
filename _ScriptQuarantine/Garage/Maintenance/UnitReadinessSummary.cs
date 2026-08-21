using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class UnitReadinessSummary
    {
        public string unitInstanceId;
        public string unitDefinitionId;
        public string displayName;

        [Header("Condition")]
        [Range(0f, 1f)]
        public float condition = 1f;

        [Range(0f, 1f)]
        public float inspectionScore = 1f;

        [Range(0f, 1f)]
        public float readinessScore = 1f;

        [Header("Status")]
        public bool requiresMaintenance;
        public bool inspectionPassed;
        public bool resourcesReady;
        public bool crewReady;
        public bool deploymentReady;

        public string GetStatus()
        {
            if (deploymentReady)
                return "READY";

            if (requiresMaintenance)
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

using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class DeploymentReadiness
    {
        public string unitInstanceId;

        public bool maintenanceReady;
        public bool inspectionPassed;
        public bool resourcesReady;
        public bool crewReady;
        public bool deploymentReady;

        [Range(0f, 1f)]
        public float readinessScore;

        public void Calculate()
        {
            float score = 0f;

            if (maintenanceReady)
                score += 0.25f;

            if (inspectionPassed)
                score += 0.25f;

            if (resourcesReady)
                score += 0.25f;

            if (crewReady)
                score += 0.25f;

            readinessScore = score;

            deploymentReady =
                maintenanceReady &&
                inspectionPassed &&
                resourcesReady &&
                crewReady;
        }
    }
}

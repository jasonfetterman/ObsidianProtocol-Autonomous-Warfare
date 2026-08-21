using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class DeploymentAuthorization
    {
        public string unitInstanceId;
        public string unitDefinitionId;

        public bool maintenanceApproved;
        public bool inspectionApproved;
        public bool resourcesApproved;
        public bool crewApproved;

        public bool modeApproved;
        public bool worldApproved;
        public bool sessionApproved;

        public bool authorized;
        public string denialReason;

        public void Evaluate()
        {
            authorized =
                maintenanceApproved &&
                inspectionApproved &&
                resourcesApproved &&
                crewApproved &&
                modeApproved &&
                worldApproved &&
                sessionApproved;

            if (authorized)
            {
                denialReason = string.Empty;
                return;
            }

            if (!maintenanceApproved)
            {
                denialReason = "MAINTENANCE";
                return;
            }

            if (!inspectionApproved)
            {
                denialReason = "INSPECTION";
                return;
            }

            if (!resourcesApproved)
            {
                denialReason = "RESOURCES";
                return;
            }

            if (!crewApproved)
            {
                denialReason = "CREW";
                return;
            }

            if (!modeApproved)
            {
                denialReason = "CONTROL MODE";
                return;
            }

            if (!worldApproved)
            {
                denialReason = "WORLD";
                return;
            }

            if (!sessionApproved)
            {
                denialReason = "SESSION";
                return;
            }

            denialReason = "UNKNOWN";
        }
    }
}

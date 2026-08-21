using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class DeploymentAuthorizationManager : MonoBehaviour
    {
        [Header("Current Authorization")]
        [SerializeField]
        private DeploymentAuthorization authorization;

        public DeploymentAuthorization Current =>
            authorization;

        public DeploymentAuthorization Create(
            string unitInstanceId,
            string unitDefinitionId)
        {
            authorization =
                new DeploymentAuthorization
                {
                    unitInstanceId = unitInstanceId,
                    unitDefinitionId = unitDefinitionId
                };

            return authorization;
        }

        public void SetMaintenanceApproved(bool value)
        {
            EnsureAuthorization();

            authorization.maintenanceApproved = value;
            authorization.Evaluate();
        }

        public void SetInspectionApproved(bool value)
        {
            EnsureAuthorization();

            authorization.inspectionApproved = value;
            authorization.Evaluate();
        }

        public void SetResourcesApproved(bool value)
        {
            EnsureAuthorization();

            authorization.resourcesApproved = value;
            authorization.Evaluate();
        }

        public void SetCrewApproved(bool value)
        {
            EnsureAuthorization();

            authorization.crewApproved = value;
            authorization.Evaluate();
        }

        public void SetModeApproved(bool value)
        {
            EnsureAuthorization();

            authorization.modeApproved = value;
            authorization.Evaluate();
        }

        public void SetWorldApproved(bool value)
        {
            EnsureAuthorization();

            authorization.worldApproved = value;
            authorization.Evaluate();
        }

        public void SetSessionApproved(bool value)
        {
            EnsureAuthorization();

            authorization.sessionApproved = value;
            authorization.Evaluate();
        }

        public bool IsAuthorized()
        {
            return authorization != null &&
                   authorization.authorized;
        }

        public string GetDenialReason()
        {
            if (authorization == null)
                return "NO AUTHORIZATION";

            return authorization.denialReason;
        }

        private void EnsureAuthorization()
        {
            if (authorization == null)
            {
                authorization =
                    new DeploymentAuthorization();
            }
        }

        public void Clear()
        {
            authorization = null;
        }
    }
}

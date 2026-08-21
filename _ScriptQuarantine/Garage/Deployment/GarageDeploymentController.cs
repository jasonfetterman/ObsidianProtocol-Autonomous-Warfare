using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageDeploymentController : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private DeploymentAuthorizationManager authorizationManager;

        [SerializeField]
        private GarageSessionManager sessionManager;

        [SerializeField]
        private GarageFleetController fleetController;

        public bool CanDeploy()
        {
            if (authorizationManager == null)
                return false;

            return authorizationManager.IsAuthorized();
        }

        public bool DeployUnit()
        {
            if (!CanDeploy())
            {
                Debug.LogWarning(
                    "GarageDeploymentController: Deployment denied.");

                return false;
            }

            if (sessionManager != null)
                sessionManager.Deploy();

            return true;
        }

        public bool DeployFleet()
        {
            if (!CanDeploy())
            {
                Debug.LogWarning(
                    "GarageDeploymentController: Fleet deployment denied.");

                return false;
            }

            if (fleetController != null)
                fleetController.DeployFleet();

            if (sessionManager != null)
                sessionManager.Deploy();

            return true;
        }

        public void Recall()
        {
            if (fleetController != null)
                fleetController.RecallFleet();

            if (sessionManager != null)
                sessionManager.EnterGarage();
        }

        public string GetDenialReason()
        {
            if (authorizationManager == null)
                return "AUTHORIZATION SYSTEM MISSING";

            return authorizationManager.GetDenialReason();
        }
    }
}

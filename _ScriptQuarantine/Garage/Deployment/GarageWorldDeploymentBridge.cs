using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageWorldDeploymentBridge : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private DeploymentAuthorizationManager authorizationManager;

        [SerializeField]
        private WorldDeploymentManager worldDeploymentManager;

        [Header("Deployment Defaults")]
        [SerializeField]
        private string defaultWorldId = "WORLD_01";

        public bool Deploy(
            string unitInstanceId,
            string unitDefinitionId,
            string spawnPointId,
            bool online,
            bool rtsControl,
            bool directControl,
            bool vrControl,
            bool freeRoam)
        {
            if (authorizationManager == null ||
                worldDeploymentManager == null)
            {
                Debug.LogWarning(
                    "GarageWorldDeploymentBridge: Deployment systems missing.");

                return false;
            }

            if (!authorizationManager.IsAuthorized())
            {
                Debug.LogWarning(
                    "GarageWorldDeploymentBridge: Deployment denied.");

                return false;
            }

            WorldDeploymentRequest request =
                new WorldDeploymentRequest
                {
                    unitInstanceId = unitInstanceId,
                    unitDefinitionId = unitDefinitionId,
                    worldId = defaultWorldId,
                    spawnPointId = spawnPointId,

                    authorized = true,

                    online = online,
                    offline = !online,

                    rtsControl = rtsControl,
                    directControl = directControl,
                    vrControl = vrControl,
                    freeRoam = freeRoam
                };

            return worldDeploymentManager.Deploy(
                request);
        }

        public void Recall()
        {
            if (worldDeploymentManager == null)
                return;

            worldDeploymentManager.Recall();
        }

        public bool IsDeployed()
        {
            return worldDeploymentManager != null &&
                   worldDeploymentManager.IsDeployed();
        }
    }
}

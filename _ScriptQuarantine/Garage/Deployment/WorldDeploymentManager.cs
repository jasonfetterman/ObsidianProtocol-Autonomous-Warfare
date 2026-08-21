using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class WorldDeploymentManager : MonoBehaviour
    {
        [Header("Current Deployment")]
        [SerializeField]
        private DeploymentState currentDeployment;

        public DeploymentState CurrentDeployment =>
            currentDeployment;

        public bool Deploy(
            WorldDeploymentRequest request)
        {
            if (request == null)
            {
                Debug.LogWarning(
                    "WorldDeploymentManager: Request is null.");

                return false;
            }

            if (!request.IsValid())
            {
                Debug.LogWarning(
                    "WorldDeploymentManager: Deployment request is invalid.");

                return false;
            }

            currentDeployment =
                new DeploymentState
                {
                    unitInstanceId =
                        request.unitInstanceId,

                    unitDefinitionId =
                        request.unitDefinitionId,

                    worldId =
                        request.worldId,

                    spawnPointId =
                        request.spawnPointId,

                    online =
                        request.online,

                    offline =
                        request.offline,

                    rtsControl =
                        request.rtsControl,

                    directControl =
                        request.directControl,

                    vrControl =
                        request.vrControl,

                    freeRoam =
                        request.freeRoam
                };

            currentDeployment.Begin();

            Debug.Log(
                $"UNIT DEPLOYED: {request.unitInstanceId}");

            return true;
        }

        public void Recall()
        {
            if (currentDeployment == null)
                return;

            currentDeployment.End();

            Debug.Log(
                $"UNIT RECALLED: {currentDeployment.unitInstanceId}");
        }

        public bool IsDeployed()
        {
            return currentDeployment != null &&
                   currentDeployment.deployed;
        }

        public void Clear()
        {
            currentDeployment = null;
        }
    }
}

using System;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class DeploymentResult
    {
        public bool success;

        public string unitInstanceId;
        public string unitDefinitionId;
        public string worldId;
        public string spawnPointId;

        public string message;

        public static DeploymentResult Success(
            string unitInstanceId,
            string unitDefinitionId,
            string worldId,
            string spawnPointId)
        {
            return new DeploymentResult
            {
                success = true,
                unitInstanceId = unitInstanceId,
                unitDefinitionId = unitDefinitionId,
                worldId = worldId,
                spawnPointId = spawnPointId,
                message = "DEPLOYMENT SUCCESSFUL"
            };
        }

        public static DeploymentResult Failure(
            string message)
        {
            return new DeploymentResult
            {
                success = false,
                message = message
            };
        }
    }
}

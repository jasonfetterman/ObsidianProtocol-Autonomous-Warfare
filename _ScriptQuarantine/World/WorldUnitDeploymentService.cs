using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldUnitDeploymentService : MonoBehaviour
    {
        [Header("World Systems")]
        [SerializeField]
        private PersistentWorldManager worldManager;

        [SerializeField]
        private WorldSpawnPointRegistry spawnPointRegistry;

        public bool DeployUnit(
            string unitInstanceId,
            string unitDefinitionId,
            string worldId,
            string spawnPointId)
        {
            if (worldManager == null)
            {
                Debug.LogWarning(
                    "WorldUnitDeploymentService: World manager missing.");

                return false;
            }

            if (spawnPointRegistry == null)
            {
                Debug.LogWarning(
                    "WorldUnitDeploymentService: Spawn registry missing.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(unitInstanceId) ||
                string.IsNullOrWhiteSpace(unitDefinitionId))
            {
                Debug.LogWarning(
                    "WorldUnitDeploymentService: Unit identity is invalid.");

                return false;
            }

            WorldSpawnPoint spawnPoint =
                spawnPointRegistry.Get(spawnPointId);

            if (spawnPoint == null)
            {
                Debug.LogWarning(
                    $"WorldUnitDeploymentService: Spawn point not found: {spawnPointId}");

                return false;
            }

            if (!spawnPoint.Available)
            {
                Debug.LogWarning(
                    $"WorldUnitDeploymentService: Spawn point unavailable: {spawnPointId}");

                return false;
            }

            if (spawnPoint.WorldId != worldId)
            {
                Debug.LogWarning(
                    "WorldUnitDeploymentService: Spawn point belongs to another world.");

                return false;
            }

            if (worldManager.GetUnit(unitInstanceId) != null)
            {
                Debug.LogWarning(
                    $"WorldUnitDeploymentService: Unit already exists: {unitInstanceId}");

                return false;
            }

            spawnPoint.Reserve();

            worldManager.RegisterUnit(
                unitInstanceId,
                unitDefinitionId,
                spawnPoint.Position,
                spawnPoint.Rotation);

            Debug.Log(
                $"WORLD DEPLOYMENT COMPLETE: {unitInstanceId}");

            return true;
        }

        public bool DeployAtAvailablePoint(
            string unitInstanceId,
            string unitDefinitionId,
            string worldId,
            WorldSpawnPointType type)
        {
            if (spawnPointRegistry == null)
                return false;

            WorldSpawnPoint spawnPoint =
                spawnPointRegistry.FindAvailableForWorld(
                    worldId,
                    type);

            if (spawnPoint == null)
                return false;

            return DeployUnit(
                unitInstanceId,
                unitDefinitionId,
                worldId,
                spawnPoint.SpawnPointId);
        }
    }
}

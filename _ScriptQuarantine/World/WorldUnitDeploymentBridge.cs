using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldUnitDeploymentBridge : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private WorldUnitDeploymentService deploymentService;

        [SerializeField]
        private WorldPrefabSpawner prefabSpawner;

        [Header("Unit Prefab")]
        [SerializeField]
        private GameObject unitPrefab;

        public GameObject Deploy(
            string unitInstanceId,
            string unitDefinitionId,
            string worldId,
            string spawnPointId)
        {
            if (deploymentService == null)
            {
                Debug.LogWarning(
                    "WorldUnitDeploymentBridge: Deployment service missing.");

                return null;
            }

            if (prefabSpawner == null)
            {
                Debug.LogWarning(
                    "WorldUnitDeploymentBridge: Prefab spawner missing.");

                return null;
            }

            if (unitPrefab == null)
            {
                Debug.LogWarning(
                    "WorldUnitDeploymentBridge: Unit prefab missing.");

                return null;
            }

            bool registered =
                deploymentService.DeployUnit(
                    unitInstanceId,
                    unitDefinitionId,
                    worldId,
                    spawnPointId);

            if (!registered)
                return null;

            WorldSpawnPoint spawnPoint =
                FindSpawnPoint(spawnPointId);

            if (spawnPoint == null)
                return null;

            return prefabSpawner.SpawnAtPoint(
                unitPrefab,
                unitInstanceId,
                unitDefinitionId,
                spawnPoint);
        }

        private WorldSpawnPoint FindSpawnPoint(
            string spawnPointId)
        {
            WorldSpawnPointRegistry registry =
                deploymentService.GetComponent<
                    WorldSpawnPointRegistry>();

            if (registry != null)
                return registry.Get(spawnPointId);

            return FindAnyObjectByType<
                WorldSpawnPointRegistry>()?.Get(
                    spawnPointId);
        }
    }
}


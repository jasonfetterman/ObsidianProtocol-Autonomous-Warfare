using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldPrefabSpawner : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private WorldEntityManager entityManager;

        [Header("Default Parent")]
        [SerializeField]
        private Transform worldEntityParent;

        public GameObject Spawn(
            GameObject prefab,
            string unitInstanceId,
            string unitDefinitionId,
            Vector3 position,
            Vector3 rotation)
        {
            if (entityManager == null)
            {
                Debug.LogWarning(
                    "WorldPrefabSpawner: Entity manager missing.");

                return null;
            }

            if (prefab == null)
            {
                Debug.LogWarning(
                    "WorldPrefabSpawner: Prefab missing.");

                return null;
            }

            GameObject entity =
                entityManager.CreateEntity(
                    prefab,
                    unitInstanceId,
                    unitDefinitionId,
                    position,
                    rotation);

            if (entity == null)
                return null;

            if (worldEntityParent != null)
                entity.transform.SetParent(
                    worldEntityParent,
                    true);

            return entity;
        }

        public GameObject SpawnAtPoint(
            GameObject prefab,
            string unitInstanceId,
            string unitDefinitionId,
            WorldSpawnPoint spawnPoint)
        {
            if (spawnPoint == null)
                return null;

            return Spawn(
                prefab,
                unitInstanceId,
                unitDefinitionId,
                spawnPoint.Position,
                spawnPoint.Rotation);
        }
    }
}

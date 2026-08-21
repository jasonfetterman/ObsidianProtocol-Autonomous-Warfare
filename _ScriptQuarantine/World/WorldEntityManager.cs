using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldEntityManager : MonoBehaviour
    {
        [Header("World Registry")]
        [SerializeField]
        private WorldUnitRegistry unitRegistry;

        [Header("Runtime Entities")]
        [SerializeField]
        private List<GameObject> activeEntities =
            new List<GameObject>();

        public IReadOnlyList<GameObject> ActiveEntities =>
            activeEntities;

        public int ActiveEntityCount =>
            activeEntities.Count;

        public void RegisterEntity(
            GameObject entity)
        {
            if (entity == null)
                return;

            if (activeEntities.Contains(entity))
                return;

            activeEntities.Add(entity);
        }

        public void UnregisterEntity(
            GameObject entity)
        {
            if (entity == null)
                return;

            activeEntities.Remove(entity);
        }

        public GameObject FindEntity(
            string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(
                    unitInstanceId))
                return null;

            foreach (GameObject entity in activeEntities)
            {
                if (entity == null)
                    continue;

                WorldEntityIdentity identity =
                    entity.GetComponent<WorldEntityIdentity>();

                if (identity == null)
                    continue;

                if (identity.UnitInstanceId ==
                    unitInstanceId)
                    return entity;
            }

            return null;
        }

        public GameObject CreateEntity(
            GameObject prefab,
            string unitInstanceId,
            string unitDefinitionId,
            Vector3 position,
            Vector3 rotation)
        {
            if (prefab == null)
            {
                Debug.LogWarning(
                    "WorldEntityManager: Prefab is missing.");

                return null;
            }

            if (FindEntity(unitInstanceId) != null)
            {
                Debug.LogWarning(
                    $"WorldEntityManager: Entity already exists: {unitInstanceId}");

                return null;
            }

            GameObject entity =
                Instantiate(
                    prefab,
                    position,
                    Quaternion.Euler(rotation));

            WorldEntityIdentity identity =
                entity.GetComponent<WorldEntityIdentity>();

            if (identity == null)
            {
                identity =
                    entity.AddComponent<WorldEntityIdentity>();
            }

            identity.Initialize(
                unitInstanceId,
                unitDefinitionId);

            RegisterEntity(entity);

            if (unitRegistry != null)
            {
                unitRegistry.Register(
                    unitInstanceId,
                    unitDefinitionId,
                    position,
                    rotation);
            }

            return entity;
        }

        public void DestroyEntity(
            string unitInstanceId)
        {
            GameObject entity =
                FindEntity(unitInstanceId);

            if (entity == null)
                return;

            UnregisterEntity(entity);

            Destroy(entity);

            if (unitRegistry != null)
                unitRegistry.Remove(unitInstanceId);
        }

        public void ClearEntities()
        {
            for (int i = activeEntities.Count - 1;
                 i >= 0;
                 i--)
            {
                GameObject entity =
                    activeEntities[i];

                if (entity != null)
                    Destroy(entity);
            }

            activeEntities.Clear();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldSpawnPointRegistry : MonoBehaviour
    {
        [Header("Registered Spawn Points")]
        [SerializeField]
        private List<WorldSpawnPoint> spawnPoints =
            new List<WorldSpawnPoint>();

        public IReadOnlyList<WorldSpawnPoint> SpawnPoints =>
            spawnPoints;

        private void Awake()
        {
            Refresh();
        }

        public void Refresh()
        {
            spawnPoints.Clear();

            WorldSpawnPoint[] found =
                FindObjectsByType<WorldSpawnPoint>(FindObjectsInactive.Include);

            foreach (WorldSpawnPoint point in found)
            {
                if (point == null)
                    continue;

                if (!spawnPoints.Contains(point))
                    spawnPoints.Add(point);
            }
        }

        public WorldSpawnPoint Get(
            string spawnPointId)
        {
            if (string.IsNullOrWhiteSpace(
                    spawnPointId))
                return null;

            foreach (WorldSpawnPoint point in spawnPoints)
            {
                if (point == null)
                    continue;

                if (point.SpawnPointId ==
                    spawnPointId)
                    return point;
            }

            return null;
        }

        public WorldSpawnPoint FindAvailable(
            WorldSpawnPointType type)
        {
            foreach (WorldSpawnPoint point in spawnPoints)
            {
                if (point == null)
                    continue;

                if (point.SpawnPointType != type)
                    continue;

                if (point.Available)
                    return point;
            }

            return null;
        }

        public WorldSpawnPoint FindAvailableForWorld(
            string worldId,
            WorldSpawnPointType type)
        {
            foreach (WorldSpawnPoint point in spawnPoints)
            {
                if (point == null)
                    continue;

                if (point.WorldId != worldId)
                    continue;

                if (point.SpawnPointType != type)
                    continue;

                if (point.Available)
                    return point;
            }

            return null;
        }

        public bool Reserve(
            string spawnPointId)
        {
            WorldSpawnPoint point =
                Get(spawnPointId);

            if (point == null ||
                !point.Available)
                return false;

            point.Reserve();

            return true;
        }

        public void Release(
            string spawnPointId)
        {
            WorldSpawnPoint point =
                Get(spawnPointId);

            if (point == null)
                return;

            point.Release();
        }
    }
}


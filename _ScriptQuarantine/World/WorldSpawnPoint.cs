using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldSpawnPoint : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField]
        private string spawnPointId = "SPAWN_01";

        [SerializeField]
        private string worldId = "WORLD_01";

        [Header("Classification")]
        [SerializeField]
        private WorldSpawnPointType spawnPointType =
            WorldSpawnPointType.GarageDeployment;

        [Header("Availability")]
        [SerializeField]
        private bool available = true;

        [SerializeField]
        private bool reserved;

        public string SpawnPointId =>
            spawnPointId;

        public string WorldId =>
            worldId;

        public WorldSpawnPointType SpawnPointType =>
            spawnPointType;

        public bool Available =>
            available && !reserved;

        public Vector3 Position =>
            transform.position;

        public Vector3 Rotation =>
            transform.eulerAngles;

        public void Reserve()
        {
            if (!available)
                return;

            reserved = true;
        }

        public void Release()
        {
            reserved = false;
        }

        public void SetAvailable(bool value)
        {
            available = value;

            if (!value)
                reserved = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(
                transform.position,
                1f);

            Gizmos.DrawLine(
                transform.position,
                transform.position +
                transform.forward * 2f);
        }
    }

    public enum WorldSpawnPointType
    {
        GarageDeployment,
        Base,
        Airfield,
        Naval,
        ForwardOperatingBase,
        RallyPoint,
        Emergency
    }
}

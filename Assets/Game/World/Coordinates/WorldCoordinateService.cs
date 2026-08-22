using UnityEngine;

namespace ObsidianProtocol.Game.World.Coordinates
{
    public sealed class WorldCoordinateService : MonoBehaviour
    {
        public static WorldCoordinateService Instance { get; private set; }

        [SerializeField] private Vector3 worldOrigin = Vector3.zero;

        public WorldCoordinate Origin => worldOrigin;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public WorldCoordinate ToWorld(Vector3 localPosition)
        {
            return worldOrigin + localPosition;
        }

        public Vector3 ToLocal(WorldCoordinate coordinate)
        {
            return coordinate.Position - worldOrigin;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}

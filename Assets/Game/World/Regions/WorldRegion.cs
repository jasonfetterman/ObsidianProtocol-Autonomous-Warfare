using UnityEngine;

namespace ObsidianProtocol.Game.World.Regions
{
    public sealed class WorldRegion : MonoBehaviour
    {
        [SerializeField] private WorldRegionDefinition definition;
        [SerializeField] private Bounds bounds;

        public WorldRegionDefinition Definition => definition;
        public Bounds Bounds => bounds;

        public bool Contains(Vector3 worldPosition)
        {
            return bounds.Contains(worldPosition);
        }
    }
}

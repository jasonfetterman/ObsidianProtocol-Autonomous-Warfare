using UnityEngine;

namespace ObsidianProtocol.Game.World.Water
{
    public sealed class WaterSystem : MonoBehaviour
    {
        [SerializeField] private WaterDefinition definition;

        public WaterDefinition Definition => definition;

        public float SurfaceHeight =>
            definition != null ? definition.SurfaceHeight : 0f;

        public bool IsUnderwater(Vector3 worldPosition)
        {
            return worldPosition.y < SurfaceHeight;
        }

        public float GetDepth(Vector3 worldPosition)
        {
            if (!IsUnderwater(worldPosition))
            {
                return 0f;
            }

            return SurfaceHeight - worldPosition.y;
        }
    }
}

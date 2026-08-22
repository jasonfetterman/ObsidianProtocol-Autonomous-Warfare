using UnityEngine;

namespace ObsidianProtocol.Game.World.Elevation
{
    public sealed class ElevationService : MonoBehaviour
    {
        [SerializeField] private ElevationDefinition definition;

        public ElevationDefinition Definition => definition;

        public float ClampHeight(float height)
        {
            if (definition == null)
            {
                return height;
            }

            return Mathf.Clamp(
                height,
                definition.MinimumHeight,
                definition.MaximumHeight);
        }

        public float GetHeight(Vector3 worldPosition)
        {
            return ClampHeight(worldPosition.y);
        }
    }
}

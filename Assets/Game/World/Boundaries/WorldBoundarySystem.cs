using UnityEngine;

namespace ObsidianProtocol.Game.World.Boundaries
{
    public sealed class WorldBoundarySystem : MonoBehaviour
    {
        [SerializeField] private WorldBoundaryDefinition definition;

        public WorldBoundaryDefinition Definition => definition;

        public bool Contains(Vector3 worldPosition)
        {
            if (definition == null)
            {
                return true;
            }

            return worldPosition.x >= definition.Minimum.x &&
                   worldPosition.x <= definition.Maximum.x &&
                   worldPosition.z >= definition.Minimum.y &&
                   worldPosition.z <= definition.Maximum.y;
        }

        public Vector3 ClampPosition(Vector3 worldPosition)
        {
            if (definition == null)
            {
                return worldPosition;
            }

            worldPosition.x = Mathf.Clamp(
                worldPosition.x,
                definition.Minimum.x,
                definition.Maximum.x);

            worldPosition.z = Mathf.Clamp(
                worldPosition.z,
                definition.Minimum.y,
                definition.Maximum.y);

            return worldPosition;
        }
    }
}

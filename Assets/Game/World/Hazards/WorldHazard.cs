using UnityEngine;

namespace ObsidianProtocol.Game.World.Hazards
{
    public sealed class WorldHazard : MonoBehaviour
    {
        [SerializeField] private WorldHazardDefinition definition;

        public WorldHazardDefinition Definition => definition;

        public float Severity =>
            definition != null ? definition.Severity : 0f;
    }
}

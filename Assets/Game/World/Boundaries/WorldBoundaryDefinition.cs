using UnityEngine;

namespace ObsidianProtocol.Game.World.Boundaries
{
    [CreateAssetMenu(
        fileName = "WorldBoundaryDefinition",
        menuName = "Obsidian Protocol/World/World Boundary Definition")]
    public sealed class WorldBoundaryDefinition : ScriptableObject
    {
        [SerializeField] private Vector2 minimum = new(-1000f, -1000f);
        [SerializeField] private Vector2 maximum = new(1000f, 1000f);

        public Vector2 Minimum => minimum;
        public Vector2 Maximum => maximum;
    }
}

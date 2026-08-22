using UnityEngine;

namespace ObsidianProtocol.Game.World.Elevation
{
    [CreateAssetMenu(
        fileName = "ElevationDefinition",
        menuName = "Obsidian Protocol/World/Elevation Definition")]
    public sealed class ElevationDefinition : ScriptableObject
    {
        [SerializeField] private float minimumHeight = 0f;
        [SerializeField] private float maximumHeight = 1000f;

        public float MinimumHeight => minimumHeight;
        public float MaximumHeight => Mathf.Max(minimumHeight, maximumHeight);
    }
}

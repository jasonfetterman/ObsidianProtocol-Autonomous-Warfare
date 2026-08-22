using UnityEngine;

namespace ObsidianProtocol.Game.World.Water
{
    [CreateAssetMenu(
        fileName = "WaterDefinition",
        menuName = "Obsidian Protocol/World/Water Definition")]
    public sealed class WaterDefinition : ScriptableObject
    {
        [SerializeField] private float surfaceHeight = 0f;
        [SerializeField] private float minimumDepth = 0.5f;
        [SerializeField] private float maximumDepth = 1000f;

        public float SurfaceHeight => surfaceHeight;
        public float MinimumDepth => Mathf.Max(0f, minimumDepth);
        public float MaximumDepth => Mathf.Max(MinimumDepth, maximumDepth);
    }
}

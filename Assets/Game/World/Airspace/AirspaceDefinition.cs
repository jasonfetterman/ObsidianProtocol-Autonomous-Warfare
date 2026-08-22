using UnityEngine;

namespace ObsidianProtocol.Game.World.Airspace
{
    [CreateAssetMenu(
        fileName = "AirspaceDefinition",
        menuName = "Obsidian Protocol/World/Airspace Definition")]
    public sealed class AirspaceDefinition : ScriptableObject
    {
        [SerializeField] private float minimumAltitude = 0f;
        [SerializeField] private float maximumAltitude = 5000f;

        public float MinimumAltitude => minimumAltitude;
        public float MaximumAltitude =>
            Mathf.Max(minimumAltitude, maximumAltitude);
    }
}

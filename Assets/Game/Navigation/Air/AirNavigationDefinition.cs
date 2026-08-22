using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Air
{
    [CreateAssetMenu(
        fileName = "AirNavigationDefinition",
        menuName = "Obsidian Protocol/Navigation/Air Navigation Definition")]
    public sealed class AirNavigationDefinition : ScriptableObject
    {
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float stoppingDistance = 2f;
        [SerializeField] private float minimumAltitude = 10f;
        [SerializeField] private float maximumAltitude = 500f;

        public float MovementSpeed => Mathf.Max(0f, movementSpeed);
        public float StoppingDistance => Mathf.Max(0.1f, stoppingDistance);
        public float MinimumAltitude => minimumAltitude;
        public float MaximumAltitude =>
            Mathf.Max(minimumAltitude, maximumAltitude);
    }
}

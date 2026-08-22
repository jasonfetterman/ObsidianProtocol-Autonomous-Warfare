using UnityEngine;

namespace ObsidianProtocol.Game.World.Weather
{
    [CreateAssetMenu(
        fileName = "WeatherDefinition",
        menuName = "Obsidian Protocol/World/Weather Definition")]
    public sealed class WeatherDefinition : ScriptableObject
    {
        [SerializeField] private string weatherId;
        [SerializeField] private string displayName;
        [SerializeField] private float visibilityMultiplier = 1f;

        public string WeatherId => weatherId;
        public string DisplayName => displayName;
        public float VisibilityMultiplier =>
            Mathf.Clamp01(visibilityMultiplier);
    }
}

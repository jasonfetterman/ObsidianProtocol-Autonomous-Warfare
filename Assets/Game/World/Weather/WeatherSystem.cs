using UnityEngine;

namespace ObsidianProtocol.Game.World.Weather
{
    public sealed class WeatherSystem : MonoBehaviour
    {
        [SerializeField] private WeatherDefinition definition;

        public WeatherDefinition Definition => definition;

        public float VisibilityMultiplier =>
            definition != null ? definition.VisibilityMultiplier : 1f;
    }
}

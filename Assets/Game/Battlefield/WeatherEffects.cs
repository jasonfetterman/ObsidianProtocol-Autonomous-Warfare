using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum WeatherType
    {
        Clear,
        Rain,
        HeavyRain,
        Fog,
        Storm,
        Snow,
        Sandstorm
    }

    public sealed class WeatherCondition
    {
        public WeatherType Type { get; private set; }

        public float Intensity { get; private set; }

        public float VisibilityMultiplier { get; private set; }

        public float MovementMultiplier { get; private set; }

        public float SensorMultiplier { get; private set; }

        public WeatherCondition(
            WeatherType type,
            float intensity)
        {
            Type = type;

            Intensity =
                Math.Max(
                    0f,
                    Math.Min(1f, intensity));

            UpdateEffects();
        }

        public bool SetCondition(
            WeatherType type,
            float intensity)
        {
            Type = type;

            Intensity =
                Math.Max(
                    0f,
                    Math.Min(1f, intensity));

            UpdateEffects();

            return true;
        }

        private void UpdateEffects()
        {
            VisibilityMultiplier = 1f;
            MovementMultiplier = 1f;
            SensorMultiplier = 1f;

            switch (Type)
            {
                case WeatherType.Rain:
                    VisibilityMultiplier =
                        1f - Intensity * 0.20f;
                    SensorMultiplier =
                        1f - Intensity * 0.10f;
                    break;

                case WeatherType.HeavyRain:
                    VisibilityMultiplier =
                        1f - Intensity * 0.40f;
                    MovementMultiplier =
                        1f - Intensity * 0.15f;
                    SensorMultiplier =
                        1f - Intensity * 0.25f;
                    break;

                case WeatherType.Fog:
                    VisibilityMultiplier =
                        1f - Intensity * 0.70f;
                    SensorMultiplier =
                        1f - Intensity * 0.50f;
                    break;

                case WeatherType.Storm:
                    VisibilityMultiplier =
                        1f - Intensity * 0.50f;
                    MovementMultiplier =
                        1f - Intensity * 0.20f;
                    SensorMultiplier =
                        1f - Intensity * 0.40f;
                    break;

                case WeatherType.Snow:
                    VisibilityMultiplier =
                        1f - Intensity * 0.35f;
                    MovementMultiplier =
                        1f - Intensity * 0.30f;
                    SensorMultiplier =
                        1f - Intensity * 0.20f;
                    break;

                case WeatherType.Sandstorm:
                    VisibilityMultiplier =
                        1f - Intensity * 0.80f;
                    MovementMultiplier =
                        1f - Intensity * 0.25f;
                    SensorMultiplier =
                        1f - Intensity * 0.70f;
                    break;
            }
        }
    }

    public sealed class WeatherEffects
    {
        private readonly Dictionary<
            string,
            WeatherCondition> regions =
            new Dictionary<
                string,
                WeatherCondition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int RegionCount =>
            regions.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            regions.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterRegion(
            string regionId,
            WeatherType type,
            float intensity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return false;
            }

            string id =
                regionId.Trim();

            if (regions.ContainsKey(id))
            {
                return false;
            }

            regions.Add(
                id,
                new WeatherCondition(
                    type,
                    intensity));

            return true;
        }

        public bool SetWeather(
            string regionId,
            WeatherType type,
            float intensity)
        {
            WeatherCondition condition =
                GetWeather(regionId);

            return condition != null &&
                   condition.SetCondition(
                       type,
                       intensity);
        }

        public WeatherCondition GetWeather(
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return null;
            }

            regions.TryGetValue(
                regionId.Trim(),
                out WeatherCondition condition);

            return condition;
        }

        public IReadOnlyCollection<WeatherCondition>
            GetWeatherConditions()
        {
            return regions.Values;
        }

        public void Reset()
        {
            regions.Clear();

            Initialized = false;
        }
    }
}

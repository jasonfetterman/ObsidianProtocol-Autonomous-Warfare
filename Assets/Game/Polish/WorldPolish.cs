using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class WorldPolish
    {
        private readonly Dictionary<
            string,
            bool> features =
            new Dictionary<
                string,
                bool>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int FeatureCount =>
            features.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            features.Clear();

            SetDefault("TerrainDetail", true);
            SetDefault("Lighting", true);
            SetDefault("Weather", true);
            SetDefault("DayNightCycle", true);
            SetDefault("EnvironmentalEffects", true);
            SetDefault("Destruction", true);
            SetDefault("AmbientLife", true);
            SetDefault("WorldAudio", true);
            SetDefault("PerformanceLOD", true);

            Initialized = true;

            return true;
        }

        public bool SetFeature(
            string featureId,
            bool enabled)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(featureId))
            {
                return false;
            }

            features[featureId.Trim()] =
                enabled;

            return true;
        }

        public bool IsEnabled(
            string featureId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(featureId))
            {
                return false;
            }

            return features.TryGetValue(
                featureId.Trim(),
                out bool enabled) &&
                   enabled;
        }

        private void SetDefault(
            string key,
            bool enabled)
        {
            features[key] = enabled;
        }

        public void Reset()
        {
            features.Clear();
            Initialized = false;
        }
    }
}

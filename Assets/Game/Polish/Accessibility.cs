using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class Accessibility
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

            SetDefault("ColorblindSupport", true);
            SetDefault("Subtitles", true);
            SetDefault("ClosedCaptions", true);
            SetDefault("TextScaling", true);
            SetDefault("HighContrast", true);
            SetDefault("ScreenReaderSupport", true);
            SetDefault("ReducedMotion", true);
            SetDefault("InputRemapping", true);
            SetDefault("AudioCues", true);

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

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class EnvironmentalVFXPolish
    {
        private readonly Dictionary<
            string,
            float> effects =
            new Dictionary<
                string,
                float>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int EffectCount =>
            effects.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            effects.Clear();

            SetDefault("Fog", 1f);
            SetDefault("Dust", 1f);
            SetDefault("Rain", 1f);
            SetDefault("Wind", 1f);
            SetDefault("Fire", 1f);
            SetDefault("Smoke", 1f);
            SetDefault("Debris", 1f);
            SetDefault("Weather", 1f);

            Initialized = true;

            return true;
        }

        public bool SetIntensity(
            string effectId,
            float intensity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(effectId))
            {
                return false;
            }

            effects[effectId.Trim()] =
                Math.Max(0f, intensity);

            return true;
        }

        public float GetIntensity(
            string effectId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(effectId))
            {
                return 0f;
            }

            effects.TryGetValue(
                effectId.Trim(),
                out float intensity);

            return intensity;
        }

        private void SetDefault(
            string key,
            float intensity)
        {
            effects[key] = intensity;
        }

        public void Reset()
        {
            effects.Clear();
            Initialized = false;
        }
    }
}

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class UIUXPolish
    {
        private readonly Dictionary<
            string,
            float> settings =
            new Dictionary<
                string,
                float>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SettingCount =>
            settings.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            settings.Clear();

            SetDefault(
                "AnimationSpeed",
                1f);

            SetDefault(
                "TransitionSpeed",
                1f);

            SetDefault(
                "FeedbackDuration",
                1f);

            SetDefault(
                "TooltipDelay",
                0.5f);

            Initialized = true;

            return true;
        }

        public bool SetValue(
            string key,
            float value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            settings[key.Trim()] =
                Math.Max(0f, value);

            return true;
        }

        public float GetValue(
            string key)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return 0f;
            }

            settings.TryGetValue(
                key.Trim(),
                out float value);

            return value;
        }

        private void SetDefault(
            string key,
            float value)
        {
            settings[key] = value;
        }

        public void Reset()
        {
            settings.Clear();
            Initialized = false;
        }
    }
}

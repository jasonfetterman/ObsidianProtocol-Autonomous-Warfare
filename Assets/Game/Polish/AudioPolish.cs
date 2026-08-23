using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class AudioPolish
    {
        private readonly Dictionary<
            string,
            float> audioLevels =
            new Dictionary<
                string,
                float>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AudioChannelCount =>
            audioLevels.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            audioLevels.Clear();

            SetDefault("Master", 1f);
            SetDefault("Music", 1f);
            SetDefault("SFX", 1f);
            SetDefault("Voice", 1f);
            SetDefault("Ambient", 1f);
            SetDefault("UI", 1f);

            Initialized = true;

            return true;
        }

        public bool SetVolume(
            string channelId,
            float volume)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(channelId))
            {
                return false;
            }

            audioLevels[channelId.Trim()] =
                Clamp(volume);

            return true;
        }

        public float GetVolume(
            string channelId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(channelId))
            {
                return 0f;
            }

            audioLevels.TryGetValue(
                channelId.Trim(),
                out float volume);

            return volume;
        }

        private void SetDefault(
            string key,
            float value)
        {
            audioLevels[key] = value;
        }

        private static float Clamp(
            float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            if (value > 1f)
            {
                return 1f;
            }

            return value;
        }

        public void Reset()
        {
            audioLevels.Clear();
            Initialized = false;
        }
    }
}

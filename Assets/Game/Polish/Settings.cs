using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class Settings
    {
        private readonly Dictionary<
            string,
            string> values =
            new Dictionary<
                string,
                string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SettingCount =>
            values.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            values.Clear();

            SetDefault("GraphicsQuality", "High");
            SetDefault("Resolution", "Native");
            SetDefault("Fullscreen", "True");
            SetDefault("MasterVolume", "1");
            SetDefault("MusicVolume", "1");
            SetDefault("SFXVolume", "1");
            SetDefault("VoiceVolume", "1");
            SetDefault("Sensitivity", "1");
            SetDefault("Language", "English");

            Initialized = true;

            return true;
        }

        public bool SetValue(
            string key,
            string value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            values[key.Trim()] =
                value ?? string.Empty;

            return true;
        }

        public string GetValue(
            string key)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            values.TryGetValue(
                key.Trim(),
                out string value);

            return value;
        }

        private void SetDefault(
            string key,
            string value)
        {
            values[key] = value;
        }

        public void Reset()
        {
            values.Clear();
            Initialized = false;
        }
    }
}

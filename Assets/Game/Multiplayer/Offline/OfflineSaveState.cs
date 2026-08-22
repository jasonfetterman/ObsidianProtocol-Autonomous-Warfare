using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public sealed class OfflineSaveState
    {
        private readonly Dictionary<string, string> values =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ValueCount =>
            values.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            values.Clear();
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

        public bool TryGetValue(
            string key,
            out string value)
        {
            value = string.Empty;

            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return values.TryGetValue(
                key.Trim(),
                out value);
        }

        public bool RemoveValue(
            string key)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return values.Remove(
                key.Trim());
        }

        public IReadOnlyDictionary<
            string,
            string> GetValues()
        {
            return values;
        }

        public void Clear()
        {
            values.Clear();
        }

        public void Reset()
        {
            values.Clear();
            Initialized = false;
        }
    }
}

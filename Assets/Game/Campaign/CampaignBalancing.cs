using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignBalancing
    {
        private readonly Dictionary<
            string,
            float> values =
            new Dictionary<
                string,
                float>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int BalanceValueCount =>
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
            float value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            values[key.Trim()] =
                value;

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

            values.TryGetValue(
                key.Trim(),
                out float value);

            return value;
        }

        public bool HasValue(
            string key)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return values.ContainsKey(
                key.Trim());
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
            float>
            GetValues()
        {
            return values;
        }

        public void Reset()
        {
            values.Clear();
            Initialized = false;
        }
    }
}

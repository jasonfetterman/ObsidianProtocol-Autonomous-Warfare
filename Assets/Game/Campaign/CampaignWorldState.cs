using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignWorldState
    {
        private readonly Dictionary<
            string,
            string> values =
            new Dictionary<
                string,
                string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int StateCount =>
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

        public bool SetState(
            string key,
            string value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            string stateKey =
                key.Trim();

            values[stateKey] =
                value ?? string.Empty;

            return true;
        }

        public bool HasState(
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

        public string GetState(
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

        public bool RemoveState(
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
            string>
            GetStates()
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

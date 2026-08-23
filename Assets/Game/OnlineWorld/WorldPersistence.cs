using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class WorldPersistence
    {
        private readonly Dictionary<
            string,
            string> worldState =
            new Dictionary<
                string,
                string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int StateCount =>
            worldState.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            worldState.Clear();
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

            worldState[key.Trim()] =
                value ?? string.Empty;

            return true;
        }

        public string GetState(
            string key)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            worldState.TryGetValue(
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

            return worldState.Remove(
                key.Trim());
        }

        public IReadOnlyDictionary<
            string,
            string>
            GetWorldState()
        {
            return worldState;
        }

        public void Reset()
        {
            worldState.Clear();
            Initialized = false;
        }
    }
}

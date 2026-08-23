using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum PersistentWorldLifecycleState
    {
        Uninitialized,
        Loading,
        Ready,
        Running,
        Saving,
        Recovering
    }

    public sealed class PersistentWorldFramework
    {
        private readonly Dictionary<string, string> worldState =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

        public PersistentWorldLifecycleState State { get; private set; }

        public string WorldId { get; private set; }

        public int StateEntryCount =>
            worldState.Count;

        public bool Initialized =>
            State != PersistentWorldLifecycleState.Uninitialized;

        public bool Initialize(string worldId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(worldId))
            {
                return false;
            }

            WorldId = worldId.Trim();
            worldState.Clear();

            State = PersistentWorldLifecycleState.Ready;

            return true;
        }

        public bool StartWorld()
        {
            if (State != PersistentWorldLifecycleState.Ready)
            {
                return false;
            }

            State = PersistentWorldLifecycleState.Running;

            return true;
        }

        public bool SetState(
            string key,
            string value)
        {
            if (State != PersistentWorldLifecycleState.Running ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            worldState[key.Trim()] =
                value ?? string.Empty;

            return true;
        }

        public bool TryGetState(
            string key,
            out string value)
        {
            value = string.Empty;

            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return worldState.TryGetValue(
                key.Trim(),
                out value);
        }

        public bool BeginSave()
        {
            if (State != PersistentWorldLifecycleState.Running)
            {
                return false;
            }

            State = PersistentWorldLifecycleState.Saving;

            return true;
        }

        public bool CompleteSave()
        {
            if (State != PersistentWorldLifecycleState.Saving)
            {
                return false;
            }

            State = PersistentWorldLifecycleState.Running;

            return true;
        }

        public bool BeginRecovery()
        {
            if (!Initialized ||
                State == PersistentWorldLifecycleState.Recovering)
            {
                return false;
            }

            State = PersistentWorldLifecycleState.Recovering;

            return true;
        }

        public bool CompleteRecovery()
        {
            if (State != PersistentWorldLifecycleState.Recovering)
            {
                return false;
            }

            State = PersistentWorldLifecycleState.Running;

            return true;
        }

        public IReadOnlyDictionary<string, string>
            GetWorldState()
        {
            return worldState;
        }

        public void Reset()
        {
            worldState.Clear();
            WorldId = string.Empty;
            State =
                PersistentWorldLifecycleState.Uninitialized;
        }
    }
}

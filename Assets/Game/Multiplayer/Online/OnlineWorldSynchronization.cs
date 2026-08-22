using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public sealed class OnlineWorldState
    {
        public long SimulationTick { get; private set; }

        public bool Running { get; private set; }

        public bool Update(
            long simulationTick,
            bool running)
        {
            if (simulationTick < 0)
            {
                return false;
            }

            SimulationTick = simulationTick;
            Running = running;

            return true;
        }

        public bool Advance()
        {
            if (!Running)
            {
                return false;
            }

            SimulationTick++;

            return true;
        }

        public void Start()
        {
            Running = true;
        }

        public void Stop()
        {
            Running = false;
        }
    }

    public sealed class OnlineWorldSynchronization
    {
        private readonly Dictionary<string, string> worldValues =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

        public OnlineWorldState State { get; } =
            new OnlineWorldState();

        public bool Initialized { get; private set; }

        public int ValueCount =>
            worldValues.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            worldValues.Clear();
            State.Update(0, false);
            Initialized = true;

            return true;
        }

        public bool SetWorldValue(
            string key,
            string value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            worldValues[key.Trim()] =
                value ?? string.Empty;

            return true;
        }

        public bool TryGetWorldValue(
            string key,
            out string value)
        {
            value = string.Empty;

            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return worldValues.TryGetValue(
                key.Trim(),
                out value);
        }

        public bool Synchronize(
            long simulationTick,
            bool running)
        {
            if (!Initialized)
            {
                return false;
            }

            return State.Update(
                simulationTick,
                running);
        }

        public bool AdvanceSimulation()
        {
            if (!Initialized)
            {
                return false;
            }

            return State.Advance();
        }

        public IReadOnlyDictionary<string, string>
            GetWorldValues()
        {
            return worldValues;
        }

        public void Reset()
        {
            worldValues.Clear();
            State.Update(0, false);
            Initialized = false;
        }
    }
}

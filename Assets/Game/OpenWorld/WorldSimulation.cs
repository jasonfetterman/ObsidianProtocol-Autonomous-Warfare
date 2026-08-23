using System;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum WorldSimulationState
    {
        Stopped,
        Running,
        Paused
    }

    public sealed class WorldSimulation
    {
        public bool Initialized { get; private set; }

        public WorldSimulationState State { get; private set; }

        public long CurrentTick { get; private set; }

        public float TickInterval { get; private set; }

        public WorldSimulation()
        {
            State =
                WorldSimulationState.Stopped;

            CurrentTick = 0;
            TickInterval = 1f;
        }

        public bool Initialize(
            float tickInterval)
        {
            if (Initialized ||
                tickInterval <= 0f)
            {
                return false;
            }

            TickInterval =
                tickInterval;

            CurrentTick = 0;

            State =
                WorldSimulationState.Stopped;

            Initialized = true;

            return true;
        }

        public bool Start()
        {
            if (!Initialized ||
                State ==
                    WorldSimulationState.Running)
            {
                return false;
            }

            State =
                WorldSimulationState.Running;

            return true;
        }

        public bool Pause()
        {
            if (!Initialized ||
                State !=
                    WorldSimulationState.Running)
            {
                return false;
            }

            State =
                WorldSimulationState.Paused;

            return true;
        }

        public bool Resume()
        {
            if (!Initialized ||
                State !=
                    WorldSimulationState.Paused)
            {
                return false;
            }

            State =
                WorldSimulationState.Running;

            return true;
        }

        public bool Stop()
        {
            if (!Initialized)
            {
                return false;
            }

            State =
                WorldSimulationState.Stopped;

            return true;
        }

        public bool Advance(
            long ticks)
        {
            if (!Initialized ||
                State !=
                    WorldSimulationState.Running ||
                ticks <= 0)
            {
                return false;
            }

            if (CurrentTick >
                long.MaxValue - ticks)
            {
                return false;
            }

            CurrentTick += ticks;

            return true;
        }

        public bool SetTick(
            long tick)
        {
            if (!Initialized ||
                tick < CurrentTick)
            {
                return false;
            }

            CurrentTick =
                tick;

            return true;
        }

        public void Reset()
        {
            Initialized = false;

            State =
                WorldSimulationState.Stopped;

            CurrentTick = 0;
            TickInterval = 1f;
        }
    }
}

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class SimulationTick
    {
        public string TickId { get; }

        public float Interval { get; private set; }

        public float Accumulator { get; private set; }

        public long TickCount { get; private set; }

        public bool Enabled { get; private set; }

        public SimulationTick(
            string tickId,
            float interval)
        {
            TickId =
                tickId ?? string.Empty;

            Interval =
                Math.Max(
                    0.001f,
                    interval);

            Accumulator = 0f;
            TickCount = 0;
            Enabled = true;
        }

        public bool SetInterval(
            float interval)
        {
            if (interval <= 0f)
            {
                return false;
            }

            Interval =
                interval;

            Accumulator =
                Math.Min(
                    Accumulator,
                    Interval);

            return true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }

        public int Update(
            float deltaTime)
        {
            if (!Enabled ||
                deltaTime <= 0f)
            {
                return 0;
            }

            Accumulator +=
                deltaTime;

            int ticks = 0;

            while (Accumulator >= Interval)
            {
                Accumulator -=
                    Interval;

                TickCount++;

                ticks++;
            }

            return ticks;
        }
    }

    public sealed class TickManagement
    {
        private readonly Dictionary<
            string,
            SimulationTick> ticks =
            new Dictionary<
                string,
                SimulationTick>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TickGroupCount =>
            ticks.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            ticks.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterTick(
            string tickId,
            float interval)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(tickId) ||
                interval <= 0f)
            {
                return false;
            }

            string id =
                tickId.Trim();

            if (ticks.ContainsKey(id))
            {
                return false;
            }

            ticks.Add(
                id,
                new SimulationTick(
                    id,
                    interval));

            return true;
        }

        public bool SetInterval(
            string tickId,
            float interval)
        {
            SimulationTick tick =
                GetTick(tickId);

            return tick != null &&
                   tick.SetInterval(interval);
        }

        public bool SetEnabled(
            string tickId,
            bool enabled)
        {
            SimulationTick tick =
                GetTick(tickId);

            return tick != null &&
                   tick.SetEnabled(enabled);
        }

        public int UpdateTick(
            string tickId,
            float deltaTime)
        {
            SimulationTick tick =
                GetTick(tickId);

            return tick == null
                ? 0
                : tick.Update(deltaTime);
        }

        public int UpdateAll(
            float deltaTime)
        {
            if (!Initialized ||
                deltaTime <= 0f)
            {
                return 0;
            }

            int totalTicks = 0;

            foreach (SimulationTick tick
                     in ticks.Values)
            {
                totalTicks +=
                    tick.Update(deltaTime);
            }

            return totalTicks;
        }

        public SimulationTick GetTick(
            string tickId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(tickId))
            {
                return null;
            }

            ticks.TryGetValue(
                tickId.Trim(),
                out SimulationTick tick);

            return tick;
        }

        public IReadOnlyCollection<SimulationTick>
            GetTicks()
        {
            return ticks.Values;
        }

        public void Reset()
        {
            ticks.Clear();

            Initialized = false;
        }
    }
}

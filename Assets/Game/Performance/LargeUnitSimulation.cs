using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class SimulatedUnit
    {
        public string UnitId { get; }

        public bool Active { get; private set; }

        public float SimulationPriority { get; private set; }

        public float LastSimulationTime { get; private set; }

        public SimulatedUnit(
            string unitId,
            float simulationPriority)
        {
            UnitId =
                unitId ?? string.Empty;

            SimulationPriority =
                Math.Max(
                    0f,
                    simulationPriority);

            Active = true;
            LastSimulationTime = 0f;
        }

        public bool SetPriority(
            float priority)
        {
            if (priority < 0f)
            {
                return false;
            }

            SimulationPriority =
                priority;

            return true;
        }

        public bool SetActive(
            bool active)
        {
            Active = active;

            return true;
        }

        public void RecordSimulation(
            float simulationTime)
        {
            LastSimulationTime =
                Math.Max(
                    0f,
                    simulationTime);
        }
    }

    public sealed class LargeUnitSimulation
    {
        private readonly Dictionary<
            string,
            SimulatedUnit> units =
            new Dictionary<
                string,
                SimulatedUnit>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int UnitCount =>
            units.Count;

        public int ActiveUnitCount
        {
            get
            {
                int count = 0;

                foreach (SimulatedUnit unit
                         in units.Values)
                {
                    if (unit.Active)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            units.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterUnit(
            string unitId,
            float simulationPriority)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId) ||
                simulationPriority < 0f)
            {
                return false;
            }

            string id =
                unitId.Trim();

            if (units.ContainsKey(id))
            {
                return false;
            }

            units.Add(
                id,
                new SimulatedUnit(
                    id,
                    simulationPriority));

            return true;
        }

        public bool RemoveUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return units.Remove(
                unitId.Trim());
        }

        public bool SetUnitPriority(
            string unitId,
            float priority)
        {
            SimulatedUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetPriority(priority);
        }

        public bool SetUnitActive(
            string unitId,
            bool active)
        {
            SimulatedUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetActive(active);
        }

        public bool RecordSimulation(
            string unitId,
            float simulationTime)
        {
            SimulatedUnit unit =
                GetUnit(unitId);

            if (unit == null ||
                !unit.Active)
            {
                return false;
            }

            unit.RecordSimulation(
                simulationTime);

            return true;
        }

        public SimulatedUnit GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            units.TryGetValue(
                unitId.Trim(),
                out SimulatedUnit unit);

            return unit;
        }

        public IReadOnlyCollection<SimulatedUnit>
            GetUnits()
        {
            return units.Values;
        }

        public void Reset()
        {
            units.Clear();

            Initialized = false;
        }
    }
}

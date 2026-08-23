using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public sealed class VerticalSliceAirUnit
    {
        public string UnitId { get; }

        public string UnitType { get; }

        public bool Deployed { get; private set; }

        public bool Operational { get; private set; }

        public bool Airborne { get; private set; }

        public VerticalSliceAirUnit(
            string unitId,
            string unitType)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitType =
                unitType ?? string.Empty;

            Deployed = false;
            Operational = true;
            Airborne = false;
        }

        public bool Deploy()
        {
            if (Deployed ||
                !Operational)
            {
                return false;
            }

            Deployed = true;

            return true;
        }

        public bool SetAirborne(
            bool airborne)
        {
            if (!Deployed ||
                !Operational)
            {
                return false;
            }

            Airborne =
                airborne;

            return true;
        }

        public bool SetOperational(
            bool operational)
        {
            Operational =
                operational;

            if (!Operational)
            {
                Deployed = false;
                Airborne = false;
            }

            return true;
        }
    }

    public sealed class VerticalSliceAirUnits
    {
        private readonly Dictionary<
            string,
            VerticalSliceAirUnit> units =
            new Dictionary<
                string,
                VerticalSliceAirUnit>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int UnitCount =>
            units.Count;

        public int AirborneUnitCount
        {
            get
            {
                int count = 0;

                foreach (VerticalSliceAirUnit unit
                         in units.Values)
                {
                    if (unit.Airborne)
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
            string unitType)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId) ||
                string.IsNullOrWhiteSpace(unitType))
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
                new VerticalSliceAirUnit(
                    id,
                    unitType.Trim()));

            return true;
        }

        public bool DeployUnit(
            string unitId)
        {
            VerticalSliceAirUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.Deploy();
        }

        public bool SetAirborne(
            string unitId,
            bool airborne)
        {
            VerticalSliceAirUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetAirborne(airborne);
        }

        public bool SetOperational(
            string unitId,
            bool operational)
        {
            VerticalSliceAirUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetOperational(operational);
        }

        public VerticalSliceAirUnit GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            units.TryGetValue(
                unitId.Trim(),
                out VerticalSliceAirUnit unit);

            return unit;
        }

        public IReadOnlyCollection<
            VerticalSliceAirUnit>
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

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public sealed class VerticalSliceSeaUnit
    {
        public string UnitId { get; }

        public string UnitType { get; }

        public bool Deployed { get; private set; }

        public bool Operational { get; private set; }

        public bool Underway { get; private set; }

        public VerticalSliceSeaUnit(
            string unitId,
            string unitType)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitType =
                unitType ?? string.Empty;

            Deployed = false;
            Operational = true;
            Underway = false;
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

        public bool SetUnderway(
            bool underway)
        {
            if (!Deployed ||
                !Operational)
            {
                return false;
            }

            Underway =
                underway;

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
                Underway = false;
            }

            return true;
        }
    }

    public sealed class VerticalSliceSeaUnits
    {
        private readonly Dictionary<
            string,
            VerticalSliceSeaUnit> units =
            new Dictionary<
                string,
                VerticalSliceSeaUnit>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int UnitCount =>
            units.Count;

        public int UnderwayUnitCount
        {
            get
            {
                int count = 0;

                foreach (VerticalSliceSeaUnit unit
                         in units.Values)
                {
                    if (unit.Underway)
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
                new VerticalSliceSeaUnit(
                    id,
                    unitType.Trim()));

            return true;
        }

        public bool DeployUnit(
            string unitId)
        {
            VerticalSliceSeaUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.Deploy();
        }

        public bool SetUnderway(
            string unitId,
            bool underway)
        {
            VerticalSliceSeaUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetUnderway(underway);
        }

        public bool SetOperational(
            string unitId,
            bool operational)
        {
            VerticalSliceSeaUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetOperational(operational);
        }

        public VerticalSliceSeaUnit GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            units.TryGetValue(
                unitId.Trim(),
                out VerticalSliceSeaUnit unit);

            return unit;
        }

        public IReadOnlyCollection<
            VerticalSliceSeaUnit>
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

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public sealed class VerticalSliceGroundUnit
    {
        public string UnitId { get; }

        public string UnitType { get; }

        public bool Deployed { get; private set; }

        public bool Operational { get; private set; }

        public bool Moving { get; private set; }

        public VerticalSliceGroundUnit(
            string unitId,
            string unitType)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitType =
                unitType ?? string.Empty;

            Deployed = false;
            Operational = true;
            Moving = false;
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

        public bool SetMoving(
            bool moving)
        {
            if (!Deployed ||
                !Operational)
            {
                return false;
            }

            Moving =
                moving;

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
                Moving = false;
            }

            return true;
        }
    }

    public sealed class VerticalSliceGroundUnits
    {
        private readonly Dictionary<
            string,
            VerticalSliceGroundUnit> units =
            new Dictionary<
                string,
                VerticalSliceGroundUnit>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int UnitCount =>
            units.Count;

        public int MovingUnitCount
        {
            get
            {
                int count = 0;

                foreach (VerticalSliceGroundUnit unit
                         in units.Values)
                {
                    if (unit.Moving)
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
                new VerticalSliceGroundUnit(
                    id,
                    unitType.Trim()));

            return true;
        }

        public bool DeployUnit(
            string unitId)
        {
            VerticalSliceGroundUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.Deploy();
        }

        public bool SetMoving(
            string unitId,
            bool moving)
        {
            VerticalSliceGroundUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetMoving(moving);
        }

        public bool SetOperational(
            string unitId,
            bool operational)
        {
            VerticalSliceGroundUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetOperational(operational);
        }

        public VerticalSliceGroundUnit GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            units.TryGetValue(
                unitId.Trim(),
                out VerticalSliceGroundUnit unit);

            return unit;
        }

        public IReadOnlyCollection<
            VerticalSliceGroundUnit>
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

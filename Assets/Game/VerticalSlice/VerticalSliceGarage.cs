using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public sealed class VerticalSliceGarageUnit
    {
        public string UnitId { get; }

        public string UnitType { get; }

        public bool Stored { get; private set; }

        public bool Configured { get; private set; }

        public VerticalSliceGarageUnit(
            string unitId,
            string unitType)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitType =
                unitType ?? string.Empty;

            Stored = true;
            Configured = false;
        }

        public bool SetStored(
            bool stored)
        {
            Stored = stored;

            return true;
        }

        public bool Configure()
        {
            if (!Stored)
            {
                return false;
            }

            Configured = true;

            return true;
        }
    }

    public sealed class VerticalSliceGarage
    {
        private readonly Dictionary<
            string,
            VerticalSliceGarageUnit> units =
            new Dictionary<
                string,
                VerticalSliceGarageUnit>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Open { get; private set; }

        public int UnitCount =>
            units.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            units.Clear();

            Open = false;
            Initialized = true;

            return true;
        }

        public bool Enter()
        {
            if (!Initialized)
            {
                return false;
            }

            Open = true;

            return true;
        }

        public bool Exit()
        {
            if (!Initialized)
            {
                return false;
            }

            Open = false;

            return true;
        }

        public bool RegisterUnit(
            string unitId,
            string unitType)
        {
            if (!Initialized ||
                !Open ||
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
                new VerticalSliceGarageUnit(
                    id,
                    unitType.Trim()));

            return true;
        }

        public bool ConfigureUnit(
            string unitId)
        {
            VerticalSliceGarageUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.Configure();
        }

        public VerticalSliceGarageUnit GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            units.TryGetValue(
                unitId.Trim(),
                out VerticalSliceGarageUnit unit);

            return unit;
        }

        public IReadOnlyCollection<
            VerticalSliceGarageUnit>
            GetUnits()
        {
            return units.Values;
        }

        public void Reset()
        {
            units.Clear();

            Open = false;
            Initialized = false;
        }
    }
}

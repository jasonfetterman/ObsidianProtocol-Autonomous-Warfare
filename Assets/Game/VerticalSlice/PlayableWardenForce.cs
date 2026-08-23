using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public sealed class WardenForceUnit
    {
        public string UnitId { get; }

        public string UnitType { get; }

        public bool Deployed { get; private set; }

        public bool Operational { get; private set; }

        public WardenForceUnit(
            string unitId,
            string unitType)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitType =
                unitType ?? string.Empty;

            Deployed = false;
            Operational = true;
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

        public bool Recall()
        {
            if (!Deployed)
            {
                return false;
            }

            Deployed = false;

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
            }

            return true;
        }
    }

    public sealed class PlayableWardenForce
    {
        private readonly Dictionary<
            string,
            WardenForceUnit> units =
            new Dictionary<
                string,
                WardenForceUnit>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Playable { get; private set; }

        public int UnitCount =>
            units.Count;

        public int DeployedUnitCount
        {
            get
            {
                int count = 0;

                foreach (WardenForceUnit unit
                         in units.Values)
                {
                    if (unit.Deployed)
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

            Playable = false;
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
                new WardenForceUnit(
                    id,
                    unitType.Trim()));

            return true;
        }

        public bool DeployUnit(
            string unitId)
        {
            WardenForceUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.Deploy();
        }

        public bool RecallUnit(
            string unitId)
        {
            WardenForceUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.Recall();
        }

        public bool SetUnitOperational(
            string unitId,
            bool operational)
        {
            WardenForceUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetOperational(operational);
        }

        public bool SetPlayable(
            bool playable)
        {
            if (!Initialized)
            {
                return false;
            }

            Playable =
                playable;

            return true;
        }

        public WardenForceUnit GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            units.TryGetValue(
                unitId.Trim(),
                out WardenForceUnit unit);

            return unit;
        }

        public IReadOnlyCollection<
            WardenForceUnit>
            GetUnits()
        {
            return units.Values;
        }

        public void Reset()
        {
            units.Clear();

            Playable = false;
            Initialized = false;
        }
    }
}

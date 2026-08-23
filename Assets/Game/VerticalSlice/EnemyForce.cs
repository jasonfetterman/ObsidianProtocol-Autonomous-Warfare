using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum EnemyForceState
    {
        Inactive,
        Active,
        Defeated
    }

    public sealed class EnemyForceUnit
    {
        public string UnitId { get; }

        public string UnitType { get; }

        public bool Deployed { get; private set; }

        public bool Operational { get; private set; }

        public EnemyForceUnit(
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

    public sealed class EnemyForce
    {
        private readonly Dictionary<
            string,
            EnemyForceUnit> units =
            new Dictionary<
                string,
                EnemyForceUnit>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public EnemyForceState State { get; private set; }

        public int UnitCount =>
            units.Count;

        public int DeployedUnitCount
        {
            get
            {
                int count = 0;

                foreach (EnemyForceUnit unit
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

            State =
                EnemyForceState.Inactive;

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
                new EnemyForceUnit(
                    id,
                    unitType.Trim()));

            return true;
        }

        public bool Activate()
        {
            if (!Initialized ||
                State == EnemyForceState.Defeated)
            {
                return false;
            }

            State =
                EnemyForceState.Active;

            return true;
        }

        public bool DeployUnit(
            string unitId)
        {
            if (State != EnemyForceState.Active)
            {
                return false;
            }

            EnemyForceUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.Deploy();
        }

        public bool SetUnitOperational(
            string unitId,
            bool operational)
        {
            EnemyForceUnit unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.SetOperational(operational);
        }

        public bool CheckDefeated()
        {
            if (!Initialized ||
                State == EnemyForceState.Defeated)
            {
                return State ==
                       EnemyForceState.Defeated;
            }

            foreach (EnemyForceUnit unit
                     in units.Values)
            {
                if (unit.Operational)
                {
                    return false;
                }
            }

            State =
                EnemyForceState.Defeated;

            return true;
        }

        public EnemyForceUnit GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            units.TryGetValue(
                unitId.Trim(),
                out EnemyForceUnit unit);

            return unit;
        }

        public IReadOnlyCollection<
            EnemyForceUnit>
            GetUnits()
        {
            return units.Values;
        }

        public void Reset()
        {
            units.Clear();

            State =
                EnemyForceState.Inactive;

            Initialized = false;
        }
    }
}

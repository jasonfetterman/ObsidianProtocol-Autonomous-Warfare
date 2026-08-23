using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum PersistentArmyState
    {
        Reserve,
        Deployed,
        Moving,
        Engaged,
        Retreating,
        Destroyed
    }

    public sealed class PersistentArmyRecord
    {
        private readonly HashSet<string> units =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public string ArmyId { get; }

        public string OwnerId { get; }

        public string RegionId { get; private set; }

        public PersistentArmyState State { get; private set; }

        public int UnitCount =>
            units.Count;

        public long LastUpdateTick { get; private set; }

        public PersistentArmyRecord(
            string armyId,
            string ownerId,
            string regionId)
        {
            ArmyId =
                armyId ?? string.Empty;

            OwnerId =
                ownerId ?? string.Empty;

            RegionId =
                regionId ?? string.Empty;

            State =
                PersistentArmyState.Reserve;

            LastUpdateTick = 0;
        }

        public bool AddUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId) ||
                State ==
                    PersistentArmyState.Destroyed)
            {
                return false;
            }

            return units.Add(unitId.Trim());
        }

        public bool RemoveUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return units.Remove(unitId.Trim());
        }

        public bool SetRegion(
            string regionId,
            long updateTick)
        {
            if (string.IsNullOrWhiteSpace(regionId) ||
                updateTick < LastUpdateTick ||
                State ==
                    PersistentArmyState.Destroyed)
            {
                return false;
            }

            RegionId =
                regionId.Trim();

            LastUpdateTick =
                updateTick;

            return true;
        }

        public bool SetState(
            PersistentArmyState state,
            long updateTick)
        {
            if (updateTick < LastUpdateTick)
            {
                return false;
            }

            if (State ==
                    PersistentArmyState.Destroyed &&
                state !=
                    PersistentArmyState.Destroyed)
            {
                return false;
            }

            State = state;
            LastUpdateTick = updateTick;

            if (state ==
                PersistentArmyState.Destroyed)
            {
                units.Clear();
            }

            return true;
        }

        public IReadOnlyCollection<string>
            GetUnits()
        {
            return units;
        }
    }

    public sealed class PersistentArmies
    {
        private readonly Dictionary<
            string,
            PersistentArmyRecord> armies =
            new Dictionary<
                string,
                PersistentArmyRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ArmyCount =>
            armies.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            armies.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterArmy(
            string armyId,
            string ownerId,
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(armyId) ||
                string.IsNullOrWhiteSpace(ownerId) ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return false;
            }

            string id =
                armyId.Trim();

            if (armies.ContainsKey(id))
            {
                return false;
            }

            armies.Add(
                id,
                new PersistentArmyRecord(
                    id,
                    ownerId.Trim(),
                    regionId.Trim()));

            return true;
        }

        public bool AddUnit(
            string armyId,
            string unitId)
        {
            PersistentArmyRecord army =
                GetArmy(armyId);

            return army != null &&
                   army.AddUnit(unitId);
        }

        public bool RemoveUnit(
            string armyId,
            string unitId)
        {
            PersistentArmyRecord army =
                GetArmy(armyId);

            return army != null &&
                   army.RemoveUnit(unitId);
        }

        public bool MoveArmy(
            string armyId,
            string regionId,
            long updateTick)
        {
            PersistentArmyRecord army =
                GetArmy(armyId);

            if (army == null)
            {
                return false;
            }

            if (!army.SetRegion(
                    regionId,
                    updateTick))
            {
                return false;
            }

            return army.SetState(
                PersistentArmyState.Moving,
                updateTick);
        }

        public bool SetArmyState(
            string armyId,
            PersistentArmyState state,
            long updateTick)
        {
            PersistentArmyRecord army =
                GetArmy(armyId);

            return army != null &&
                   army.SetState(
                       state,
                       updateTick);
        }

        public PersistentArmyRecord GetArmy(
            string armyId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(armyId))
            {
                return null;
            }

            armies.TryGetValue(
                armyId.Trim(),
                out PersistentArmyRecord army);

            return army;
        }

        public IReadOnlyCollection<
            PersistentArmyRecord>
            GetArmies()
        {
            return armies.Values;
        }

        public void Reset()
        {
            armies.Clear();
            Initialized = false;
        }
    }
}

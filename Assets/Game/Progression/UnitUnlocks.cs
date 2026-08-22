using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class UnitUnlockRequirement
    {
        public int RequiredPlayerLevel { get; }
        public int RequiredFleetLevel { get; }
        public string RequiredTechnologyId { get; }
        public int RequiredTechnologyLevel { get; }

        public UnitUnlockRequirement(
            int requiredPlayerLevel = 1,
            int requiredFleetLevel = 1,
            string requiredTechnologyId = "",
            int requiredTechnologyLevel = 0)
        {
            RequiredPlayerLevel =
                Math.Max(1, requiredPlayerLevel);

            RequiredFleetLevel =
                Math.Max(1, requiredFleetLevel);

            RequiredTechnologyId =
                requiredTechnologyId ?? string.Empty;

            RequiredTechnologyLevel =
                Math.Max(0, requiredTechnologyLevel);
        }
    }

    public sealed class UnitUnlock
    {
        public string UnitId { get; }

        public UnitUnlockRequirement Requirement { get; }

        public bool Unlocked
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(UnitId) &&
            Requirement != null;

        public UnitUnlock(
            string unitId,
            UnitUnlockRequirement requirement)
        {
            UnitId =
                unitId ?? string.Empty;

            Requirement =
                requirement ??
                new UnitUnlockRequirement();

            Unlocked = false;
        }

        public bool Evaluate(
            int playerLevel,
            int fleetLevel,
            int technologyLevel)
        {
            if (!Valid)
                return false;

            bool playerValid =
                playerLevel >=
                Requirement.RequiredPlayerLevel;

            bool fleetValid =
                fleetLevel >=
                Requirement.RequiredFleetLevel;

            bool technologyValid =
                technologyLevel >=
                Requirement.RequiredTechnologyLevel;

            Unlocked =
                playerValid &&
                fleetValid &&
                technologyValid;

            return Unlocked;
        }

        public void ForceUnlock()
        {
            Unlocked = true;
        }

        public void Lock()
        {
            Unlocked = false;
        }
    }

    public sealed class UnitUnlocks
    {
        private readonly Dictionary<
            string,
            UnitUnlock> unlocks =
            new Dictionary<
                string,
                UnitUnlock>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            UnitUnlock unlock)
        {
            if (unlock == null ||
                !unlock.Valid ||
                unlocks.ContainsKey(
                    unlock.UnitId))
            {
                return false;
            }

            unlocks.Add(
                unlock.UnitId,
                unlock);

            return true;
        }

        public bool Remove(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(
                    unitId))
            {
                return false;
            }

            return unlocks.Remove(
                unitId);
        }

        public bool TryGet(
            string unitId,
            out UnitUnlock unlock)
        {
            return unlocks.TryGetValue(
                unitId,
                out unlock);
        }

        public bool IsUnlocked(
            string unitId)
        {
            return unlocks.TryGetValue(
                       unitId,
                       out UnitUnlock unlock) &&
                   unlock.Unlocked;
        }

        public bool Evaluate(
            string unitId,
            int playerLevel,
            int fleetLevel,
            int technologyLevel)
        {
            if (!unlocks.TryGetValue(
                    unitId,
                    out UnitUnlock unlock))
            {
                return false;
            }

            return unlock.Evaluate(
                playerLevel,
                fleetLevel,
                technologyLevel);
        }

        public IReadOnlyCollection<
            UnitUnlock>
            GetUnlocks()
        {
            return unlocks.Values;
        }

        public void Clear()
        {
            unlocks.Clear();
        }
    }
}

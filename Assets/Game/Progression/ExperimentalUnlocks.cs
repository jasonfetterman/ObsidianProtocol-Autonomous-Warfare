using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class ExperimentalUnlockRequirement
    {
        public int RequiredPlayerLevel { get; }
        public int RequiredFleetLevel { get; }
        public int RequiredTechnologyLevel { get; }
        public int RequiredResearchLevel { get; }

        public ExperimentalUnlockRequirement(
            int requiredPlayerLevel = 10,
            int requiredFleetLevel = 5,
            int requiredTechnologyLevel = 5,
            int requiredResearchLevel = 5)
        {
            RequiredPlayerLevel =
                Math.Max(1, requiredPlayerLevel);

            RequiredFleetLevel =
                Math.Max(1, requiredFleetLevel);

            RequiredTechnologyLevel =
                Math.Max(1, requiredTechnologyLevel);

            RequiredResearchLevel =
                Math.Max(1, requiredResearchLevel);
        }
    }

    public sealed class ExperimentalUnlock
    {
        public string UnitId { get; }

        public ExperimentalUnlockRequirement Requirement { get; }

        public bool Unlocked
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(UnitId) &&
            Requirement != null;

        public ExperimentalUnlock(
            string unitId,
            ExperimentalUnlockRequirement requirement)
        {
            UnitId =
                unitId ?? string.Empty;

            Requirement =
                requirement ??
                new ExperimentalUnlockRequirement();

            Unlocked = false;
        }

        public bool Evaluate(
            int playerLevel,
            int fleetLevel,
            int technologyLevel,
            int researchLevel)
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

            bool researchValid =
                researchLevel >=
                Requirement.RequiredResearchLevel;

            Unlocked =
                playerValid &&
                fleetValid &&
                technologyValid &&
                researchValid;

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

    public sealed class ExperimentalUnlocks
    {
        private readonly Dictionary<
            string,
            ExperimentalUnlock> unlocks =
            new Dictionary<
                string,
                ExperimentalUnlock>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            ExperimentalUnlock unlock)
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
            out ExperimentalUnlock unlock)
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
                       out ExperimentalUnlock unlock) &&
                   unlock.Unlocked;
        }

        public bool Evaluate(
            string unitId,
            int playerLevel,
            int fleetLevel,
            int technologyLevel,
            int researchLevel)
        {
            if (!unlocks.TryGetValue(
                    unitId,
                    out ExperimentalUnlock unlock))
            {
                return false;
            }

            return unlock.Evaluate(
                playerLevel,
                fleetLevel,
                technologyLevel,
                researchLevel);
        }

        public IReadOnlyCollection<
            ExperimentalUnlock>
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

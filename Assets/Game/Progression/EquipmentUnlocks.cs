using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class EquipmentUnlockRequirement
    {
        public int RequiredPlayerLevel { get; }
        public int RequiredFleetLevel { get; }
        public string RequiredTechnologyId { get; }
        public int RequiredTechnologyLevel { get; }

        public EquipmentUnlockRequirement(
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

    public sealed class EquipmentUnlock
    {
        public string EquipmentId { get; }

        public EquipmentUnlockRequirement Requirement { get; }

        public bool Unlocked
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(EquipmentId) &&
            Requirement != null;

        public EquipmentUnlock(
            string equipmentId,
            EquipmentUnlockRequirement requirement)
        {
            EquipmentId =
                equipmentId ?? string.Empty;

            Requirement =
                requirement ??
                new EquipmentUnlockRequirement();

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

    public sealed class EquipmentUnlocks
    {
        private readonly Dictionary<
            string,
            EquipmentUnlock> unlocks =
            new Dictionary<
                string,
                EquipmentUnlock>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            EquipmentUnlock unlock)
        {
            if (unlock == null ||
                !unlock.Valid ||
                unlocks.ContainsKey(
                    unlock.EquipmentId))
            {
                return false;
            }

            unlocks.Add(
                unlock.EquipmentId,
                unlock);

            return true;
        }

        public bool Remove(
            string equipmentId)
        {
            if (string.IsNullOrWhiteSpace(
                    equipmentId))
            {
                return false;
            }

            return unlocks.Remove(
                equipmentId);
        }

        public bool TryGet(
            string equipmentId,
            out EquipmentUnlock unlock)
        {
            return unlocks.TryGetValue(
                equipmentId,
                out unlock);
        }

        public bool IsUnlocked(
            string equipmentId)
        {
            return unlocks.TryGetValue(
                       equipmentId,
                       out EquipmentUnlock unlock) &&
                   unlock.Unlocked;
        }

        public bool Evaluate(
            string equipmentId,
            int playerLevel,
            int fleetLevel,
            int technologyLevel)
        {
            if (!unlocks.TryGetValue(
                    equipmentId,
                    out EquipmentUnlock unlock))
            {
                return false;
            }

            return unlock.Evaluate(
                playerLevel,
                fleetLevel,
                technologyLevel);
        }

        public IReadOnlyCollection<
            EquipmentUnlock>
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

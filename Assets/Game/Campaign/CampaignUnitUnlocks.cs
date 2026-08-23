using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignUnitUnlocks
    {
        private readonly HashSet<string> unlockedUnits =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int UnlockedUnitCount =>
            unlockedUnits.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            unlockedUnits.Clear();
            Initialized = true;

            return true;
        }

        public bool UnlockUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return unlockedUnits.Add(
                unitId.Trim());
        }

        public bool IsUnitUnlocked(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return unlockedUnits.Contains(
                unitId.Trim());
        }

        public bool LockUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return unlockedUnits.Remove(
                unitId.Trim());
        }

        public IReadOnlyCollection<string>
            GetUnlockedUnits()
        {
            return unlockedUnits;
        }

        public void Reset()
        {
            unlockedUnits.Clear();
            Initialized = false;
        }
    }
}

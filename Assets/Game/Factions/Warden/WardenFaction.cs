using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Factions.Warden
{
    public sealed class WardenFaction
    {
        public const string FactionId = "WARDEN";
        public const string DisplayName = "Warden";

        public bool IsUnlocked { get; private set; }

        private readonly HashSet<string> registeredUnits =
            new HashSet<string>();

        public WardenFaction()
        {
            IsUnlocked = true;
        }

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            registeredUnits.Add(unitId);
        }

        public bool ContainsUnit(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId) &&
                   registeredUnits.Contains(unitId);
        }

        public IReadOnlyCollection<string> GetRegisteredUnits()
        {
            return registeredUnits;
        }

        public void SetUnlocked(bool unlocked)
        {
            IsUnlocked = unlocked;
        }

        public void ClearUnits()
        {
            registeredUnits.Clear();
        }
    }
}

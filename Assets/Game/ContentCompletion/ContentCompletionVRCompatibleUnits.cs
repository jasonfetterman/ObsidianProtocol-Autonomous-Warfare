using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionVRCompatibleUnits
    {
        private readonly HashSet<string> units =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int UnitCount =>
            units.Count;

        public bool Complete =>
            UnitCount > 0;

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
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return units.Add(
                unitId.Trim());
        }

        public bool ContainsUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return units.Contains(
                unitId.Trim());
        }

        public IReadOnlyCollection<string>
            GetUnits()
        {
            return units;
        }

        public void Reset()
        {
            units.Clear();
            Initialized = false;
        }
    }
}

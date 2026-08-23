using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public sealed class CompatibleUnitEntry
    {
        private readonly HashSet<string> compatibleUnits =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int CompatibleUnitCount =>
            compatibleUnits.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            compatibleUnits.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterCompatibleUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return compatibleUnits.Add(
                unitId.Trim());
        }

        public bool RemoveCompatibleUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return compatibleUnits.Remove(
                unitId.Trim());
        }

        public bool IsCompatible(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return compatibleUnits.Contains(
                unitId.Trim());
        }

        public IReadOnlyCollection<string>
            GetCompatibleUnits()
        {
            return compatibleUnits;
        }

        public void Reset()
        {
            compatibleUnits.Clear();
            Initialized = false;
        }
    }
}

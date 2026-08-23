using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public sealed class VerticalSliceVROperatorMode
    {
        private readonly HashSet<string> compatibleUnits =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Active { get; private set; }

        public string CurrentUnitId { get; private set; }

        public int CompatibleUnitCount =>
            compatibleUnits.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            compatibleUnits.Clear();

            Active = false;
            CurrentUnitId = null;
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

        public bool EnterUnit(
            string unitId)
        {
            if (!Initialized ||
                Active ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            string id =
                unitId.Trim();

            if (!compatibleUnits.Contains(id))
            {
                return false;
            }

            CurrentUnitId = id;
            Active = true;

            return true;
        }

        public bool ExitUnit()
        {
            if (!Active)
            {
                return false;
            }

            Active = false;
            CurrentUnitId = null;

            return true;
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

            Active = false;
            CurrentUnitId = null;
            Initialized = false;
        }
    }
}

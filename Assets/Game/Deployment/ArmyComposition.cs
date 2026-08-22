using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class ArmyComposition
    {
        private readonly List<string> unitIds =
            new List<string>();

        public int UnitCount =>
            unitIds.Count;

        public bool AddUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            unitIds.Add(unitId);
            return true;
        }

        public bool RemoveUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return unitIds.Remove(unitId);
        }

        public bool ContainsUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return unitIds.Contains(unitId);
        }

        public IReadOnlyList<string> GetUnits()
        {
            return unitIds;
        }

        public void Clear()
        {
            unitIds.Clear();
        }
    }
}

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class MultiSelectionSystem
    {
        private readonly List<string> selectedUnitIds =
            new List<string>();

        public int SelectionCount =>
            selectedUnitIds.Count;

        public bool HasSelection =>
            selectedUnitIds.Count > 0;

        public bool AddUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            if (selectedUnitIds.Contains(unitId))
                return false;

            selectedUnitIds.Add(unitId);
            return true;
        }

        public bool RemoveUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return selectedUnitIds.Remove(unitId);
        }

        public bool IsSelected(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return selectedUnitIds.Contains(unitId);
        }

        public void SetSelection(
            IEnumerable<string> unitIds)
        {
            selectedUnitIds.Clear();

            if (unitIds == null)
                return;

            foreach (string unitId in unitIds)
            {
                if (string.IsNullOrWhiteSpace(unitId))
                    continue;

                if (!selectedUnitIds.Contains(unitId))
                    selectedUnitIds.Add(unitId);
            }
        }

        public IReadOnlyList<string> GetSelection()
        {
            return selectedUnitIds;
        }

        public void ClearSelection()
        {
            selectedUnitIds.Clear();
        }
    }
}

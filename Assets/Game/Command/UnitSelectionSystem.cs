using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class UnitSelectionSystem
    {
        private string selectedUnitId;

        public bool HasSelection =>
            !string.IsNullOrWhiteSpace(selectedUnitId);

        public string SelectedUnitId =>
            selectedUnitId ?? string.Empty;

        public bool SelectUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            selectedUnitId = unitId;
            return true;
        }

        public void ClearSelection()
        {
            selectedUnitId = string.Empty;
        }

        public bool IsSelected(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return string.Equals(
                selectedUnitId,
                unitId,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}

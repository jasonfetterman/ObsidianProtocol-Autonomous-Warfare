using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class SquadSelectionSystem
    {
        private readonly List<string> selectedSquadIds =
            new List<string>();

        public int SelectionCount =>
            selectedSquadIds.Count;

        public bool HasSelection =>
            selectedSquadIds.Count > 0;

        public bool SelectSquad(string squadId)
        {
            if (string.IsNullOrWhiteSpace(squadId))
                return false;

            selectedSquadIds.Clear();
            selectedSquadIds.Add(squadId);

            return true;
        }

        public bool AddSquad(string squadId)
        {
            if (string.IsNullOrWhiteSpace(squadId))
                return false;

            if (selectedSquadIds.Contains(squadId))
                return false;

            selectedSquadIds.Add(squadId);
            return true;
        }

        public bool RemoveSquad(string squadId)
        {
            if (string.IsNullOrWhiteSpace(squadId))
                return false;

            return selectedSquadIds.Remove(squadId);
        }

        public bool IsSelected(string squadId)
        {
            if (string.IsNullOrWhiteSpace(squadId))
                return false;

            return selectedSquadIds.Contains(squadId);
        }

        public void SetSelection(
            IEnumerable<string> squadIds)
        {
            selectedSquadIds.Clear();

            if (squadIds == null)
                return;

            foreach (string squadId in squadIds)
            {
                if (string.IsNullOrWhiteSpace(squadId))
                    continue;

                if (!selectedSquadIds.Contains(squadId))
                    selectedSquadIds.Add(squadId);
            }
        }

        public IReadOnlyList<string> GetSelection()
        {
            return selectedSquadIds;
        }

        public void ClearSelection()
        {
            selectedSquadIds.Clear();
        }
    }
}

using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public enum VisibilityState
    {
        Unknown,
        Explored,
        Visible
    }

    public sealed class FogOfWarCell
    {
        public int CellId;
        public VisibilityState State;

        public FogOfWarCell(int cellId)
        {
            CellId = cellId;
            State = VisibilityState.Unknown;
        }
    }

    public sealed class FogOfWarSystem
    {
        private readonly Dictionary<int, FogOfWarCell> cells =
            new Dictionary<int, FogOfWarCell>();

        public void RegisterCell(int cellId)
        {
            if (!cells.ContainsKey(cellId))
            {
                cells.Add(
                    cellId,
                    new FogOfWarCell(cellId));
            }
        }

        public void SetVisible(int cellId)
        {
            RegisterCell(cellId);
            cells[cellId].State =
                VisibilityState.Visible;
        }

        public void SetExplored(int cellId)
        {
            RegisterCell(cellId);

            if (cells[cellId].State !=
                VisibilityState.Visible)
            {
                cells[cellId].State =
                    VisibilityState.Explored;
            }
        }

        public void SetUnknown(int cellId)
        {
            RegisterCell(cellId);
            cells[cellId].State =
                VisibilityState.Unknown;
        }

        public VisibilityState GetVisibility(int cellId)
        {
            return cells.TryGetValue(
                       cellId,
                       out FogOfWarCell cell)
                ? cell.State
                : VisibilityState.Unknown;
        }

        public bool IsVisible(int cellId)
        {
            return GetVisibility(cellId) ==
                   VisibilityState.Visible;
        }

        public bool IsExplored(int cellId)
        {
            VisibilityState state =
                GetVisibility(cellId);

            return state ==
                       VisibilityState.Explored ||
                   state ==
                       VisibilityState.Visible;
        }

        public void Clear()
        {
            cells.Clear();
        }
    }
}

using System;

namespace ObsidianProtocol.Game.Command
{
    public sealed class RTSCommandInterface
    {
        public bool Visible { get; private set; }
        public bool CommandModeActive { get; private set; }
        public bool TacticalModeActive { get; private set; }

        public string SelectedUnitId { get; private set; }
        public string SelectedSquadId { get; private set; }
        public string ActiveCommand { get; private set; }
        public string StatusMessage { get; private set; }

        public RTSCommandInterface()
        {
            SelectedUnitId = string.Empty;
            SelectedSquadId = string.Empty;
            ActiveCommand = string.Empty;
            StatusMessage = string.Empty;
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
            CommandModeActive = false;
            TacticalModeActive = false;
        }

        public void EnterCommandMode()
        {
            if (!Visible)
                return;

            CommandModeActive = true;
        }

        public void ExitCommandMode()
        {
            CommandModeActive = false;
            ActiveCommand = string.Empty;
        }

        public void EnterTacticalMode()
        {
            if (!Visible)
                return;

            TacticalModeActive = true;
        }

        public void ExitTacticalMode()
        {
            TacticalModeActive = false;
        }

        public void SelectUnit(string unitId)
        {
            SelectedUnitId =
                unitId ?? string.Empty;

            SelectedSquadId = string.Empty;
        }

        public void SelectSquad(string squadId)
        {
            SelectedSquadId =
                squadId ?? string.Empty;

            SelectedUnitId = string.Empty;
        }

        public void SetCommand(string command)
        {
            ActiveCommand =
                command ?? string.Empty;
        }

        public void SetStatus(string message)
        {
            StatusMessage =
                message ?? string.Empty;
        }

        public void ClearSelection()
        {
            SelectedUnitId = string.Empty;
            SelectedSquadId = string.Empty;
        }

        public void Reset()
        {
            Visible = false;
            CommandModeActive = false;
            TacticalModeActive = false;

            SelectedUnitId = string.Empty;
            SelectedSquadId = string.Empty;
            ActiveCommand = string.Empty;
            StatusMessage = string.Empty;
        }
    }
}

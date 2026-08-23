using System;

namespace ObsidianProtocol.Game.VR
{
    public enum OperatorState
    {
        Offline,
        Ready,
        Entering,
        Active,
        Exiting
    }

    public sealed class OperatorFramework
    {
        public bool Initialized { get; private set; }

        public OperatorState State { get; private set; }

        public string OperatorId { get; private set; }

        public string CurrentUnitId { get; private set; }

        public bool IsOperatingUnit =>
            State == OperatorState.Active &&
            !string.IsNullOrWhiteSpace(CurrentUnitId);

        public bool Initialize(
            string operatorId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(operatorId))
            {
                return false;
            }

            OperatorId =
                operatorId.Trim();

            CurrentUnitId =
                string.Empty;

            State =
                OperatorState.Ready;

            Initialized = true;

            return true;
        }

        public bool BeginEntry(
            string unitId)
        {
            if (!Initialized ||
                State != OperatorState.Ready ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            CurrentUnitId =
                unitId.Trim();

            State =
                OperatorState.Entering;

            return true;
        }

        public bool CompleteEntry()
        {
            if (!Initialized ||
                State != OperatorState.Entering ||
                string.IsNullOrWhiteSpace(CurrentUnitId))
            {
                return false;
            }

            State =
                OperatorState.Active;

            return true;
        }

        public bool BeginExit()
        {
            if (!Initialized ||
                State != OperatorState.Active)
            {
                return false;
            }

            State =
                OperatorState.Exiting;

            return true;
        }

        public bool CompleteExit()
        {
            if (!Initialized ||
                State != OperatorState.Exiting)
            {
                return false;
            }

            CurrentUnitId =
                string.Empty;

            State =
                OperatorState.Ready;

            return true;
        }

        public bool IsCurrentUnit(
            string unitId)
        {
            return IsOperatingUnit &&
                   !string.IsNullOrWhiteSpace(unitId) &&
                   string.Equals(
                       CurrentUnitId,
                       unitId.Trim(),
                       StringComparison.OrdinalIgnoreCase);
        }

        public void Reset()
        {
            Initialized = false;

            OperatorId =
                string.Empty;

            CurrentUnitId =
                string.Empty;

            State =
                OperatorState.Offline;
        }
    }
}

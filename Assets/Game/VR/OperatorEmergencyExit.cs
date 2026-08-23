using System;

namespace ObsidianProtocol.Game.VR
{
    public enum EmergencyExitState
    {
        Ready,
        Requested,
        Exiting,
        Complete,
        Blocked
    }

    public sealed class OperatorEmergencyExit
    {
        public bool Initialized { get; private set; }

        public EmergencyExitState State { get; private set; }

        public string UnitId { get; private set; }

        public string ExitPointId { get; private set; }

        public bool EmergencyExitActive =>
            State == EmergencyExitState.Exiting;

        public bool Initialize(
            string unitId,
            string exitPointId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(unitId) ||
                string.IsNullOrWhiteSpace(exitPointId))
            {
                return false;
            }

            UnitId =
                unitId.Trim();

            ExitPointId =
                exitPointId.Trim();

            State =
                EmergencyExitState.Ready;

            Initialized = true;

            return true;
        }

        public bool RequestExit()
        {
            if (!Initialized ||
                State != EmergencyExitState.Ready)
            {
                return false;
            }

            State =
                EmergencyExitState.Requested;

            return true;
        }

        public bool BeginExit()
        {
            if (!Initialized ||
                State != EmergencyExitState.Requested)
            {
                return false;
            }

            State =
                EmergencyExitState.Exiting;

            return true;
        }

        public bool CompleteExit()
        {
            if (!Initialized ||
                State != EmergencyExitState.Exiting)
            {
                return false;
            }

            State =
                EmergencyExitState.Complete;

            return true;
        }

        public bool BlockExit()
        {
            if (!Initialized ||
                State != EmergencyExitState.Requested &&
                State != EmergencyExitState.Exiting)
            {
                return false;
            }

            State =
                EmergencyExitState.Blocked;

            return true;
        }

        public bool ResetExit()
        {
            if (!Initialized)
            {
                return false;
            }

            State =
                EmergencyExitState.Ready;

            return true;
        }

        public void Reset()
        {
            Initialized = false;

            UnitId =
                string.Empty;

            ExitPointId =
                string.Empty;

            State =
                EmergencyExitState.Ready;
        }
    }
}

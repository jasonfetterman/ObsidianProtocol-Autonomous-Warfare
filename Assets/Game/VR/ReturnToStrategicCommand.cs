using System;

namespace ObsidianProtocol.Game.VR
{
    public enum StrategicReturnState
    {
        Ready,
        Requested,
        Returning,
        Complete
    }

    public sealed class ReturnToStrategicCommand
    {
        public bool Initialized { get; private set; }

        public StrategicReturnState State { get; private set; }

        public string OperatorId { get; private set; }

        public string UnitId { get; private set; }

        public bool Returning =>
            State == StrategicReturnState.Returning;

        public bool Initialize(
            string operatorId,
            string unitId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(operatorId) ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            OperatorId =
                operatorId.Trim();

            UnitId =
                unitId.Trim();

            State =
                StrategicReturnState.Ready;

            Initialized = true;

            return true;
        }

        public bool RequestReturn()
        {
            if (!Initialized ||
                State != StrategicReturnState.Ready)
            {
                return false;
            }

            State =
                StrategicReturnState.Requested;

            return true;
        }

        public bool BeginReturn()
        {
            if (!Initialized ||
                State != StrategicReturnState.Requested)
            {
                return false;
            }

            State =
                StrategicReturnState.Returning;

            return true;
        }

        public bool CompleteReturn()
        {
            if (!Initialized ||
                State != StrategicReturnState.Returning)
            {
                return false;
            }

            State =
                StrategicReturnState.Complete;

            return true;
        }

        public bool ResetReturn()
        {
            if (!Initialized)
            {
                return false;
            }

            State =
                StrategicReturnState.Ready;

            return true;
        }

        public void Reset()
        {
            Initialized = false;

            OperatorId =
                string.Empty;

            UnitId =
                string.Empty;

            State =
                StrategicReturnState.Ready;
        }
    }
}

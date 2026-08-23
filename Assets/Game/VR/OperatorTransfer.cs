using System;

namespace ObsidianProtocol.Game.VR
{
    public enum OperatorTransferState
    {
        Ready,
        Requested,
        Transferring,
        Complete,
        Failed
    }

    public sealed class OperatorTransfer
    {
        public bool Initialized { get; private set; }

        public OperatorTransferState State { get; private set; }

        public string OperatorId { get; private set; }

        public string SourceUnitId { get; private set; }

        public string TargetUnitId { get; private set; }

        public bool TransferActive =>
            State == OperatorTransferState.Transferring;

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

            SourceUnitId =
                string.Empty;

            TargetUnitId =
                string.Empty;

            State =
                OperatorTransferState.Ready;

            Initialized = true;

            return true;
        }

        public bool RequestTransfer(
            string sourceUnitId,
            string targetUnitId)
        {
            if (!Initialized ||
                State != OperatorTransferState.Ready ||
                string.IsNullOrWhiteSpace(sourceUnitId) ||
                string.IsNullOrWhiteSpace(targetUnitId) ||
                string.Equals(
                    sourceUnitId,
                    targetUnitId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            SourceUnitId =
                sourceUnitId.Trim();

            TargetUnitId =
                targetUnitId.Trim();

            State =
                OperatorTransferState.Requested;

            return true;
        }

        public bool BeginTransfer()
        {
            if (!Initialized ||
                State != OperatorTransferState.Requested)
            {
                return false;
            }

            State =
                OperatorTransferState.Transferring;

            return true;
        }

        public bool CompleteTransfer()
        {
            if (!Initialized ||
                State != OperatorTransferState.Transferring)
            {
                return false;
            }

            SourceUnitId =
                TargetUnitId;

            TargetUnitId =
                string.Empty;

            State =
                OperatorTransferState.Complete;

            return true;
        }

        public bool FailTransfer()
        {
            if (!Initialized ||
                State != OperatorTransferState.Transferring)
            {
                return false;
            }

            State =
                OperatorTransferState.Failed;

            return true;
        }

        public bool ResetTransfer()
        {
            if (!Initialized)
            {
                return false;
            }

            SourceUnitId =
                string.Empty;

            TargetUnitId =
                string.Empty;

            State =
                OperatorTransferState.Ready;

            return true;
        }

        public void Reset()
        {
            Initialized = false;

            OperatorId =
                string.Empty;

            SourceUnitId =
                string.Empty;

            TargetUnitId =
                string.Empty;

            State =
                OperatorTransferState.Ready;
        }
    }
}

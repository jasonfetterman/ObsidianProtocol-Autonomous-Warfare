using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum RescueOperationType
    {
        Recovery,
        Evacuation,
        Repair,
        Extraction,
        WreckRecovery
    }

    public enum RescueOperationState
    {
        Available,
        Assigned,
        Active,
        Complete,
        Cancelled
    }

    public sealed class RescueOperation
    {
        public string OperationId { get; }
        public RescueOperationType Type { get; }

        public string RescueUnitId { get; private set; }
        public string TargetUnitId { get; private set; }

        public RescueOperationState State { get; private set; }

        public float Progress { get; private set; }

        public RescueOperation(
            string operationId,
            RescueOperationType type)
        {
            OperationId =
                operationId ?? string.Empty;

            Type =
                type;

            State =
                RescueOperationState.Available;

            Progress = 0f;
        }

        public void Assign(
            string rescueUnitId,
            string targetUnitId)
        {
            RescueUnitId =
                rescueUnitId ?? string.Empty;

            TargetUnitId =
                targetUnitId ?? string.Empty;

            State =
                RescueOperationState.Assigned;
        }

        public void Start()
        {
            if (State ==
                RescueOperationState.Assigned)
            {
                State =
                    RescueOperationState.Active;
            }
        }

        public void Update(
            float progressAmount)
        {
            if (State !=
                RescueOperationState.Active)
            {
                return;
            }

            Progress +=
                Math.Max(
                    0f,
                    progressAmount);

            if (Progress >= 1f)
            {
                Progress = 1f;

                State =
                    RescueOperationState.Complete;
            }
        }

        public void Cancel()
        {
            State =
                RescueOperationState.Cancelled;
        }
    }

    public sealed class RescueVehicleSystem
    {
        private readonly Dictionary<string, RescueOperation> operations =
            new Dictionary<string, RescueOperation>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterOperation(
            string operationId,
            RescueOperationType type)
        {
            if (string.IsNullOrWhiteSpace(operationId))
            {
                return;
            }

            operations[operationId] =
                new RescueOperation(
                    operationId,
                    type);
        }

        public void AssignOperation(
            string operationId,
            string rescueUnitId,
            string targetUnitId)
        {
            if (operations.TryGetValue(
                    operationId,
                    out RescueOperation operation))
            {
                operation.Assign(
                    rescueUnitId,
                    targetUnitId);
            }
        }

        public void StartOperation(
            string operationId)
        {
            if (operations.TryGetValue(
                    operationId,
                    out RescueOperation operation))
            {
                operation.Start();
            }
        }

        public void UpdateOperation(
            string operationId,
            float progressAmount)
        {
            if (operations.TryGetValue(
                    operationId,
                    out RescueOperation operation))
            {
                operation.Update(
                    progressAmount);
            }
        }

        public void CancelOperation(
            string operationId)
        {
            if (operations.TryGetValue(
                    operationId,
                    out RescueOperation operation))
            {
                operation.Cancel();
            }
        }

        public bool IsComplete(
            string operationId)
        {
            return operations.TryGetValue(
                       operationId,
                       out RescueOperation operation) &&
                   operation.State ==
                   RescueOperationState.Complete;
        }

        public bool TryGetOperation(
            string operationId,
            out RescueOperation operation)
        {
            return operations.TryGetValue(
                operationId,
                out operation);
        }

        public void RemoveOperation(
            string operationId)
        {
            operations.Remove(operationId);
        }

        public void Clear()
        {
            operations.Clear();
        }
    }
}

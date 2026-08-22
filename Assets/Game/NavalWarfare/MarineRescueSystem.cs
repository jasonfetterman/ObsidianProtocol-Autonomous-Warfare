using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum MarineRescueType
    {
        VesselRecovery,
        CrewEvacuation,
        DisabledUnitRecovery,
        SubmersibleRecovery,
        WreckRecovery
    }

    public enum MarineRescueState
    {
        Available,
        Assigned,
        Approaching,
        Active,
        Complete,
        Aborted
    }

    public sealed class MarineRescueOperation
    {
        public string OperationId { get; }
        public MarineRescueType Type { get; }

        public MarineRescueState State { get; private set; }

        public string RescueUnitId { get; private set; }
        public string TargetUnitId { get; private set; }

        public float Progress { get; private set; }

        public MarineRescueOperation(
            string operationId,
            MarineRescueType type)
        {
            OperationId =
                operationId ?? string.Empty;

            Type =
                type;

            State =
                MarineRescueState.Available;
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
                MarineRescueState.Assigned;
        }

        public void BeginApproach()
        {
            if (State ==
                MarineRescueState.Assigned)
            {
                State =
                    MarineRescueState.Approaching;
            }
        }

        public void Activate()
        {
            if (State ==
                MarineRescueState.Approaching)
            {
                State =
                    MarineRescueState.Active;
            }
        }

        public void Update(
            float progressAmount)
        {
            if (State !=
                MarineRescueState.Active)
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
                    MarineRescueState.Complete;
            }
        }

        public void Abort()
        {
            State =
                MarineRescueState.Aborted;
        }
    }

    public sealed class MarineRescueSystem
    {
        private readonly Dictionary<string, MarineRescueOperation> operations =
            new Dictionary<string, MarineRescueOperation>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterOperation(
            string operationId,
            MarineRescueType type)
        {
            if (string.IsNullOrWhiteSpace(operationId))
            {
                return;
            }

            operations[operationId] =
                new MarineRescueOperation(
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
                    out MarineRescueOperation operation))
            {
                operation.Assign(
                    rescueUnitId,
                    targetUnitId);
            }
        }

        public void BeginApproach(
            string operationId)
        {
            if (operations.TryGetValue(
                    operationId,
                    out MarineRescueOperation operation))
            {
                operation.BeginApproach();
            }
        }

        public void Activate(
            string operationId)
        {
            if (operations.TryGetValue(
                    operationId,
                    out MarineRescueOperation operation))
            {
                operation.Activate();
            }
        }

        public void UpdateOperation(
            string operationId,
            float progressAmount)
        {
            if (operations.TryGetValue(
                    operationId,
                    out MarineRescueOperation operation))
            {
                operation.Update(
                    progressAmount);
            }
        }

        public void Abort(
            string operationId)
        {
            if (operations.TryGetValue(
                    operationId,
                    out MarineRescueOperation operation))
            {
                operation.Abort();
            }
        }

        public bool IsComplete(
            string operationId)
        {
            return operations.TryGetValue(
                       operationId,
                       out MarineRescueOperation operation) &&
                   operation.State ==
                   MarineRescueState.Complete;
        }

        public bool TryGetOperation(
            string operationId,
            out MarineRescueOperation operation)
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

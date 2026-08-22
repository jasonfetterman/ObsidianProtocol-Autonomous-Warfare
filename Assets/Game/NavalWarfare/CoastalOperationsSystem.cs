using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum CoastalOperationType
    {
        Landing,
        Beaching,
        ShorePatrol,
        CoastalRecon,
        Extraction,
        HarborDefense,
        HarborEntry
    }

    public enum CoastalOperationState
    {
        Planned,
        Approaching,
        Active,
        Complete,
        Aborted
    }

    public sealed class CoastalOperation
    {
        public string OperationId { get; }
        public CoastalOperationType Type { get; }

        public CoastalOperationState State { get; private set; }

        public string AssignedUnitId { get; private set; }
        public string ObjectiveId { get; private set; }

        public float Progress { get; private set; }

        public CoastalOperation(
            string operationId,
            CoastalOperationType type)
        {
            OperationId =
                operationId ?? string.Empty;

            Type =
                type;

            State =
                CoastalOperationState.Planned;
        }

        public void Assign(
            string unitId,
            string objectiveId)
        {
            AssignedUnitId =
                unitId ?? string.Empty;

            ObjectiveId =
                objectiveId ?? string.Empty;
        }

        public void BeginApproach()
        {
            if (State ==
                CoastalOperationState.Planned)
            {
                State =
                    CoastalOperationState.Approaching;
            }
        }

        public void Activate()
        {
            if (State ==
                CoastalOperationState.Approaching)
            {
                State =
                    CoastalOperationState.Active;
            }
        }

        public void Update(
            float progressAmount)
        {
            if (State !=
                CoastalOperationState.Active)
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
                    CoastalOperationState.Complete;
            }
        }

        public void Abort()
        {
            State =
                CoastalOperationState.Aborted;
        }
    }

    public sealed class CoastalOperationsSystem
    {
        private readonly Dictionary<string, CoastalOperation> operations =
            new Dictionary<string, CoastalOperation>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterOperation(
            string operationId,
            CoastalOperationType type)
        {
            if (string.IsNullOrWhiteSpace(operationId))
            {
                return;
            }

            operations[operationId] =
                new CoastalOperation(
                    operationId,
                    type);
        }

        public void AssignOperation(
            string operationId,
            string unitId,
            string objectiveId)
        {
            if (operations.TryGetValue(
                    operationId,
                    out CoastalOperation operation))
            {
                operation.Assign(
                    unitId,
                    objectiveId);
            }
        }

        public void BeginApproach(
            string operationId)
        {
            if (operations.TryGetValue(
                    operationId,
                    out CoastalOperation operation))
            {
                operation.BeginApproach();
            }
        }

        public void Activate(
            string operationId)
        {
            if (operations.TryGetValue(
                    operationId,
                    out CoastalOperation operation))
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
                    out CoastalOperation operation))
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
                    out CoastalOperation operation))
            {
                operation.Abort();
            }
        }

        public bool IsComplete(
            string operationId)
        {
            return operations.TryGetValue(
                       operationId,
                       out CoastalOperation operation) &&
                   operation.State ==
                   CoastalOperationState.Complete;
        }

        public bool TryGetOperation(
            string operationId,
            out CoastalOperation operation)
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

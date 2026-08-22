using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Logistics
{
    public enum AutonomousLogisticsState
    {
        Idle,
        Assigned,
        Executing,
        Returning,
        Complete,
        Failed
    }

    public sealed class AutonomousLogisticsAssignment
    {
        public string AssignmentId { get; }

        public string UnitId { get; }

        public string OriginId { get; }

        public string DestinationId { get; }

        public string RequestId { get; }

        public AutonomousLogisticsState State { get; private set; }

        public AutonomousLogisticsAssignment(
            string assignmentId,
            string unitId,
            string originId,
            string destinationId,
            string requestId)
        {
            AssignmentId =
                assignmentId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            OriginId =
                originId ?? string.Empty;

            DestinationId =
                destinationId ?? string.Empty;

            RequestId =
                requestId ?? string.Empty;

            State =
                AutonomousLogisticsState.Idle;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(AssignmentId) &&
            !string.IsNullOrWhiteSpace(UnitId) &&
            !string.IsNullOrWhiteSpace(OriginId) &&
            !string.IsNullOrWhiteSpace(DestinationId) &&
            !string.IsNullOrWhiteSpace(RequestId) &&
            !string.Equals(
                OriginId,
                DestinationId,
                StringComparison.OrdinalIgnoreCase);

        public void Assign()
        {
            if (State ==
                AutonomousLogisticsState.Idle)
            {
                State =
                    AutonomousLogisticsState.Assigned;
            }
        }

        public void Execute()
        {
            if (State ==
                AutonomousLogisticsState.Assigned)
            {
                State =
                    AutonomousLogisticsState.Executing;
            }
        }

        public void Return()
        {
            if (State ==
                AutonomousLogisticsState.Executing)
            {
                State =
                    AutonomousLogisticsState.Returning;
            }
        }

        public void Complete()
        {
            if (State ==
                    AutonomousLogisticsState.Executing ||
                State ==
                    AutonomousLogisticsState.Returning)
            {
                State =
                    AutonomousLogisticsState.Complete;
            }
        }

        public void Fail()
        {
            if (State !=
                    AutonomousLogisticsState.Complete)
            {
                State =
                    AutonomousLogisticsState.Failed;
            }
        }
    }

    public sealed class AutonomousLogisticsSystem
    {
        private readonly Dictionary<string, AutonomousLogisticsAssignment>
            assignments =
                new Dictionary<string, AutonomousLogisticsAssignment>(
                    StringComparer.OrdinalIgnoreCase);

        public bool RegisterAssignment(
            AutonomousLogisticsAssignment assignment)
        {
            if (assignment == null ||
                !assignment.Valid ||
                assignments.ContainsKey(
                    assignment.AssignmentId))
            {
                return false;
            }

            assignments.Add(
                assignment.AssignmentId,
                assignment);

            return true;
        }

        public bool RemoveAssignment(
            string assignmentId)
        {
            if (string.IsNullOrWhiteSpace(
                    assignmentId))
            {
                return false;
            }

            return assignments.Remove(
                assignmentId);
        }

        public bool TryGetAssignment(
            string assignmentId,
            out AutonomousLogisticsAssignment assignment)
        {
            return assignments.TryGetValue(
                assignmentId,
                out assignment);
        }

        public bool Assign(
            string assignmentId)
        {
            if (!assignments.TryGetValue(
                    assignmentId,
                    out AutonomousLogisticsAssignment assignment))
            {
                return false;
            }

            assignment.Assign();

            return true;
        }

        public bool Execute(
            string assignmentId)
        {
            if (!assignments.TryGetValue(
                    assignmentId,
                    out AutonomousLogisticsAssignment assignment))
            {
                return false;
            }

            assignment.Execute();

            return true;
        }

        public bool ReturnUnit(
            string assignmentId)
        {
            if (!assignments.TryGetValue(
                    assignmentId,
                    out AutonomousLogisticsAssignment assignment))
            {
                return false;
            }

            assignment.Return();

            return true;
        }

        public bool Complete(
            string assignmentId)
        {
            if (!assignments.TryGetValue(
                    assignmentId,
                    out AutonomousLogisticsAssignment assignment))
            {
                return false;
            }

            assignment.Complete();

            return true;
        }

        public bool Fail(
            string assignmentId)
        {
            if (!assignments.TryGetValue(
                    assignmentId,
                    out AutonomousLogisticsAssignment assignment))
            {
                return false;
            }

            assignment.Fail();

            return true;
        }

        public IReadOnlyCollection<AutonomousLogisticsAssignment>
            GetAssignments()
        {
            return assignments.Values;
        }

        public IReadOnlyCollection<AutonomousLogisticsAssignment>
            GetActiveAssignments()
        {
            List<AutonomousLogisticsAssignment> active =
                new List<AutonomousLogisticsAssignment>();

            foreach (
                AutonomousLogisticsAssignment assignment
                in assignments.Values)
            {
                if (assignment.State ==
                        AutonomousLogisticsState.Assigned ||
                    assignment.State ==
                        AutonomousLogisticsState.Executing ||
                    assignment.State ==
                        AutonomousLogisticsState.Returning)
                {
                    active.Add(
                        assignment);
                }
            }

            return active;
        }

        public void Clear()
        {
            assignments.Clear();
        }
    }
}

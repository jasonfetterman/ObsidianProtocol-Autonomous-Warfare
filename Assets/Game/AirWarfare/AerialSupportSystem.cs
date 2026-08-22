using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public enum AerialSupportType
    {
        Reconnaissance,
        Surveillance,
        Relay,
        Resupply,
        Recovery,
        TargetDesignation,
        ElectronicSupport,
        SearchAndRescue
    }

    public sealed class AerialSupportAssignment
    {
        public string UnitId { get; }
        public string TargetId { get; }
        public AerialSupportType SupportType { get; }

        public bool Active { get; private set; }

        public AerialSupportAssignment(
            string unitId,
            string targetId,
            AerialSupportType supportType)
        {
            UnitId = unitId ?? string.Empty;
            TargetId = targetId ?? string.Empty;
            SupportType = supportType;

            Active = true;
        }

        public void Complete()
        {
            Active = false;
        }

        public void Cancel()
        {
            Active = false;
        }
    }

    public sealed class AerialSupportSystem
    {
        private readonly Dictionary<string, AerialSupportAssignment> assignments =
            new Dictionary<string, AerialSupportAssignment>(
                StringComparer.OrdinalIgnoreCase);

        public void AssignSupport(
            string unitId,
            string targetId,
            AerialSupportType supportType)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            assignments[unitId] =
                new AerialSupportAssignment(
                    unitId,
                    targetId,
                    supportType);
        }

        public void CompleteSupport(
            string unitId)
        {
            if (assignments.TryGetValue(
                    unitId,
                    out AerialSupportAssignment assignment))
            {
                assignment.Complete();
            }
        }

        public void CancelSupport(
            string unitId)
        {
            if (assignments.TryGetValue(
                    unitId,
                    out AerialSupportAssignment assignment))
            {
                assignment.Cancel();
            }
        }

        public bool HasActiveSupport(
            string unitId)
        {
            return assignments.TryGetValue(
                       unitId,
                       out AerialSupportAssignment assignment) &&
                   assignment.Active;
        }

        public bool TryGetAssignment(
            string unitId,
            out AerialSupportAssignment assignment)
        {
            return assignments.TryGetValue(
                unitId,
                out assignment);
        }

        public void RemoveAssignment(
            string unitId)
        {
            assignments.Remove(unitId);
        }

        public void Clear()
        {
            assignments.Clear();
        }
    }
}

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum GroundSupportType
    {
        Repair,
        Resupply,
        Recovery,
        Recon,
        Transport,
        Construction,
        Medical,
        ElectronicSupport
    }

    public sealed class GroundSupportAssignment
    {
        public string SupportUnitId { get; }
        public string RecipientUnitId { get; private set; }

        public GroundSupportType SupportType { get; private set; }

        public float Range { get; private set; }
        public float EffectStrength { get; private set; }

        public bool Active { get; private set; }

        public GroundSupportAssignment(
            string supportUnitId)
        {
            SupportUnitId =
                supportUnitId ?? string.Empty;

            RecipientUnitId =
                string.Empty;

            Active = false;
        }

        public void Configure(
            GroundSupportType supportType,
            float range,
            float effectStrength)
        {
            SupportType =
                supportType;

            Range =
                Math.Max(
                    0f,
                    range);

            EffectStrength =
                Math.Max(
                    0f,
                    effectStrength);
        }

        public void Assign(
            string recipientUnitId)
        {
            RecipientUnitId =
                recipientUnitId ?? string.Empty;

            Active =
                !string.IsNullOrWhiteSpace(
                    RecipientUnitId);
        }

        public void Clear()
        {
            RecipientUnitId =
                string.Empty;

            Active = false;
        }
    }

    public sealed class GroundSupportSystem
    {
        private readonly Dictionary<string, GroundSupportAssignment> assignments =
            new Dictionary<string, GroundSupportAssignment>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterSupportUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!assignments.ContainsKey(unitId))
            {
                assignments.Add(
                    unitId,
                    new GroundSupportAssignment(unitId));
            }
        }

        public void ConfigureSupport(
            string unitId,
            GroundSupportType supportType,
            float range,
            float effectStrength)
        {
            RegisterSupportUnit(unitId);

            assignments[unitId].Configure(
                supportType,
                range,
                effectStrength);
        }

        public void AssignSupport(
            string supportUnitId,
            string recipientUnitId)
        {
            RegisterSupportUnit(supportUnitId);

            assignments[supportUnitId].Assign(
                recipientUnitId);
        }

        public void ClearAssignment(
            string supportUnitId)
        {
            if (assignments.TryGetValue(
                    supportUnitId,
                    out GroundSupportAssignment assignment))
            {
                assignment.Clear();
            }
        }

        public bool TryGetAssignment(
            string supportUnitId,
            out GroundSupportAssignment assignment)
        {
            return assignments.TryGetValue(
                supportUnitId,
                out assignment);
        }

        public void RemoveSupportUnit(
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

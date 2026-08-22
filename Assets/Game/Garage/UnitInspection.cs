using System;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class UnitInspection
    {
        public string OwnershipId { get; private set; }
        public string UnitId { get; private set; }

        public float Health { get; private set; }
        public float Armor { get; private set; }
        public float Energy { get; private set; }

        public bool Operational { get; private set; }
        public bool InspectionActive { get; private set; }

        public string Condition { get; private set; }
        public string DiagnosticSummary { get; private set; }

        public UnitInspection()
        {
            OwnershipId = string.Empty;
            UnitId = string.Empty;

            Condition = string.Empty;
            DiagnosticSummary = string.Empty;

            Operational = false;
            InspectionActive = false;
        }

        public void Begin(
            string ownershipId,
            string unitId)
        {
            OwnershipId =
                ownershipId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            InspectionActive = true;
        }

        public void End()
        {
            InspectionActive = false;
        }

        public void SetCondition(
            float health,
            float armor,
            float energy)
        {
            Health =
                Math.Max(
                    0f,
                    Math.Min(
                        100f,
                        health));

            Armor =
                Math.Max(
                    0f,
                    Math.Min(
                        100f,
                        armor));

            Energy =
                Math.Max(
                    0f,
                    Math.Min(
                        100f,
                        energy));

            Operational =
                Health > 0f;
        }

        public void SetConditionLabel(
            string condition)
        {
            Condition =
                condition ?? string.Empty;
        }

        public void SetDiagnosticSummary(
            string summary)
        {
            DiagnosticSummary =
                summary ?? string.Empty;
        }

        public void Clear()
        {
            OwnershipId = string.Empty;
            UnitId = string.Empty;

            Health = 0f;
            Armor = 0f;
            Energy = 0f;

            Operational = false;
            InspectionActive = false;

            Condition = string.Empty;
            DiagnosticSummary = string.Empty;
        }
    }
}

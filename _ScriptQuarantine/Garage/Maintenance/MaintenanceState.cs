using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class MaintenanceState
    {
        [Header("Condition")]
        [Range(0f, 1f)]
        public float overallCondition = 1f;

        [Range(0f, 1f)]
        public float mechanicalCondition = 1f;

        [Range(0f, 1f)]
        public float electricalCondition = 1f;

        [Range(0f, 1f)]
        public float structuralCondition = 1f;

        [Header("Wear")]
        [Min(0f)]
        public float operatingHours;

        [Min(0)]
        public int damageEvents;

        [Min(0)]
        public int repairCount;

        [Header("Readiness")]
        [Range(0f, 1f)]
        public float readiness = 1f;

        public bool requiresMaintenance;
        public bool grounded;
        public bool underRepair;

        [Header("Maintenance")]
        [Min(0f)]
        public float estimatedRepairTimeHours;

        [Min(0f)]
        public float estimatedRepairCost;

        public void ApplyWear(float amount)
        {
            amount = Mathf.Max(0f, amount);

            overallCondition =
                Mathf.Clamp01(overallCondition - amount);

            mechanicalCondition =
                Mathf.Clamp01(mechanicalCondition - amount);

            structuralCondition =
                Mathf.Clamp01(structuralCondition - amount);

            UpdateMaintenanceState();
        }

        public void ApplyDamage(float amount)
        {
            amount = Mathf.Max(0f, amount);

            damageEvents++;

            overallCondition =
                Mathf.Clamp01(overallCondition - amount);

            structuralCondition =
                Mathf.Clamp01(structuralCondition - amount);

            UpdateMaintenanceState();
        }

        public void Repair(float amount)
        {
            amount = Mathf.Max(0f, amount);

            overallCondition =
                Mathf.Clamp01(overallCondition + amount);

            mechanicalCondition =
                Mathf.Clamp01(mechanicalCondition + amount);

            electricalCondition =
                Mathf.Clamp01(electricalCondition + amount);

            structuralCondition =
                Mathf.Clamp01(structuralCondition + amount);

            repairCount++;

            UpdateMaintenanceState();
        }

        private void UpdateMaintenanceState()
        {
            requiresMaintenance =
                overallCondition < 0.75f;

            grounded =
                overallCondition <= 0.15f;

            readiness =
                Mathf.Clamp01(overallCondition);
        }
    }
}

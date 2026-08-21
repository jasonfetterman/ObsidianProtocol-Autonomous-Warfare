using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class ProtectionStats
    {
        [Header("Structure")]
        [Min(0f)]
        public float hullIntegrity;

        [Min(0f)]
        public float armor;

        [Min(0f)]
        public float structuralStability;

        [Header("Damage Resistance")]
        [Range(0f, 1f)]
        public float kineticResistance;

        [Range(0f, 1f)]
        public float explosiveResistance;

        [Range(0f, 1f)]
        public float thermalResistance;

        [Range(0f, 1f)]
        public float electricalResistance;

        [Range(0f, 1f)]
        public float environmentalResistance;

        [Header("Survivability")]
        [Min(0f)]
        public float survivabilityRating;

        [Min(0f)]
        public float criticalDamageThreshold;

        public bool canOperateWhileDamaged;
        public bool canSelfStabilize;
        public bool canContinueMissionAfterCriticalDamage;
    }
}

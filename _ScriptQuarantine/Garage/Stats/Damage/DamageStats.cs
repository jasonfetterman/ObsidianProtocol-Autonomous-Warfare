using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class DamageStats
    {
        [Header("Durability")]
        [Min(0f)] public float structuralIntegrity;
        [Min(0f)] public float armorRating;
        [Min(0f)] public float impactResistance;

        [Header("Damage Resistance")]
        [Min(0f)] public float ballisticResistance;
        [Min(0f)] public float explosiveResistance;
        [Min(0f)] public float thermalResistance;
        [Min(0f)] public float environmentalResistance;

        [Header("Critical Systems")]
        [Min(0f)] public float engineProtection;
        [Min(0f)] public float powerSystemProtection;
        [Min(0f)] public float sensorProtection;
        [Min(0f)] public float communicationProtection;

        [Header("Mobility")]
        [Min(0f)] public float mobilitySystemProtection;
        [Min(0f)] public float mobilityDamageThreshold;

        [Header("Recovery")]
        [Min(0f)] public float damageControlRate;
        [Min(0f)] public float repairability;

        [Header("Failure")]
        [Min(0f)] public float criticalFailureThreshold;
        [Min(0f)] public float catastrophicFailureThreshold;

        public bool canOperateDamaged;
        public bool canRecoverFromCriticalDamage;
    }
}

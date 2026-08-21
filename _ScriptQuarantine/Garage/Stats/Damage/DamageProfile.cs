using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class DamageProfile
    {
        [Header("Damage Zones")]
        [Range(0f, 1f)]
        public float frontProtection = 1f;

        [Range(0f, 1f)]
        public float rearProtection = 1f;

        [Range(0f, 1f)]
        public float leftProtection = 1f;

        [Range(0f, 1f)]
        public float rightProtection = 1f;

        [Range(0f, 1f)]
        public float topProtection = 1f;

        [Range(0f, 1f)]
        public float bottomProtection = 1f;

        [Header("Critical Systems")]
        [Range(0f, 1f)]
        public float powerCoreProtection = 1f;

        [Range(0f, 1f)]
        public float propulsionProtection = 1f;

        [Range(0f, 1f)]
        public float sensorProtection = 1f;

        [Range(0f, 1f)]
        public float communicationProtection = 1f;

        [Range(0f, 1f)]
        public float controlSystemProtection = 1f;

        [Header("Failure Behavior")]
        public bool subsystemDamageEnabled = true;

        public bool criticalFailureEnabled = true;

        public bool catastrophicFailureEnabled = true;

        [Min(0f)]
        public float criticalFailureMultiplier = 1f;

        [Min(0f)]
        public float catastrophicFailureMultiplier = 1f;
    }
}

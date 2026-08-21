using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class PowerStats
    {
        [Header("Power Generation")]
        [Min(0f)]
        public float generatorOutputKw;

        [Min(0f)]
        public float peakPowerOutputKw;

        [Header("Energy Storage")]
        [Min(0f)]
        public float batteryCapacityKwh;

        [Min(0f)]
        public float reserveCapacityKwh;

        [Range(0f, 1f)]
        public float startingCharge = 1f;

        [Header("Consumption")]
        [Min(0f)]
        public float idleConsumptionKw;

        [Min(0f)]
        public float cruiseConsumptionKw;

        [Min(0f)]
        public float combatConsumptionKw;

        [Min(0f)]
        public float sensorConsumptionKw;

        [Min(0f)]
        public float communicationsConsumptionKw;

        [Header("Power Management")]
        public bool regenerativeCharging;
        public bool emergencyPowerMode;
        public bool automaticPowerPrioritization;

        [Range(0f, 1f)]
        public float emergencyReservePercent = 0.15f;
    }
}

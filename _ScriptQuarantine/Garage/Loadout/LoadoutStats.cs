using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class LoadoutStats
    {
        [Header("Capacity")]
        [Min(0)]
        public int equipmentSlots;

        [Min(0f)]
        public float equipmentMassCapacityKg;

        [Min(0f)]
        public float powerCapacityKw;

        [Header("Configuration")]
        public bool modularLoadout;

        public bool fieldSwappable;

        public bool requiresMaintenanceAfterSwap;

        [Header("Hardpoints")]
        [Min(0)]
        public int weaponHardpoints;

        [Min(0)]
        public int utilityHardpoints;

        [Min(0)]
        public int sensorHardpoints;

        [Min(0)]
        public int communicationHardpoints;

        [Header("Loadout Effects")]
        [Range(0f, 1f)]
        public float loadoutEfficiency = 1f;

        [Range(0f, 1f)]
        public float compatibility = 1f;
    }
}

using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class LogisticsStats
    {
        [Header("Payload")]
        [Min(0f)] public float payloadCapacityKg;
        [Min(0f)] public float cargoCapacity;
        [Min(0f)] public float equipmentCapacity;

        [Header("Supply")]
        [Min(0f)] public float supplyCapacity;
        [Min(0f)] public float resupplyRate;
        [Min(0f)] public float resourceEfficiency;

        [Header("Transport")]
        [Min(0f)] public float transportCapacity;
        [Min(0f)] public float loadingRate;
        [Min(0f)] public float unloadingRate;

        [Header("Operational Range")]
        [Min(0f)] public float operationalRangeKm;
        [Min(0f)] public float enduranceHours;

        [Header("Support")]
        [Min(0f)] public float repairSupport;
        [Min(0f)] public float refuelSupport;
        [Min(0f)] public float rearmSupport;

        [Header("Capabilities")]
        public bool canResupply;
        public bool canRepair;
        public bool canRefuel;
        public bool canRearm;
        public bool canTransportUnits;
        public bool canRecoverUnits;
    }
}

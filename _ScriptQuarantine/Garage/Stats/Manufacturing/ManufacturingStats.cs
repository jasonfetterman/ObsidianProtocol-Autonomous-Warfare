using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class ManufacturingStats
    {
        [Header("Production")]
        [Min(0f)] public float productionEfficiency;
        [Min(0f)] public float assemblySpeed;
        [Min(0f)] public float materialEfficiency;

        [Header("Construction")]
        [Min(0f)] public float constructionCapacity;
        [Min(0f)] public float fabricationRate;
        [Min(0f)] public float precision;

        [Header("Maintenance")]
        [Min(0f)] public float maintenanceEfficiency;
        [Min(0f)] public float componentRecovery;
        [Min(0f)] public float repairSpeed;

        [Header("Resource Usage")]
        [Min(0f)] public float resourceConsumption;
        [Min(0f)] public float energyConsumption;

        [Header("Capabilities")]
        public bool canManufacture;
        public bool canRepair;
        public bool canRefit;
        public bool canRecycle;
        public bool canUpgrade;
    }
}

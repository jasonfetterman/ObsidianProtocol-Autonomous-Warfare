using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class UnitStats
    {
        [Header("Core Identity")]
        public PhysicalStats physical;
        public MobilityStats mobility;
        public PowerStats power;

        [Header("Combat")]
        public CombatStats combat;
        public DamageStats damage;

        [Header("Sensors & Communications")]
        public SensorStats sensors;
        public CommunicationStats communications;

        [Header("Operations")]
        public LogisticsStats logistics;
        public ManufacturingStats manufacturing;
        public EnvironmentStats environment;

        [Header("Control & Intelligence")]
        public OperatorStats operatorStats;
        public AIStats ai;

        [Header("Balance")]
        public BalanceStats balance;
    }
}

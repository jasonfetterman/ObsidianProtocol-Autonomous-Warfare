using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class CombatStats
    {
        [Header("Offense")]
        [Min(0f)] public float damagePerSecond;
        [Min(0f)] public float burstDamage;
        [Min(0f)] public float effectiveRange;
        [Min(0f)] public float accuracy;
        [Min(0f)] public float rateOfFire;

        [Header("Targeting")]
        [Min(0f)] public float targetAcquisitionSpeed;
        [Min(0f)] public float trackingSpeed;
        [Min(0f)] public float engagementCapacity;
        [Min(0f)] public float targetingPrecision;

        [Header("Engagement")]
        [Min(0f)] public float closeCombat;
        [Min(0f)] public float rangedCombat;
        [Min(0f)] public float antiArmor;
        [Min(0f)] public float antiAir;
        [Min(0f)] public float antiStructure;

        [Header("Command")]
        [Min(0f)] public float commandRange;
        [Min(0f)] public float supportRange;
        [Min(0f)] public float coordinationBonus;

        [Header("Combat Behavior")]
        [Min(0f)] public float suppression;
        [Min(0f)] public float threatGeneration;
        [Min(0f)] public float survivability;

        [Header("Capabilities")]
        public bool canEngageGround;
        public bool canEngageAir;
        public bool canEngageSea;
        public bool canEngageStructures;
        public bool canProvideCombatSupport;
    }
}

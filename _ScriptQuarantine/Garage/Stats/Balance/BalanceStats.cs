using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class BalanceStats
    {
        [Header("Combat Balance")]
        [Min(0f)] public float damageOutput;
        [Min(0f)] public float survivability;
        [Min(0f)] public float combatEfficiency;

        [Header("Operational Balance")]
        [Min(0f)] public float versatility;
        [Min(0f)] public float reliability;
        [Min(0f)] public float availability;

        [Header("Cost Efficiency")]
        [Min(0f)] public float resourceEfficiency;
        [Min(0f)] public float maintenanceEfficiency;
        [Min(0f)] public float battlefieldValue;

        [Header("Strategic Value")]
        [Min(0f)] public float strategicValue;
        [Min(0f)] public float tacticalValue;
        [Min(0f)] public float supportValue;

        [Header("Game Balance")]
        [Min(0f)] public float powerRating;
        [Min(0f)] public float roleSpecialization;
        [Min(0f)] public float deploymentPriority;
    }
}

using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class AIStats
    {
        [Header("Autonomy")]
        [Range(0f, 1f)] public float autonomyLevel;
        [Range(0f, 1f)] public float decisionQuality;
        [Range(0f, 1f)] public float reactionSpeed;

        [Header("Navigation")]
        [Range(0f, 1f)] public float navigationSkill;
        [Range(0f, 1f)] public float pathfinding;
        [Range(0f, 1f)] public float obstacleAvoidance;

        [Header("Tactical Intelligence")]
        [Range(0f, 1f)] public float threatAssessment;
        [Range(0f, 1f)] public float targetSelection;
        [Range(0f, 1f)] public float tacticalAdaptation;
        [Range(0f, 1f)] public float formationCoordination;

        [Header("Mission Behavior")]
        [Range(0f, 1f)] public float missionExecution;
        [Range(0f, 1f)] public float taskPriority;
        [Range(0f, 1f)] public float riskAssessment;
        [Range(0f, 1f)] public float retreatDiscipline;

        [Header("Learning / Adaptation")]
        [Range(0f, 1f)] public float adaptability;
        [Range(0f, 1f)] public float battlefieldAwareness;
        [Range(0f, 1f)] public float memory;

        [Header("Capabilities")]
        public bool autonomousNavigation;
        public bool autonomousTargeting;
        public bool autonomousCombat;
        public bool autonomousMissionExecution;
        public bool enableFormationCoordination;
        public bool adaptiveBehavior;
    }
}


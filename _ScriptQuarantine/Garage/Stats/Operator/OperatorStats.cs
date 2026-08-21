using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class OperatorStats
    {
        [Header("Control")]
        [Min(0f)] public float controlResponsiveness;
        [Min(0f)] public float controlPrecision;
        [Min(0f)] public float handlingDifficulty;

        [Header("Operator Assistance")]
        [Min(0f)] public float stabilization;
        [Min(0f)] public float aimAssist;
        [Min(0f)] public float navigationAssist;
        [Min(0f)] public float collisionAvoidance;

        [Header("Situational Awareness")]
        [Min(0f)] public float operatorAwareness;
        [Min(0f)] public float threatWarning;
        [Min(0f)] public float targetIdentification;

        [Header("Remote Operation")]
        [Min(0f)] public float remoteControlQuality;
        [Min(0f)] public float cameraControlQuality;
        [Min(0f)] public float operatorViewRange;

        [Header("VR / Direct Control")]
        [Min(0f)] public float vrControlQuality;
        [Min(0f)] public float immersion;
        [Min(0f)] public float motionStability;

        [Header("Cognitive Load")]
        [Min(0f)] public float automationLevel;
        [Min(0f)] public float informationClarity;
        [Min(0f)] public float operatorWorkload;

        public bool supportsDirectControl;
        public bool supportsRemoteControl;
        public bool supportsVRControl;
        public bool supportsAutonomousControl;
    }
}

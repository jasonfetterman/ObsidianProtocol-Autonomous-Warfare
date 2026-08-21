using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class CommunicationsStats
    {
        [Header("Control Link")]
        public bool controlLinkEnabled;

        [Min(0f)]
        public float controlRangeMeters;

        [Min(0f)]
        public float controlLatencyMilliseconds;

        [Header("Data Link")]
        public bool dataLinkEnabled;

        [Min(0f)]
        public float dataRangeMeters;

        [Min(0f)]
        public float dataBandwidthMbps;

        [Header("Telemetry")]
        public bool telemetryEnabled;

        [Min(0f)]
        public float telemetryRangeMeters;

        [Min(0f)]
        public float telemetryUpdateRateHz;

        [Header("Network")]
        public bool meshNetworking;

        [Min(0f)]
        public float meshRangeMeters;

        [Min(0)]
        public int maxNetworkPeers;

        [Header("Electronic Warfare")]
        public bool encryptedCommunications;

        public bool frequencyHopping;

        public bool antiJamming;

        [Range(0f, 1f)]
        public float communicationsSecurity = 1f;

        [Range(0f, 1f)]
        public float electronicWarfareResistance = 1f;

        [Header("Autonomy")]
        public bool autonomousOperation;

        public bool communicationLossFailsafe;

        [Min(0f)]
        public float autonomousOperationMinutes;
    }
}

using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class CommunicationStats
    {
        [Header("Control")]
        public string controlLink;
        [Min(0f)]
        public float controlRange;

        [Header("Data")]
        public string dataLink;
        [Min(0f)]
        public float dataRange;

        [Header("Telemetry")]
        public string telemetryLink;
        [Min(0f)]
        public float telemetryRange;

        [Header("Network")]
        [Min(0f)]
        public float bandwidth;

        [Min(0f)]
        public float latency;

        [Range(0f, 1f)]
        public float reliability;

        [Range(0f, 1f)]
        public float interferenceResistance;

        [Range(0f, 1f)]
        public float jammingResistance;

        [Header("Command Network")]
        [Min(0f)]
        public float commandRange;

        [Min(0)]
        public int networkPriority;

        [Min(0)]
        public int commandAuthority;

        [Header("Capabilities")]
        public bool meshNetworking;
        public bool relayCapability;
        public bool encryptedCommunication;
        public bool autonomousCommunication;
        public bool operateWithoutLink;
    }
}

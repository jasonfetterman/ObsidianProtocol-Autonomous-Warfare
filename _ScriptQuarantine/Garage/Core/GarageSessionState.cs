using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class GarageSessionState
    {
        [Header("Session")]
        public string sessionId;
        public string playerId;

        [Header("Current Unit")]
        public string activeUnitInstanceId;
        public string activeUnitDefinitionId;

        [Header("Operation")]
        public GarageOperatingMode operatingMode =
            GarageOperatingMode.RTSCommand;

        public GarageSessionMode sessionMode =
            GarageSessionMode.Offline;

        [Header("World")]
        public string worldId;
        public string regionId;

        [Header("State")]
        public bool inGarage = true;
        public bool deployed;
        public bool paused;
        public bool spectating;

        [Header("Persistence")]
        public bool dirty;
        public long lastSaveTimestamp;
    }
}

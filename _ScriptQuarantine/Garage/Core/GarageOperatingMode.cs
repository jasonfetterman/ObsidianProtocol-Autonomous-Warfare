using System;

namespace ObsidianProtocol.Garage
{
    public enum GarageOperatingMode
    {
        RTSCommand,
        DirectUnitControl,
        VRControl,
        FreeRoam
    }

    public enum GarageSessionMode
    {
        Offline,
        Online
    }

    [Serializable]
    public class GarageModeState
    {
        public GarageOperatingMode operatingMode =
            GarageOperatingMode.RTSCommand;

        public GarageSessionMode sessionMode =
            GarageSessionMode.Offline;

        public bool isPaused;
        public bool isInGarage;
        public bool isDeployed;
        public bool isSpectating;
    }
}

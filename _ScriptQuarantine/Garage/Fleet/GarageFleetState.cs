using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class GarageFleetState
    {
        public string fleetId;
        public string fleetName;

        public List<string> unitInstanceIds =
            new List<string>();

        public string activeUnitInstanceId;

        public bool deployed;
        public bool locked;
    }
}

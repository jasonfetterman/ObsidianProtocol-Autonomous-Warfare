using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class GaragePersistenceState
    {
        public int version = 1;

        public string playerId;
        public string worldId;

        public List<OwnedUnit> ownedUnits =
            new List<OwnedUnit>();

        public List<GarageFleetState> fleets =
            new List<GarageFleetState>();

        public string activeFleetId;
        public string activeUnitInstanceId;

        public long lastSavedTimestamp;

        public bool onlineSession;
    }
}

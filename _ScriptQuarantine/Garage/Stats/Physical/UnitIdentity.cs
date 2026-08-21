using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class UnitIdentity
    {
        [Header("Identity")]
        public string unitId;
        public string displayName;

        [Header("Classification")]
        public UnitCategory category;
        public UnitRole primaryRole;

        [Header("Description")]
        [TextArea(2, 5)]
        public string description;
    }

    public enum UnitCategory
    {
        Air,
        Ground,
        Sea,
        Command,
        Experimental
    }

    public enum UnitRole
    {
        Combat,
        Recon,
        Surveillance,
        Support,
        Logistics,
        Rescue,
        Command,
        Relay,
        Mapping,
        Survey,
        Experimental
    }
}

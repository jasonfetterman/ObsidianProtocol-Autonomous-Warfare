using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class ServiceBayUIState
    {
        public string bayId;
        public string bayName;

        public string unitInstanceId;

        public bool occupied;
        public bool servicing;
        public bool inspecting;
        public bool available;

        [Range(0f, 1f)]
        public float serviceProgress;

        [Range(0f, 1f)]
        public float inspectionProgress;

        public string Status
        {
            get
            {
                if (!occupied)
                    return "AVAILABLE";

                if (servicing)
                    return "SERVICING";

                if (inspecting)
                    return "INSPECTING";

                return "OCCUPIED";
            }
        }
    }
}

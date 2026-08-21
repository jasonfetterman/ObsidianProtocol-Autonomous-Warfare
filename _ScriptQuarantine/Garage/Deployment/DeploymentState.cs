using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class DeploymentState
    {
        public string unitInstanceId;
        public string unitDefinitionId;

        public string worldId;
        public string spawnPointId;

        public bool deployed;
        public bool active;

        public bool online;
        public bool offline;

        public bool rtsControl;
        public bool directControl;
        public bool vrControl;
        public bool freeRoam;

        [Range(0f, 1f)]
        public float deploymentProgress;

        public void Begin()
        {
            deployed = true;
            active = true;
            deploymentProgress = 1f;
        }

        public void End()
        {
            deployed = false;
            active = false;
            deploymentProgress = 0f;
        }

        public void SetOnline(bool value)
        {
            online = value;

            if (value)
                offline = false;
        }

        public void SetOffline(bool value)
        {
            offline = value;

            if (value)
                online = false;
        }

        public void SetRTSControl(bool value)
        {
            rtsControl = value;
        }

        public void SetDirectControl(bool value)
        {
            directControl = value;
        }

        public void SetVRControl(bool value)
        {
            vrControl = value;
        }

        public void SetFreeRoam(bool value)
        {
            freeRoam = value;
        }
    }
}

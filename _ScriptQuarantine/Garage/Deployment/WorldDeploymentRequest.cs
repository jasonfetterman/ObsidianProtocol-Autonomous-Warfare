using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class WorldDeploymentRequest
    {
        [Header("Unit")]
        public string unitInstanceId;
        public string unitDefinitionId;

        [Header("World")]
        public string worldId;
        public string spawnPointId;

        [Header("Deployment")]
        public bool authorized;
        public bool online;
        public bool offline;

        [Header("Control")]
        public bool rtsControl = true;
        public bool directControl;
        public bool vrControl;
        public bool freeRoam = true;

        [Header("Transform")]
        public Vector3 position;
        public Vector3 rotation;

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(unitInstanceId) &&
                !string.IsNullOrWhiteSpace(unitDefinitionId) &&
                !string.IsNullOrWhiteSpace(worldId) &&
                authorized;
        }
    }
}

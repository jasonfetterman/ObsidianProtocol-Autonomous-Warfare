// Project: WorldCreatorBridge
// Filename: SettingsBridge.cs
// Copyright (c) 2025 BiteTheBytes GmbH. All rights reserved
// *********************************************************

using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR

namespace BtB.WC.Bridge
{
    [Serializable]
    public class ParamsBridge
    {
        public int userSplit = 4;
        public int internalSplit = 4096;
        public int userDetailResolution = 6;
        public int detailResolution = 4096;

        public string bridgeFilePath = "";
        public string terrainsFolderName = "WorldCreatorTerrains";
        public string assetName = "WC_Terrain";
        public bool deleteUnusedAssets = true;
        public bool overwritePrefabs = false;

        public bool isImportLayers = true;
        public bool layerWarning = false;

        public float worldScale = 1;
        public string worldScaleString = "1.00";

        public MaterialType materialType = MaterialType.Standard;
        public Material customMaterial;

        public int InternalSplit => Math.Min(internalSplit, 128 << userSplit);
        public int UserSplit => 128 << userSplit;
        public int DetailResolution => Math.Min(detailResolution, 16 << userDetailResolution);
        public int UserDetailResolution => 16 << userDetailResolution;

        public ParamsBridge()
        {
            bridgeFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/World Creator/Sync/bridge.xml";
        }

        public bool IsBridgeFileValid()
        {
            return File.Exists(bridgeFilePath);
        }
    }

    public enum MaterialType
    {
        Standard,
        HDRP,
        URP,
        Custom
    }
}

#endif
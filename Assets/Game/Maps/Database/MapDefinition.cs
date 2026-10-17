using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Maps
{
    [CreateAssetMenu(fileName = "MapDefinition", menuName = "Obsidian Protocol/Maps/Map Definition")]
    public sealed class MapDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string mapId;
        [SerializeField] private string displayName;
        [SerializeField] private string environment;
        [SerializeField] private string warfareFocus;

        [TextArea(4, 12)]
        [SerializeField] private string executiveSummary;

        [Header("Gameplay")]
        [SerializeField] private List<MapTextEntry> designGoals = new List<MapTextEntry>();
        [TextArea(2, 6)]
        [SerializeField] private string deployment;

        [Header("AR / Device")]
        [SerializeField] private ARMapSettings ar = new ARMapSettings();

        [Header("Layout")]
        [SerializeField] private string coordinateSystem;
        [SerializeField] private List<MapLocation> keyLocations = new List<MapLocation>();
        [SerializeField] private List<MapTextEntry> chokepointsAndSightlines = new List<MapTextEntry>();

        [Header("Environment")]
        [TextArea(2, 6)] [SerializeField] private string terrain;
        [TextArea(2, 4)] [SerializeField] private string vegetation;
        [TextArea(2, 6)] [SerializeField] private string buildingsAndProps;
        [TextArea(2, 4)] [SerializeField] private string effects;
        [TextArea(2, 4)] [SerializeField] private string audio;

        [Header("LOD / Budget")]
        [SerializeField] private List<MapLodEntry> lodBudget = new List<MapLodEntry>();
        [SerializeField] private string textureBudget;

        [Header("Engine / Networking")]
        [SerializeField] private string engineAndPlugins;
        [SerializeField] private string rendering;
        [SerializeField] private string anchors;
        [SerializeField] private string interaction;
        [SerializeField] private string multiplayer;

        [Header("Implementation")]
        [SerializeField] private List<MapTextEntry> milestones = new List<MapTextEntry>();
        [SerializeField] private List<MapTextEntry> testing = new List<MapTextEntry>();

        public string MapId => mapId;
        public string DisplayName => displayName;
        public string Environment => environment;
        public string WarfareFocus => warfareFocus;

        public void SetIdentity(string id, string name, string env, string focus)
        {
            mapId = id;
            displayName = name;
            environment = env;
            warfareFocus = focus;
        }

        public void SetCore(
            string summary,
            List<MapTextEntry> goals,
            string deploymentText,
            ARMapSettings arSettings,
            string coordinates,
            List<MapLocation> locations,
            List<MapTextEntry> chokepoints,
            string terrainText,
            string vegetationText,
            string propsText,
            string effectsText,
            string audioText,
            List<MapLodEntry> lod,
            string textures,
            string engine,
            string render,
            string anchorText,
            string interactionText,
            string multiplayerText,
            List<MapTextEntry> implementation,
            List<MapTextEntry> testPlan)
        {
            executiveSummary = summary;
            designGoals = goals ?? new List<MapTextEntry>();
            deployment = deploymentText;
            ar = arSettings ?? new ARMapSettings();
            coordinateSystem = coordinates;
            keyLocations = locations ?? new List<MapLocation>();
            chokepointsAndSightlines = chokepoints ?? new List<MapTextEntry>();
            terrain = terrainText;
            vegetation = vegetationText;
            buildingsAndProps = propsText;
            effects = effectsText;
            audio = audioText;
            lodBudget = lod ?? new List<MapLodEntry>();
            textureBudget = textures;
            engineAndPlugins = engine;
            rendering = render;
            anchors = anchorText;
            interaction = interactionText;
            multiplayer = multiplayerText;
            milestones = implementation ?? new List<MapTextEntry>();
            testing = testPlan ?? new List<MapTextEntry>();
        }
    }

    [Serializable]
    public sealed class MapTextEntry
    {
        public string name;
        [TextArea(2, 6)] public string description;
    }

    [Serializable]
    public sealed class MapLocation
    {
        public string name;
        public string coordinate;
        public string sizeOrElevation;
        [TextArea(2, 5)] public string description;
    }

    [Serializable]
    public sealed class MapLodEntry
    {
        public string category;
        public int count;
        public int trianglesPerItem;
        public int totalTriangles;
    }

    [Serializable]
    public sealed class ARMapSettings
    {
        public string viewingDistance;
        public string physicalArea;
        public string representedArea;
        public string scale;
        public string fov;
        public string resolution;
        [TextArea(2, 4)] public string tracking;
        [TextArea(2, 4)] public string interaction;
    }
}

using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [CreateAssetMenu(
        fileName = "GarageConfiguration",
        menuName = "Obsidian Protocol/Garage/Garage Configuration"
    )]
    public class GarageConfiguration : ScriptableObject
    {
        [Header("Unit Database")]
        public UnitDefinitionDatabase unitDatabase;

        [Header("Game Modes")]
        public bool enableRTSCommand = true;
        public bool enableDirectUnitControl = true;
        public bool enableVRControl = true;
        public bool enableFreeRoam = true;

        [Header("World Operation")]
        public bool enableOfflinePlay = true;
        public bool enableOnlinePlay = true;
        public bool enablePersistentWorldState = true;
        public bool enableDynamicBattlefields = true;

        [Header("Garage Systems")]
        public bool enableCustomization = true;
        public bool enableLoadouts = true;
        public bool enableMaintenance = true;
        public bool enableFleetManagement = true;

        [Header("Progression")]
        public bool enableExperience = true;
        public bool enableUnitLevels = true;
        public bool enableUnitSpecialization = true;

        [Header("Autonomous Warfare")]
        public bool enableAIProfiles = true;
        public bool enableAutonomousUnits = true;
        public bool enableSquadAutonomy = true;
        public bool enableCommanderIntent = true;

        [Header("Environment")]
        public bool enableEnvironmentalSimulation = true;
        public bool enableDestructibleEnvironment = true;
        public bool enableFogOfWar = true;
        public bool enableTerrainEffects = true;

        [Header("Networking")]
        public bool enableCooperativePlay = true;
        public bool enableCompetitivePlay = true;
        public bool enableDedicatedServers = true;
        public bool enableServerPersistence = true;

        [Header("Validation")]
        public bool validateDefinitionsOnStartup = true;
        public bool logGarageInitialization = true;
    }
}

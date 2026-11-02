using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Units
{
    public enum UnitCategory
{
    AirUnit = 0,
    GroundUnit = 1,
    Naval = 2,
    Station = 3,
    Exploration = 4,
    Satellite = 5,
    Fighter = 6,
    Transport = 7,
    Recon = 8,
    Shuttle = 9,
    Medical = 10,
}

    [CreateAssetMenu(
        fileName = "UnitDefinition",
        menuName = "Obsidian Protocol/Units/Unit Definition")]
    public sealed class UnitDefinition : ScriptableObject
    {
        [Header("IDENTITY")]
        [SerializeField] private string unitId;
        [SerializeField] private string displayName;
        [SerializeField] private UnitCategory category;
        [SerializeField] private string role;
        [SerializeField] private string tier;
        [SerializeField] private string status;

        [Header("STORE PRESENTATION")]
        [SerializeField] private Sprite storeImage;
        [SerializeField] private GameObject modelPrefab;

        [Header("PHYSICAL & TECHNICAL")]
        [SerializeField] private PhysicalTechnical physical = new PhysicalTechnical();

        [Header("AUTONOMY & AI")]
        [SerializeField] private AutonomyAI autonomy = new AutonomyAI();

        [Header("SENSORS")]
        [SerializeField] private SensorData sensors = new SensorData();

        [Header("COMMUNICATION")]
        [SerializeField] private CommunicationData communication = new CommunicationData();

        [Header("COMBAT & THREAT")]
        [SerializeField] private CombatThreat combat = new CombatThreat();

        [Header("SQUAD & COMMAND")]
        [SerializeField] private SquadCommand squad = new SquadCommand();

        [Header("VR OPERATOR")]
        [SerializeField] private VROperator vrOperator = new VROperator();

        [Header("ECONOMY & LOGISTICS")]
        [SerializeField] private EconomyLogistics economy = new EconomyLogistics();

        [Header("BUILD REQUIREMENTS")]
        [SerializeField] private BuildRequirements buildRequirements = new BuildRequirements();

        [Header("ENVIRONMENT")]
        [SerializeField] private EnvironmentalData environment = new EnvironmentalData();

        [Header("NETWORK")]
        [SerializeField] private NetworkDynamics network = new NetworkDynamics();

        [Header("AI COGNITIVE")]
        [SerializeField] private CognitiveData cognitive = new CognitiveData();

        [Header("AI BEHAVIOR")]
        [SerializeField] private List<BehaviorNode> behaviorNodes = new List<BehaviorNode>();

        [Header("MISSION BEHAVIOR")]
        [SerializeField] private List<MissionBehavior> missionBehaviors = new List<MissionBehavior>();

        [Header("UNIT COORDINATION")]
        [SerializeField] private List<UnitSynergy> unitSynergies = new List<UnitSynergy>();

        [Header("DAMAGE RESPONSE")]
        [SerializeField] private List<DamageResponse> damageResponses = new List<DamageResponse>();

        [Header("UPGRADES")]
        [SerializeField] private List<UpgradeEffect> upgrades = new List<UpgradeEffect>();

        [Header("TACTICAL")]
        [SerializeField] private TacticalIntegration tactical = new TacticalIntegration();

        [Header("MAINTENANCE")]
        [SerializeField] private MaintenanceAnalytics maintenance = new MaintenanceAnalytics();

        [Header("THREAT RESPONSE")]
        [SerializeField] private List<ThreatResponse> threatResponses = new List<ThreatResponse>();

        [Header("COMMAND NETWORK")]
        [SerializeField] private List<CommandNetworkNode> commandNetwork = new List<CommandNetworkNode>();

        [Header("PERFORMANCE")]
        [SerializeField] private PerformanceMetrics performance = new PerformanceMetrics();

        public string UnitId => unitId;
        public string DisplayName => displayName;
        public UnitCategory Category => category;
        public string Role => role;
        public string Tier => tier;
        public string Status => status;
        public Sprite StoreImage => storeImage;
        public GameObject ModelPrefab => modelPrefab;
        public PhysicalTechnical Physical => physical;
        public AutonomyAI Autonomy => autonomy;
        public SensorData Sensors => sensors;
        public CommunicationData Communication => communication;
        public CombatThreat Combat => combat;
        public SquadCommand Squad => squad;
        public VROperator VROperator => vrOperator;
        public EconomyLogistics Economy => economy;
        public BuildRequirements BuildRequirements => buildRequirements;
        public EnvironmentalData Environment => environment;
        public NetworkDynamics Network => network;
        public CognitiveData Cognitive => cognitive;
        public IReadOnlyList<BehaviorNode> BehaviorNodes => behaviorNodes;
        public IReadOnlyList<MissionBehavior> MissionBehaviors => missionBehaviors;
        public IReadOnlyList<UnitSynergy> UnitSynergies => unitSynergies;
        public IReadOnlyList<DamageResponse> DamageResponses => damageResponses;
        public IReadOnlyList<UpgradeEffect> Upgrades => upgrades;
        public TacticalIntegration Tactical => tactical;
        public MaintenanceAnalytics Maintenance => maintenance;
        public IReadOnlyList<ThreatResponse> ThreatResponses => threatResponses;
        public IReadOnlyList<CommandNetworkNode> CommandNetwork => commandNetwork;
        public PerformanceMetrics Performance => performance;
    }

    [Serializable]
    public class PhysicalTechnical
    {
        public float lengthMeters;
        public float widthMeters;
        public float heightMeters;
        public float massKg;

        public string powerSource;

        public float energyCapacityWh;
        public float energyConsumptionWhPerMinute;

        public string endurance;
        public float payloadCapacityKg;
        public List<string> modules = new List<string>();

        public string navigationSystem;

        public float structuralIntegrityHp;
        public float armorRating;

        public string heatSignature;
        public string noiseSignature;

        public float topSpeedMetersPerSecond;
        public string topSpeedDescription;

        public string acceleration;
        public string turningRadius;
        public string climbRate;
        public string stabilityRating;
    }

    [Serializable]
    public class AutonomyAI
    {
        public string behaviorProfile;

        [Range(0f, 1f)]
        public float aggression;

        [Range(0f, 1f)]
        public float caution;

        [Range(0f, 1f)]
        public float persistence;

        [Range(0f, 1f)]
        public float flankingLikelihood;

        [Range(0f, 1f)]
        public float retreatThreshold;

        public string retreatBehaviorDescription;
        public string targetPriority;
        public string formationDiscipline;

        [Range(0f, 1f)]
        public float riskTolerance;
    }

    [Serializable]
    public class SensorData
    {
        public float visualRangeMeters;
        public string visualRangeDescription;

        public float thermalRangeMeters;
        public string thermalRangeDescription;

        public string acousticRange;
        public string radarRange;
        public string lidarRange;
        public string signalDetectionRange;
    }

    [Serializable]
    public class CommunicationData
    {
        public float meshRangeKm;
        public string meshRangeDescription;

        public float relayCapacity;
        public string commandLinkType;
        public string networkPriority;
    }

    [Serializable]
    public class CombatThreat
    {
        public float firepowerRating;
        public string firepowerDescription;

        public string accuracy;
        public string stabilization;
        public string weaponMounts;
        public string weaponTypes;

        public float suppressionValue;

        public string trackingStrength;
        public string disruptionStrength;
        public string breachCapability;
        public string countermeasureRating;

        public int threatLevel;

        public string engagementDistance;
        public string preferredTerrain;
        public string preferredTargets;
        public string vulnerabilityProfile;
    }

    [Serializable]
    public class SquadCommand
    {
        public string squadRole;
        public int squadSizeContribution;
        public string formationBehavior;
        public string intentInterpretationRules;
        public string retreatBehavior;
        public string breachBehavior;
        public string flankingBehavior;
        public string reinforcementBehavior;
        public string parentCommandUnit;
        public List<string> subordinateUnits = new List<string>();
        public List<string> relayNodes = new List<string>();
        public string networkDependencies;
    }

    [Serializable]
    public class VROperator
    {
        public string operatorSeatType;
        public string operatorHud;
        public string cameraFeeds;

        public bool thermalView;
        public bool lidarView;
        public bool sonarView;
        public bool tacticalOverlay;

        public string movementControls;
        public string sensorControls;
        public string abilityControls;
        public string weaponControls;
        public string stabilizationControls;

        public List<string> operatorAbilities = new List<string>();
    }

    [Serializable]
    public class EconomyLogistics
    {
        public int creditCost;
        public string buildCost;
        public float buildTimeSeconds;
        public string factoryType;
        public string techRequirements;
        public string maintenanceCost;
        public float repairTimeSeconds;
        public string supplyConsumption;
        public string operationalCost;
        public string deploymentRequirements;
        public string transportCompatibility;
        public string launchMethod;

        public bool deploymentPointsDefined;
        public int deploymentPoints;
    }

    [Serializable]
    public class BuildRequirements
    {
        public List<ResourceRequirement> commonComponents =
            new List<ResourceRequirement>();

        public List<ResourceRequirement> electronics =
            new List<ResourceRequirement>();

        public List<ResourceRequirement> power =
            new List<ResourceRequirement>();

        public List<ResourceRequirement> rareTradeable =
            new List<ResourceRequirement>();

        public List<string> optionalUpgrades =
            new List<string>();
    }

    [Serializable]
    public class ResourceRequirement
    {
        public string component;
        public int quantity;
    }

    [Serializable]
    public class EnvironmentalData
    {
        public string terrainAdaptationCoefficients;
        public float weatherSensitivityIndex;
        public string visibilityModifiers;
        public string signalPropagationCurve;

        public List<EnvironmentalModifier> modifiers =
            new List<EnvironmentalModifier>();
    }

    [Serializable]
    public class EnvironmentalModifier
    {
        public string condition;
        public string effect;
    }

    [Serializable]
    public class NetworkDynamics
    {
        public float nodeHandoffEfficiency;
        public string packetLossRate;
        public float encryptionLatencySeconds;
        public string meshSelfHealingThreshold;
        public string relaySaturationCurve;
    }

    [Serializable]
    public class CognitiveData
    {
        [Range(0f, 1f)]
        public float decisionConfidence;

        [Range(0f, 1f)]
        public float threatRecognitionAccuracy;

        public float adaptiveLearningRatePerMission;

        [Range(0f, 1f)]
        public float commandOverrideResistance;

        [Range(0f, 1f)]
        public float cooperativeBehaviorIndex;
    }

    [Serializable]
    public class BehaviorNode
    {
        public string behaviorNode;

        [Range(0f, 1f)]
        public float weight;

        public string description;
    }

    [Serializable]
    public class MissionBehavior
    {
        public string missionType;
        public string behaviorFocus;

        [Range(0f, 1f)]
        public float efficiency;
    }

    [Serializable]
    public class UnitSynergy
    {
        public string partnerUnit;

        [Range(0f, 1f)]
        public float synergyRating;

        public string function;
    }

    [Serializable]
    public class DamageResponse
    {
        public string damageType;
        public string effect;
        public string recovery;
    }

    [Serializable]
    public class UpgradeEffect
    {
        public string upgrade;
        public string effect;

        [Range(0f, 1f)]
        public float efficiencyGain;
    }

    [Serializable]
    public class TacticalIntegration
    {
        [Range(0f, 1f)]
        public float formationAdaptability;

        [Range(0f, 1f)]
        public float tacticalRoleWeight;

        public float commandResponseDelaySeconds;
        public string reinforcementTriggerConditions;
        public string retreatLogicThresholds;
    }

    [Serializable]
    public class MaintenanceAnalytics
    {
        public float meanTimeBetweenFailuresHours;
        public float repairCycleSeconds;
        public float resourceDrainPerHourPercent;
        public float batteryEfficiencyPercent;
        public float rotorStabilityPercent;
        public float sensorCalibrationDriftPercentPerHour;
        public float coolingEfficiencyPercent;
        public float operationalUptimePercent;
        public float energyRecoveryRatePercent;
        public float energyRecoveryRatePercentPerIdleMinute;
        public float componentWearPercentPerHour;
        public string maintenancePriorityLevel;
        public float sparePartDependencyIndex;
        public string operationalCostPerHour;
    }

    [Serializable]
    public class ThreatResponse
    {
        public string threatType;
        public float reactionTimeSeconds;

        [Range(0f, 1f)]
        public float countermeasureEfficiency;
    }

    [Serializable]
    public class CommandNetworkNode
    {
        public string node;
        public float loadPercent;
        public float latencySeconds;
        public string status;
    }

    [Serializable]
    public class PerformanceMetrics
    {
        [Range(0f, 1f)]
        public float missionSuccessProbability;

        public float averageEngagementDurationMinutes;
        public float reconDataYieldMbPerMinute;

        [Range(0f, 1f)]
        public float communicationUptime;

        [Range(0f, 1f)]
        public float threatAvoidanceEfficiency;
    }
}




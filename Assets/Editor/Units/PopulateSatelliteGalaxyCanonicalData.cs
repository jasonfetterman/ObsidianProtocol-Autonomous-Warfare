using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateSatelliteGalaxyCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Satellite/Galaxy/Galaxy.asset";

        private static bool S(SerializedObject so, string value, params string[] names)
        {
            foreach (string name in names)
            {
                SerializedProperty p = so.FindProperty(name);
                if (p != null && p.propertyType == SerializedPropertyType.String)
                {
                    p.stringValue = value;
                    return true;
                }
            }
            return false;
        }

        private static bool N(SerializedObject so, float value, params string[] names)
        {
            foreach (string name in names)
            {
                SerializedProperty p = so.FindProperty(name);
                if (p == null) continue;

                if (p.propertyType == SerializedPropertyType.Float)
                {
                    p.floatValue = value;
                    return true;
                }

                if (p.propertyType == SerializedPropertyType.Integer)
                {
                    p.intValue = Mathf.RoundToInt(value);
                    return true;
                }
            }
            return false;
        }

        [MenuItem("Obsidian Protocol/Units/Populate Satellite Galaxy Canonical Data")]
        public static void Populate()
        {
            ScriptableObject asset =
                AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetPath);

            if (asset == null)
            {
                Debug.LogError("[GALAXY] Asset not found: " + AssetPath);
                return;
            }

            SerializedObject so = new SerializedObject(asset);
            so.Update();

            // Identity
            S(so, "obs-sat-galaxy", "unitID", "unitId", "UnitID", "id", "ID");
            S(so, "GALAXY", "displayName", "unitName", "UnitName", "unitDisplayName");
            S(so, "Ultra-Long-Range Relay / Intercontinental Signal Projection",
                "role", "unitRole", "description", "roleDescription");
            S(so, "Satellite", "tier", "unitTier", "category", "unitCategory");
            S(so, "Active", "status", "unitStatus");
            S(so, "Massive high-orbit strategic relay satellite with an oversized ultra-long-range relay dish, expanded solar arrays, dual micro-reactor backups, protected signal amplification hardware, stabilization gyros, orbital correction thrusters, and hardened communications architecture. Heavy-duty military aerospace construction with minimal exposed components, very low thermal signature, no weapon systems, and extensive long-range relay infrastructure. Designed for intercontinental communication, strategic mesh projection, deep-orbit relay operations, and global command network extension.",
                "visualIdentity", "visualDescription", "appearanceDescription", "loreDescription");

            // Operational parameters
            S(so, "Ultra-Long-Range Relay / Intercontinental Signal Projection",
                "missionType", "operationalMissionType");
            S(so, "Deep orbit / maximum intercontinental coverage",
                "optimalDeploymentZone", "deploymentZone");
            N(so, 0.35f, "commandLatency", "commandLatencySeconds");
            N(so, 44f, "relayLoad", "relayLoadPercent");
            N(so, 99f, "signalIntegrity", "signalIntegrityPercent");
            N(so, 99.6f, "aiSyncRate", "aiSyncRatePercent");
            N(so, 3f, "tacticalRefreshInterval", "tacticalRefreshSeconds");
            N(so, 0.75f, "strategicRefreshInterval", "strategicRefreshSeconds", "relayRefresh");
            S(so, "Tier IV", "autonomousDecisionDepth", "decisionDepth");
            S(so, "Command Core > Nexus / Fusion > Galaxy AI > Manual",
                "overridePriority", "commandOverridePriority");
            S(so, "Strategic Relay Support", "commandAuthority");
            S(so, "Global", "commandRange");
            S(so, "Very High", "processingCapacity");
            N(so, 14f, "networkCapacity", "networkCapacityNodes");
            S(so, "Yes", "autonomousOperation", "isAutonomous");

            // Physical and technical
            N(so, 18f, "length", "Length");
            N(so, 10f, "width", "Width");
            N(so, 6f, "height", "Height");
            N(so, 15000f, "mass", "Mass", "massKg");
            S(so, "Expanded solar array + dual micro-reactor backups", "powerSource", "energySource");
            N(so, 85000f, "energyCapacity", "energyCapacityWh", "powerCapacity");
            N(so, 200f, "fuelCapacity", "fuelCapacityLiters");
            S(so, "12–18 years", "endurance", "operationalEndurance");
            S(so, "Internal systems only", "payloadCapacity", "payload");
            S(so, "Ultra-long-range relay dish; Intercontinental signal amplifier; Stabilization gyros; Micro-thruster array",
                "modules", "moduleList", "installedModules");
            S(so, "Orbital AI + star tracker", "navigationSystem", "navigation");
            N(so, 3000f, "structuralIntegrity", "integrity", "hitPoints", "maxHealth");
            N(so, 0.70f, "armorRating", "armor");
            S(so, "Very Low", "heatSignature", "heatSignatureLevel");
            S(so, "None", "noiseSignature", "noiseSignatureLevel");
            S(so, "Orbital velocity", "topSpeed", "maximumSpeed");
            S(so, "Maximum", "stabilityRating", "stability");

            // Environmental interaction
            S(so, "Deep Orbit 1.00 / High Orbit 0.96 / Debris Zone 0.72 / Radiation Zone 0.84 / Solar Exposure 0.78",
                "orbitalEnvironment", "environmentModifiers");
            N(so, 0.04f, "weatherSensitivityIndex", "weatherSensitivity");
            S(so, "Direct Solar 1.0 / Eclipse 0.84 / Orbital Debris 0.72 / Solar Distortion 0.80",
                "visibilityModifiers");
            S(so, "-0.025% effective relay efficiency per 10% orbital interference",
                "signalPropagationCurve", "orbitalSignalPropagationCurve");
            N(so, 99f, "orbitalDataReliability", "orbitalDataReliabilityPercent");
            N(so, 99f, "starTrackerReliability", "starTrackerReliabilityPercent");
            S(so, "-18% signal stability / -12% orbital sensor accuracy",
                "severeSolarStormEffect", "solarStormEffect");
            S(so, "-22% external signal efficiency", "electromagneticInterference", "emiEffect");
            S(so, "Orbital position, solar conditions, radiation, debris, and electromagnetic interference primarily affect relay efficiency and orbital stability.",
                "environmentalRole", "environmentalDescription");

            // Autonomy and AI behavior
            S(so, "Ultra-long-range relay / deep-orbit stabilization",
                "behaviorProfile", "aiBehaviorProfile", "autonomyProfile");
            N(so, 0f, "aggression", "aggressionLevel");
            N(so, 1f, "caution", "cautionLevel");
            N(so, 1f, "persistence", "persistenceLevel");
            N(so, 0f, "flankingLikelihood", "flankingChance");
            S(so, "N/A", "retreatThreshold", "aiRetreatThreshold");
            S(so, "None", "targetPriority", "aiTargetPriority");
            S(so, "Maximum", "formationDiscipline", "aiFormationDiscipline");
            S(so, "Very Low", "riskTolerance", "aiRiskTolerance");
            S(so, "Ultra-Range Relay: 1.00 — Maintain assigned intercontinental relay coverage; Signal Amplification: 0.98 — Maximize long-distance transmission efficiency; Mesh Routing: 0.98 — Route traffic through available strategic relay nodes; Orbital Stabilization: 0.97 — Maintain orbital position and attitude; Coverage Optimization: 0.98 — Preserve required intercontinental coverage; Signal Prioritization: 0.99 — Preserve command-critical communications; Interference Detection: 0.95 — Identify degraded communication paths; Deep-Orbit Monitoring: 0.93 — Monitor orbital and solar conditions; Redundant Relay Management: 0.96 — Shift traffic during node degradation; Operator Override: 0.93 — Accept authorized strategic direction",
                "aiBehaviorMatrix", "behaviorMatrix");

            // Sensors
            S(so, "Global", "opticalRange", "opticalSensorRange");
            S(so, "Medium", "thermalRange", "thermalSensorRange");
            S(so, "None", "acousticRange", "acousticSensorRange");
            S(so, "Medium", "radarRange", "radarSensorRange");
            S(so, "None", "lidarRange", "lidarSensorRange");
            S(so, "Extremely High", "signalDetectionRange", "signalRange");
            S(so, "Continuous", "orbitalPositionTracking", "orbitalTracking");
            S(so, "Continuous", "solarConditionMonitoring", "solarMonitoring");
            S(so, "Medium", "debrisDetection", "debrisDetectionRange");
            S(so, "High", "radiationMonitoring", "radiationMonitoringRange");

            // Communication
            S(so, "Global", "meshRange", "communicationMeshRange");
            N(so, 14f, "relayCapacity", "communicationRelayCapacity");
            S(so, "Satellite Mesh", "commandLinkType", "networkType");
            S(so, "Maximum", "networkPriority", "communicationPriority");
            N(so, 98.5f, "nodeHandoffEfficiency", "nodeHandoffEfficiencyPercent");
            S(so, "<0.6% nominal", "packetLossRate", "packetLoss");
            N(so, 0.04f, "encryptionLatency", "encryptionLatencySeconds");
            N(so, 3f, "meshSelfHealingThreshold", "selfHealingThreshold");
            S(so, "Throughput decreases 3% per additional active relay beyond 14",
                "relaySaturationCurve", "networkSaturationCurve");
            S(so, "Satellite Mesh / deep-space relay / hardened terrestrial",
                "redundantLinks", "redundantCommunicationLinks");
            S(so, "Ultra-long-range strategic relay", "commandNetworkRole");

            // Combat and threat model
            S(so, "None", "firepowerRating", "firepower");
            S(so, "None", "weaponMounts", "weapons");
            S(so, "Medium", "trackingStrength", "tracking");
            S(so, "High", "disruptionStrength", "disruption");
            S(so, "Medium", "countermeasureRating", "countermeasures");
            N(so, 8f, "threatLevel", "threat");
            S(so, "N/A", "engagementDistance", "engagementRange");
            S(so, "Deep orbit", "preferredTerrain", "preferredOrbit");
            S(so, "None", "preferredTargets", "targetPreferences");
            S(so, "Debris / EMP / solar storms", "vulnerabilityProfile", "vulnerabilities");

            // AI cognitive layer
            N(so, 0.97f, "decisionConfidence");
            N(so, 99f, "signalRecognitionAccuracy");
            N(so, 99f, "relayPriorityAccuracy");
            N(so, 97f, "orbitalPredictionAccuracy");
            N(so, 0.04f, "adaptiveLearningRate");
            N(so, 0.03f, "commandOverrideResistance");
            N(so, 0.99f, "cooperativeBehaviorIndex");
            S(so, "30 minutes", "strategicForecastHorizon", "forecastHorizon");
            S(so, "Yes", "autonomousRecovery", "canRecoverAutonomously");

            // Energy curve layer
            N(so, 1600f, "activeRelayEnergy", "activeRelayWhPerMinute");
            N(so, 2900f, "maximumSignalAmplificationEnergy", "maxAmplificationWhPerMinute");
            N(so, 900f, "orbitalStabilizationEnergy", "stabilizationWhPerMinute");
            N(so, 4200f, "maximumNetworkLoadEnergy", "maxNetworkLoadWhPerMinute");
            N(so, 240f, "idleEnergy", "idleWhPerMinute");
            N(so, 3f, "recoveryRate", "energyRecoveryPercentPerMinute");
            N(so, 500f, "emergencyPower", "emergencyPowerWhPerMinute");
            S(so, "Continuous when solar array is illuminated", "solarRecovery");
            S(so, "Continuous emergency regeneration", "reactorBackup", "reactorRecovery");

            // Damage and failure response
            S(so, "Debris Impact: Isolate damaged systems and preserve relay hardware; EMP: Harden communication systems and transfer traffic to protected channels; Solar Storm: Reduce nonessential amplification and protect relay core; Signal Jamming: Shift to alternate frequencies and redundant relay paths; Orbital Instability: Activate stabilization gyros and correction thrusters; Thermal Overload: Reduce amplifier load and activate thermal safeguards; Satellite Loss: Transfer relay responsibility to alternate Galaxy/Starcast; Network Failure: Re-route traffic through available strategic command nodes",
                "damageFailureResponse", "failureResponse", "damageResponse");

            // Maintenance and logistics
            N(so, 5500f, "meanTimeBetweenFailures", "mtbfHours");
            N(so, 80f, "repairCycle", "repairTime", "repairTimeSeconds");
            N(so, 0.10f, "resourceDrainPerHour");
            N(so, 97f, "solarArrayEfficiency");
            N(so, 96f, "reactorEfficiency");
            N(so, 96f, "coolingEfficiency");
            N(so, 0.08f, "communicationsHardwareWear");
            S(so, "Tier II", "maintenancePriority");
            N(so, 0.38f, "sparePartDependencyIndex");
            N(so, 8f, "operationalMetalPerHour", "metalCostPerHour");
            N(so, 12f, "operationalElectronicsPerHour", "electronicsCostPerHour");
            S(so, "Satellite Bay / Heavy Launch Infrastructure",
                "infrastructureRequirement", "infrastructureRequirements");

            // Upgrade scaling layer
            S(so, "Enhanced Relay Dish: +30% relay range / +8% signal strength; Thermal Boost: +25% thermal monitoring / +10% solar-storm resilience; Anti-Jamming Shielding: +35% signal resilience / +12% network uptime; Expanded Solar Array: +30% solar recovery / +10% sustained relay capacity; Advanced Orbital Stabilization: +20% orbital stability / +8% correction; Intercontinental Amplifier: +35% signal throughput / +10% relay efficiency; Deep-Orbit Processing Core: +20% processing capacity / +8% relay optimization",
                "upgradeScaling", "upgradeEffects", "upgradeDescriptions");

            // Tactical and strategic integration
            N(so, 0.99f, "formationAdaptability");
            N(so, 0.98f, "strategicRoleWeight");
            N(so, 0.35f, "commandResponseDelay", "commandResponseDelaySeconds");
            S(so, "Relay load >80% OR signal degradation >20%", "reinforcementTrigger");
            S(so, "Orbital instability OR primary intercontinental relay loss", "emergencyTrigger");
            S(so, "None", "retreatLogic");
            S(so, "Maximum", "strategicPriority");
            S(so, "Intercontinental relay; ultra-long-range communication; strategic mesh stabilization; deep-orbit signal projection",
                "preferredAssignments", "strategicAssignments");
            S(so, "Prohibited", "directCombatAssignment");
            S(so, "Not required", "directUnitMicromanagement");
            S(so, "Yes", "autonomousAssignment", "isAutonomousAssignment");

            // Performance metrics
            N(so, 98f, "ultraLongRangeRelayReliability");
            N(so, 98f, "intercontinentalSignalProjection");
            N(so, 99.5f, "communicationUptime");
            N(so, 97f, "networkRecoveryEfficiency");
            N(so, 99f, "coverageEfficiency");
            N(so, 320f, "strategicDataThroughput", "strategicDataThroughputMBPerMinute");
            N(so, 0.75f, "relayRefresh", "relayRefreshSeconds", "strategicRefreshInterval");
            N(so, 98f, "orbitalPositionAccuracy");
            N(so, 99f, "commandRelayReliability");

            // Threat-response model
            S(so, "Debris Impact: Reaction Time 0.20 s / Countermeasure Efficiency 82%; EMP: Reaction Time 0.10 s / Countermeasure Efficiency 93%; Solar Storm: Reaction Time 0.40 s / Countermeasure Efficiency 88%; Signal Jam: Reaction Time 0.07 s / Countermeasure Efficiency 96%; Orbital Instability: Reaction Time 0.25 s / Countermeasure Efficiency 92%; Thermal Overload: Reaction Time 0.35 s / Countermeasure Efficiency 90%; Satellite Failure: Reaction Time 0.20 s / Recovery Efficiency 97%; Radiation Surge: Reaction Time 0.30 s / Countermeasure Efficiency 86%",
                "threatResponseModel", "threatResponses");

            // Command-network analytics
            S(so, "Satellite Mesh: Load 44% / Latency 0.35 s / Status Optimal; Ultra-Range Relay Core: Load 48% / Latency 0.12 s / Status Optimal; Intercontinental Amplifier: Load 52% / Latency 0.10 s / Status Active; Orbital Control Core: Load 29% / Latency 0.15 s / Status Stable; Nexus: Load 46% / Latency 0.20 s / Status Active; Fusion: Load 43% / Latency 0.22 s / Status Active; Command Core: Load 39% / Latency 0.25 s / Status Active",
                "commandNetworkAnalytics", "networkAnalytics");

            // Squad and command integration
            S(so, "Ultra-Long-Range Relay", "squadRole", "unitSquadRole");
            N(so, 7f, "squadSizeContribution", "squadContribution");
            S(so, "Orbital spacing", "formationBehavior", "formation");
            S(so, "Maintain intercontinental communications coverage",
                "intentInterpretationRules", "squadIntent", "intentRules");
            S(so, "None", "retreatBehavior", "squadRetreatBehavior");
            S(so, "Supports all strategic and operational grids",
                "reinforcementBehavior", "reinforcement");
            S(so, "Nexus / Fusion", "parentCommandUnit", "parentCommand", "parentUnit");
            S(so, "None", "subordinateUnits", "subordinates");
            N(so, 14f, "relayNodes", "relayNodeCount");
            S(so, "Satellite Mesh", "networkDependencies", "networkDependency");
            S(so, "Nexus 0.98 / Fusion 0.97 / Starcast 0.96 / Command Core 0.94",
                "interunitCoordination", "coordinationIndex");

            // Mission behavior
            S(so, "Intercontinental Coverage 99%; Ultra-Range Relay Availability 98%; Signal Projection 98%; Mesh Relay Availability 97%; Orbital Stability 98%; Strategic Communication Support 99%; Interference Detection 95%; Network Recovery 97%; Autonomous Relay Management 99%",
                "missionBehavior", "missionPerformanceMetrics");

            // Command interpretation
            S(so, "ESTABLISH INTERCONTINENTAL COVERAGE; BOOST SIGNAL TO REGION; EXTEND STRATEGIC MESH; STABILIZE LONG-RANGE RELAY; PRIORITIZE COMMAND TRAFFIC; SYNCHRONIZE SATELLITE NETWORK; RESTORE DEGRADED INTERCONTINENTAL PATH; MAINTAIN ORBITAL POSITION; RELAY STRATEGIC COMMAND DATA. Example: COMMAND CORE → NEXUS / FUSION → GALAXY → EXTEND STRATEGIC MESH TO REGION 8. Galaxy evaluates orbital conditions and relay load, establishes a long-range relay path, synchronizes available satellite nodes, and reports coverage and signal status to Nexus / Fusion.",
                "commandInterpretation", "commandExamples", "supportedCommands");

            // VR operator mode
            S(so, "Ultra-Range Relay Console", "operatorSeatType", "operatorSeat");
            S(so, "Intercontinental Signal Overlay", "operatorHUD", "operatorHud", "hudName");
            S(so, "Optical + thermal", "cameraFeeds", "operatorCameraFeeds");
            S(so, "Yes", "thermalView", "hasThermalView");
            S(so, "Relay routes / signal load / mesh health", "tacticalOverlay", "operatorTacticalOverlay");
            S(so, "Relay Routing; Signal Amplification; Thermal View; Orbital Diagnostics; Mesh Monitoring; Intercontinental Coverage",
                "operatorControls", "controls");
            S(so, "Ultra-Range Boost; Mesh Stabilization; Signal Sync; Thermal Sweep; Orbital Correction; Emergency Relay Transfer",
                "operatorAbilities", "abilities");

            // Economy and logistics
            N(so, 2800f, "metalCost", "buildMetalCost", "costMetal");
            N(so, 1800f, "electronicsCost", "buildElectronicsCost", "costElectronics");
            N(so, 340f, "buildTime", "buildTimeSeconds");
            S(so, "Satellite Bay", "factoryType", "productionBuilding");
            S(so, "Satellite Systems III", "techRequirements", "technologyRequirement");
            S(so, "Low", "maintenanceCost", "maintenanceLevel");
            N(so, 80f, "repairTime", "repairTimeSeconds");
            S(so, "Very Low", "supplyConsumption", "supplyConsumptionLevel");
            S(so, "Very Low", "operationalCost", "operationalCostLevel");
            S(so, "Launch pad", "deploymentRequirements", "deploymentRequirement");
            S(so, "Satellite carrier", "transportCompatibility", "transportType");
            S(so, "Orbital insertion", "launchMethod", "deploymentMethod");
            S(so, "Satellite Technology / Construction", "playerAcquisition", "acquisitionMethod");

            // Player collection build requirements
            S(so, "150x Satellite Alloy Plates; 75x Reinforced Satellite Rods; 14x Hydraulic Actuators; 1x Ultra-Range Relay Assembly",
                "commonComponents", "commonBuildRequirements");
            S(so, "60x Circuit Bundles; 40x Signal Processors; 12x Satellite Control Chips; 4x Relay AI Chips",
                "electronicsComponents", "electronicsBuildRequirements");
            S(so, "1x Expanded Solar Array Assembly; 2x Micro-Reactor Cores; 3x Power Regulation Nodes",
                "powerComponents", "powerBuildRequirements");
            S(so, "1x Intercontinental Amplifier Module", "rareComponents", "rareBuildRequirements");
            S(so, "Enhanced Relay Dish; Thermal Boost; Anti-Jamming Shielding; Expanded Solar Array; Advanced Orbital Stabilization; Intercontinental Amplifier; Deep-Orbit Processing Core",
                "optionalUpgrades", "upgradeOptions");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log("[GALAXY] Canonical population pass finished. Verify serialized fields in the Inspector; unmatched field names are left unchanged.", asset);
        }
    }
}

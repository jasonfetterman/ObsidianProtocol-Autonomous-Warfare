using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.EditorTools
{
    public static class PopulateSatelliteStarcastCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Satellite/Starcast/Starcast.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Satellite Starcast Canonical Data")]
        public static void Populate()
        {
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetPath);
            if (asset == null)
            {
                Debug.LogError("[STARCAST] Unit asset not found: " + AssetPath +
                    ". Find the actual Starcast asset path before rerunning.");
                return;
            }

            var so = new SerializedObject(asset);
            so.Update();

            // Identity
            S(so, "obs-sat-starcast", "unitId", "id", "unitID");
            S(so, "Starcast", "displayName", "unitName");
            S(so, "High-Orbit Broadcast / Strategic Signal Distribution", "role", "unitRole");
            S(so, "Satellite", "tier", "unitTier");
            S(so, "Active", "status", "unitStatus");
            S(so, "High-Orbit Broadcast / Strategic Signal Distribution", "missionType");
            S(so, "High orbit / strategic satellite coverage", "optimalDeploymentZone");

            // Operational parameters
            F(so, 0.25f, "commandLatency", "commandLatencySeconds", "commandResponseDelay");
            F(so, 36f, "relayLoad", "relayLoadPercent");
            F(so, 99.20f, "signalIntegrity", "signalIntegrityPercent", "commandRelayReliability");
            F(so, 99.70f, "aiSyncRate", "aiSyncRatePercent");
            F(so, 2f, "tacticalRefreshInterval", "tacticalRefreshIntervalSeconds");
            F(so, 0.5f, "strategicRefreshInterval", "strategicRefreshIntervalSeconds", "broadcastRefresh");
            S(so, "Tier III", "autonomousDecisionDepth");
            S(so, "Command Core > Fusion / Nexus > Starcast AI > Manual", "overridePriority");
            S(so, "Strategic Support", "commandAuthority");
            S(so, "Global", "commandRange");
            S(so, "High", "processingCapacity");
            I(so, 8, "networkCapacity", "relayCapacity", "relayNodes");
            B(so, true, "autonomousOperation", "autonomousAssignment");

            // Physical and technical
            F(so, 10f, "length", "lengthMeters");
            F(so, 6f, "width", "widthMeters");
            F(so, 4f, "height", "heightMeters");
            F(so, 7200f, "mass", "massKg");
            S(so, "Expanded solar array + micro-reactor backup", "powerSource");
            F(so, 32000f, "energyCapacity", "energyCapacityWh");
            F(so, 80f, "fuelCapacity", "fuelCapacityLiters");
            S(so, "80 L (orbital correction thrusters)", "fuelDescription", "fuelType");
            S(so, "6–10 years", "endurance");
            S(so, "Internal systems only", "payloadCapacity");
            S(so, "High-orbit broadcast dish; Wide-band signal amplifier; Stabilization gyros; Micro-thruster array", "modules", "moduleDescription");
            S(so, "Orbital AI + star tracker", "navigationSystem");
            F(so, 1600f, "structuralIntegrity", "maxHealth", "health");
            F(so, 0.50f, "armorRating", "armor");
            S(so, "Very Low", "heatSignature");
            S(so, "None", "noiseSignature");
            S(so, "Orbital velocity", "topSpeed", "speedDescription");
            S(so, "Maximum", "stabilityRating");

            // Environmental interaction
            S(so, "High Orbit 1.00 / Low Orbit 0.92 / Debris Zone 0.70 / Radiation Zone 0.82 / Solar Exposure 0.75", "orbitalEnvironment", "orbitalEnvironmentModifiers");
            F(so, 0.05f, "weatherSensitivityIndex");
            S(so, "Direct Solar 1.0 / Eclipse 0.82 / Orbital Debris 0.70 / Solar Distortion 0.78", "visibilityModifiers");
            S(so, "−0.03% effective relay efficiency per 10% orbital interference", "signalPropagationCurve");
            F(so, 99f, "orbitalDataReliability");
            F(so, 98f, "starTrackerReliability");
            S(so, "−20% signal stability / −15% orbital sensor accuracy", "severeSolarStormEffect");
            S(so, "−25% external signal efficiency", "electromagneticInterference");
            S(so, "Orbital positioning, solar conditions, radiation, debris, and electromagnetic interference primarily affect signal quality and orbital stability.", "environmentalRole");

            // Autonomy and AI
            S(so, "Broadcast / long-range relay / orbital stabilization", "behaviorProfile", "aiBehaviorProfile");
            F(so, 0f, "aggression");
            F(so, 1f, "caution");
            F(so, 1f, "persistence");
            F(so, 0f, "flankingLikelihood");
            S(so, "N/A", "retreatThreshold");
            S(so, "None (non-combat)", "targetPriority");
            S(so, "Maximum", "formationDiscipline");
            S(so, "Very Low", "riskTolerance");
            S(so, "1.00 — Maintain assigned strategic broadcast coverage", "broadcastManagement");
            S(so, "0.98 — Optimize long-range transmission strength", "signalAmplification");
            S(so, "0.97 — Route traffic through available satellite nodes", "meshRouting");
            S(so, "0.96 — Maintain orbital position and attitude", "orbitalStabilization");
            S(so, "0.98 — Preserve high-priority command traffic", "signalPrioritization");
            S(so, "0.94 — Identify degraded communication paths", "interferenceDetection");
            S(so, "0.95 — Maintain required strategic coverage", "coverageOptimization");
            S(so, "0.90 — Monitor thermal and solar conditions", "thermalMonitoring");
            S(so, "0.92 — Accept authorized command direction", "operatorOverride");

            // Sensors
            S(so, "Global", "opticalRange");
            S(so, "Medium", "thermalRange");
            S(so, "None", "acousticRange");
            S(so, "Medium", "radarRange");
            S(so, "None", "lidarRange", "lidarRangeDescription");
            S(so, "Extremely High", "signalDetectionRange");
            S(so, "Continuous", "orbitalPositionTracking");
            S(so, "Continuous", "solarConditionMonitoring");
            S(so, "Medium", "debrisDetection");

            // Communication
            S(so, "Global", "meshRange");
            S(so, "Satellite Mesh", "commandLinkType");
            S(so, "Maximum", "networkPriority");
            F(so, 98f, "nodeHandoffEfficiency");
            S(so, "<0.8% nominal", "packetLossRate");
            F(so, 0.05f, "encryptionLatency", "encryptionLatencySeconds");
            I(so, 2, "meshSelfHealingThreshold");
            S(so, "Throughput decreases 4% per additional active relay beyond 8", "relaySaturationCurve");
            S(so, "Satellite Mesh / terrestrial command network", "redundantLinks");
            S(so, "High-orbit strategic signal distribution", "broadcastRole");

            // Combat and threat model
            S(so, "None", "firepowerRating", "firepower");
            S(so, "None", "weaponMounts");
            S(so, "Medium", "trackingStrength");
            S(so, "High", "disruptionStrength");
            S(so, "Medium", "countermeasureRating");
            I(so, 6, "threatLevel");
            S(so, "N/A", "engagementDistance");
            S(so, "High orbit", "preferredTerrain");
            S(so, "None", "preferredTargets");
            S(so, "Debris / EMP / solar storms", "vulnerabilityProfile");

            // Cognitive layer
            F(so, 0.96f, "decisionConfidence");
            F(so, 98f, "signalRecognitionAccuracy");
            F(so, 99f, "broadcastPriorityAccuracy");
            F(so, 95f, "orbitalPredictionAccuracy");
            F(so, 0.04f, "adaptiveLearningRate");
            F(so, 0.03f, "commandOverrideResistance");
            F(so, 0.98f, "cooperativeBehaviorIndex");
            S(so, "20 minutes", "forecastHorizon");
            B(so, true, "autonomousRecovery");

            // Energy curve
            F(so, 820f, "activeBroadcast", "activeBroadcastWhPerMinute");
            F(so, 1450f, "maximumSignalAmplification", "maximumSignalAmplificationWhPerMinute");
            F(so, 520f, "orbitalStabilizationEnergy", "orbitalStabilizationWhPerMinute");
            F(so, 1900f, "maximumNetworkLoad", "maximumNetworkLoadWhPerMinute");
            F(so, 140f, "idleEnergy", "idleWhPerMinute");
            F(so, 3f, "recoveryPercentPerMinute");
            F(so, 300f, "emergencyPower", "emergencyPowerWhPerMinute");
            S(so, "Continuous when solar array is illuminated", "solarRecovery");
            S(so, "Continuous emergency regeneration", "reactorBackup");

            // Damage and failure response
            S(so, "Isolate damaged hardware and preserve broadcast", "ballisticDebrisImpactResponse");
            S(so, "Harden communications and transfer traffic to protected channels", "empResponse");
            S(so, "Reduce nonessential transmission and protect core systems", "solarStormResponse");
            S(so, "Shift to alternate frequencies and relay paths", "signalJammingResponse");
            S(so, "Activate stabilization gyros and correction thrusters", "orbitalInstabilityResponse");
            S(so, "Reduce amplifier load and activate thermal safeguards", "thermalOverloadResponse");
            S(so, "Transfer broadcast responsibility to alternate nodes", "satelliteLossResponse");

            // Maintenance and logistics
            F(so, 4000f, "meanTimeBetweenFailures", "mtbfHours");
            F(so, 50f, "repairCycle", "repairTime", "repairTimeSeconds");
            F(so, 0.12f, "resourceDrainPerHour");
            F(so, 96f, "solarArrayEfficiency");
            F(so, 94f, "reactorEfficiency");
            F(so, 95f, "coolingEfficiency");
            F(so, 0.10f, "communicationsHardwareWearPer100Hours");
            S(so, "Tier II", "maintenancePriority");
            F(so, 0.34f, "sparePartDependencyIndex");
            S(so, "5 Metal / 8 Electronics per hour", "operationalCostPerHour");
            S(so, "Satellite Bay / Launch Infrastructure", "infrastructureRequirement");

            // Upgrade scaling
            S(so, "+25% broadcast range / +8% signal strength", "enhancedBroadcastDish");
            S(so, "+25% thermal monitoring / +10% solar-storm resilience", "thermalBoost");
            S(so, "+35% signal resilience / +12% network uptime", "antiJammingShielding");
            S(so, "+30% solar recovery / +10% operational endurance", "expandedSolarArray");
            S(so, "+20% orbital stability / +8% correction", "orbitalStabilizationCore");
            S(so, "+30% throughput / +10% relay efficiency", "advancedSignalAmplifier");

            // Tactical / strategic integration
            F(so, 0.98f, "formationAdaptability");
            F(so, 0.94f, "strategicRoleWeight");
            S(so, "Signal degradation >20% OR relay capacity >80%", "reinforcementTrigger");
            S(so, "Orbital instability OR primary broadcast failure", "emergencyTrigger");
            S(so, "None", "retreatLogic");
            S(so, "Maximum", "strategicPriority");
            S(so, "Strategic broadcast, signal distribution, mesh stabilization, orbital relay, command-network support", "preferredAssignments");
            S(so, "Prohibited", "directCombatAssignment");
            S(so, "Not required", "directUnitMicromanagement");
            B(so, true, "autonomousAssignment");

            // Performance metrics
            F(so, 98f, "broadcastReliability");
            F(so, 97f, "signalDistributionEfficiency");
            F(so, 99.30f, "communicationUptime");
            F(so, 96f, "networkRecoveryEfficiency");
            F(so, 98f, "coverageEfficiency");
            F(so, 180f, "strategicDataThroughputMBPerMinute");
            F(so, 97f, "orbitalPositionAccuracy");
            F(so, 99.20f, "commandRelayReliability");

            // Threat-response model
            S(so, "Reaction Time 0.20 s / Countermeasure Efficiency 78%", "debrisImpactResponse");
            S(so, "Reaction Time 0.10 s / Countermeasure Efficiency 91%", "empThreatResponse");
            S(so, "Reaction Time 0.50 s / Countermeasure Efficiency 84%", "solarStormThreatResponse");
            S(so, "Reaction Time 0.08 s / Countermeasure Efficiency 94%", "signalJamThreatResponse");
            S(so, "Reaction Time 0.30 s / Countermeasure Efficiency 89%", "orbitalInstabilityThreatResponse");
            S(so, "Reaction Time 0.40 s / Countermeasure Efficiency 87%", "thermalOverloadThreatResponse");
            S(so, "Reaction Time 0.25 s / Recovery Efficiency 96%", "satelliteFailureThreatResponse");

            // Command-network analytics
            S(so, "Load 36% / Latency 0.25 s / Status Optimal", "satelliteMeshAnalytics");
            S(so, "Load 43% / Latency 0.08 s / Status Active", "broadcastAmplifierAnalytics");
            S(so, "Load 31% / Latency 0.12 s / Status Stable", "orbitalControlCoreAnalytics");
            S(so, "Load 39% / Latency 0.18 s / Status Active", "fusionAnalytics");
            S(so, "Load 42% / Latency 0.16 s / Status Active", "nexusAnalytics");
            S(so, "Load 35% / Latency 0.20 s / Status Active", "commandCoreAnalytics");

            // Squad and command integration
            S(so, "Strategic Broadcast", "squadRole");
            I(so, 4, "squadSizeContribution");
            S(so, "Orbital spacing", "formationBehavior");
            S(so, "Maintain broadcast coverage and strategic signal availability", "intentInterpretationRules");
            S(so, "None", "retreatBehavior");
            S(so, "Supports all command and operational grids", "reinforcementBehavior");
            S(so, "Fusion / Nexus", "parentCommandUnit");
            S(so, "None", "subordinateUnits");
            S(so, "Satellite Mesh", "networkDependencies");
            F(so, 0.95f, "fusionCoordination");
            F(so, 0.97f, "nexusCoordination");
            F(so, 0.94f, "commandCoreCoordination");

            // Mission behavior
            F(so, 98f, "broadcastCoverage");
            F(so, 97f, "signalDistribution");
            F(so, 96f, "meshRelayAvailability");
            F(so, 98f, "orbitalStability");
            F(so, 98f, "strategicCommunicationSupport");
            F(so, 95f, "networkRecovery");
            F(so, 98f, "autonomousBroadcastManagement");

            // Command interpretation
            S(so, "ESTABLISH BROADCAST COVERAGE; BOOST SIGNAL TO SECTOR; STABILIZE SATELLITE MESH; PRIORITIZE COMMAND TRAFFIC; SYNCHRONIZE SATELLITE NETWORK; RESTORE DEGRADED SIGNAL PATH; MAINTAIN ORBITAL POSITION; RELAY STRATEGIC COMMAND DATA", "commandInterpretation", "supportedCommands");
            S(so, "COMMAND CORE → FUSION / NEXUS → STARCAST → BOOST SIGNAL TO SECTOR 4 → evaluates orbital conditions and network load → increases assigned broadcast capacity → synchronizes connected relay nodes → reports signal status to Fusion / Nexus.", "commandExample");

            // VR operator mode
            S(so, "Broadcast Console", "operatorSeatType");
            S(so, "High-Orbit Signal Overlay", "operatorHUD", "operatorHud");
            S(so, "Optical + thermal", "cameraFeeds");
            B(so, true, "thermalView");
            S(so, "Broadcast routes / signal load / mesh health", "tacticalOverlay");
            S(so, "Broadcast Routing; Signal Amplification; Thermal View; Orbital Diagnostics; Mesh Monitoring", "operatorControls");
            S(so, "Broadcast Boost; Mesh Stabilization; Signal Sync; Thermal Sweep; Orbital Correction; Emergency Relay Transfer", "operatorAbilities");

            // Economy and logistics
            S(so, "1,600 Metal / 1,000 Electronics", "buildCost");
            I(so, 1600, "metalCost", "buildCostMetal");
            I(so, 1000, "electronicsCost", "buildCostElectronics");
            F(so, 220f, "buildTime", "buildTimeSeconds");
            S(so, "Satellite Bay", "factoryType");
            S(so, "Satellite Systems II", "techRequirements", "technologyRequirements");
            S(so, "Low", "maintenanceCost");
            S(so, "Very Low", "supplyConsumption");
            S(so, "Very Low", "operationalCost");
            S(so, "Launch pad", "deploymentRequirements");
            S(so, "Satellite carrier", "transportCompatibility");
            S(so, "Orbital insertion", "launchMethod");
            S(so, "Satellite Technology / Construction", "playerAcquisition");

            // Player collection build requirements
            S(so, "80x Satellite Alloy Plates; 40x Reinforced Satellite Rods; 8x Hydraulic Actuators; 1x Broadcast Dish Assembly", "commonComponents");
            S(so, "36x Circuit Bundles; 26x Signal Processors; 8x Satellite Control Chips; 3x Broadcast AI Chips", "electronicsComponents");
            S(so, "1x Expanded Solar Array Assembly; 1x Micro-Reactor Core; 2x Power Regulation Nodes", "powerComponents");
            S(so, "1x Wide-Band Broadcast Module", "rareComponents", "tradeableComponents");
            S(so, "Enhanced Broadcast Dish; Thermal Boost; Anti-Jamming Shielding; Expanded Solar Array; Orbital Stabilization Core; Advanced Signal Amplifier", "optionalUpgrades");

            // Visual identity
            S(so, "Large high-orbit autonomous communications satellite with a broad directional broadcast dish, expanded solar arrays, protected signal amplification hardware, stabilization gyros, micro-thruster correction systems, and compact micro-reactor backup. Clean military aerospace construction, hardened communications housings, minimal exposed components, low thermal signature, and no weapon systems. Designed exclusively for strategic broadcast coverage, long-range signal distribution, orbital relay, and command-network stabilization.", "visualIdentity", "description");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log("[STARCAST] Population pass finished for " + AssetPath +
                ". Asset loaded and pass ran; matching serialized fields still require verification.");
        }

        private static void S(SerializedObject so, string value, params string[] names)
        {
            foreach (var name in names)
            {
                var p = so.FindProperty(name);
                if (p == null || p.propertyType != SerializedPropertyType.String) continue;
                p.stringValue = value;
                return;
            }
        }

        private static void I(SerializedObject so, int value, params string[] names)
        {
            foreach (var name in names)
            {
                var p = so.FindProperty(name);
                if (p == null) continue;
                if (p.propertyType == SerializedPropertyType.Integer)
                {
                    p.intValue = value;
                    return;
                }
                if (p.propertyType == SerializedPropertyType.Float)
                {
                    p.floatValue = value;
                    return;
                }
            }
        }

        private static void F(SerializedObject so, float value, params string[] names)
        {
            foreach (var name in names)
            {
                var p = so.FindProperty(name);
                if (p == null) continue;
                if (p.propertyType == SerializedPropertyType.Float)
                {
                    p.floatValue = value;
                    return;
                }
                if (p.propertyType == SerializedPropertyType.Integer)
                {
                    p.intValue = Mathf.RoundToInt(value);
                    return;
                }
            }
        }

        private static void B(SerializedObject so, bool value, params string[] names)
        {
            foreach (var name in names)
            {
                var p = so.FindProperty(name);
                if (p == null || p.propertyType != SerializedPropertyType.Boolean) continue;
                p.boolValue = value;
                return;
            }
        }
    }
}

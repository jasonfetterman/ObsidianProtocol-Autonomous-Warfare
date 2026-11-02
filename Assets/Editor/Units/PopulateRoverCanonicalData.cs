using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateRoverCanonicalData
{
    private const string AssetPath = "Assets/Game/Units/Database/Ground/Rover/Rover.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Rover Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Rover] UnitDefinition not found at " + AssetPath +
                ". Verify the actual Rover asset path; no asset was modified.");
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        SetString(so, "unitId", "obs-ground-rover");
        SetString(so, "displayName", "Rover");
        SetEnum(so, "category", "Ground");
        SetString(so, "role", "Terrain Survey / Mobile Mapping");
        SetString(so, "tier", "Recon / Engineering Hybrid");
        SetString(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 3.7f);
        SetFloat(so, "physical.widthMeters", 2.0f);
        SetFloat(so, "physical.heightMeters", 1.6f);
        SetFloat(so, "physical.massKg", 820f);
        SetString(so, "physical.powerSource", "Electric-hybrid");
        SetFloat(so, "physical.energyCapacityWh", 5000f);
        SetFloat(so, "physical.energyConsumptionWhPerMinute", 42f);
        SetString(so, "physical.endurance", "5–9 hours");
        SetFloat(so, "physical.payloadCapacityKg", 150f);
        SetStrings(so, "physical.modules", new[]
        {
            "Terrain scanner",
            "Mapping suite",
            "Stabilization cradle"
        });
        SetString(so, "physical.navigationSystem", "GPS + Survey AI");
        SetFloat(so, "physical.structuralIntegrityHp", 650f);
        SetFloat(so, "physical.armorRating", 0.30f);
        SetString(so, "physical.heatSignature", "Low");
        SetString(so, "physical.noiseSignature", "Low");
        SetFloat(so, "physical.topSpeedMetersPerSecond", 17f);
        SetString(so, "physical.topSpeedDescription", "17 m/s");
        SetString(so, "physical.acceleration", "Medium");
        SetString(so, "physical.turningRadius", "Tight");
        SetString(so, "physical.stabilityRating", "High");
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 35f);

        // Autonomy and AI behavior
        SetString(so, "autonomy.behaviorProfile", "Survey / Mapping / Recon");
        SetFloat(so, "autonomy.aggression", 0f);
        SetFloat(so, "autonomy.caution", 0.85f);
        SetFloat(so, "autonomy.persistence", 0.9f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.8f);
        SetFloat(so, "autonomy.riskTolerance", 0.2f);
        SetString(so, "autonomy.retreatBehaviorDescription", "Early; withdraw when damage exceeds threshold");
        SetString(so, "autonomy.targetPriority", "Terrain anomalies / structural features");
        SetString(so, "autonomy.formationDiscipline", "Medium");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1800f);
        SetFloat(so, "sensors.thermalRangeMeters", 700f);
        SetString(so, "sensors.visualRangeDescription", "Optical survey range; sensor fusion with thermal, radar, LiDAR and acoustic");
        SetString(so, "sensors.thermalRangeDescription", "700 m thermal range");
        SetString(so, "sensors.acousticRange", "Medium");
        SetString(so, "sensors.radarRange", "Short");
        SetString(so, "sensors.lidarRange", "Long");
        SetString(so, "sensors.signalDetectionRange", "Medium");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 6f);
        SetString(so, "communication.meshRangeDescription", "6 km; Recon Grid range 6 km");
        SetFloat(so, "communication.relayCapacity", 1f);
        SetString(so, "communication.commandLinkType", "Recon Grid");
        SetString(so, "communication.networkPriority", "High");

        // Combat and threat model: non-combat survey unit
        SetFloat(so, "combat.firepowerRating", 0f);
        SetString(so, "combat.firepowerDescription", "None; direct combat assignment prohibited");
        SetString(so, "combat.accuracy", "N/A");
        SetString(so, "combat.stabilization", "High; automatic survey stabilization");
        SetString(so, "combat.weaponMounts", "None");
        SetString(so, "combat.weaponTypes", "None");
        SetFloat(so, "combat.suppressionValue", 0f);
        SetString(so, "combat.trackingStrength", "Medium (terrain only)");
        SetString(so, "combat.disruptionStrength", "Low");
        SetString(so, "combat.breachCapability", "None");
        SetString(so, "combat.countermeasureRating", "Low");
        SetInt(so, "combat.threatLevel", 2);
        SetString(so, "combat.engagementDistance", "N/A");
        SetString(so, "combat.preferredTerrain", "Rough ground / survey lanes");
        SetString(so, "combat.preferredTargets", "None");
        SetString(so, "combat.vulnerabilityProfile", "Ballistic / explosives");

        // Squad and command integration
        SetString(so, "squad.squadRole", "Terrain Survey / Recon Support");
        SetInt(so, "squad.squadSizeContribution", 1);
        SetString(so, "squad.formationBehavior", "Forward survey spacing");
        SetString(so, "squad.intentInterpretationRules", "Map terrain and identify hazards; survey sectors, scan resource fields, analyze routes and mark engineering sites");
        SetString(so, "squad.retreatBehavior", "Early");
        SetString(so, "squad.breachBehavior", "None");
        SetString(so, "squad.flankingBehavior", "None");
        SetString(so, "squad.reinforcementBehavior", "Supports recon and engineering squads");
        SetString(so, "squad.parentCommandUnit", "Command Core / Fusion");
        SetStrings(so, "squad.subordinateUnits", new string[0]);
        SetStrings(so, "squad.relayNodes", new[] { "Relay Node 1" });
        SetString(so, "squad.networkDependencies", "Recon Grid; redundant link to Engineering Net");

        // VR operator
        SetString(so, "vrOperator.operatorSeatType", "Survey Cockpit");
        SetString(so, "vrOperator.operatorHud", "Terrain Mapping Overlay");
        SetString(so, "vrOperator.cameraFeeds", "Optical");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.lidarView", true);
        SetBool(so, "vrOperator.sonarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetString(so, "vrOperator.movementControls", "Full vehicle drive");
        SetString(so, "vrOperator.sensorControls", "LiDAR / thermal / zoom");
        SetString(so, "vrOperator.abilityControls", "Scan Terrain / Mark Hazard / Stabilize");
        SetString(so, "vrOperator.weaponControls", "None");
        SetString(so, "vrOperator.stabilizationControls", "Automatic survey stabilization");
        SetStrings(so, "vrOperator.operatorAbilities", new[]
        {
            "Terrain Scan",
            "Hazard Detection",
            "Mapping Sweep",
            "Recon Relay"
        });

        // Economy and logistics
        SetInt(so, "economy.creditCost", 490);
        SetString(so, "economy.buildCost", "320 Metal / 170 Electronics");
        SetFloat(so, "economy.buildTimeSeconds", 75f);
        SetString(so, "economy.factoryType", "Vehicle Bay");
        SetString(so, "economy.techRequirements", "Recon Systems II");
        SetString(so, "economy.maintenanceCost", "Low; MTBF 84 h");
        SetFloat(so, "economy.repairTimeSeconds", 20f);
        SetString(so, "economy.supplyConsumption", "Low");
        SetString(so, "economy.operationalCost", "Low; 8 Metal / 4 Electronics per hour");
        SetString(so, "economy.deploymentRequirements", "Vehicle pad");
        SetString(so, "economy.transportCompatibility", "Light racks");
        SetString(so, "economy.launchMethod", "Drive-out");
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);

        // Player collection build requirements
        SetRequirements(so, "buildRequirements.commonComponents", new[]
        {
            new Requirement("Light Alloy Plates", 26),
            new Requirement("Reinforced Frame Rods", 14),
            new Requirement("Hydraulic Actuators", 4),
            new Requirement("Armor Panels", 2),
            new Requirement("Survey Sensor Housing", 1)
        });
        SetRequirements(so, "buildRequirements.electronics", new[]
        {
            new Requirement("Circuit Bundles", 16),
            new Requirement("Signal Processors", 12),
            new Requirement("Vehicle Control Chips", 3),
            new Requirement("Recon AI Chips", 2)
        });
        SetRequirements(so, "buildRequirements.power", new[]
        {
            new Requirement("Hybrid Engine Core", 1),
            new Requirement("Power Regulation Nodes", 2)
        });
        SetRequirements(so, "buildRequirements.rareTradeable", new[]
        {
            new Requirement("Long-Range LiDAR Core", 1)
        });
        SetStrings(so, "buildRequirements.optionalUpgrades", new[]
        {
            "Extended Battery Pack",
            "Silent Drive Kit",
            "Anti-Jamming Shielding"
        });

        // Extra operational/environmental analytics: set only if matching fields exist.
        SetOptionalString(so, "operationalParameters.missionType", "Terrain Survey / Mobile Mapping / Recon Support");
        SetOptionalString(so, "operationalParameters.optimalDeploymentZone", "Survey Routes / Resource Fields / Engineering Corridors");
        SetOptionalFloat(so, "operationalParameters.commandLatencySeconds", 0.4f);
        SetOptionalFloat(so, "operationalParameters.relayLoadPercent", 22f);
        SetOptionalFloat(so, "operationalParameters.signalIntegrityPercent", 95f);
        SetOptionalFloat(so, "operationalParameters.aiSyncRatePercent", 98f);
        SetOptionalFloat(so, "operationalParameters.tacticalRefreshIntervalSeconds", 4f);
        SetOptionalFloat(so, "operationalParameters.strategicRefreshIntervalSeconds", 2f);
        SetOptionalString(so, "operationalParameters.autonomousDecisionDepth", "Tier II");
        SetOptionalString(so, "operationalParameters.overridePriority", "Network > AI > Manual");
        SetOptionalString(so, "operationalParameters.commandAuthority", "Tactical");
        SetOptionalFloat(so, "operationalParameters.commandRangeKm", 6f);
        SetOptionalString(so, "operationalParameters.processingCapacity", "High");
        SetOptionalString(so, "operationalParameters.networkCapacity", "Medium");
        SetOptionalBool(so, "operationalParameters.autonomousOperation", true);

        SetOptionalFloat(so, "environmentalInteraction.openGround", 1.00f);
        SetOptionalFloat(so, "environmentalInteraction.urban", 0.92f);
        SetOptionalFloat(so, "environmentalInteraction.forest", 0.75f);
        SetOptionalFloat(so, "environmentalInteraction.snow", 0.80f);
        SetOptionalFloat(so, "environmentalInteraction.rockyTerrain", 0.88f);
        SetOptionalFloat(so, "environmentalInteraction.mud", 0.70f);
        SetOptionalFloat(so, "environmentalInteraction.sand", 0.78f);
        SetOptionalFloat(so, "environmentalInteraction.water", 0f);
        SetOptionalFloat(so, "environmentalInteraction.weatherSensitivityIndex", 0.52f);
        SetOptionalString(so, "environmentalInteraction.visibilityModifiers", "Day 1.00 / Night 0.85 / Fog 0.70 / Rain 0.80 / Dust Storm 0.65");
        SetOptionalString(so, "environmentalInteraction.signalPropagationCurve", "-0.15% signal strength per 10% humidity increase");
        SetOptionalString(so, "environmentalInteraction.elevationModifier", "+3% signal quality per 50 m elevation gain");
        SetOptionalString(so, "environmentalInteraction.environmentalEffects", "Primarily affect mobility, sensor accuracy, mapping quality and communications");

        SetOptionalString(so, "aiBehaviorMatrix.terrainScan", "Weight 0.95 — Generate terrain topology map");
        SetOptionalString(so, "aiBehaviorMatrix.hazardDetection", "Weight 0.92 — Identify environmental threats");
        SetOptionalString(so, "aiBehaviorMatrix.mappingSweep", "Weight 0.90 — Expand explored territory");
        SetOptionalString(so, "aiBehaviorMatrix.pathAnalysis", "Weight 0.88 — Locate efficient travel routes");
        SetOptionalString(so, "aiBehaviorMatrix.stabilize", "Weight 0.85 — Improve scanner accuracy");
        SetOptionalString(so, "aiBehaviorMatrix.retreat", "Weight 0.65 — Withdraw when damage exceeds threshold");
        SetOptionalString(so, "aiBehaviorMatrix.assist", "Weight 0.75 — Support engineering operations");
        SetOptionalString(so, "aiBehaviorMatrix.overrideResponse", "Weight 0.87 — Accept command updates immediately");

        SetOptionalString(so, "energyCurve.surveyOperation", "42 Wh/min");
        SetOptionalString(so, "energyCurve.maximumScannerLoad", "68 Wh/min");
        SetOptionalString(so, "energyCurve.terrainMapping", "55 Wh/min");
        SetOptionalString(so, "energyCurve.engineeringSupport", "48 Wh/min");
        SetOptionalString(so, "energyCurve.idle", "10 Wh/min; recovery 4% per idle minute");
        SetOptionalString(so, "energyCurve.emergencyPower", "14 Wh/min");
        SetOptionalString(so, "energyCurve.hybridRangeExtender", "Supports extended operation when battery reserve is depleted");

        SetOptionalString(so, "resourceAnalytics.componentWearCurve", "0.22% degradation per hour");
        SetOptionalString(so, "resourceAnalytics.maintenancePriorityLevel", "Tier II");
        SetOptionalFloat(so, "resourceAnalytics.sparePartDependencyIndex", 0.51f);
        SetOptionalString(so, "maintenanceAnalytics.meanTimeBetweenFailures", "84 h");
        SetOptionalString(so, "maintenanceAnalytics.resourceDrainPerHour", "0.80%");
        SetOptionalFloat(so, "maintenanceAnalytics.batteryEfficiencyPercent", 95f);
        SetOptionalFloat(so, "maintenanceAnalytics.suspensionStabilityPercent", 97f);
        SetOptionalFloat(so, "maintenanceAnalytics.sensorCalibrationDriftPercentPerHour", 0.25f);
        SetOptionalFloat(so, "maintenanceAnalytics.coolingEfficiencyPercent", 91f);
        SetOptionalFloat(so, "maintenanceAnalytics.operationalUptimePercent", 98f);

        SetOptionalFloat(so, "tacticalIntegration.formationAdaptability", 0.89f);
        SetOptionalFloat(so, "tacticalIntegration.tacticalRoleWeight", 0.83f);
        SetOptionalString(so, "tacticalIntegration.reinforcementTriggerConditions", "Survey Coverage < 60% OR Recon Route Incomplete");
        SetOptionalString(so, "tacticalIntegration.retreatLogicThresholds", "Structural Integrity < 35% OR Signal Loss > 40%");
        SetOptionalString(so, "tacticalIntegration.strategicPriority", "High");
        SetOptionalString(so, "tacticalIntegration.preferredAssignments", "Terrain survey / mapping / hazard detection / engineering reconnaissance / route analysis");
        SetOptionalString(so, "tacticalIntegration.directCombatAssignment", "Prohibited");
        SetOptionalBool(so, "tacticalIntegration.directUnitMicromanagementRequired", false);
        SetOptionalBool(so, "tacticalIntegration.autonomousAssignment", true);

        SetOptionalString(so, "performanceMetrics.missionSuccessProbability", "94%");
        SetOptionalString(so, "performanceMetrics.averageSurveyDuration", "18.5 min");
        SetOptionalString(so, "performanceMetrics.terrainDataYield", "24 MB/min");
        SetOptionalString(so, "performanceMetrics.communicationUptime", "97%");
        SetOptionalString(so, "performanceMetrics.threatAvoidanceEfficiency", "86%");
        SetOptionalString(so, "performanceMetrics.terrainMappingAccuracy", "93%");
        SetOptionalString(so, "performanceMetrics.hazardIdentificationAccuracy", "90%");
        SetOptionalString(so, "performanceMetrics.routeAnalysisEfficiency", "88%");
        SetOptionalString(so, "performanceMetrics.surveyCoverageEfficiency", "94%");

        SetOptionalString(so, "threatResponseModel.ballistic", "Reaction 0.4 s / countermeasure efficiency 43%");
        SetOptionalString(so, "threatResponseModel.explosive", "Reaction 0.7 s / countermeasure efficiency 36%");
        SetOptionalString(so, "threatResponseModel.emp", "Reaction 1.1 s / countermeasure efficiency 29%");
        SetOptionalString(so, "threatResponseModel.thermal", "Reaction 0.5 s / countermeasure efficiency 48%");
        SetOptionalString(so, "threatResponseModel.signalJam", "Reaction 0.8 s / countermeasure efficiency 44%");
        SetOptionalString(so, "threatResponseModel.responses", "Ballistic: relocate from exposed survey lane; Explosive: recalculate survey path; EMP: isolate affected systems and continue cached mapping; Thermal: relocate to cover; Signal Jam: cache data and restore transmission when link recovers");

        SetOptionalString(so, "commandNetworkAnalytics.reconGrid", "Load 22% / latency 0.4 s / Optimal");
        SetOptionalString(so, "commandNetworkAnalytics.wardenRelay", "Load 31% / latency 0.5 s / Active");
        SetOptionalString(so, "commandNetworkAnalytics.engineeringNet", "Load 27% / latency 0.6 s / Stable");
        SetOptionalString(so, "commandNetworkAnalytics.commandCore", "Load 35% / latency 0.4 s / Active");

        SetOptionalString(so, "missionBehavior.terrainSurveyCompletion", "94%");
        SetOptionalString(so, "missionBehavior.terrainTopologyGeneration", "93%");
        SetOptionalString(so, "missionBehavior.hazardDetection", "90%");
        SetOptionalString(so, "missionBehavior.mappingSweepCompletion", "92%");
        SetOptionalString(so, "missionBehavior.reconRelay", "91%");
        SetOptionalString(so, "missionBehavior.efficientRouteIdentification", "88%");
        SetOptionalString(so, "missionBehavior.engineeringSupportResponse", "89%");
        SetOptionalString(so, "commandInterpretation.receives", "SURVEY SECTOR; MAP TERRAIN; IDENTIFY HAZARDS; SCAN RESOURCE FIELD; ANALYZE TRAVEL ROUTE; MARK ENGINEERING SITE; RELAY RECON DATA");

        SetOptionalFloat(so, "aiCognitiveLayer.decisionConfidence", 0.93f);
        SetOptionalFloat(so, "aiCognitiveLayer.threatRecognitionAccuracy", 0.84f);
        SetOptionalFloat(so, "aiCognitiveLayer.adaptiveLearningRate", 0.05f);
        SetOptionalFloat(so, "aiCognitiveLayer.commandOverrideResistance", 0.07f);
        SetOptionalFloat(so, "aiCognitiveLayer.cooperativeBehaviorIndex", 0.90f);
        SetOptionalFloat(so, "aiCognitiveLayer.terrainForecastHorizonMinutes", 15f);
        SetOptionalBool(so, "aiCognitiveLayer.autonomousRecovery", true);

        // Apply once after all core and optional property attempts.
        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Rover] Population pass completed for " + AssetPath +
            ". Review Console warnings for unavailable or incompatible serialized fields. " +
            "Existing modelPrefab and storeImage were not changed.");
    }

    private struct Requirement
    {
        public string component;
        public int quantity;
        public Requirement(string component, int quantity)
        {
            this.component = component;
            this.quantity = quantity;
        }
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Rover] Serialized property not found: " + path);
        return p;
    }

    private static void SetString(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.String)
        {
            Debug.LogWarning("[Rover] Expected string property: " + path);
            return;
        }
        p.stringValue = value;
    }

    private static void SetOptionalString(SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p != null && p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Rover] Expected numeric property: " + path);
    }

    private static void SetOptionalFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Integer)
        {
            Debug.LogWarning("[Rover] Expected integer property: " + path);
            return;
        }
        p.intValue = value;
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Boolean)
        {
            Debug.LogWarning("[Rover] Expected boolean property: " + path);
            return;
        }
        p.boolValue = value;
    }

    private static void SetOptionalBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
    }

    private static void SetEnum(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Rover] Expected enum property: " + path);
            return;
        }

        for (int i = 0; i < p.enumNames.Length; i++)
        {
            if (string.Equals(p.enumNames[i], value, StringComparison.OrdinalIgnoreCase))
            {
                p.enumValueIndex = i;
                return;
            }
        }
        Debug.LogWarning("[Rover] Enum value '" + value + "' is not defined for " + path);
    }

    private static void SetStrings(SerializedObject so, string path, string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Rover] Expected array/list property: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType != SerializedPropertyType.String)
            {
                Debug.LogWarning("[Rover] Expected string list element: " + path);
                return;
            }
            element.stringValue = values[i];
        }
    }

    private static void SetRequirements(SerializedObject so, string path, Requirement[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Rover] Expected requirement list: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component == null || quantity == null)
            {
                Debug.LogWarning("[Rover] Requirement entry schema mismatch: " + path);
                return;
            }

            component.stringValue = values[i].component;
            quantity.intValue = values[i].quantity;
        }
    }
}

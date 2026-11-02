using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateSeaRescueCanonicalData
{
    private const string SeaFolder = "Assets/Game/Units/Database/Sea";

    [MenuItem("Obsidian Protocol/Units/Populate Sea Rescue Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = FindAsset("Rescue.asset");
        if (target == null)
        {
            Debug.LogError("[Sea Rescue] Could not find Rescue.asset under " +
                           SeaFolder + ". No asset was modified.");
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(target);
        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        SetText(so, "unitId", "obs-sea-rescue");
        SetText(so, "displayName", "Rescue");
        SetText(so, "category", "Sea");
        SetText(so, "role", "Marine Emergency Response / Extraction");
        SetText(so, "tier", "Support");
        SetText(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 7f);
        SetFloat(so, "physical.widthMeters", 3.2f);
        SetFloat(so, "physical.heightMeters", 2.5f);
        SetFloat(so, "physical.massKg", 2800f);
        SetText(so, "physical.powerSource", "Marine diesel-electric hybrid");
        SetFloat(so, "physical.energyCapacityWh", 10000f);
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 160f);
        SetText(so, "physical.endurance", "6–12 hours");
        SetFloat(so, "physical.payloadCapacityKg", 250f);
        SetStrings(so, "physical.modules",
            "Medical bay",
            "Extraction cradle",
            "Stabilization winch");
        SetText(so, "physical.navigationSystem", "Marine GPS + Rescue AI");
        SetFloat(so, "physical.structuralIntegrityHp", 950f);
        SetFloat(so, "physical.armorRating", 0.40f);
        SetText(so, "physical.heatSignature", "Low");
        SetText(so, "physical.noiseSignature", "Low");
        SetFloat(so, "physical.topSpeedMetersPerSecond", 10f);
        SetText(so, "physical.topSpeedDescription", "10 m/s");
        SetText(so, "physical.acceleration", "Medium");
        SetText(so, "physical.turningRadius", "Tight marine turn");
        SetText(so, "physical.stabilityRating", "Very High");
        SetFloat(so, "physical.energyConsumptionWhPerMinute", 55f);

        // Autonomy and AI
        SetText(so, "autonomy.behaviorProfile",
            "Emergency Response / Extraction");
        SetFloat(so, "autonomy.aggression", 0f);
        SetFloat(so, "autonomy.caution", 0.95f);
        SetFloat(so, "autonomy.persistence", 1f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.9f);
        SetFloat(so, "autonomy.riskTolerance", 0.2f);
        SetText(so, "autonomy.retreatBehaviorDescription", "Early");
        SetText(so, "autonomy.targetPriority", "Downed units / distress signals");
        SetText(so, "autonomy.formationDiscipline", "High");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 2000f);
        SetFloat(so, "sensors.thermalRangeMeters", 900f);
        SetText(so, "sensors.visualRangeDescription", "2000 m");
        SetText(so, "sensors.thermalRangeDescription", "900 m");
        SetText(so, "sensors.acousticRange", "High");
        SetText(so, "sensors.radarRange", "Short");
        SetText(so, "sensors.signalDetectionRange", "Medium");
        SetOptionalText(so, "sensors.sonarRange", "Short");
        SetOptionalText(so, "sensors.sensorFusion",
            "Optical + thermal + acoustic + sonar + radar");
        SetOptionalText(so, "sensors.distressSignalDetection", "High");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 6f);
        SetText(so, "communication.meshRangeDescription", "6 km");
        SetFloat(so, "communication.relayCapacity", 1f);
        SetText(so, "communication.commandLinkType", "Marine Support Grid");
        SetText(so, "communication.networkPriority", "High");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0f);
        SetText(so, "combat.firepowerDescription", "None");
        SetText(so, "combat.accuracy", "N/A");
        SetText(so, "combat.stabilization", "Very High");
        SetText(so, "combat.weaponMounts", "None");
        SetText(so, "combat.weaponTypes", "None");
        SetFloat(so, "combat.suppressionValue", 0f);
        SetText(so, "combat.trackingStrength", "Low");
        SetText(so, "combat.disruptionStrength", "Low");
        SetText(so, "combat.breachCapability", "None");
        SetText(so, "combat.countermeasureRating", "Medium");
        SetInt(so, "combat.threatLevel", 3);
        SetText(so, "combat.engagementDistance", "N/A");
        SetText(so, "combat.preferredTerrain",
            "Surface waters / rescue corridors");
        SetText(so, "combat.preferredTargets", "None");
        SetText(so, "combat.vulnerabilityProfile",
            "Ballistic / collision / waves");

        // Squad and command integration
        SetText(so, "squad.squadRole", "Marine Rescue / Extraction");
        SetInt(so, "squad.squadSizeContribution", 2);
        SetText(so, "squad.formationBehavior", "Rear-line support spacing");
        SetText(so, "squad.intentInterpretationRules",
            "Recover and stabilize allies");
        SetText(so, "squad.retreatBehavior", "Early");
        SetText(so, "squad.breachBehavior", "None");
        SetText(so, "squad.flankingBehavior", "None");
        SetText(so, "squad.reinforcementBehavior",
            "Supports all marine squads");
        SetText(so, "squad.parentCommandUnit", "Worldmap / Insight");
        SetStrings(so, "squad.subordinateUnits");
        SetStrings(so, "squad.relayNodes", "Relay Node 1");
        SetText(so, "squad.networkDependencies", "Marine Support Grid");

        // VR operator
        SetText(so, "vrOperator.operatorSeatType", "Marine Rescue Console");
        SetText(so, "vrOperator.operatorHud", "Medical & Extraction Overlay");
        SetText(so, "vrOperator.cameraFeeds", "Optical + winch cam");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.sonarView", true);
        SetBool(so, "vrOperator.lidarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetText(so, "vrOperator.movementControls", "Marine drive");
        SetText(so, "vrOperator.sensorControls", "Thermal / sonar / zoom");
        SetText(so, "vrOperator.abilityControls",
            "Stabilize / Extract / Mark Safe Zone");
        SetText(so, "vrOperator.weaponControls", "None");
        SetText(so, "vrOperator.stabilizationControls",
            "Automatic marine stabilization");
        SetStrings(so, "vrOperator.operatorAbilities",
            "Emergency Extraction",
            "Stabilization Pulse",
            "Rapid Recovery",
            "Support Relay",
            "Safe-Zone Marking");

        // Economy and logistics
        SetText(so, "economy.buildCost", "480 Metal / 260 Electronics");
        SetInt(so, "economy.creditCost", 740);
        SetFloat(so, "economy.buildTimeSeconds", 100f);
        SetText(so, "economy.factoryType", "Marine Bay");
        SetText(so, "economy.techRequirements", "Marine Systems I");
        SetText(so, "economy.maintenanceCost", "Low");
        SetFloat(so, "economy.repairTimeSeconds", 26f);
        SetText(so, "economy.supplyConsumption", "Low");
        SetText(so, "economy.operationalCost", "Low");
        SetText(so, "economy.deploymentRequirements", "Water launch pad");
        SetText(so, "economy.transportCompatibility", "Marine racks");
        SetText(so, "economy.launchMethod", "Water drop");
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);

        // Player collection requirements
        SetRequirements(so, "buildRequirements.commonComponents",
            ("Marine Alloy Plates", 34),
            ("Reinforced Marine Rods", 18),
            ("Hydraulic Actuators", 5),
            ("Extraction Cradle Assembly", 1));

        SetRequirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 18),
            ("Signal Processors", 12),
            ("Marine Control Chips", 3),
            ("Support AI Chips", 2));

        SetRequirements(so, "buildRequirements.power",
            ("Marine Hybrid Engine Core", 1),
            ("Power Regulation Nodes", 2));

        SetRequirements(so, "buildRequirements.rareTradeable",
            ("Medical Stabilization Module", 1));

        SetStrings(so, "buildRequirements.optionalUpgrades",
            "Reinforced Cradle",
            "Silent Propulsion",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding");

        // Optional extended operational parameters.
        // These are only written if the UnitDefinition schema contains them.
        SetOptionalText(so, "operationalParameters.missionType",
            "Marine Emergency Response / Extraction");
        SetOptionalText(so, "operationalParameters.optimalDeploymentZone",
            "Surface Waters / Rescue Corridors");
        SetOptionalFloat(so, "operationalParameters.commandLatencySeconds", 0.35f);
        SetOptionalFloat(so, "operationalParameters.relayLoadPercent", 28f);
        SetOptionalFloat(so, "operationalParameters.signalIntegrityPercent", 96f);
        SetOptionalFloat(so, "operationalParameters.aiSyncRatePercent", 99f);
        SetOptionalFloat(so, "operationalParameters.tacticalRefreshIntervalSeconds", 2f);
        SetOptionalFloat(so, "operationalParameters.strategicRefreshIntervalSeconds", 5f);
        SetOptionalText(so, "operationalParameters.autonomousDecisionDepth", "Tier III");
        SetOptionalText(so, "operationalParameters.overridePriority",
            "Marine Support Grid > AI > Manual");
        SetOptionalText(so, "operationalParameters.commandAuthority", "Tactical Support");
        SetOptionalFloat(so, "operationalParameters.commandRangeKm", 6f);
        SetOptionalText(so, "operationalParameters.processingCapacity", "High");
        SetOptionalText(so, "operationalParameters.networkCapacity", "Medium");
        SetOptionalBool(so, "operationalParameters.autonomousOperation", true);

        // Optional performance and maintenance metrics.
        SetOptionalFloat(so, "operationalParameters.missionSuccessProbability", 0.93f);
        SetOptionalFloat(so, "operationalParameters.averageRescueDurationMinutes", 11.5f);
        SetOptionalFloat(so, "operationalParameters.medicalSupportResponsePercent", 94f);
        SetOptionalFloat(so, "operationalParameters.communicationUptimePercent", 98f);
        SetOptionalFloat(so, "operationalParameters.distressDetectionEfficiencyPercent", 97f);
        SetOptionalFloat(so, "operationalParameters.threatAvoidanceEfficiencyPercent", 89f);
        SetOptionalFloat(so, "operationalParameters.navigationAccuracyPercent", 92f);
        SetOptionalFloat(so, "operationalParameters.meanTimeBetweenFailuresHours", 112f);
        SetOptionalFloat(so, "operationalParameters.batteryEfficiencyPercent", 94f);
        SetOptionalFloat(so, "operationalParameters.propulsionStabilityPercent", 96f);
        SetOptionalFloat(so, "operationalParameters.medicalSystemReliabilityPercent", 97f);
        SetOptionalFloat(so, "operationalParameters.coolingEfficiencyPercent", 92f);
        SetOptionalFloat(so, "operationalParameters.operationalUptimePercent", 97f);
        SetOptionalFloat(so, "operationalParameters.componentWearPercentPerHour", 0.28f);
        SetOptionalFloat(so, "operationalParameters.sparePartDependencyIndex", 0.58f);
        SetOptionalFloat(so, "operationalParameters.resourceDrainPercentPerHour", 0.95f);
        SetOptionalFloat(so, "operationalParameters.winchCalibrationDriftPercentPerHour", 0.20f);
        SetOptionalFloat(so, "operationalParameters.energyRecoveryPercentPerIdleMinute", 5f);
        SetOptionalText(so, "operationalParameters.maintenancePriorityLevel", "Tier II");
        SetOptionalText(so, "operationalParameters.operationalCostPerHour",
            "10 Metal / 6 Electronics");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Sea Rescue] Population pass completed for " + assetPath +
                  ". This confirms the pass ran; inspect warnings and the asset to " +
                  "verify which optional metrics are supported by the schema.");
    }

    private static UnitDefinition FindAsset(string filename)
    {
        string[] guids = AssetDatabase.FindAssets("t:UnitDefinition", new[] { SeaFolder });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.Equals(System.IO.Path.GetFileName(path),
                              filename, StringComparison.OrdinalIgnoreCase))
                return AssetDatabase.LoadAssetAtPath<UnitDefinition>(path);
        }
        return null;
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Sea Rescue] Serialized property not found: " + path);
        return p;
    }

    private static void SetText(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
        {
            p.stringValue = value;
            return;
        }

        if (p.propertyType == SerializedPropertyType.Enum)
        {
            int index = Array.IndexOf(p.enumNames, value);
            if (index >= 0) p.enumValueIndex = index;
            else Debug.LogWarning("[Sea Rescue] Enum value '" + value +
                                  "' unavailable for " + path);
            return;
        }

        Debug.LogWarning("[Sea Rescue] Expected string or enum at " + path +
                         ", found " + p.propertyType);
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Sea Rescue] Expected numeric property at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else
            Debug.LogWarning("[Sea Rescue] Expected integer at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Sea Rescue] Expected boolean at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetOptionalFloat(
        SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Sea Rescue] Optional numeric property has incompatible type: " +
                             path);
    }

    private static void SetOptionalText(
        SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else if (p.propertyType == SerializedPropertyType.Enum)
        {
            int index = Array.IndexOf(p.enumNames, value);
            if (index >= 0) p.enumValueIndex = index;
            else Debug.LogWarning("[Sea Rescue] Optional enum value '" + value +
                                  "' unavailable for " + path);
        }
        else
            Debug.LogWarning("[Sea Rescue] Optional text property has incompatible type: " +
                             path);
    }

    private static void SetOptionalBool(
        SerializedObject so, string path, bool value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Sea Rescue] Optional boolean property has incompatible type: " +
                             path);
    }

    private static void SetStrings(
        SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Sea Rescue] Expected list at " + path);
            return;
        }

        p.ClearArray();
        for (int i = 0; i < values.Length; i++)
        {
            p.InsertArrayElementAtIndex(i);
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Sea Rescue] Expected string list element at " +
                                 path + "[" + i + "]");
        }
    }

    private static void SetRequirements(
        SerializedObject so, string path,
        params (string component, int quantity)[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Sea Rescue] Expected requirement list at " + path);
            return;
        }

        p.ClearArray();
        for (int i = 0; i < values.Length; i++)
        {
            p.InsertArrayElementAtIndex(i);
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component == null ||
                component.propertyType != SerializedPropertyType.String ||
                quantity == null ||
                quantity.propertyType != SerializedPropertyType.Integer)
            {
                Debug.LogWarning("[Sea Rescue] Requirement schema mismatch at " +
                                 path + "[" + i + "]");
                continue;
            }

            component.stringValue = values[i].component;
            quantity.intValue = values[i].quantity;
        }
    }
}

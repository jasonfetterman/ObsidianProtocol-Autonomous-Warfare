using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateSeaDepthwatchCanonicalData
{
    private const string SeaFolder = "Assets/Game/Units/Database/Sea";

    [MenuItem("Obsidian Protocol/Units/Populate Sea Depthwatch Canonical Data")]
    public static void Populate()
    {
        string assetPath = null;

        foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition", new[] { SeaFolder }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.Equals(Path.GetFileName(path), "Depthwatch.asset", StringComparison.OrdinalIgnoreCase))
            {
                if (assetPath != null)
                {
                    Debug.LogError("[Depthwatch] Multiple Depthwatch.asset files found. No changes made.");
                    return;
                }
                assetPath = path;
            }
        }

        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.LogError("[Depthwatch] UnitDefinition asset Depthwatch.asset not found under " + SeaFolder);
            return;
        }

        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);
        if (target == null)
        {
            Debug.LogError("[Depthwatch] Could not load UnitDefinition: " + assetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        SetText(so, "unitId", "obs-sea-depthwatch");
        SetText(so, "displayName", "Depthwatch");
        SetText(so, "category", "Sea");
        SetText(so, "role", "Deep-Water Monitoring / Subsurface Recon");
        SetText(so, "tier", "Recon / Specialist");
        SetText(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 7f);
        SetFloat(so, "physical.widthMeters", 3f);
        SetFloat(so, "physical.heightMeters", 2.5f);
        SetFloat(so, "physical.massKg", 3000f);
        SetFloat(so, "physical.energyCapacityWh", 14000f);
        SetFloat(so, "physical.payloadCapacityKg", 200f);
        SetFloat(so, "physical.structuralIntegrityHp", 1000f);
        SetFloat(so, "physical.armorRating", 0.45f);
        SetFloat(so, "physical.topSpeedMetersPerSecond", 7f);
        SetFloat(so, "physical.energyConsumptionWhPerMinute", 70f);
        SetText(so, "physical.powerSource", "Marine diesel-electric hybrid");
        SetText(so, "physical.endurance", "8–16 hours");
        SetText(so, "physical.navigationSystem", "Marine GPS + DeepScan AI");
        SetText(so, "physical.heatSignature", "Very Low");
        SetText(so, "physical.noiseSignature", "Ultra-Low");
        SetText(so, "physical.topSpeedDescription", "7 m/s");
        SetText(so, "physical.acceleration", "Low");
        SetText(so, "physical.turningRadius", "Moderate");
        SetText(so, "physical.stabilityRating", "Maximum (deep-water optimized)");
        SetStrings(so, "physical.modules", new string[]
        {
            "Deep sonar array",
            "Subsurface mapping suite",
            "Stability fins"
        });
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 180f);

        // Autonomy and AI
        SetText(so, "autonomy.behaviorProfile", "Deep-water scanning / anomaly detection");
        SetFloat(so, "autonomy.aggression", 0f);
        SetFloat(so, "autonomy.caution", 0.95f);
        SetFloat(so, "autonomy.persistence", 1f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.9f);
        SetFloat(so, "autonomy.riskTolerance", 0.2f);
        SetText(so, "autonomy.retreatBehaviorDescription", "Early");
        SetText(so, "autonomy.targetPriority", "Subsurface anomalies / deep acoustic signatures");
        SetText(so, "autonomy.formationDiscipline", "High");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1400f);
        SetFloat(so, "sensors.thermalRangeMeters", 900f);
        SetText(so, "sensors.visualRangeDescription", "1400 m");
        SetText(so, "sensors.thermalRangeDescription", "900 m");
        SetText(so, "sensors.acousticRange", "Very High");
        SetText(so, "sensors.radarRange", "Short");
        SetText(so, "sensors.lidarRange", "Not specified");
        SetText(so, "sensors.signalDetectionRange", "Medium");
        SetOptionalText(so, "sensors.sonarRange", "Long (deep-water tuned)");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 6f);
        SetText(so, "communication.meshRangeDescription", "6 km");
        SetFloat(so, "communication.relayCapacity", 1f);
        SetText(so, "communication.commandLinkType", "Marine Recon Grid");
        SetText(so, "communication.networkPriority", "High");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0f);
        SetText(so, "combat.firepowerDescription", "None");
        SetText(so, "combat.weaponMounts", "None");
        SetText(so, "combat.weaponTypes", "None");
        SetText(so, "combat.accuracy", "Not specified");
        SetFloat(so, "combat.suppressionValue", 0f);
        SetText(so, "combat.trackingStrength", "Very High (deep sonar)");
        SetText(so, "combat.disruptionStrength", "Medium");
        SetText(so, "combat.countermeasureRating", "Medium");
        SetText(so, "combat.breachCapability", "None");
        SetInt(so, "combat.threatLevel", 4);
        SetText(so, "combat.engagementDistance", "Not applicable");
        SetText(so, "combat.preferredTerrain", "Deep water / subsurface corridors");
        SetText(so, "combat.preferredTargets", "Subsurface anomalies / deep acoustic signatures");
        SetText(so, "combat.vulnerabilityProfile", "Ballistic / collision / extreme waves");

        // Squad and command integration
        SetText(so, "squad.squadRole", "Deep-water Recon / Acoustic Mapping");
        SetInt(so, "squad.squadSizeContribution", 2);
        SetText(so, "squad.formationBehavior", "Sensor-optimized spacing");
        SetText(so, "squad.intentInterpretationRules", "Scan deep zones & identify anomalies");
        SetText(so, "squad.retreatBehavior", "Early");
        SetText(so, "squad.breachBehavior", "None");
        SetText(so, "squad.flankingBehavior", "None");
        SetText(so, "squad.reinforcementBehavior", "Supports recon & marine squads");
        SetText(so, "squad.parentCommandUnit", "Insight / Pulse");
        SetText(so, "squad.networkDependencies", "Marine Recon Grid");
        SetStrings(so, "squad.subordinateUnits", new string[0]);
        SetStrings(so, "squad.relayNodes", new string[] { "Relay Node 1" });

        // VR operator
        SetText(so, "vrOperator.operatorSeatType", "DeepScan Console");
        SetText(so, "vrOperator.operatorHud", "Deep Acoustic Overlay");
        SetText(so, "vrOperator.cameraFeeds", "Optical");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.sonarView", true);
        SetBool(so, "vrOperator.lidarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetText(so, "vrOperator.movementControls", "Marine drive");
        SetText(so, "vrOperator.sensorControls", "Deep sonar / acoustic filters / zoom");
        SetText(so, "vrOperator.abilityControls", "Deep Scan, Mark Anomaly, Stabilize");
        SetText(so, "vrOperator.weaponControls", "None");
        SetText(so, "vrOperator.stabilizationControls", "Deep-water stability control");
        SetStrings(so, "vrOperator.operatorAbilities", new string[]
        {
            "Deep Sonar Sweep",
            "Subsurface Mapping",
            "Hazard Detection",
            "Recon Relay"
        });

        // Economy and logistics
        SetInt(so, "economy.creditCost", 960);
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);
        SetFloat(so, "economy.buildTimeSeconds", 130f);
        SetFloat(so, "economy.repairTimeSeconds", 32f);
        SetText(so, "economy.buildCost", "620 Metal / 340 Electronics");
        SetText(so, "economy.factoryType", "Marine Bay");
        SetText(so, "economy.techRequirements", "Marine Systems II");
        SetText(so, "economy.maintenanceCost", "Medium");
        SetText(so, "economy.supplyConsumption", "Medium");
        SetText(so, "economy.operationalCost", "Medium");
        SetText(so, "economy.deploymentRequirements", "Water launch pad");
        SetText(so, "economy.transportCompatibility", "Marine racks");
        SetText(so, "economy.launchMethod", "Water drop");

        // Player collection / build requirements
        SetRequirements(so, "buildRequirements.commonComponents", new string[]
        {
            "Marine Alloy Plates",
            "Reinforced Marine Rods",
            "Hydraulic Actuators",
            "Deep Sonar Housing"
        }, new int[] { 40, 20, 6, 1 });

        SetRequirements(so, "buildRequirements.electronics", new string[]
        {
            "Circuit Bundles",
            "Signal Processors",
            "Marine Control Chips",
            "Recon AI Chips"
        }, new int[] { 24, 16, 4, 2 });

        SetRequirements(so, "buildRequirements.power", new string[]
        {
            "Marine Hybrid Engine Core",
            "Power Regulation Nodes"
        }, new int[] { 1, 2 });

        SetRequirements(so, "buildRequirements.rareTradeable", new string[]
        {
            "Deep Sonar Core"
        }, new int[] { 1 });

        SetStrings(so, "buildRequirements.optionalUpgrades", new string[]
        {
            "Silent Propulsion",
            "Extended Battery Pack",
            "Anti-Jamming Shielding"
        });

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Depthwatch] Canonical data populated: " + assetPath +
                  ". Existing modelPrefab and storeImage references were not changed.");
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Depthwatch] Serialized property not found; skipped: " + path);
        return p;
    }

    private static void SetText(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else if (p.propertyType == SerializedPropertyType.Enum)
        {
            int index = Array.IndexOf(p.enumNames, value);
            if (index >= 0) p.enumValueIndex = index;
            else Debug.LogWarning("[Depthwatch] Enum value '" + value + "' unavailable for " + path);
        }
        else Debug.LogWarning("[Depthwatch] Property is not text/enum; skipped: " + path);
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Depthwatch] Property is not numeric; skipped: " + path);
    }

    private static void SetOptionalFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Depthwatch] Optional property is not numeric; skipped: " + path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        else Debug.LogWarning("[Depthwatch] Property is not integer; skipped: " + path);
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean) p.boolValue = value;
        else Debug.LogWarning("[Depthwatch] Property is not boolean; skipped: " + path);
    }

    private static void SetOptionalText(SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else if (p.propertyType == SerializedPropertyType.Enum)
        {
            int index = Array.IndexOf(p.enumNames, value);
            if (index >= 0) p.enumValueIndex = index;
        }
    }

    private static void SetStrings(SerializedObject so, string path, string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Depthwatch] Property is not a list; skipped: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Depthwatch] List element is not text; skipped: " + path);
        }
    }

    private static void SetRequirements(SerializedObject so, string path, string[] names, int[] quantities)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Depthwatch] Requirements field is not a list; skipped: " + path);
            return;
        }

        p.arraySize = names.Length;
        for (int i = 0; i < names.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component != null && component.propertyType == SerializedPropertyType.String)
                component.stringValue = names[i];
            else
                Debug.LogWarning("[Depthwatch] Missing component field in " + path);

            if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                quantity.intValue = quantities[i];
            else
                Debug.LogWarning("[Depthwatch] Missing quantity field in " + path);
        }
    }
}

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateSeaSonarCanonicalData
{
    private const string SeaFolder = "Assets/Game/Units/Database/Sea";

    [MenuItem("Obsidian Protocol/Units/Populate Sea Sonar Canonical Data")]
    public static void Populate()
    {
        string assetPath = null;

        foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition", new[] { SeaFolder }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.Equals(Path.GetFileName(path), "Sonar.asset", StringComparison.OrdinalIgnoreCase))
            {
                if (assetPath != null)
                {
                    Debug.LogError("[Sea Sonar] Multiple Sonar.asset files found. No changes made.");
                    return;
                }
                assetPath = path;
            }
        }

        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.LogError("[Sea Sonar] UnitDefinition asset Sonar.asset not found under " + SeaFolder);
            return;
        }

        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);
        if (target == null)
        {
            Debug.LogError("[Sea Sonar] Could not load UnitDefinition: " + assetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        SetText(so, "unitId", "obs-sea-sonar");
        SetText(so, "displayName", "Sonar");
        SetText(so, "category", "Sea");
        SetText(so, "role", "Dedicated Marine Sonar Platform / Acoustic Mapping");
        SetText(so, "tier", "Recon / Specialist");
        SetText(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 5.5f);
        SetFloat(so, "physical.widthMeters", 2.5f);
        SetFloat(so, "physical.heightMeters", 2f);
        SetFloat(so, "physical.massKg", 2000f);
        SetFloat(so, "physical.energyCapacityWh", 11000f);
        SetFloat(so, "physical.payloadCapacityKg", 120f);
        SetFloat(so, "physical.structuralIntegrityHp", 700f);
        SetFloat(so, "physical.armorRating", 0.30f);
        SetFloat(so, "physical.topSpeedMetersPerSecond", 8f);
        SetFloat(so, "physical.energyConsumptionWhPerMinute", 60f);
        SetText(so, "physical.powerSource", "Marine diesel-electric hybrid");
        SetText(so, "physical.endurance", "6–14 hours");
        SetText(so, "physical.navigationSystem", "Marine GPS + Sonar AI");
        SetText(so, "physical.heatSignature", "Very Low");
        SetText(so, "physical.noiseSignature", "Ultra-Low (acoustic stealth)");
        SetText(so, "physical.topSpeedDescription", "8 m/s");
        SetText(so, "physical.acceleration", "Medium");
        SetText(so, "physical.turningRadius", "Tight");
        SetText(so, "physical.stabilityRating", "High");
        SetStrings(so, "physical.modules", new string[]
        {
            "High-resolution sonar array",
            "Acoustic mapping suite"
        });
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 120f);

        // Autonomy
        SetText(so, "autonomy.behaviorProfile", "Acoustic Recon / Deep Scan");
        SetFloat(so, "autonomy.aggression", 0f);
        SetFloat(so, "autonomy.caution", 0.9f);
        SetFloat(so, "autonomy.persistence", 1f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.85f);
        SetFloat(so, "autonomy.riskTolerance", 0.2f);
        SetText(so, "autonomy.retreatBehaviorDescription", "Early");
        SetText(so, "autonomy.targetPriority", "Subsurface anomalies / acoustic signatures");
        SetText(so, "autonomy.formationDiscipline", "High");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1600f);
        SetFloat(so, "sensors.thermalRangeMeters", 700f);
        SetText(so, "sensors.visualRangeDescription", "1600 m");
        SetText(so, "sensors.thermalRangeDescription", "700 m");
        SetText(so, "sensors.acousticRange", "Very High");
        SetText(so, "sensors.radarRange", "Short");
        SetText(so, "sensors.lidarRange", "Not specified");
        SetText(so, "sensors.signalDetectionRange", "Medium");
        SetOptionalText(so, "sensors.sonarRange", "Long (primary system)");

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
        SetText(so, "combat.trackingStrength", "High (acoustic)");
        SetText(so, "combat.disruptionStrength", "Medium (sonar interference)");
        SetText(so, "combat.countermeasureRating", "Medium");
        SetText(so, "combat.breachCapability", "None");
        SetInt(so, "combat.threatLevel", 3);
        SetText(so, "combat.engagementDistance", "Not applicable");
        SetText(so, "combat.preferredTerrain", "Surface + shallow subsurface");
        SetText(so, "combat.preferredTargets", "Subsurface anomalies / acoustic signatures");
        SetText(so, "combat.vulnerabilityProfile", "Ballistic / collision / heavy waves");

        // Squad and command integration
        SetText(so, "squad.squadRole", "Sonar Recon / Acoustic Mapping");
        SetInt(so, "squad.squadSizeContribution", 1);
        SetText(so, "squad.formationBehavior", "Sensor-optimized spacing");
        SetText(so, "squad.intentInterpretationRules", "Scan and identify subsurface anomalies");
        SetText(so, "squad.retreatBehavior", "Early");
        SetText(so, "squad.breachBehavior", "None");
        SetText(so, "squad.flankingBehavior", "None");
        SetText(so, "squad.reinforcementBehavior", "Supports recon & marine squads");
        SetText(so, "squad.parentCommandUnit", "Insight / Pulse");
        SetText(so, "squad.networkDependencies", "Marine Recon Grid");
        SetStrings(so, "squad.subordinateUnits", new string[0]);
        SetStrings(so, "squad.relayNodes", new string[] { "Relay Node 1" });

        // VR operator
        SetText(so, "vrOperator.operatorSeatType", "Sonar Console");
        SetText(so, "vrOperator.operatorHud", "Acoustic Mapping Overlay");
        SetText(so, "vrOperator.cameraFeeds", "Optical");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.sonarView", true);
        SetBool(so, "vrOperator.lidarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetText(so, "vrOperator.movementControls", "Marine drive");
        SetText(so, "vrOperator.sensorControls", "Long-range high-resolution sonar / acoustic filters / zoom");
        SetText(so, "vrOperator.abilityControls", "Deep Scan, Mark Anomaly, Stabilize");
        SetText(so, "vrOperator.weaponControls", "None");
        SetText(so, "vrOperator.stabilizationControls", "Marine platform stabilization");
        SetStrings(so, "vrOperator.operatorAbilities", new string[]
        {
            "Long-Range Sonar Sweep",
            "Acoustic Mapping",
            "Subsurface Hazard Detection",
            "Recon Relay"
        });

        // Economy and logistics: creditCost uses the supplied material-cost sum.
        SetInt(so, "economy.creditCost", 820);
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);
        SetFloat(so, "economy.buildTimeSeconds", 120f);
        SetFloat(so, "economy.repairTimeSeconds", 30f);
        SetText(so, "economy.buildCost", "520 Metal / 300 Electronics");
        SetText(so, "economy.factoryType", "Marine Bay");
        SetText(so, "economy.techRequirements", "Marine Systems II");
        SetText(so, "economy.maintenanceCost", "Low");
        SetText(so, "economy.supplyConsumption", "Low");
        SetText(so, "economy.operationalCost", "Low");
        SetText(so, "economy.deploymentRequirements", "Water launch pad");
        SetText(so, "economy.transportCompatibility", "Marine racks");
        SetText(so, "economy.launchMethod", "Water drop");

        // Player collection / build requirements
        SetRequirements(so, "buildRequirements.commonComponents", new string[]
        {
            "Marine Alloy Plates",
            "Reinforced Marine Rods",
            "Hydraulic Actuators",
            "Sonar Array Housing"
        }, new int[] { 36, 18, 5, 1 });

        SetRequirements(so, "buildRequirements.electronics", new string[]
        {
            "Circuit Bundles",
            "Signal Processors",
            "Marine Control Chips",
            "Recon AI Chips"
        }, new int[] { 22, 16, 4, 2 });

        SetRequirements(so, "buildRequirements.power", new string[]
        {
            "Marine Hybrid Engine Core",
            "Power Regulation Nodes"
        }, new int[] { 1, 2 });

        SetRequirements(so, "buildRequirements.rareTradeable", new string[]
        {
            "High-Resolution Sonar Core"
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

        Debug.Log("[Sea Sonar] Canonical data populated: " + assetPath +
                  ". Existing modelPrefab and storeImage references were not changed.");
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Sea Sonar] Serialized property not found; skipped: " + path);
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
            else Debug.LogWarning("[Sea Sonar] Enum value '" + value + "' unavailable for " + path);
        }
        else Debug.LogWarning("[Sea Sonar] Property is not text/enum; skipped: " + path);
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Sea Sonar] Property is not numeric; skipped: " + path);
    }

    private static void SetOptionalFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Sea Sonar] Optional property is not numeric; skipped: " + path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        else Debug.LogWarning("[Sea Sonar] Property is not integer; skipped: " + path);
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean) p.boolValue = value;
        else Debug.LogWarning("[Sea Sonar] Property is not boolean; skipped: " + path);
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
            Debug.LogWarning("[Sea Sonar] Property is not a list; skipped: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Sea Sonar] List element is not text; skipped: " + path);
        }
    }

    private static void SetRequirements(SerializedObject so, string path, string[] names, int[] quantities)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Sea Sonar] Requirements field is not a list; skipped: " + path);
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
                Debug.LogWarning("[Sea Sonar] Missing component field in " + path);

            if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                quantity.intValue = quantities[i];
            else
                Debug.LogWarning("[Sea Sonar] Missing quantity field in " + path);
        }
    }
}

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateSeaScoutCanonicalData
{
    private const string SeaFolder = "Assets/Game/Units/Database/Sea";

    [MenuItem("Obsidian Protocol/Units/Populate Sea Scout Canonical Data")]
    public static void Populate()
    {
        string assetPath = null;

        foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition", new[] { SeaFolder }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.Equals(Path.GetFileName(path), "Scout.asset", StringComparison.OrdinalIgnoreCase))
            {
                if (assetPath != null)
                {
                    Debug.LogError("[Sea Scout] Multiple Scout.asset files found in the Sea database. No changes made.");
                    return;
                }

                assetPath = path;
            }
        }

        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.LogError("[Sea Scout] UnitDefinition asset named Scout.asset not found under " + SeaFolder);
            return;
        }

        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);
        if (target == null)
        {
            Debug.LogError("[Sea Scout] Asset could not be loaded as UnitDefinition: " + assetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        SetText(so, "unitId", "obs-sea-scout");
        SetText(so, "displayName", "Scout");
        SetText(so, "category", "Sea");
        SetText(so, "role", "Marine Recon / Fast Surface Scouting");
        SetText(so, "tier", "Recon");
        SetText(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 4.5f);
        SetFloat(so, "physical.widthMeters", 2.2f);
        SetFloat(so, "physical.heightMeters", 1.8f);
        SetFloat(so, "physical.massKg", 1600f);
        SetFloat(so, "physical.energyCapacityWh", 7000f);
        SetFloat(so, "physical.payloadCapacityKg", 80f);
        SetFloat(so, "physical.structuralIntegrityHp", 600f);
        SetFloat(so, "physical.armorRating", 0.25f);
        SetFloat(so, "physical.topSpeedMetersPerSecond", 14f);
        SetFloat(so, "physical.energyConsumptionWhPerMinute", 45f);
        SetText(so, "physical.powerSource", "Electric-hybrid marine drive");
        SetText(so, "physical.endurance", "4–8 hours");
        SetText(so, "physical.navigationSystem", "Marine GPS + Recon AI");
        SetText(so, "physical.heatSignature", "Very Low");
        SetText(so, "physical.noiseSignature", "Very Low");
        SetText(so, "physical.topSpeedDescription", "14 m/s");
        SetText(so, "physical.acceleration", "High");
        SetText(so, "physical.turningRadius", "Very tight");
        SetText(so, "physical.stabilityRating", "High");
        SetStrings(so, "physical.modules", new string[]
        {
            "Recon optics",
            "Light sonar",
            "Silent propulsion"
        });
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 90f);

        // Autonomy and AI
        SetText(so, "autonomy.behaviorProfile", "Recon / Forward Scouting");
        SetFloat(so, "autonomy.aggression", 0.1f);
        SetFloat(so, "autonomy.caution", 0.9f);
        SetFloat(so, "autonomy.persistence", 0.8f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.85f);
        SetFloat(so, "autonomy.riskTolerance", 0.2f);
        SetText(so, "autonomy.retreatBehaviorDescription", "Early");
        SetText(so, "autonomy.targetPriority", "Movement / anomalies");
        SetText(so, "autonomy.formationDiscipline", "Medium");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 2400f);
        SetFloat(so, "sensors.thermalRangeMeters", 900f);
        SetText(so, "sensors.visualRangeDescription", "2400 m");
        SetText(so, "sensors.thermalRangeDescription", "900 m");
        SetText(so, "sensors.acousticRange", "High");
        SetText(so, "sensors.radarRange", "Short");
        SetText(so, "sensors.lidarRange", "Not specified");
        SetText(so, "sensors.signalDetectionRange", "Medium");
        SetOptionalText(so, "sensors.sonarRange", "Short");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 6f);
        SetText(so, "communication.meshRangeDescription", "6 km");
        SetFloat(so, "communication.relayCapacity", 0f);
        SetText(so, "communication.commandLinkType", "Marine Recon Grid");
        SetText(so, "communication.networkPriority", "High");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0.1f);
        SetText(so, "combat.firepowerDescription", "Very Low");
        SetText(so, "combat.weaponMounts", "1 (light)");
        SetText(so, "combat.weaponTypes", "Light turret / recon suppressor");
        SetText(so, "combat.accuracy", "Medium");
        SetFloat(so, "combat.suppressionValue", 0.2f);
        SetText(so, "combat.stabilization", "Not specified");
        SetText(so, "combat.trackingStrength", "Medium");
        SetText(so, "combat.disruptionStrength", "Low");
        SetText(so, "combat.breachCapability", "None");
        SetText(so, "combat.countermeasureRating", "Low");
        SetInt(so, "combat.threatLevel", 3);
        SetText(so, "combat.engagementDistance", "200–400 m");
        SetText(so, "combat.preferredTerrain", "Surface waters / recon lanes");
        SetText(so, "combat.preferredTargets", "Movement / anomalies");
        SetText(so, "combat.vulnerabilityProfile", "Ballistic / waves / collision");

        // Squad and command integration
        SetText(so, "squad.squadRole", "Marine Recon / Forward Eyes");
        SetInt(so, "squad.squadSizeContribution", 1);
        SetText(so, "squad.formationBehavior", "Forward spacing");
        SetText(so, "squad.intentInterpretationRules", "Identify threats early");
        SetText(so, "squad.retreatBehavior", "Early");
        SetText(so, "squad.breachBehavior", "None");
        SetText(so, "squad.flankingBehavior", "None");
        SetText(so, "squad.reinforcementBehavior", "Supports recon & marine squads");
        SetText(so, "squad.parentCommandUnit", "Insight / Pulse");
        SetText(so, "squad.networkDependencies", "Marine Recon Grid");
        SetStrings(so, "squad.subordinateUnits", new string[0]);
        SetStrings(so, "squad.relayNodes", new string[0]);

        // VR operator
        SetText(so, "vrOperator.operatorSeatType", "Recon Cockpit");
        SetText(so, "vrOperator.operatorHud", "Marine Recon Overlay");
        SetText(so, "vrOperator.cameraFeeds", "Optical");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.sonarView", true);
        SetBool(so, "vrOperator.lidarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetText(so, "vrOperator.movementControls", "Marine drive");
        SetText(so, "vrOperator.sensorControls", "Thermal / sonar / zoom");
        SetText(so, "vrOperator.abilityControls", "Mark Target, Scan, Stabilize");
        SetText(so, "vrOperator.weaponControls", "Light turret");
        SetText(so, "vrOperator.stabilizationControls", "Marine drive stabilization");
        SetStrings(so, "vrOperator.operatorAbilities", new string[]
        {
            "Recon Scan",
            "Silent Approach",
            "Threat Marking",
            "Recon Relay"
        });

        // Economy and logistics
        SetInt(so, "economy.creditCost", 460);
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);
        SetFloat(so, "economy.buildTimeSeconds", 70f);
        SetFloat(so, "economy.repairTimeSeconds", 18f);
        SetText(so, "economy.buildCost", "300 Metal / 160 Electronics");
        SetText(so, "economy.factoryType", "Marine Bay");
        SetText(so, "economy.techRequirements", "Marine Systems I");
        SetText(so, "economy.maintenanceCost", "Low");
        SetText(so, "economy.supplyConsumption", "Low");
        SetText(so, "economy.operationalCost", "Low");
        SetText(so, "economy.deploymentRequirements", "Water launch pad");
        SetText(so, "economy.transportCompatibility", "Light marine racks");
        SetText(so, "economy.launchMethod", "Water drop");

        // Player collection / build requirements
        SetRequirements(so, "buildRequirements.commonComponents", new string[]
        {
            "Marine Alloy Plates", "Reinforced Marine Rods",
            "Hydraulic Actuators", "Recon Sensor Housing"
        }, new int[] { 24, 12, 4, 1 });

        SetRequirements(so, "buildRequirements.electronics", new string[]
        {
            "Circuit Bundles", "Signal Processors",
            "Marine Control Chips", "Recon AI Chips"
        }, new int[] { 14, 10, 3, 2 });

        SetRequirements(so, "buildRequirements.power", new string[]
        {
            "Hybrid Marine Engine Core", "Power Regulation Nodes"
        }, new int[] { 1, 2 });

        SetRequirements(so, "buildRequirements.rareTradeable", new string[]
        {
            "Silent Propulsion Module"
        }, new int[] { 1 });

        SetStrings(so, "buildRequirements.optionalUpgrades", new string[]
        {
            "Extended Battery Pack",
            "Silent Drive Kit",
            "Anti-Jamming Shielding"
        });

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Sea Scout] Canonical data populated: " + assetPath +
                  ". Existing modelPrefab and storeImage references were not changed.");
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty property = so.FindProperty(path);
        if (property == null)
            Debug.LogWarning("[Sea Scout] Serialized property not found; skipped: " + path);
        return property;
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
            else Debug.LogWarning("[Sea Scout] Enum value '" + value + "' unavailable for " + path);
        }
        else
            Debug.LogWarning("[Sea Scout] Property is not text/enum; skipped: " + path);
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Sea Scout] Property is not numeric; skipped: " + path);
    }

    private static void SetOptionalFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Sea Scout] Optional property is not numeric; skipped: " + path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        else Debug.LogWarning("[Sea Scout] Property is not integer; skipped: " + path);
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean) p.boolValue = value;
        else Debug.LogWarning("[Sea Scout] Property is not boolean; skipped: " + path);
    }

    private static void SetOptionalText(SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String) p.stringValue = value;
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
            Debug.LogWarning("[Sea Scout] Property is not a list; skipped: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Sea Scout] List element is not text; skipped: " + path);
        }
    }

    private static void SetRequirements(
        SerializedObject so, string path, string[] names, int[] quantities)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Sea Scout] Requirements field is not a list; skipped: " + path);
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
                Debug.LogWarning("[Sea Scout] Missing component field in " + path);

            if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                quantity.intValue = quantities[i];
            else
                Debug.LogWarning("[Sea Scout] Missing quantity field in " + path);
        }
    }
}

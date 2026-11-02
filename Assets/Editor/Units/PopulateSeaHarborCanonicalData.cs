using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateSeaHarborCanonicalData
{
    private const string SeaFolder = "Assets/Game/Units/Database/Sea";

    [MenuItem("Obsidian Protocol/Units/Populate Sea Harbor Canonical Data")]
    public static void Populate()
    {
        string assetPath = null;

        foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition", new[] { SeaFolder }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.Equals(Path.GetFileName(path), "Harbor.asset", StringComparison.OrdinalIgnoreCase))
            {
                if (assetPath != null)
                {
                    Debug.LogError("[Harbor] Multiple Harbor.asset files found. No changes made.");
                    return;
                }
                assetPath = path;
            }
        }

        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.LogError("[Harbor] UnitDefinition asset Harbor.asset not found under " + SeaFolder);
            return;
        }

        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);
        if (target == null)
        {
            Debug.LogError("[Harbor] Could not load UnitDefinition: " + assetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        SetText(so, "unitId", "obs-sea-harbor");
        SetText(so, "displayName", "Harbor");
        SetText(so, "category", "Sea");
        SetText(so, "role", "Marine Logistics / Mobile Dock & Support Platform");
        SetText(so, "tier", "Heavy Support");
        SetText(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 10f);
        SetFloat(so, "physical.widthMeters", 4.5f);
        SetFloat(so, "physical.heightMeters", 3.5f);
        SetFloat(so, "physical.massKg", 6800f);
        SetFloat(so, "physical.energyCapacityWh", 22000f);
        SetFloat(so, "physical.payloadCapacityKg", 1200f);
        SetFloat(so, "physical.structuralIntegrityHp", 1600f);
        SetFloat(so, "physical.armorRating", 0.55f);
        SetFloat(so, "physical.topSpeedMetersPerSecond", 6f);
        SetFloat(so, "physical.energyConsumptionWhPerMinute", 90f);
        SetText(so, "physical.powerSource", "Heavy marine diesel-electric hybrid");
        SetText(so, "physical.endurance", "12–24 hours");
        SetText(so, "physical.navigationSystem", "Marine GPS + Logistics AI");
        SetText(so, "physical.heatSignature", "Medium");
        SetText(so, "physical.noiseSignature", "Medium");
        SetText(so, "physical.topSpeedDescription", "6 m/s");
        SetText(so, "physical.acceleration", "Low");
        SetText(so, "physical.turningRadius", "Heavy marine turn");
        SetText(so, "physical.stabilityRating", "Maximum (platform-grade)");
        SetStrings(so, "physical.modules", new string[]
        {
            "Mobile dock platform",
            "Cargo lift",
            "Stabilization pontoons",
            "Marine repair suite"
        });
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 300f);

        // Autonomy
        SetText(so, "autonomy.behaviorProfile", "Marine logistics / repair / support");
        SetFloat(so, "autonomy.aggression", 0f);
        SetFloat(so, "autonomy.caution", 0.9f);
        SetFloat(so, "autonomy.persistence", 1f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.9f);
        SetFloat(so, "autonomy.riskTolerance", 0.2f);
        SetText(so, "autonomy.retreatBehaviorDescription", "Early");
        SetText(so, "autonomy.targetPriority", "Friendly units requiring resupply or repair");
        SetText(so, "autonomy.formationDiscipline", "High");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1800f);
        SetFloat(so, "sensors.thermalRangeMeters", 700f);
        SetText(so, "sensors.visualRangeDescription", "1800 m");
        SetText(so, "sensors.thermalRangeDescription", "700 m");
        SetText(so, "sensors.acousticRange", "Medium");
        SetText(so, "sensors.radarRange", "Short");
        SetText(so, "sensors.lidarRange", "Not specified");
        SetText(so, "sensors.signalDetectionRange", "Medium");
        SetOptionalText(so, "sensors.sonarRange", "Short");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 8f);
        SetText(so, "communication.meshRangeDescription", "8 km");
        SetFloat(so, "communication.relayCapacity", 2f);
        SetText(so, "communication.commandLinkType", "Marine Logistics Grid");
        SetText(so, "communication.networkPriority", "High");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0f);
        SetText(so, "combat.firepowerDescription", "None");
        SetText(so, "combat.weaponMounts", "None");
        SetText(so, "combat.weaponTypes", "None");
        SetText(so, "combat.accuracy", "Not applicable");
        SetFloat(so, "combat.suppressionValue", 0f);
        SetText(so, "combat.trackingStrength", "Low");
        SetText(so, "combat.disruptionStrength", "Low");
        SetText(so, "combat.countermeasureRating", "Medium");
        SetText(so, "combat.breachCapability", "None");
        SetInt(so, "combat.threatLevel", 3);
        SetText(so, "combat.engagementDistance", "Not applicable");
        SetText(so, "combat.preferredTerrain", "Calm waters / marine staging zones");
        SetText(so, "combat.preferredTargets", "Friendly units requiring resupply or repair");
        SetText(so, "combat.vulnerabilityProfile", "Ballistic / collision / waves");

        // Squad and command integration
        SetText(so, "squad.squadRole", "Heavy Marine Logistics / Repair");
        SetInt(so, "squad.squadSizeContribution", 3);
        SetText(so, "squad.formationBehavior", "Rear-line support spacing");
        SetText(so, "squad.intentInterpretationRules", "Resupply, repair, stabilize");
        SetText(so, "squad.retreatBehavior", "Early");
        SetText(so, "squad.breachBehavior", "None");
        SetText(so, "squad.flankingBehavior", "None");
        SetText(so, "squad.reinforcementBehavior", "Supports all marine squads");
        SetText(so, "squad.parentCommandUnit", "Worldmap / Fusion");
        SetText(so, "squad.networkDependencies", "Marine Logistics Grid");
        SetStrings(so, "squad.subordinateUnits", new string[] { "Current", "Rescue" });
        SetStrings(so, "squad.relayNodes", new string[] { "Relay Node 1", "Relay Node 2" });

        // VR operator: cargo and repair controls are represented in the available text fields.
        SetText(so, "vrOperator.operatorSeatType", "Marine Logistics Console");
        SetText(so, "vrOperator.operatorHud", "Dock & Repair Overlay");
        SetText(so, "vrOperator.cameraFeeds", "Optical");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.sonarView", true);
        SetBool(so, "vrOperator.lidarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetText(so, "vrOperator.movementControls", "Heavy marine drive");
        SetText(so, "vrOperator.sensorControls", "Thermal / sonar / zoom");
        SetText(so, "vrOperator.abilityControls", "Mark Dock Zone, Stabilize, Route Lock; Cargo: Load / unload / lift; Repair: Marine repair suite");
        SetText(so, "vrOperator.weaponControls", "None");
        SetText(so, "vrOperator.stabilizationControls", "Platform stabilization / stabilization pontoons");
        SetStrings(so, "vrOperator.operatorAbilities", new string[]
        {
            "Marine Repair",
            "Cargo Transfer",
            "Stabilization Pulse",
            "Logistics Relay"
        });

        // Economy and logistics
        SetInt(so, "economy.creditCost", 1380);
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);
        SetFloat(so, "economy.buildTimeSeconds", 160f);
        SetFloat(so, "economy.repairTimeSeconds", 40f);
        SetText(so, "economy.buildCost", "900 Metal / 480 Electronics");
        SetText(so, "economy.factoryType", "Marine Bay");
        SetText(so, "economy.techRequirements", "Marine Systems II");
        SetText(so, "economy.maintenanceCost", "Medium");
        SetText(so, "economy.supplyConsumption", "Medium");
        SetText(so, "economy.operationalCost", "Medium");
        SetText(so, "economy.deploymentRequirements", "Heavy water launch pad");
        SetText(so, "economy.transportCompatibility", "Heavy marine racks");
        SetText(so, "economy.launchMethod", "Water drop");

        // Player collection / build requirements
        SetRequirements(so, "buildRequirements.commonComponents", new string[]
        {
            "Marine Alloy Plates",
            "Reinforced Marine Rods",
            "Hydraulic Actuators",
            "Armor Panels",
            "Dock Platform Assembly"
        }, new int[] { 60, 30, 8, 2, 1 });

        SetRequirements(so, "buildRequirements.electronics", new string[]
        {
            "Circuit Bundles",
            "Signal Processors",
            "Marine Control Chips",
            "Logistics AI Chips"
        }, new int[] { 28, 18, 5, 3 });

        SetRequirements(so, "buildRequirements.power", new string[]
        {
            "Heavy Marine Hybrid Engine Core",
            "Power Regulation Nodes"
        }, new int[] { 1, 3 });

        SetRequirements(so, "buildRequirements.rareTradeable", new string[]
        {
            "Marine Repair Module"
        }, new int[] { 1 });

        SetStrings(so, "buildRequirements.optionalUpgrades", new string[]
        {
            "Reinforced Hull",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding"
        });

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Harbor] Canonical data populated: " + assetPath +
                  ". Existing modelPrefab and storeImage references were not changed.");
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Harbor] Serialized property not found; skipped: " + path);
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
            else Debug.LogWarning("[Harbor] Enum value '" + value + "' unavailable for " + path);
        }
        else Debug.LogWarning("[Harbor] Property is not text/enum; skipped: " + path);
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Harbor] Property is not numeric; skipped: " + path);
    }

    private static void SetOptionalFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Harbor] Optional property is not numeric; skipped: " + path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        else Debug.LogWarning("[Harbor] Property is not integer; skipped: " + path);
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean) p.boolValue = value;
        else Debug.LogWarning("[Harbor] Property is not boolean; skipped: " + path);
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
            Debug.LogWarning("[Harbor] Property is not a list; skipped: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Harbor] List element is not text; skipped: " + path);
        }
    }

    private static void SetRequirements(SerializedObject so, string path, string[] names, int[] quantities)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Harbor] Requirements field is not a list; skipped: " + path);
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
                Debug.LogWarning("[Harbor] Missing component field in " + path);

            if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                quantity.intValue = quantities[i];
            else
                Debug.LogWarning("[Harbor] Missing quantity field in " + path);
        }
    }
}

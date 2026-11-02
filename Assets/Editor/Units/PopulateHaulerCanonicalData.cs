using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateHaulerCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Ground/Hauler/Hauler.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Hauler Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError("[Hauler] UnitDefinition not found at: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        SetText(so, "unitId", "obs-ground-hauler");
        SetText(so, "displayName", "Hauler");
        SetText(so, "category", "Ground");
        SetText(so, "role", "Heavy Logistics / Bulk Transport");
        SetText(so, "tier", "Support");
        SetText(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 5.4f);
        SetFloat(so, "physical.widthMeters", 2.6f);
        SetFloat(so, "physical.heightMeters", 2.2f);
        SetFloat(so, "physical.massKg", 2000f);
        SetText(so, "physical.powerSource", "Diesel-electric hybrid");
        SetFloat(so, "physical.energyCapacityWh", 7200f);
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 120f);
        SetText(so, "physical.endurance", "8–14 hours");
        SetFloat(so, "physical.payloadCapacityKg", 1500f);
        SetStrings(so, "physical.modules",
            "Heavy cargo bed", "Reinforced suspension");
        SetText(so, "physical.navigationSystem", "GPS + Logistics AI");
        SetFloat(so, "physical.structuralIntegrityHp", 1100f);
        SetFloat(so, "physical.armorRating", 0.50f);
        SetText(so, "physical.heatSignature", "Medium");
        SetText(so, "physical.noiseSignature", "Medium");
        SetFloat(so, "physical.topSpeedMetersPerSecond", 14f);
        SetText(so, "physical.topSpeedDescription", "14 m/s");
        SetText(so, "physical.acceleration", "Low");
        SetText(so, "physical.turningRadius", "Moderate");
        SetText(so, "physical.stabilityRating", "Very High");

        // Autonomy and AI behavior
        SetText(so, "autonomy.behaviorProfile",
            "Bulk Transport / Supply Chain");
        SetFloat(so, "autonomy.aggression", 0f);
        SetFloat(so, "autonomy.caution", 0.9f);
        SetFloat(so, "autonomy.persistence", 1f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.85f);
        SetFloat(so, "autonomy.riskTolerance", 0.2f);
        SetText(so, "autonomy.retreatBehaviorDescription", "Early");
        SetText(so, "autonomy.targetPriority",
            "Supply routes / logistics nodes");
        SetText(so, "autonomy.formationDiscipline", "High");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1400f);
        SetFloat(so, "sensors.thermalRangeMeters", 600f);
        SetText(so, "sensors.visualRangeDescription", "1400 m");
        SetText(so, "sensors.thermalRangeDescription", "600 m");
        SetText(so, "sensors.acousticRange", "Medium");
        SetText(so, "sensors.radarRange", "None");
        SetText(so, "sensors.lidarRange", "Short");
        SetText(so, "sensors.signalDetectionRange", "Short");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 6f);
        SetText(so, "communication.meshRangeDescription", "6 km");
        SetFloat(so, "communication.relayCapacity", 1f);
        SetText(so, "communication.commandLinkType", "Logistics Grid");
        SetText(so, "communication.networkPriority", "High");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0f);
        SetText(so, "combat.firepowerDescription", "None");
        SetText(so, "combat.weaponMounts", "None");
        SetText(so, "combat.weaponTypes", "None");
        SetFloat(so, "combat.suppressionValue", 0f);
        SetText(so, "combat.accuracy", "N/A");
        SetText(so, "combat.stabilization", "N/A");
        SetText(so, "combat.trackingStrength", "None");
        SetText(so, "combat.disruptionStrength", "Low");
        SetText(so, "combat.breachCapability", "None");
        SetText(so, "combat.countermeasureRating", "Low");
        SetInt(so, "combat.threatLevel", 2);
        SetText(so, "combat.engagementDistance", "N/A");
        SetText(so, "combat.preferredTerrain",
            "Supply lanes / safe corridors");
        SetText(so, "combat.preferredTargets", "None");
        SetText(so, "combat.vulnerabilityProfile",
            "Ballistic / explosives");

        // Squad and command integration
        SetText(so, "squad.squadRole", "Heavy Logistics");
        SetInt(so, "squad.squadSizeContribution", 3);
        SetText(so, "squad.formationBehavior",
            "Rear-line supply spacing");
        SetText(so, "squad.intentInterpretationRules",
            "Deliver bulk supplies safely");
        SetText(so, "squad.retreatBehavior", "Early");
        SetText(so, "squad.breachBehavior", "None");
        SetText(so, "squad.flankingBehavior", "None");
        SetText(so, "squad.reinforcementBehavior",
            "Supports all squads");
        SetText(so, "squad.parentCommandUnit", "Command Core / Fusion");
        SetStrings(so, "squad.subordinateUnits", "Mule", "Forge");
        SetStrings(so, "squad.relayNodes", "Relay Node 1");
        SetText(so, "squad.networkDependencies", "Logistics Grid");

        // VR operator
        SetText(so, "vrOperator.operatorSeatType",
            "Heavy Transport Cockpit");
        SetText(so, "vrOperator.operatorHud", "Logistics Overlay");
        SetText(so, "vrOperator.cameraFeeds", "Optical");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.lidarView", true);
        SetBool(so, "vrOperator.sonarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetText(so, "vrOperator.movementControls", "Heavy vehicle drive");
        SetText(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        SetText(so, "vrOperator.abilityControls",
            "Mark Drop Zone / Stabilize / Route Lock");
        SetText(so, "vrOperator.weaponControls",
            "Cargo controls: Load / unload / secure");
        SetText(so, "vrOperator.stabilizationControls",
            "Cargo stabilization");
        SetStrings(so, "vrOperator.operatorAbilities",
            "Bulk Supply Drop",
            "Cargo Stabilization",
            "Route Optimization",
            "Logistics Relay");

        // Economy and logistics
        SetText(so, "economy.buildCost", "650 Metal / 300 Electronics");
        SetInt(so, "economy.creditCost", 950);
        SetFloat(so, "economy.buildTimeSeconds", 130f);
        SetText(so, "economy.factoryType", "Vehicle Bay");
        SetText(so, "economy.techRequirements", "Logistics Systems II");
        SetText(so, "economy.maintenanceCost", "Medium");
        SetFloat(so, "economy.repairTimeSeconds", 30f);
        SetText(so, "economy.supplyConsumption", "Medium");
        SetText(so, "economy.operationalCost", "Medium");
        SetText(so, "economy.deploymentRequirements", "Heavy vehicle pad");
        SetText(so, "economy.transportCompatibility", "Heavy racks");
        SetText(so, "economy.launchMethod", "Drive-out");
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);

        // Player collection build requirements
        SetRequirements(so, "buildRequirements.commonComponents",
            ("Heavy Alloy Plates", 48),
            ("Reinforced Frame Rods", 24),
            ("Hydraulic Actuators", 8),
            ("Armor Panels", 3),
            ("Heavy Cargo Bed Assembly", 1));

        SetRequirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 20),
            ("Signal Processors", 14),
            ("Vehicle Control Chips", 4),
            ("Logistics AI Chips", 2));

        SetRequirements(so, "buildRequirements.power",
            ("Hybrid Engine Core", 1),
            ("Power Regulation Nodes", 3));

        SetRequirements(so, "buildRequirements.rareTradeable",
            ("Cargo Stabilization Module", 1));

        SetStrings(so, "buildRequirements.optionalUpgrades",
            "Reinforced Cargo Bed",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Hauler] Population pass completed for " + AssetPath +
                  ". Review any serialized-property warnings above.");
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Hauler] Serialized property not found: " + path);
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
            if (index >= 0)
                p.enumValueIndex = index;
            else
                Debug.LogWarning("[Hauler] Enum value '" + value +
                                 "' not available for " + path);
            return;
        }

        Debug.LogWarning("[Hauler] Expected string or enum at " + path +
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
            Debug.LogWarning("[Hauler] Expected numeric property at " + path +
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
            Debug.LogWarning("[Hauler] Optional property has incompatible type: " +
                             path + " (" + p.propertyType + ")");
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else
            Debug.LogWarning("[Hauler] Expected integer at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Hauler] Expected boolean at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetStrings(
        SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Hauler] Expected a list at " + path);
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
                Debug.LogWarning("[Hauler] Expected string list element at " +
                                 path + "[" + i + "]");
        }
    }

    private static void SetRequirements(
        SerializedObject so,
        string path,
        params (string component, int quantity)[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Hauler] Expected a requirement list at " + path);
            return;
        }

        p.ClearArray();
        for (int i = 0; i < values.Length; i++)
        {
            p.InsertArrayElementAtIndex(i);
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component == null || component.propertyType != SerializedPropertyType.String ||
                quantity == null || quantity.propertyType != SerializedPropertyType.Integer)
            {
                Debug.LogWarning("[Hauler] Requirement element schema mismatch at " +
                                 path + "[" + i + "]");
                continue;
            }

            component.stringValue = values[i].component;
            quantity.intValue = values[i].quantity;
        }
    }
}

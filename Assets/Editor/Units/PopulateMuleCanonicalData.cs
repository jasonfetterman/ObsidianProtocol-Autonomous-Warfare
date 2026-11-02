using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateMuleCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Ground/Mule/Mule.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Mule Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Mule] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        SetString(so, "unitId", "obs-ground-mule");
        SetString(so, "displayName", "Mule");
        SetEnum(so, "category", "Ground");
        SetString(so, "role", "Logistics / Field Transport");
        SetString(so, "tier", "Support");
        SetString(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 4.4f);
        SetFloat(so, "physical.widthMeters", 2.2f);
        SetFloat(so, "physical.heightMeters", 1.9f);
        SetFloat(so, "physical.massKg", 1300f);
        SetString(so, "physical.powerSource", "Diesel-electric hybrid");
        SetFloat(so, "physical.energyCapacityWh", 5000f);
        SetFloatOptional(so, "physical.fuelCapacityLiters", 70f);
        SetString(so, "physical.endurance", "6-12 hours");
        SetFloat(so, "physical.payloadCapacityKg", 800f);
        SetStrings(so, "physical.modules",
            "Cargo bed",
            "Modular rack system");
        SetString(so, "physical.navigationSystem", "GPS + Logistics AI");
        SetFloat(so, "physical.structuralIntegrityHp", 850f);
        SetFloat(so, "physical.armorRating", 0.40f);
        SetString(so, "physical.heatSignature", "Medium");
        SetString(so, "physical.noiseSignature", "Medium");
        SetFloat(so, "physical.topSpeedMetersPerSecond", 16f);
        SetString(so, "physical.topSpeedDescription", "16 m/s");
        SetString(so, "physical.acceleration", "Medium");
        SetString(so, "physical.turningRadius", "Tight");
        SetString(so, "physical.stabilityRating", "High");

        // Autonomy and AI
        SetString(so, "autonomy.behaviorProfile", "Transport / Supply Delivery");
        SetFloat(so, "autonomy.aggression", 0.0f);
        SetFloat(so, "autonomy.caution", 0.9f);
        SetFloat(so, "autonomy.persistence", 1.0f);
        SetFloat(so, "autonomy.flankingLikelihood", 0.0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.8f);
        SetFloat(so, "autonomy.riskTolerance", 0.2f);
        SetString(so, "autonomy.retreatBehaviorDescription", "Early");
        SetString(so, "autonomy.targetPriority", "Supply routes / friendly units");
        SetString(so, "autonomy.formationDiscipline", "High");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1400f);
        SetFloat(so, "sensors.thermalRangeMeters", 600f);
        SetString(so, "sensors.visualRangeDescription", "1400 m");
        SetString(so, "sensors.thermalRangeDescription", "600 m");
        SetString(so, "sensors.acousticRange", "Medium");
        SetString(so, "sensors.radarRange", "None");
        SetString(so, "sensors.lidarRange", "Short");
        SetString(so, "sensors.signalDetectionRange", "Short");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 5f);
        SetString(so, "communication.meshRangeDescription", "5 km");
        SetFloat(so, "communication.relayCapacity", 1f);
        SetString(so, "communication.commandLinkType", "Logistics Grid");
        SetString(so, "communication.networkPriority", "High");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0f);
        SetString(so, "combat.firepowerDescription", "None");
        SetString(so, "combat.accuracy", "N/A");
        SetString(so, "combat.stabilization", "Cargo stabilization");
        SetString(so, "combat.weaponMounts", "None");
        SetString(so, "combat.weaponTypes", "None");
        SetFloat(so, "combat.suppressionValue", 0f);
        SetString(so, "combat.trackingStrength", "None");
        SetString(so, "combat.disruptionStrength", "Low");
        SetString(so, "combat.breachCapability", "None");
        SetString(so, "combat.countermeasureRating", "Low");
        SetInt(so, "combat.threatLevel", 2);
        SetString(so, "combat.engagementDistance", "N/A");
        SetString(so, "combat.preferredTerrain", "Supply routes / safe lanes");
        SetString(so, "combat.preferredTargets", "None");
        SetString(so, "combat.vulnerabilityProfile", "Ballistic / explosives");

        // Squad and command integration
        SetString(so, "squad.squadRole", "Logistics / Transport");
        SetInt(so, "squad.squadSizeContribution", 2);
        SetString(so, "squad.formationBehavior", "Rear-line spacing");
        SetString(so, "squad.intentInterpretationRules", "Deliver supplies safely");
        SetString(so, "squad.retreatBehavior", "Early");
        SetString(so, "squad.breachBehavior", "None");
        SetString(so, "squad.flankingBehavior", "None");
        SetString(so, "squad.reinforcementBehavior", "Supports all squads");
        SetString(so, "squad.parentCommandUnit", "Command Core / Fusion");
        SetStrings(so, "squad.subordinateUnits", "Hauler", "Forge");
        SetStrings(so, "squad.relayNodes", "Relay Node 1");
        SetString(so, "squad.networkDependencies", "Logistics Grid");

        // VR operator
        SetString(so, "vrOperator.operatorSeatType", "Transport Cockpit");
        SetString(so, "vrOperator.operatorHud", "Logistics Overlay");
        SetString(so, "vrOperator.cameraFeeds", "Optical");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.lidarView", true);
        SetBool(so, "vrOperator.sonarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetString(so, "vrOperator.movementControls", "Full vehicle drive");
        SetString(so, "vrOperator.weaponControls",
            "Cargo controls: Load / unload / secure");
        SetString(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        SetString(so, "vrOperator.stabilizationControls", "Cargo stabilization");
        SetString(so, "vrOperator.abilityControls",
            "Mark Drop Zone, Stabilize, Route Lock");
        SetStrings(so, "vrOperator.operatorAbilities",
            "Supply Drop",
            "Cargo Stabilization",
            "Route Optimization",
            "Logistics Relay");

        // Economy and logistics
        SetInt(so, "economy.creditCost", 750);
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);
        SetString(so, "economy.buildCost", "500 Metal / 250 Electronics");
        SetFloat(so, "economy.buildTimeSeconds", 100f);
        SetString(so, "economy.factoryType", "Vehicle Bay");
        SetString(so, "economy.techRequirements", "Logistics Systems I");
        SetString(so, "economy.maintenanceCost", "Low");
        SetFloat(so, "economy.repairTimeSeconds", 25f);
        SetString(so, "economy.supplyConsumption", "Low");
        SetString(so, "economy.operationalCost", "Low");
        SetString(so, "economy.deploymentRequirements", "Vehicle pad");
        SetString(so, "economy.transportCompatibility", "Heavy racks");
        SetString(so, "economy.launchMethod", "Drive-out");

        // Player collection / build requirements
        SetRequirements(so, "buildRequirements.commonComponents",
            ("Heavy Alloy Plates", 36),
            ("Reinforced Frame Rods", 18),
            ("Hydraulic Actuators", 6),
            ("Armor Panels", 3),
            ("Cargo Bed Assembly", 1));

        SetRequirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 18),
            ("Signal Processors", 12),
            ("Vehicle Control Chips", 4),
            ("Logistics AI Chips", 2));

        SetRequirements(so, "buildRequirements.power",
            ("Hybrid Engine Core", 1),
            ("Power Regulation Nodes", 2));

        SetRequirements(so, "buildRequirements.rareTradeable",
            ("Cargo Stabilization Module", 1));

        SetStrings(so, "buildRequirements.optionalUpgrades",
            "Reinforced Cargo Bed",
            "Silent Drive Kit",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[Mule] Population routine finished. Check Console warnings and verify the asset in the Inspector.",
            target);
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Mule] Serialized property not found: " + path);
        return p;
    }

    private static void SetString(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.String)
        {
            Debug.LogWarning("[Mule] Expected String property: " + path);
            return;
        }
        p.stringValue = value;
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
            Debug.LogWarning("[Mule] Expected numeric property: " + path);
    }

    private static void SetFloatOptional(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
        {
            Debug.LogWarning("[Mule] Optional fuel-capacity field is unavailable in this schema: " + path);
            return;
        }

        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Mule] Optional fuel-capacity field is not numeric: " + path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Integer)
        {
            Debug.LogWarning("[Mule] Expected Integer property: " + path);
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
            Debug.LogWarning("[Mule] Expected Boolean property: " + path);
            return;
        }
        p.boolValue = value;
    }

    private static void SetEnum(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Enum)
        {
            int index = Array.IndexOf(p.enumNames, value);
            if (index < 0)
            {
                Debug.LogWarning("[Mule] Enum value '" + value + "' is not defined for " + path);
                return;
            }
            p.enumValueIndex = index;
        }
        else if (p.propertyType == SerializedPropertyType.String)
        {
            p.stringValue = value;
        }
        else
        {
            Debug.LogWarning("[Mule] Expected Enum or String property: " + path);
        }
    }

    private static void SetStrings(SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Mule] Expected list/array property: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Mule] Expected string list element: " + path);
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
            Debug.LogWarning("[Mule] Expected requirement list/array property: " + path);
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
                Debug.LogWarning("[Mule] Requirement element schema mismatch: " + path);
                continue;
            }

            component.stringValue = values[i].component;
            quantity.intValue = values[i].quantity;
        }
    }
}

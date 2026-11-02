using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateHammerCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Ground/Hammer/Hammer.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Hammer Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError("[Hammer] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        SetString(so, "unitId", "obs-ground-hammer");
        SetString(so, "displayName", "Hammer");
        SetEnum(so, "category", "Ground");
        SetString(so, "role", "Heavy Breach / Structural Breaker");
        SetString(so, "tier", "Assault Engineering");
        SetString(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 4.9f);
        SetFloat(so, "physical.widthMeters", 2.4f);
        SetFloat(so, "physical.heightMeters", 2.1f);
        SetFloat(so, "physical.massKg", 1800f);
        SetString(so, "physical.powerSource", "Industrial diesel-electric hybrid");
        SetFloat(so, "physical.energyCapacityWh", 7200f);
        SetFloatOptional(so, "physical.fuelCapacityLiters", 90f);
        SetString(so, "physical.endurance", "4-7 hours");
        SetFloat(so, "physical.payloadCapacityKg", 400f);
        SetStrings(so, "physical.modules",
            "Hydraulic breach ram",
            "Reinforced chassis");
        SetString(so, "physical.navigationSystem", "GPS + Breach AI");
        SetFloat(so, "physical.structuralIntegrityHp", 1300f);
        SetFloat(so, "physical.armorRating", 0.75f);
        SetString(so, "physical.heatSignature", "High");
        SetString(so, "physical.noiseSignature", "High");
        SetFloat(so, "physical.topSpeedMetersPerSecond", 10f);
        SetString(so, "physical.topSpeedDescription", "10 m/s");
        SetString(so, "physical.acceleration", "Low");
        SetString(so, "physical.turningRadius", "Tight");
        SetString(so, "physical.stabilityRating", "Maximum");

        // Autonomy and AI
        SetString(so, "autonomy.behaviorProfile", "Breach / Structural Assault");
        SetFloat(so, "autonomy.aggression", 0.6f);
        SetFloat(so, "autonomy.caution", 0.5f);
        SetFloat(so, "autonomy.persistence", 1.0f);
        SetFloat(so, "autonomy.flankingLikelihood", 0.0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.4f);
        SetFloat(so, "autonomy.riskTolerance", 0.8f);
        SetString(so, "autonomy.retreatBehaviorDescription", "Late");
        SetString(so, "autonomy.targetPriority",
            "Barriers / fortifications / hardened structures");
        SetString(so, "autonomy.formationDiscipline", "High");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1200f);
        SetFloat(so, "sensors.thermalRangeMeters", 500f);
        SetString(so, "sensors.visualRangeDescription", "1200 m");
        SetString(so, "sensors.thermalRangeDescription", "500 m");
        SetString(so, "sensors.acousticRange", "Medium");
        SetString(so, "sensors.radarRange", "None");
        SetString(so, "sensors.lidarRange", "Medium");
        SetString(so, "sensors.signalDetectionRange", "Short");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 4f);
        SetString(so, "communication.meshRangeDescription", "4 km");
        SetFloat(so, "communication.relayCapacity", 0f);
        SetString(so, "communication.commandLinkType", "Engineering Grid");
        SetString(so, "communication.networkPriority", "Medium");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0.25f);
        SetString(so, "combat.firepowerDescription", "Low (ram only)");
        SetString(so, "combat.accuracy", "N/A");
        SetString(so, "combat.stabilization", "Reinforced breach stance");
        SetString(so, "combat.weaponMounts", "None");
        SetString(so, "combat.weaponTypes", "Hydraulic breach ram");
        SetFloat(so, "combat.suppressionValue", 0.25f);
        SetString(so, "combat.trackingStrength", "None");
        SetString(so, "combat.disruptionStrength", "High");
        SetString(so, "combat.breachCapability", "Very High");
        SetString(so, "combat.countermeasureRating", "Medium");
        SetInt(so, "combat.threatLevel", 6);
        SetString(so, "combat.engagementDistance", "Contact");
        SetString(so, "combat.preferredTerrain", "Fortified zones / breach points");
        SetString(so, "combat.preferredTargets", "Walls, gates, bunkers");
        SetString(so, "combat.vulnerabilityProfile", "Ballistic / explosives");

        // Squad and command integration
        SetString(so, "squad.squadRole", "Breach / Assault Engineering");
        SetInt(so, "squad.squadSizeContribution", 3);
        SetString(so, "squad.formationBehavior", "Front-line breach spacing");
        SetString(so, "squad.intentInterpretationRules", "Break fortifications");
        SetString(so, "squad.retreatBehavior", "Late");
        SetString(so, "squad.breachBehavior", "Primary");
        SetString(so, "squad.flankingBehavior", "None");
        SetString(so, "squad.reinforcementBehavior", "Supports assault squads");
        SetString(so, "squad.parentCommandUnit", "Command Core / Fusion");
        SetStrings(so, "squad.subordinateUnits", "Mule", "Forge");
        SetStrings(so, "squad.relayNodes");
        SetString(so, "squad.networkDependencies", "Engineering Grid");

        // VR operator mode
        SetString(so, "vrOperator.operatorSeatType", "Heavy Engineering Cockpit");
        SetString(so, "vrOperator.operatorHud", "Breach Overlay");
        SetString(so, "vrOperator.cameraFeeds", "Optical + ram cam");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.lidarView", true);
        SetBool(so, "vrOperator.sonarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetString(so, "vrOperator.movementControls", "Heavy vehicle drive");
        SetString(so, "vrOperator.weaponControls", "Ram charge / impact / retract");
        SetString(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        SetString(so, "vrOperator.abilityControls",
            "Breach Mode, Stabilize, Structural Scan");
        SetStrings(so, "vrOperator.operatorAbilities",
            "Hydraulic Ram Charge",
            "Structural Weak-Point Scan",
            "Breach Execution",
            "Reinforced Stance");

        // Economy and logistics
        SetInt(so, "economy.creditCost", 1350);
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);
        SetString(so, "economy.buildCost", "900 Metal / 450 Electronics");
        SetFloat(so, "economy.buildTimeSeconds", 160f);
        SetString(so, "economy.factoryType", "Vehicle Bay");
        SetString(so, "economy.techRequirements", "Engineering Systems II");
        SetString(so, "economy.maintenanceCost", "High");
        SetFloat(so, "economy.repairTimeSeconds", 40f);
        SetString(so, "economy.supplyConsumption", "High");
        SetString(so, "economy.operationalCost", "High");
        SetString(so, "economy.deploymentRequirements", "Heavy vehicle pad");
        SetString(so, "economy.transportCompatibility", "Heavy racks");
        SetString(so, "economy.launchMethod", "Drive-out");

        // Player collection / build requirements
        SetRequirements(so, "buildRequirements.commonComponents",
            ("Heavy Alloy Plates", 60),
            ("Reinforced Frame Rods", 30),
            ("Hydraulic Actuators", 10),
            ("Armor Panels", 6),
            ("Breach Ram Housing", 1));

        SetRequirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 28),
            ("Signal Processors", 18),
            ("Vehicle Control Chips", 6),
            ("Engineering AI Chips", 4));

        SetRequirements(so, "buildRequirements.power",
            ("Industrial Hybrid Engine Core", 1),
            ("Power Regulation Nodes", 3));

        SetRequirements(so, "buildRequirements.rareTradeable",
            ("Hydraulic Ram Core", 1));

        SetStrings(so, "buildRequirements.optionalUpgrades",
            "Reinforced Ram",
            "Shock-Dampened Chassis",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Hammer] Population routine finished. Review any Console warnings and verify the asset in the Inspector.", target);
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Hammer] Serialized property not found: " + path);
        return p;
    }

    private static void SetString(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.String)
        {
            Debug.LogWarning("[Hammer] Expected String property: " + path);
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
            Debug.LogWarning("[Hammer] Expected numeric property: " + path);
    }

    private static void SetFloatOptional(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
        {
            Debug.LogWarning("[Hammer] Optional schema field unavailable; fuel capacity was not saved: " + path);
            return;
        }
        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Hammer] Optional fuel capacity field is not numeric: " + path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Integer)
        {
            Debug.LogWarning("[Hammer] Expected Integer property: " + path);
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
            Debug.LogWarning("[Hammer] Expected Boolean property: " + path);
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
                Debug.LogWarning("[Hammer] Enum value '" + value + "' is not defined for " + path);
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
            Debug.LogWarning("[Hammer] Expected Enum or String property: " + path);
        }
    }

    private static void SetStrings(SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Hammer] Expected list/array property: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Hammer] Expected string list element: " + path);
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
            Debug.LogWarning("[Hammer] Expected requirement list/array property: " + path);
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
                Debug.LogWarning("[Hammer] Requirement element schema mismatch: " + path);
                continue;
            }

            component.stringValue = values[i].component;
            quantity.intValue = values[i].quantity;
        }
    }
}

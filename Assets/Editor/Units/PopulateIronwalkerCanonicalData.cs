using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateIronwalkerCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Ground/Ironwalker/Ironwalker.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Ironwalker Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Ironwalker] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        SetString(so, "unitId", "obs-ground-ironwalker");
        SetString(so, "displayName", "Ironwalker");
        SetEnum(so, "category", "Ground");
        SetString(so, "role", "Heavy Walker / Mobile Fortification");
        SetString(so, "tier", "Heavy Assault");
        SetString(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 3.8f);
        SetFloat(so, "physical.widthMeters", 2.6f);
        SetFloat(so, "physical.heightMeters", 3.4f);
        SetFloat(so, "physical.massKg", 2200f);
        SetString(so, "physical.powerSource", "Dual industrial battery + hydraulic drive");
        SetFloat(so, "physical.energyCapacityWh", 8200f);
        SetString(so, "physical.endurance", "3-5 hours");
        SetFloat(so, "physical.payloadCapacityKg", 250f);
        SetStrings(so, "physical.modules",
            "Reinforced walker legs",
            "Shock-dampened chassis");
        SetString(so, "physical.navigationSystem", "GPS + Walker-Stability AI");
        SetFloat(so, "physical.structuralIntegrityHp", 1600f);
        SetFloat(so, "physical.armorRating", 0.85f);
        SetString(so, "physical.heatSignature", "Medium");
        SetString(so, "physical.noiseSignature", "High");
        SetFloat(so, "physical.topSpeedMetersPerSecond", 8f);
        SetString(so, "physical.topSpeedDescription", "8 m/s");
        SetString(so, "physical.acceleration", "Very Low");
        SetString(so, "physical.turningRadius", "Precise pivot");
        SetString(so, "physical.stabilityRating", "Maximum");

        // Autonomy and AI
        SetString(so, "autonomy.behaviorProfile", "Heavy Assault / Mobile Cover");
        SetFloat(so, "autonomy.aggression", 0.7f);
        SetFloat(so, "autonomy.caution", 0.5f);
        SetFloat(so, "autonomy.persistence", 1.0f);
        SetFloat(so, "autonomy.flankingLikelihood", 0.0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.3f);
        SetFloat(so, "autonomy.riskTolerance", 0.8f);
        SetString(so, "autonomy.retreatBehaviorDescription", "Late");
        SetString(so, "autonomy.targetPriority", "Hardened targets / fortified positions");
        SetString(so, "autonomy.formationDiscipline", "High");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1600f);
        SetFloat(so, "sensors.thermalRangeMeters", 700f);
        SetString(so, "sensors.visualRangeDescription", "1600 m");
        SetString(so, "sensors.thermalRangeDescription", "700 m");
        SetString(so, "sensors.acousticRange", "Medium");
        SetString(so, "sensors.radarRange", "Short");
        SetString(so, "sensors.lidarRange", "Medium");
        SetString(so, "sensors.signalDetectionRange", "Short");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 4f);
        SetString(so, "communication.meshRangeDescription", "4 km");
        SetFloat(so, "communication.relayCapacity", 1f);
        SetString(so, "communication.commandLinkType", "Assault Grid");
        SetString(so, "communication.networkPriority", "High");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0.5f);
        SetString(so, "combat.firepowerDescription", "Medium");
        SetString(so, "combat.accuracy", "Medium");
        SetString(so, "combat.stabilization", "Hydraulic walker stabilization");
        SetString(so, "combat.weaponMounts", "2");
        SetString(so, "combat.weaponTypes", "Heavy machine gun / suppression cannon");
        SetFloat(so, "combat.suppressionValue", 0.85f);
        SetString(so, "combat.trackingStrength", "Low");
        SetString(so, "combat.disruptionStrength", "Medium");
        SetString(so, "combat.breachCapability", "Medium");
        SetString(so, "combat.countermeasureRating", "High");
        SetInt(so, "combat.threatLevel", 7);
        SetString(so, "combat.engagementDistance", "200-600 m");
        SetString(so, "combat.preferredTerrain", "Urban / fortified zones");
        SetString(so, "combat.preferredTargets", "Heavy infantry / light vehicles");
        SetString(so, "combat.vulnerabilityProfile", "EMP / concentrated fire");

        // Squad and command integration
        SetString(so, "squad.squadRole", "Heavy Assault / Mobile Cover");
        SetInt(so, "squad.squadSizeContribution", 4);
        SetString(so, "squad.formationBehavior", "Front-line anchor");
        SetString(so, "squad.intentInterpretationRules", "Push forward and absorb fire");
        SetString(so, "squad.retreatBehavior", "Late");
        SetString(so, "squad.breachBehavior", "Secondary");
        SetString(so, "squad.flankingBehavior", "None");
        SetString(so, "squad.reinforcementBehavior", "Supports assault squads");
        SetString(so, "squad.parentCommandUnit", "Command Core / Fusion");
        SetStrings(so, "squad.subordinateUnits", "Hammer", "Crusher");
        SetStrings(so, "squad.relayNodes", "Relay Node 1");
        SetString(so, "squad.networkDependencies", "Assault Grid");

        // VR operator
        SetString(so, "vrOperator.operatorSeatType", "Walker Cockpit");
        SetString(so, "vrOperator.operatorHud", "Assault Overlay");
        SetString(so, "vrOperator.cameraFeeds", "Optical + weapon cams");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.lidarView", true);
        SetBool(so, "vrOperator.sonarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetString(so, "vrOperator.movementControls", "Walker stride + pivot");
        SetString(so, "vrOperator.weaponControls", "Dual mounts");
        SetString(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        SetString(so, "vrOperator.stabilizationControls", "Brace / walker stabilization");
        SetString(so, "vrOperator.abilityControls", "Brace, Suppress, Stabilize");
        SetStrings(so, "vrOperator.operatorAbilities",
            "Heavy Suppression",
            "Brace Mode",
            "Mobile Cover Deployment",
            "Threat Marking");

        // Economy and logistics
        SetInt(so, "economy.creditCost", 1800);
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);
        SetString(so, "economy.buildCost", "1,200 Metal / 600 Electronics");
        SetFloat(so, "economy.buildTimeSeconds", 180f);
        SetString(so, "economy.factoryType", "Heavy Vehicle Bay");
        SetString(so, "economy.techRequirements", "Assault Systems I");
        SetString(so, "economy.maintenanceCost", "High");
        SetFloat(so, "economy.repairTimeSeconds", 45f);
        SetString(so, "economy.supplyConsumption", "High");
        SetString(so, "economy.operationalCost", "High; 12 Metal / 6 Electronics per hour");
        SetString(so, "economy.deploymentRequirements", "Heavy walker pad");
        SetString(so, "economy.transportCompatibility", "Walker racks");
        SetString(so, "economy.launchMethod", "Walk-out");

        // Build requirements
        SetRequirements(so, "buildRequirements.commonComponents",
            ("Heavy Alloy Plates", 70),
            ("Reinforced Frame Rods", 36),
            ("Hydraulic Actuators", 12),
            ("Armor Panels", 8),
            ("Walker Leg Assembly", 1));

        SetRequirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 32),
            ("Signal Processors", 20),
            ("Vehicle Control Chips", 7),
            ("Assault AI Chips", 4));

        SetRequirements(so, "buildRequirements.power",
            ("Dual Industrial Battery Core", 1),
            ("Power Regulation Nodes", 3));

        SetRequirements(so, "buildRequirements.rareTradeable",
            ("Shock-Dampened Chassis Core", 1));

        SetStrings(so, "buildRequirements.optionalUpgrades",
            "Reinforced Armor",
            "Stabilized Weapon Mounts",
            "Extended Battery Pack",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[Ironwalker] Population routine finished. Check Console warnings and verify the asset in the Inspector.",
            target);
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Ironwalker] Serialized property not found: " + path);
        return p;
    }

    private static void SetString(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.String)
        {
            Debug.LogWarning("[Ironwalker] Expected String property: " + path);
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
            Debug.LogWarning("[Ironwalker] Expected numeric property: " + path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Integer)
        {
            Debug.LogWarning("[Ironwalker] Expected Integer property: " + path);
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
            Debug.LogWarning("[Ironwalker] Expected Boolean property: " + path);
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
                Debug.LogWarning("[Ironwalker] Enum value '" + value + "' is not defined for " + path);
                return;
            }
            p.enumValueIndex = index;
        }
        else if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else
            Debug.LogWarning("[Ironwalker] Expected Enum or String property: " + path);
    }

    private static void SetStrings(SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Ironwalker] Expected list/array property: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Ironwalker] Expected string list element: " + path);
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
            Debug.LogWarning("[Ironwalker] Expected requirement list/array property: " + path);
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
                Debug.LogWarning("[Ironwalker] Requirement element schema mismatch: " + path);
                continue;
            }

            component.stringValue = values[i].component;
            quantity.intValue = values[i].quantity;
        }
    }
}

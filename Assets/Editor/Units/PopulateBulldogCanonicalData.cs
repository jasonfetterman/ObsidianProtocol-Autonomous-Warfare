using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateBulldogCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Ground/Bulldog/Bulldog.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Bulldog Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Bulldog] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        E(so, "category", "Ground");
        S(so, "unitId", "obs-ground-bulldog");
        S(so, "displayName", "Bulldog");
        S(so, "role", "Armored Patrol / Perimeter Security");
        S(so, "tier", "Security");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 4.2f);
        F(so, "physical.widthMeters", 2.1f);
        F(so, "physical.heightMeters", 1.8f);
        F(so, "physical.massKg", 1200f);
        S(so, "physical.powerSource", "Hybrid diesel-electric");
        F(so, "physical.energyCapacityWh", 5200f);
        F(so, "physical.payloadCapacityKg", 300f);
        F(so, "physical.structuralIntegrityHp", 900f);
        F(so, "physical.armorRating", 0.65f);
        S(so, "physical.endurance", "6–10 hours");
        S(so, "physical.navigationSystem", "GPS + Patrol AI");
        S(so, "physical.heatSignature", "Medium");
        S(so, "physical.noiseSignature", "Medium");
        F(so, "physical.topSpeedMetersPerSecond", 18f);
        S(so, "physical.topSpeedDescription", "18 m/s");
        S(so, "physical.acceleration", "Low");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.stabilityRating", "High");
        Strings(so, "physical.modules", new string[]
        {
            "Armor plating",
            "Patrol optics"
        });

        // Optional fuel field: set only if this schema contains it.
        F(so, "physical.fuelCapacityLiters", 60f);

        // Autonomy and AI behavior
        S(so, "autonomy.behaviorProfile", "Patrol / Perimeter Defense");
        F(so, "autonomy.aggression", 0.4f);
        F(so, "autonomy.caution", 0.7f);
        F(so, "autonomy.persistence", 0.8f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.6f);
        F(so, "autonomy.riskTolerance", 0.5f);
        S(so, "autonomy.targetPriority", "Hostile movement near perimeter");
        S(so, "autonomy.formationDiscipline", "High");
        S(so, "autonomy.retreatBehaviorDescription", "Moderate");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1800f);
        F(so, "sensors.thermalRangeMeters", 900f);
        S(so, "sensors.visualRangeDescription", "1800 m");
        S(so, "sensors.thermalRangeDescription", "900 m");
        S(so, "sensors.acousticRange", "Medium");
        S(so, "sensors.radarRange", "Short");
        S(so, "sensors.lidarRange", "Short");
        S(so, "sensors.signalDetectionRange", "Medium");

        // Communication
        F(so, "communication.meshRangeKm", 5f);
        S(so, "communication.meshRangeDescription", "5 km");
        F(so, "communication.relayCapacity", 1f);
        S(so, "communication.commandLinkType", "Ground Grid");
        S(so, "communication.networkPriority", "Medium");

        // Combat and threat model
        S(so, "combat.firepowerDescription", "Medium");
        F(so, "combat.firepowerRating", 0.5f);
        S(so, "combat.weaponMounts", "1");
        S(so, "combat.weaponTypes", "Light turret / suppression gun");
        S(so, "combat.accuracy", "Medium");
        S(so, "combat.stabilization", "Medium");
        S(so, "combat.suppressionValue", "Medium");
        S(so, "combat.trackingStrength", "Low");
        S(so, "combat.disruptionStrength", "Medium");
        S(so, "combat.breachCapability", "Low");
        S(so, "combat.countermeasureRating", "Medium");
        I(so, "combat.threatLevel", 5);
        S(so, "combat.engagementDistance", "300–700 m");
        S(so, "combat.preferredTerrain", "Perimeter roads / open ground");
        S(so, "combat.preferredTargets", "Light vehicles / infantry");
        S(so, "combat.vulnerabilityProfile", "EMP / heavy weapons");

        // Squad and command integration
        S(so, "squad.squadRole", "Patrol Security");
        I(so, "squad.squadSizeContribution", 2);
        S(so, "squad.formationBehavior", "Perimeter spacing");
        S(so, "squad.intentInterpretationRules", "Maintain patrol route");
        S(so, "squad.retreatBehavior", "Moderate");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports security squads");
        S(so, "squad.parentCommandUnit", "Command Core / Pulse");
        Strings(so, "squad.subordinateUnits", new string[]
        {
            "Patrol",
            "Scout"
        });
        Strings(so, "squad.relayNodes", new string[]
        {
            "Relay Node 1"
        });
        S(so, "squad.networkDependencies", "Ground Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Armored Vehicle Cockpit");
        S(so, "vrOperator.operatorHud", "Patrol Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical + turret cam");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.tacticalOverlay", true);
        B(so, "vrOperator.sonarView", false);
        S(so, "vrOperator.movementControls", "Full vehicle drive");
        S(so, "vrOperator.weaponControls", "Turret fire / suppression");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.abilityControls", "Mark Threat, Stabilize, Patrol Mode");
        Strings(so, "vrOperator.operatorAbilities", new string[]
        {
            "Suppression Burst",
            "Threat Marking",
            "Patrol Route Lock",
            "Relay Ground Data"
        });

        // Economy and logistics
        I(so, "economy.creditCost", 900);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "600 Metal / 300 Electronics");
        F(so, "economy.buildTimeSeconds", 120f);
        S(so, "economy.factoryType", "Vehicle Bay");
        S(so, "economy.techRequirements", "Ground Systems I");
        S(so, "economy.maintenanceCost", "Medium");
        F(so, "economy.repairTimeSeconds", 30f);
        S(so, "economy.supplyConsumption", "Medium");
        S(so, "economy.operationalCost", "Moderate");
        S(so, "economy.deploymentRequirements", "Vehicle pad");
        S(so, "economy.transportCompatibility", "Heavy racks");
        S(so, "economy.launchMethod", "Drive-out");

        // Player collection build requirements
        Requirements(so, "buildRequirements.commonComponents", new string[]
        {
            "Heavy Alloy Plates", "Reinforced Frame Rods", "Hydraulic Actuators",
            "Armor Panels", "Patrol Sensor Housing"
        }, new int[] { 40, 20, 6, 4, 1 });

        Requirements(so, "buildRequirements.electronics", new string[]
        {
            "Circuit Bundles", "Signal Processors", "Vehicle Control Chips",
            "Security AI Chips"
        }, new int[] { 20, 14, 4, 3 });

        Requirements(so, "buildRequirements.power", new string[]
        {
            "Hybrid Engine Core", "Power Regulation Nodes"
        }, new int[] { 1, 2 });

        Requirements(so, "buildRequirements.rareTradeable", new string[]
        {
            "Turret Stabilization Module"
        }, new int[] { 1 });

        Strings(so, "buildRequirements.optionalUpgrades", new string[]
        {
            "Reinforced Armor",
            "Silent Drive Kit",
            "Extended Patrol Battery",
            "Anti-Jamming Shielding"
        });

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Bulldog] Canonical data population finished. Review any property warnings above, then verify the asset values in the Inspector.", target);
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Bulldog] Serialized property not found: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else
            Debug.LogWarning("[Bulldog] Expected string property: " + path + " (actual: " + p.propertyType + ")");
    }

    private static void F(SerializedObject so, string path, float value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Bulldog] Expected numeric property: " + path + " (actual: " + p.propertyType + ")");
    }

    private static void I(SerializedObject so, string path, int value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else
            Debug.LogWarning("[Bulldog] Expected integer property: " + path + " (actual: " + p.propertyType + ")");
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Bulldog] Expected boolean property: " + path + " (actual: " + p.propertyType + ")");
    }

    private static void E(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Bulldog] Expected enum property: " + path + " (actual: " + p.propertyType + ")");
            return;
        }

        string[] names = p.enumNames;
        for (int i = 0; i < names.Length; i++)
        {
            if (string.Equals(names[i], value, StringComparison.OrdinalIgnoreCase))
            {
                p.enumValueIndex = i;
                return;
            }
        }

        Debug.LogWarning("[Bulldog] Enum value '" + value + "' is not defined for " + path +
                         ". Available: " + string.Join(", ", names));
    }

    private static void Strings(SerializedObject so, string path, string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Bulldog] Expected string list: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Bulldog] Expected string element in " + path);
        }
    }

    private static void Requirements(SerializedObject so, string path, string[] components, int[] quantities)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Bulldog] Expected ResourceRequirement list: " + path);
            return;
        }

        p.arraySize = components.Length;
        for (int i = 0; i < components.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component != null && component.propertyType == SerializedPropertyType.String)
                component.stringValue = components[i];
            else
                Debug.LogWarning("[Bulldog] Missing string field 'component' in " + path);

            if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                quantity.intValue = quantities[i];
            else
                Debug.LogWarning("[Bulldog] Missing integer field 'quantity' in " + path);
        }
    }
}

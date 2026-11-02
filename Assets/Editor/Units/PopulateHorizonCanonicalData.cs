using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateHorizonCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/Horizon/Horizon.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Horizon Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError("[Horizon] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-air-horizon");
        S(so, "displayName", "Horizon");
        E(so, "category", "Air");
        S(so, "role", "High-Altitude Surveillance / Persistent Watch");
        S(so, "tier", "Advanced Recon");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 3.4f);
        F(so, "physical.widthMeters", 2.6f);
        F(so, "physical.heightMeters", 1.2f);
        F(so, "physical.massKg", 32f);
        S(so, "physical.powerSource", "Long-endurance battery");
        F(so, "physical.energyCapacityWh", 3600f);
        F(so, "physical.energyConsumptionWhPerMinute", 50f);
        S(so, "physical.endurance", "60-90 min");
        F(so, "physical.payloadCapacityKg", 2.0f);
        Strings(so, "physical.modules", new[]
        {
            "Long-range optical suite",
            "Atmospheric stability suite"
        });
        S(so, "physical.navigationSystem", "GPS + High-Altitude Recon AI");
        F(so, "physical.structuralIntegrityHp", 180f);
        F(so, "physical.armorRating", 0.30f);
        S(so, "physical.heatSignature", "Low");
        S(so, "physical.noiseSignature", "Low");
        F(so, "physical.topSpeedMetersPerSecond", 21f);
        S(so, "physical.topSpeedDescription", "21 m/s");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Moderate");
        S(so, "physical.climbRate", "Very High");
        S(so, "physical.stabilityRating", "Extremely High");

        // Autonomy and AI behavior
        S(so, "autonomy.behaviorProfile",
            "Persistent Surveillance / High-Altitude Recon");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.95f);
        F(so, "autonomy.persistence", 1.0f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.95f);
        F(so, "autonomy.riskTolerance", 0.1f);
        S(so, "autonomy.retreatBehaviorDescription", "Very Early");
        S(so, "autonomy.targetPriority", "Long-range visibility targets");
        S(so, "autonomy.formationDiscipline", "Very High");

        // Sensors
        F(so, "sensors.visualRangeMeters", 3000f);
        F(so, "sensors.thermalRangeMeters", 1200f);
        S(so, "sensors.visualRangeDescription", "3000 m");
        S(so, "sensors.thermalRangeDescription", "1200 m");
        S(so, "sensors.acousticRange", "Low");
        S(so, "sensors.radarRange", "Medium");
        S(so, "sensors.lidarRange", "Medium");
        S(so, "sensors.signalDetectionRange", "High");

        // Communication
        F(so, "communication.meshRangeKm", 10f);
        S(so, "communication.meshRangeDescription", "10 km");
        F(so, "communication.relayCapacity", 2f);
        S(so, "communication.commandLinkType", "Recon Grid");
        S(so, "communication.networkPriority", "High");

        // Combat and threat model
        F(so, "combat.firepowerRating", 0f);
        F(so, "combat.suppressionValue", 0f);
        S(so, "combat.accuracy", "N/A");
        S(so, "combat.stabilization", "Extremely High");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        S(so, "combat.trackingStrength", "Very High");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Medium");
        I(so, "combat.threatLevel", 3);
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "High-altitude recon lanes");
        S(so, "combat.preferredTargets", "None");
        S(so, "combat.vulnerabilityProfile", "Ballistic / weather");

        // Squad and command integration
        S(so, "squad.squadRole", "High-Altitude Recon");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "High-altitude spacing");
        S(so, "squad.intentInterpretationRules",
            "Maintain persistent visibility");
        S(so, "squad.retreatBehavior", "Very Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior",
            "Supports recon, command, and mesh squads");
        S(so, "squad.parentCommandUnit", "Insight / Overseer");
        Strings(so, "squad.subordinateUnits", new[]
        {
            "Skywatch",
            "Survey",
            "Terrain",
            "Thermal"
        });

        // The specification supplies a relay-node count, not node identities.
        // Keep relayNodes empty rather than fabricating named nodes.
        Strings(so, "squad.relayNodes", new string[0]);
        S(so, "squad.networkDependencies", "Recon Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "High-Altitude Drone View");
        S(so, "vrOperator.operatorHud", "Long-Range Recon Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical / Radar");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full drone flight");
        S(so, "vrOperator.sensorControls", "Multi-sensor switching");
        S(so, "vrOperator.abilityControls", "Scan, Mark Target, Stabilize");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls",
            "High-altitude auto-hover");
        Strings(so, "vrOperator.operatorAbilities", new[]
        {
            "Long-Range Scan",
            "High-Altitude Stabilize",
            "Target Mark",
            "Relay Recon Data"
        });

        // Economy and logistics
        I(so, "economy.creditCost", 430);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "210 Metal / 220 Electronics");
        F(so, "economy.buildTimeSeconds", 70f);
        S(so, "economy.factoryType", "Drone Bay");
        S(so, "economy.techRequirements", "Recon Systems III");
        S(so, "economy.maintenanceCost", "Medium");
        F(so, "economy.repairTimeSeconds", 18f);
        S(so, "economy.supplyConsumption", "Medium");
        S(so, "economy.operationalCost", "Moderate");
        S(so, "economy.deploymentRequirements", "High-altitude drone pad");
        S(so, "economy.transportCompatibility", "All aerial racks");
        S(so, "economy.launchMethod", "Vertical takeoff");

        // Player collection build requirements
        Requirements(so, "buildRequirements.commonComponents", new[]
        {
            Req("Medium Alloy Plates", 16),
            Req("Composite Frame Rods", 12),
            Req("Micro-Servo Actuators", 6),
            Req("Stabilizer Fins", 3),
            Req("High-Altitude Sensor Housing", 1)
        });

        Requirements(so, "buildRequirements.electronics", new[]
        {
            Req("Circuit Bundles", 16),
            Req("Signal Processors", 12),
            Req("Drone Control Chips", 4),
            Req("Recon AI Chips", 4)
        });

        Requirements(so, "buildRequirements.power", new[]
        {
            Req("Long-Endurance Battery Pack", 1),
            Req("Power Regulation Nodes", 3)
        });

        Requirements(so, "buildRequirements.rareTradeable", new[]
        {
            Req("Long-Range Optics Core", 1),
            Req("High-Altitude Stability Processor", 1)
        });

        Strings(so, "buildRequirements.optionalUpgrades", new[]
        {
            "Extended Altitude Module",
            "Silent Rotor Kit",
            "Ultra-Long-Range Lens",
            "Anti-Jamming Shielding"
        });

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("[Horizon] Canonical data saved: " + AssetPath +
                  ". Existing model prefab and store image were preserved.");
    }

    private static void S(SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null || p.propertyType != SerializedPropertyType.String)
        {
            Warn(path, "string");
            return;
        }
        p.stringValue = value;
    }

    private static void F(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
        {
            Warn(path, "float");
            return;
        }

        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Warn(path, "float or integer");
    }

    private static void I(SerializedObject so, string path, int value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null || p.propertyType != SerializedPropertyType.Integer)
        {
            Warn(path, "integer");
            return;
        }
        p.intValue = value;
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null || p.propertyType != SerializedPropertyType.Boolean)
        {
            Warn(path, "boolean");
            return;
        }
        p.boolValue = value;
    }

    private static void E(SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null || p.propertyType != SerializedPropertyType.Enum)
        {
            Warn(path, "enum");
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

        Debug.LogWarning("[Horizon] Enum value '" + value +
                         "' not found for " + path + ". Available: " +
                         string.Join(", ", names));
    }

    private static void Strings(SerializedObject so, string path, string[] values)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null || !p.isArray)
        {
            Warn(path, "string array/list");
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType != SerializedPropertyType.String)
            {
                Warn(path + "[" + i + "]", "string");
                continue;
            }
            element.stringValue = values[i];
        }
    }

    private static void Requirements(
        SerializedObject so, string path, Requirement[] values)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null || !p.isArray)
        {
            Warn(path, "ResourceRequirement array/list");
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            SerializedProperty component =
                element.FindPropertyRelative("component");
            SerializedProperty quantity =
                element.FindPropertyRelative("quantity");

            if (component == null ||
                component.propertyType != SerializedPropertyType.String)
                Warn(path + "[" + i + "].component", "string");
            else
                component.stringValue = values[i].component;

            if (quantity == null ||
                quantity.propertyType != SerializedPropertyType.Integer)
                Warn(path + "[" + i + "].quantity", "integer");
            else
                quantity.intValue = values[i].quantity;
        }
    }

    private static Requirement Req(string component, int quantity)
    {
        return new Requirement { component = component, quantity = quantity };
    }

    private static void Warn(string path, string expected)
    {
        Debug.LogWarning("[Horizon] Missing or incompatible serialized property '" +
                         path + "' (expected " + expected + ").");
    }

    private struct Requirement
    {
        public string component;
        public int quantity;
    }
}

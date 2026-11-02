using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateSkywatchCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/Skywatch/Skywatch.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Skywatch Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError("[Skywatch] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-air-skywatch");
        S(so, "displayName", "Skywatch");
        E(so, "category", "Air");
        S(so, "role", "Airspace Security / Perimeter Enforcement");
        S(so, "tier", "Security");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 2.8f);
        F(so, "physical.widthMeters", 2.1f);
        F(so, "physical.heightMeters", 1.0f);
        F(so, "physical.massKg", 26f);
        S(so, "physical.powerSource", "High-capacity patrol battery");
        F(so, "physical.energyCapacityWh", 2600f);
        F(so, "physical.energyConsumptionWhPerMinute", 55f);
        S(so, "physical.endurance", "35-50 min");
        F(so, "physical.payloadCapacityKg", 2.0f);
        Strings(so, "physical.modules", new[]
        {
            "Security optics",
            "Deterrent pulse"
        });
        S(so, "physical.navigationSystem", "GPS + Patrol AI");
        F(so, "physical.structuralIntegrityHp", 170f);
        F(so, "physical.armorRating", 0.28f);
        S(so, "physical.heatSignature", "Medium");
        S(so, "physical.noiseSignature", "Medium");
        F(so, "physical.topSpeedMetersPerSecond", 23f);
        S(so, "physical.topSpeedDescription", "23 m/s");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.climbRate", "Medium");
        S(so, "physical.stabilityRating", "Very High");

        // Autonomy and AI
        S(so, "autonomy.behaviorProfile", "Airspace Security / Patrol");
        F(so, "autonomy.aggression", 0.3f);
        F(so, "autonomy.caution", 0.7f);
        F(so, "autonomy.persistence", 0.9f);
        F(so, "autonomy.flankingLikelihood", 0.2f);
        F(so, "autonomy.retreatThreshold", 0.6f);
        F(so, "autonomy.riskTolerance", 0.5f);
        S(so, "autonomy.retreatBehaviorDescription", "Medium");
        S(so, "autonomy.targetPriority", "Unauthorized drones / intruders");
        S(so, "autonomy.formationDiscipline", "High");

        // Sensors
        F(so, "sensors.visualRangeMeters", 2000f);
        F(so, "sensors.thermalRangeMeters", 900f);
        S(so, "sensors.visualRangeDescription", "2000 m");
        S(so, "sensors.thermalRangeDescription", "900 m");
        S(so, "sensors.acousticRange", "Medium");
        S(so, "sensors.radarRange", "Short-range");
        S(so, "sensors.lidarRange", "Medium");
        S(so, "sensors.signalDetectionRange", "High");

        // Communication
        F(so, "communication.meshRangeKm", 8f);
        S(so, "communication.meshRangeDescription", "8 km");
        F(so, "communication.relayCapacity", 2f);
        S(so, "communication.commandLinkType", "Security Grid");
        S(so, "communication.networkPriority", "High");

        // Combat and threat model
        // Ratings use a normalized 0-5 interpretation; Medium is represented as 3.
        F(so, "combat.firepowerRating", 3f);
        F(so, "combat.suppressionValue", 3f);
        S(so, "combat.accuracy", "Medium");
        S(so, "combat.stabilization", "Very High");
        S(so, "combat.weaponMounts", "1 (deterrent pulse emitter)");
        S(so, "combat.weaponTypes", "Pulse / marker beam");
        S(so, "combat.trackingStrength", "High");
        S(so, "combat.disruptionStrength", "Medium");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Medium");
        I(so, "combat.threatLevel", 5);
        S(so, "combat.engagementDistance", "150-350 m");
        S(so, "combat.preferredTerrain", "Patrol corridors");
        S(so, "combat.preferredTargets", "Unauthorized drones / intruders");
        S(so, "combat.vulnerabilityProfile", "Ballistic / EMP / jamming");

        // Squad and command integration
        S(so, "squad.squadRole", "Airspace Security");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Patrol spacing");
        S(so, "squad.intentInterpretationRules", "Maintain perimeter integrity");
        S(so, "squad.retreatBehavior", "Medium");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "Light");
        S(so, "squad.reinforcementBehavior",
            "Supports command, recon, and mesh squads");
        S(so, "squad.parentCommandUnit", "Overseer / Command Core");
        Strings(so, "squad.subordinateUnits", new string[0]);

        // The specification gives a relay count but no relay-node identities.
        // Preserve the count in communication.relayCapacity; do not invent nodes.
        Strings(so, "squad.relayNodes", new string[0]);
        S(so, "squad.networkDependencies", "Security Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Drone View");
        S(so, "vrOperator.operatorHud", "Security Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical / Radar");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full drone flight");
        S(so, "vrOperator.sensorControls", "Multi-sensor switching");
        S(so, "vrOperator.abilityControls", "Pulse, Mark Target, Scan");
        S(so, "vrOperator.weaponControls", "Deterrent pulse");
        S(so, "vrOperator.stabilizationControls", "Auto-hover");
        Strings(so, "vrOperator.operatorAbilities", new[]
        {
            "Deterrent Pulse",
            "Multi-Sensor Scan",
            "Target Mark",
            "Patrol Pathing",
            "Relay Security Data"
        });

        // Economy and logistics
        I(so, "economy.creditCost", 370);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "180 Metal / 190 Electronics");
        F(so, "economy.buildTimeSeconds", 60f);
        S(so, "economy.factoryType", "Drone Bay");
        S(so, "economy.techRequirements", "Security Systems I");
        S(so, "economy.maintenanceCost", "Medium");
        F(so, "economy.repairTimeSeconds", 15f);
        S(so, "economy.supplyConsumption", "Medium");
        S(so, "economy.operationalCost", "Moderate");
        S(so, "economy.deploymentRequirements", "Standard drone pad");
        S(so, "economy.transportCompatibility", "All aerial racks");
        S(so, "economy.launchMethod", "Vertical takeoff");

        // Player collection build requirements
        Requirements(so, "buildRequirements.commonComponents", new[]
        {
            Req("Medium Alloy Plates", 14),
            Req("Composite Frame Rods", 10),
            Req("Micro-Servo Actuators", 6),
            Req("Stabilizer Fins", 3),
            Req("Security Sensor Housing", 1)
        });

        Requirements(so, "buildRequirements.electronics", new[]
        {
            Req("Circuit Bundles", 14),
            Req("Signal Processors", 10),
            Req("Drone Control Chips", 4),
            Req("Security AI Chips", 3)
        });

        Requirements(so, "buildRequirements.power", new[]
        {
            Req("High-Capacity Patrol Battery", 1),
            Req("Power Regulation Nodes", 3)
        });

        Requirements(so, "buildRequirements.rareTradeable", new[]
        {
            Req("Security Pulse Core", 1),
            Req("Intrusion Detection Processor", 1)
        });

        Strings(so, "buildRequirements.optionalUpgrades", new[]
        {
            "Extended Patrol Range Module",
            "Anti-Jamming Shielding",
            "Silent Rotor Kit",
            "High-Density Sensor Grid"
        });

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("[Skywatch] Canonical data saved: " + AssetPath +
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

        Debug.LogWarning("[Skywatch] Enum value '" + value +
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
        Debug.LogWarning("[Skywatch] Missing or incompatible serialized property '" +
                         path + "' (expected " + expected + ").");
    }

    private struct Requirement
    {
        public string component;
        public int quantity;
    }
}

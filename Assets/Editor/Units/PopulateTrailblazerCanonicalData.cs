using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateTrailblazerCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/Trailblazer/Trailblazer.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Trailblazer Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError("[Trailblazer] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-air-trailblazer");
        S(so, "displayName", "Trailblazer");
        E(so, "category", "Air");
        S(so, "role", "Forward Recon / Pathfinding");
        S(so, "tier", "Recon");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 2.9f);
        F(so, "physical.widthMeters", 2.1f);
        F(so, "physical.heightMeters", 1.0f);
        F(so, "physical.massKg", 24f);
        S(so, "physical.powerSource", "Mid-endurance battery");
        F(so, "physical.energyCapacityWh", 2600f);
        F(so, "physical.energyConsumptionWhPerMinute", 42f);
        S(so, "physical.endurance", "45-70 min");
        F(so, "physical.payloadCapacityKg", 1.4f);
        Strings(so, "physical.modules", new[]
        {
            "Pathfinding optics",
            "Terrain-scan suite"
        });
        S(so, "physical.navigationSystem", "GPS + Route-Mapping AI");
        F(so, "physical.structuralIntegrityHp", 140f);
        F(so, "physical.armorRating", 0.22f);
        S(so, "physical.heatSignature", "Low");
        S(so, "physical.noiseSignature", "Low");
        F(so, "physical.topSpeedMetersPerSecond", 23f);
        S(so, "physical.topSpeedDescription", "23 m/s");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.climbRate", "High");
        S(so, "physical.stabilityRating", "High");

        // Autonomy and AI behavior
        S(so, "autonomy.behaviorProfile", "Forward Recon / Route Discovery");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.85f);
        F(so, "autonomy.persistence", 0.9f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.9f);
        F(so, "autonomy.riskTolerance", 0.2f);
        S(so, "autonomy.retreatBehaviorDescription", "Early");
        S(so, "autonomy.targetPriority", "Terrain anomalies / movement");
        S(so, "autonomy.formationDiscipline", "Medium");

        // Sensors
        F(so, "sensors.visualRangeMeters", 2400f);
        F(so, "sensors.thermalRangeMeters", 900f);
        S(so, "sensors.visualRangeDescription", "2400 m");
        S(so, "sensors.thermalRangeDescription", "900 m");
        S(so, "sensors.acousticRange", "Low");
        S(so, "sensors.radarRange", "None");
        S(so, "sensors.lidarRange", "Medium");
        S(so, "sensors.signalDetectionRange", "Medium");

        // Communication
        F(so, "communication.meshRangeKm", 7f);
        S(so, "communication.meshRangeDescription", "7 km");
        F(so, "communication.relayCapacity", 0f);
        S(so, "communication.commandLinkType", "Recon Grid");
        S(so, "communication.networkPriority", "Medium");

        // Combat and threat model
        F(so, "combat.firepowerRating", 0f);
        F(so, "combat.suppressionValue", 0f);
        S(so, "combat.accuracy", "N/A");
        S(so, "combat.stabilization", "High");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        S(so, "combat.trackingStrength", "Medium");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Low");
        I(so, "combat.threatLevel", 2);
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "Forward recon lanes");
        S(so, "combat.preferredTargets", "None");
        S(so, "combat.vulnerabilityProfile", "Ballistic / weather");

        // Squad and command integration
        S(so, "squad.squadRole", "Pathfinding Recon");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Forward spacing");
        S(so, "squad.intentInterpretationRules", "Discover safe routes");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior",
            "Supports recon and command squads");
        S(so, "squad.parentCommandUnit", "Insight / Overseer");
        Strings(so, "squad.subordinateUnits", new[]
        {
            "Survey",
            "Terrain",
            "Scout Eye"
        });
        Strings(so, "squad.relayNodes", new string[0]);
        S(so, "squad.networkDependencies", "Recon Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Recon Drone View");
        S(so, "vrOperator.operatorHud", "Pathfinding Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full drone flight");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.abilityControls", "Scan, Mark Route, Stabilize");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls", "Auto-hover");
        Strings(so, "vrOperator.operatorAbilities", new[]
        {
            "Route Scan",
            "Terrain Mapping",
            "Mark Safe Path",
            "Relay Recon Data"
        });

        // Economy and logistics
        I(so, "economy.creditCost", 260);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "150 Metal / 110 Electronics");
        F(so, "economy.buildTimeSeconds", 50f);
        S(so, "economy.factoryType", "Drone Bay");
        S(so, "economy.techRequirements", "Recon Systems II");
        S(so, "economy.maintenanceCost", "Low");
        F(so, "economy.repairTimeSeconds", 14f);
        S(so, "economy.supplyConsumption", "Low");
        S(so, "economy.operationalCost", "Minimal");
        S(so, "economy.deploymentRequirements", "Standard drone pad");
        S(so, "economy.transportCompatibility", "All aerial racks");
        S(so, "economy.launchMethod", "Vertical takeoff");

        // Player collection build requirements
        Requirements(so, "buildRequirements.commonComponents", new[]
        {
            Req("Light Alloy Plates", 14),
            Req("Composite Frame Rods", 10),
            Req("Micro-Servo Actuators", 4),
            Req("Stabilizer Fins", 2),
            Req("Pathfinding Sensor Housing", 1)
        });

        Requirements(so, "buildRequirements.electronics", new[]
        {
            Req("Circuit Bundles", 14),
            Req("Signal Processors", 10),
            Req("Drone Control Chips", 3),
            Req("Recon AI Chips", 2)
        });

        Requirements(so, "buildRequirements.power", new[]
        {
            Req("Mid-Endurance Battery Pack", 1),
            Req("Power Regulation Nodes", 2)
        });

        Requirements(so, "buildRequirements.rareTradeable", new[]
        {
            Req("Terrain-Scan Optics Core", 1)
        });

        Strings(so, "buildRequirements.optionalUpgrades", new[]
        {
            "Extended Battery Pack",
            "Silent Rotor Kit",
            "High-Density Recon Grid"
        });

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("[Trailblazer] Canonical data saved: " + AssetPath +
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

        Debug.LogWarning("[Trailblazer] Enum value '" + value +
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
        Debug.LogWarning("[Trailblazer] Missing or incompatible serialized property '" +
                         path + "' (expected " + expected + ").");
    }

    private struct Requirement
    {
        public string component;
        public int quantity;
    }
}

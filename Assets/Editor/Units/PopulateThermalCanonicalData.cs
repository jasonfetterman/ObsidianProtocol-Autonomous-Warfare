using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateThermalCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/Thermal/Thermal.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Thermal Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError("[Thermal] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-air-thermal");
        S(so, "displayName", "Thermal");
        E(so, "category", "Air");
        S(so, "role", "High-Resolution Heat Detection / Night Recon");
        S(so, "tier", "Recon Support");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 2.1f);
        F(so, "physical.widthMeters", 1.5f);
        F(so, "physical.heightMeters", 0.8f);
        F(so, "physical.massKg", 15f);
        S(so, "physical.powerSource", "Mid-capacity battery");
        F(so, "physical.energyCapacityWh", 1700f);
        F(so, "physical.energyConsumptionWhPerMinute", 36f);
        S(so, "physical.endurance", "35-55 min");
        F(so, "physical.payloadCapacityKg", 1.0f);
        Strings(so, "physical.modules", new[]
        {
            "High-resolution thermal camera"
        });
        S(so, "physical.navigationSystem", "GPS + Thermal AI");
        F(so, "physical.structuralIntegrityHp", 110f);
        F(so, "physical.armorRating", 0.14f);
        S(so, "physical.heatSignature", "Very Low");
        S(so, "physical.noiseSignature", "Very Low");
        F(so, "physical.topSpeedMetersPerSecond", 19f);
        S(so, "physical.topSpeedDescription", "19 m/s");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.climbRate", "Medium");
        S(so, "physical.stabilityRating", "High");

        // Autonomy and AI
        S(so, "autonomy.behaviorProfile", "Thermal Recon / Heat Detection");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.9f);
        F(so, "autonomy.persistence", 0.85f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.9f);
        F(so, "autonomy.riskTolerance", 0.2f);
        S(so, "autonomy.retreatBehaviorDescription", "Early");
        S(so, "autonomy.targetPriority", "Heat signatures / movement");
        S(so, "autonomy.formationDiscipline", "Medium");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1400f);
        F(so, "sensors.thermalRangeMeters", 2000f);
        S(so, "sensors.visualRangeDescription", "1400 m");
        S(so, "sensors.thermalRangeDescription", "2000 m - Very High");
        S(so, "sensors.acousticRange", "Low");
        S(so, "sensors.radarRange", "None");
        S(so, "sensors.lidarRange", "Short-range");
        S(so, "sensors.signalDetectionRange", "Medium");

        // Communication
        F(so, "communication.meshRangeKm", 5f);
        S(so, "communication.meshRangeDescription", "5 km");
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
        S(so, "combat.trackingStrength", "High");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Low");
        I(so, "combat.threatLevel", 2);
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "Low-light airspace");
        S(so, "combat.preferredTargets", "None");
        S(so, "combat.vulnerabilityProfile", "Ballistic / weather");

        // Squad and command integration
        S(so, "squad.squadRole", "Thermal Recon");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Night-ops spacing");
        S(so, "squad.intentInterpretationRules", "Maintain thermal visibility");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports recon and command squads");
        S(so, "squad.parentCommandUnit", "Overseer / Insight");
        Strings(so, "squad.subordinateUnits", new string[0]);
        Strings(so, "squad.relayNodes", new string[0]);
        S(so, "squad.networkDependencies", "Recon Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Drone View");
        S(so, "vrOperator.operatorHud", "Thermal Recon Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full drone flight");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.abilityControls", "Thermal Scan, Mark Target, Stabilize");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls", "Auto-hover");
        Strings(so, "vrOperator.operatorAbilities", new[]
        {
            "High-Resolution Thermal Scan",
            "Mark Heat Target",
            "LiDAR Sweep",
            "Relay Recon Data"
        });

        // Economy and logistics
        I(so, "economy.creditCost", 190);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "100 Metal / 90 Electronics");
        F(so, "economy.buildTimeSeconds", 30f);
        S(so, "economy.factoryType", "Drone Bay");
        S(so, "economy.techRequirements", "Recon Systems II");
        S(so, "economy.maintenanceCost", "Low");
        F(so, "economy.repairTimeSeconds", 10f);
        S(so, "economy.supplyConsumption", "Low");
        S(so, "economy.operationalCost", "Minimal");
        S(so, "economy.deploymentRequirements", "Standard drone pad");
        S(so, "economy.transportCompatibility", "All aerial racks");
        S(so, "economy.launchMethod", "Vertical takeoff");

        // Player collection build requirements
        Requirements(so, "buildRequirements.commonComponents", new[]
        {
            Req("Light Alloy Plates", 10),
            Req("Composite Frame Rods", 6),
            Req("Micro-Servo Actuators", 4),
            Req("Stabilizer Fins", 2),
            Req("Thermal Sensor Housing", 1)
        });

        Requirements(so, "buildRequirements.electronics", new[]
        {
            Req("Circuit Bundles", 10),
            Req("Signal Processors", 8),
            Req("Drone Control Chips", 3),
            Req("Thermal AI Chips", 2)
        });

        Requirements(so, "buildRequirements.power", new[]
        {
            Req("Mid-Capacity Battery Pack", 1),
            Req("Power Regulation Nodes", 2)
        });

        Requirements(so, "buildRequirements.rareTradeable", new[]
        {
            Req("High-Resolution Thermal Core", 1)
        });

        Strings(so, "buildRequirements.optionalUpgrades", new[]
        {
            "Silent Rotor Kit",
            "Extended Battery Pack",
            "Ultra-Thermal Lens"
        });

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("[Thermal] Canonical data saved: " + AssetPath +
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

        Debug.LogWarning("[Thermal] Enum value '" + value +
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
        SerializedObject so,
        string path,
        Requirement[] values)
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
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component == null || component.propertyType != SerializedPropertyType.String)
                Warn(path + "[" + i + "].component", "string");
            else
                component.stringValue = values[i].component;

            if (quantity == null || quantity.propertyType != SerializedPropertyType.Integer)
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
        Debug.LogWarning("[Thermal] Missing or incompatible serialized property '" +
                         path + "' (expected " + expected + ").");
    }

    private struct Requirement
    {
        public string component;
        public int quantity;
    }
}

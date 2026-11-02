using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateForgeCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Ground/Forge/Forge.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Forge Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Forge] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        E(so, "category", "Ground");
        S(so, "unitId", "obs-ground-forge");
        S(so, "displayName", "Forge");
        S(so, "role", "Field Construction / Mobile Fabrication");
        S(so, "tier", "Engineering");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 4.6f);
        F(so, "physical.widthMeters", 2.3f);
        F(so, "physical.heightMeters", 2.0f);
        F(so, "physical.massKg", 1450f);
        S(so, "physical.powerSource", "Heavy industrial generator + auxiliary battery");
        F(so, "physical.energyCapacityWh", 6800f);
        F(so, "physical.payloadCapacityKg", 500f);
        F(so, "physical.structuralIntegrityHp", 1000f);
        F(so, "physical.armorRating", 0.55f);
        S(so, "physical.endurance", "5–8 hours");
        S(so, "physical.navigationSystem", "GPS + Construction AI");
        S(so, "physical.heatSignature", "Medium");
        S(so, "physical.noiseSignature", "High");
        F(so, "physical.topSpeedMetersPerSecond", 12f);
        S(so, "physical.topSpeedDescription", "12 m/s");
        S(so, "physical.acceleration", "Low");
        S(so, "physical.turningRadius", "Moderate");
        S(so, "physical.stabilityRating", "Very High");
        Strings(so, "physical.modules", new string[]
        {
            "Fabrication arm",
            "Welding suite",
            "Structural scanner"
        });
        F(so, "physical.fuelCapacityLiters", 80f);

        // Autonomy and AI behavior
        S(so, "autonomy.behaviorProfile", "Construction / Repair / Deployment");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.9f);
        F(so, "autonomy.persistence", 1f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.7f);
        F(so, "autonomy.riskTolerance", 0.2f);
        S(so, "autonomy.targetPriority", "Damaged structures / build sites");
        S(so, "autonomy.formationDiscipline", "High");
        S(so, "autonomy.retreatBehaviorDescription", "Early");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1500f);
        F(so, "sensors.thermalRangeMeters", 600f);
        S(so, "sensors.visualRangeDescription", "1500 m");
        S(so, "sensors.thermalRangeDescription", "600 m");
        S(so, "sensors.acousticRange", "Medium");
        S(so, "sensors.radarRange", "None");
        S(so, "sensors.lidarRange", "Medium");
        S(so, "sensors.signalDetectionRange", "Short");

        // Communication
        F(so, "communication.meshRangeKm", 4f);
        S(so, "communication.meshRangeDescription", "4 km");
        F(so, "communication.relayCapacity", 1f);
        S(so, "communication.commandLinkType", "Engineering Grid");
        S(so, "communication.networkPriority", "High");

        // Combat and threat model
        F(so, "combat.firepowerRating", 0f);
        S(so, "combat.firepowerDescription", "None");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        F(so, "combat.suppressionValue", 0f);
        S(so, "combat.accuracy", "N/A");
        S(so, "combat.trackingStrength", "Low");
        S(so, "combat.disruptionStrength", "Medium (construction tools)");
        S(so, "combat.breachCapability", "Medium (structural arm)");
        S(so, "combat.countermeasureRating", "Low");
        I(so, "combat.threatLevel", 3);
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "Build zones / forward bases");
        S(so, "combat.preferredTargets", "None");
        S(so, "combat.vulnerabilityProfile", "Ballistic / explosives");

        // Squad and command integration
        S(so, "squad.squadRole", "Engineering / Construction");
        I(so, "squad.squadSizeContribution", 3);
        S(so, "squad.formationBehavior", "Build-site spacing");
        S(so, "squad.intentInterpretationRules", "Prioritize repairs and construction");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "Structural arm only");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports all squads via repairs");
        S(so, "squad.parentCommandUnit", "Command Core / Fusion");
        Strings(so, "squad.subordinateUnits", new string[]
        {
            "Mule",
            "Hauler"
        });
        Strings(so, "squad.relayNodes", new string[]
        {
            "Relay Node 1"
        });
        S(so, "squad.networkDependencies", "Engineering Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Engineering Cockpit");
        S(so, "vrOperator.operatorHud", "Construction Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical + arm cam");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.tacticalOverlay", true);
        B(so, "vrOperator.sonarView", false);
        S(so, "vrOperator.movementControls", "Full vehicle drive");
        S(so, "vrOperator.weaponControls", "N/A - construction tools only");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.abilityControls", "Deploy Structure, Repair, Scan Integrity");
        Strings(so, "vrOperator.operatorAbilities", new string[]
        {
            "Structural Repair",
            "Rapid Deployment",
            "Material Fabrication",
            "Build Zone Marking"
        });

        // Economy and logistics
        I(so, "economy.creditCost", 1100);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "700 Metal / 400 Electronics");
        F(so, "economy.buildTimeSeconds", 140f);
        S(so, "economy.factoryType", "Vehicle Bay");
        S(so, "economy.techRequirements", "Engineering Systems I");
        S(so, "economy.maintenanceCost", "Medium");
        F(so, "economy.repairTimeSeconds", 35f);
        S(so, "economy.supplyConsumption", "Medium");
        S(so, "economy.operationalCost", "Moderate");
        S(so, "economy.deploymentRequirements", "Vehicle pad");
        S(so, "economy.transportCompatibility", "Heavy racks");
        S(so, "economy.launchMethod", "Drive-out");

        // Player collection build requirements
        Requirements(so, "buildRequirements.commonComponents", new string[]
        {
            "Heavy Alloy Plates",
            "Reinforced Frame Rods",
            "Hydraulic Actuators",
            "Armor Panels",
            "Fabrication Arm Housing"
        }, new int[] { 50, 24, 8, 4, 1 });

        Requirements(so, "buildRequirements.electronics", new string[]
        {
            "Circuit Bundles",
            "Signal Processors",
            "Vehicle Control Chips",
            "Engineering AI Chips"
        }, new int[] { 24, 16, 5, 3 });

        Requirements(so, "buildRequirements.power", new string[]
        {
            "Industrial Generator Core",
            "Power Regulation Nodes"
        }, new int[] { 1, 3 });

        Requirements(so, "buildRequirements.rareTradeable", new string[]
        {
            "Precision Welding Module"
        }, new int[] { 1 });

        Strings(so, "buildRequirements.optionalUpgrades", new string[]
        {
            "Reinforced Arm",
            "Silent Generator Kit",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding"
        });

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Forge] Population routine finished. Check property warnings and verify the asset in the Inspector.", target);
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Forge] Serialized property not found: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else
            Debug.LogWarning("[Forge] Expected string property: " + path +
                             " (actual: " + p.propertyType + ")");
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
            Debug.LogWarning("[Forge] Expected numeric property: " + path +
                             " (actual: " + p.propertyType + ")");
    }

    private static void I(SerializedObject so, string path, int value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else
            Debug.LogWarning("[Forge] Expected integer property: " + path +
                             " (actual: " + p.propertyType + ")");
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Forge] Expected boolean property: " + path +
                             " (actual: " + p.propertyType + ")");
    }

    private static void E(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Forge] Expected enum property: " + path +
                             " (actual: " + p.propertyType + ")");
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

        Debug.LogWarning("[Forge] Enum value '" + value + "' is not defined for " +
                         path + ". Available: " + string.Join(", ", names));
    }

    private static void Strings(SerializedObject so, string path, string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Forge] Expected string list: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Forge] Expected string element in " + path);
        }
    }

    private static void Requirements(SerializedObject so, string path,
                                     string[] components, int[] quantities)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Forge] Expected ResourceRequirement list: " + path);
            return;
        }

        if (components.Length != quantities.Length)
        {
            Debug.LogError("[Forge] Component and quantity counts differ for " + path);
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
                Debug.LogWarning("[Forge] Missing string field 'component' in " + path);

            if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                quantity.intValue = quantities[i];
            else
                Debug.LogWarning("[Forge] Missing integer field 'quantity' in " + path);
        }
    }
}

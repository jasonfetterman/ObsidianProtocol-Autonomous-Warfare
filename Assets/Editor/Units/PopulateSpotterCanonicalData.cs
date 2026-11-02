using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateSpotterCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/Spotter/Spotter.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Spotter Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError("Spotter asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        S(so, "unitId", "obs-air-spotter");
        S(so, "displayName", "Spotter");
        S(so, "role", "Target Identification / Precision Marking");
        S(so, "tier", "Recon Support");
        S(so, "status", "Active");
        EnumChoice(so, "category", "Air");

        // Physical and technical
        F(so, "physical.lengthMeters", 1.8f);
        F(so, "physical.widthMeters", 1.3f);
        F(so, "physical.heightMeters", 0.7f);
        F(so, "physical.massKg", 12f);
        S(so, "physical.powerSource", "Compact battery");
        F(so, "physical.energyCapacityWh", 1400f);
        F(so, "physical.energyConsumptionWhPerMinute", 30f);
        S(so, "physical.endurance", "35-50 min");
        F(so, "physical.payloadCapacityKg", 0.8f);
        S(so, "physical.navigationSystem", "GPS + Targeting AI");
        F(so, "physical.structuralIntegrityHp", 90f);
        F(so, "physical.armorRating", 0.10f);
        S(so, "physical.heatSignature", "Low");
        S(so, "physical.noiseSignature", "Very Low");
        F(so, "physical.topSpeedMetersPerSecond", 22f);
        S(so, "physical.topSpeedDescription", "22 m/s");
        S(so, "physical.acceleration", "High");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.climbRate", "Medium");
        S(so, "physical.stabilityRating", "High");
        Strings(so, "physical.modules", "Precision marker beam");

        // Autonomy
        S(so, "autonomy.behaviorProfile",
            "Target Identification / Recon Support");
        F(so, "autonomy.aggression", 0.1f);
        F(so, "autonomy.caution", 0.85f);
        F(so, "autonomy.persistence", 0.7f);
        F(so, "autonomy.flankingLikelihood", 0.1f);
        F(so, "autonomy.retreatThreshold", 0.85f);
        F(so, "autonomy.riskTolerance", 0.25f);
        S(so, "autonomy.targetPriority",
            "High-value targets / moving targets");
        S(so, "autonomy.formationDiscipline", "Medium");
        S(so, "autonomy.retreatBehaviorDescription", "Early");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1600f);
        F(so, "sensors.thermalRangeMeters", 600f);
        S(so, "sensors.visualRangeDescription", "1600 m");
        S(so, "sensors.thermalRangeDescription", "600 m");
        S(so, "sensors.acousticRange", "Medium");
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
        F(so, "combat.firepowerRating", 1f);
        F(so, "combat.suppressionValue", 1f);
        I(so, "combat.threatLevel", 2);
        S(so, "combat.firepowerDescription", "Low (marker beam only)");
        S(so, "combat.accuracy", "High");
        S(so, "combat.stabilization", "High");
        S(so, "combat.weaponMounts", "1 (marker beam)");
        S(so, "combat.weaponTypes", "Target marker beam");
        S(so, "combat.trackingStrength", "High");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Low");
        S(so, "combat.engagementDistance", "200-400 m");
        S(so, "combat.preferredTerrain", "Recon corridors");
        S(so, "combat.preferredTargets", "High-value targets");
        S(so, "combat.vulnerabilityProfile", "Ballistic / weather");

        // Squad and command integration
        S(so, "squad.squadRole", "Target Spotting");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Recon spacing");
        S(so, "squad.intentInterpretationRules",
            "Maintain visibility, avoid conflict");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "Minimal");
        S(so, "squad.reinforcementBehavior",
            "Supports recon and command squads");
        S(so, "squad.parentCommandUnit", "Overseer / Insight");
        S(so, "squad.networkDependencies", "Recon Grid");
        Strings(so, "squad.subordinateUnits");
        Strings(so, "squad.relayNodes");

        // VR operator
        S(so, "vrOperator.operatorSeatType", "Drone View");
        S(so, "vrOperator.operatorHud", "Targeting Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full drone flight");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.abilityControls", "Mark Target, Scan, Stabilize");
        S(so, "vrOperator.weaponControls", "Marker beam");
        S(so, "vrOperator.stabilizationControls", "Auto-hover");

        Strings(so, "vrOperator.operatorAbilities",
            "Precision Target Mark",
            "Recon Scan",
            "Thermal Search",
            "Stabilize",
            "Relay Target Data");

        // Economy and logistics
        S(so, "economy.buildCost", "70 Metal / 60 Electronics");
        F(so, "economy.buildTimeSeconds", 25f);
        S(so, "economy.factoryType", "Drone Bay");
        S(so, "economy.techRequirements", "Recon Systems I");
        S(so, "economy.maintenanceCost", "Low");
        F(so, "economy.repairTimeSeconds", 8f);
        S(so, "economy.supplyConsumption", "Low");
        S(so, "economy.operationalCost", "Minimal");
        S(so, "economy.deploymentRequirements", "Standard drone pad");
        S(so, "economy.transportCompatibility", "All aerial racks");
        S(so, "economy.launchMethod", "Vertical takeoff");

        // Build requirements
        Requirements(so, "buildRequirements.commonComponents",
            ("Light Alloy Plates", 8),
            ("Composite Frame Rods", 6),
            ("Micro-Servo Actuators", 3),
            ("Stabilizer Fin", 1),
            ("Marker Beam Housing", 1));

        Requirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 8),
            ("Signal Processors", 6),
            ("Drone Control Chips", 2),
            ("Targeting AI Chips", 2));

        Requirements(so, "buildRequirements.power",
            ("Compact Battery Pack", 1),
            ("Power Regulation Node", 1));

        Requirements(so, "buildRequirements.rareTradeable",
            ("Precision Marker Core", 1));

        Strings(so, "buildRequirements.optionalUpgrades",
            "Silent Rotor Kit",
            "Thermal Boost Module",
            "Extended Battery Pack");

        // Do not overwrite unspecified fields, modelPrefab, or storeImage.
        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log(
            "Spotter canonical data saved: " + AssetPath +
            ". Existing model prefab and store image were preserved.");
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("Spotter: serialized property not found: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else
            Debug.LogWarning("Spotter: expected string property: " + path);
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
            Debug.LogWarning("Spotter: expected numeric property: " + path);
    }

    private static void I(SerializedObject so, string path, int value)
    {
        SerializedProperty p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else if (p != null)
            Debug.LogWarning("Spotter: expected integer property: " + path);
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else if (p != null)
            Debug.LogWarning("Spotter: expected boolean property: " + path);
    }

    private static void EnumChoice(
        SerializedObject so, string path, string requested)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning(
                "Spotter: category is not an enum; left unchanged.");
            return;
        }

        for (int i = 0; i < p.enumNames.Length; i++)
        {
            if (string.Equals(p.enumNames[i], requested,
                StringComparison.OrdinalIgnoreCase))
            {
                p.enumValueIndex = i;
                return;
            }
        }

        Debug.LogWarning(
            "Spotter: category enum option '" + requested +
            "' not found; left unchanged.");
    }

    private static void Strings(
        SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("Spotter: expected string array: " + path);
            return;
        }

        p.arraySize = values.Length;

        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType != SerializedPropertyType.String)
            {
                Debug.LogWarning(
                    "Spotter: array element is not a string: " + path);
                return;
            }
            element.stringValue = values[i];
        }
    }

    private static void Requirements(
        SerializedObject so,
        string path,
        params (string component, int quantity)[] items)
    {
        SerializedProperty array = P(so, path);
        if (array == null) return;

        if (!array.isArray || array.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("Spotter: expected requirement array: " + path);
            return;
        }

        array.arraySize = items.Length;

        for (int i = 0; i < items.Length; i++)
        {
            SerializedProperty element = array.GetArrayElementAtIndex(i);
            SerializedProperty component =
                element.FindPropertyRelative("component");
            SerializedProperty quantity =
                element.FindPropertyRelative("quantity");

            if (component == null || quantity == null)
            {
                Debug.LogWarning(
                    "Spotter: requirement schema mismatch in " + path);
                continue;
            }

            component.stringValue = items[i].component;
            quantity.intValue = items[i].quantity;
        }
    }
}

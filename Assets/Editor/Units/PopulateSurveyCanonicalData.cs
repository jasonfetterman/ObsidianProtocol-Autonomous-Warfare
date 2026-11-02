using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateSurveyCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/Survey/Survey.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Survey Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError("Survey asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        S(so, "unitId", "obs-air-survey");
        S(so, "displayName", "Survey");
        S(so, "role", "Wide-Area Recon / Terrain Scanning");
        S(so, "tier", "Recon Support");
        S(so, "status", "Active");
        EnumChoice(so, "category", "Air");

        // Physical and technical
        F(so, "physical.lengthMeters", 2.0f);
        F(so, "physical.widthMeters", 1.5f);
        F(so, "physical.heightMeters", 0.8f);
        F(so, "physical.massKg", 14f);
        S(so, "physical.powerSource", "Mid-capacity battery");
        F(so, "physical.energyCapacityWh", 1600f);
        F(so, "physical.energyConsumptionWhPerMinute", 32f);
        S(so, "physical.endurance", "40-60 min");
        F(so, "physical.payloadCapacityKg", 1.0f);
        S(so, "physical.navigationSystem", "GPS + Survey AI");
        F(so, "physical.structuralIntegrityHp", 100f);
        F(so, "physical.armorRating", 0.12f);
        S(so, "physical.heatSignature", "Low");
        S(so, "physical.noiseSignature", "Low");
        F(so, "physical.topSpeedMetersPerSecond", 20f);
        S(so, "physical.topSpeedDescription", "20 m/s");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.climbRate", "Medium");
        S(so, "physical.stabilityRating", "High");
        Strings(so, "physical.modules",
            "Wide-area optical + terrain scan suite");

        // Autonomy and AI behavior
        S(so, "autonomy.behaviorProfile",
            "Wide-Area Recon / Terrain Survey");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.85f);
        F(so, "autonomy.persistence", 0.8f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.9f);
        F(so, "autonomy.riskTolerance", 0.25f);
        S(so, "autonomy.targetPriority", "Terrain features / movement");
        S(so, "autonomy.formationDiscipline", "Medium");
        S(so, "autonomy.retreatBehaviorDescription", "Early");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1800f);
        F(so, "sensors.thermalRangeMeters", 700f);
        S(so, "sensors.visualRangeDescription", "1800 m");
        S(so, "sensors.thermalRangeDescription", "700 m");
        S(so, "sensors.acousticRange", "Low");
        S(so, "sensors.radarRange", "None");
        S(so, "sensors.lidarRange", "Medium");
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
        I(so, "combat.threatLevel", 2);
        S(so, "combat.firepowerDescription", "0");
        S(so, "combat.accuracy", "N/A");
        S(so, "combat.stabilization", "High");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        S(so, "combat.trackingStrength", "Medium");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Low");
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "Open air / recon corridors");
        S(so, "combat.preferredTargets", "None");
        S(so, "combat.vulnerabilityProfile", "Ballistic / weather");

        // Squad and command integration
        S(so, "squad.squadRole", "Wide-Area Recon");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Broad recon spacing");
        S(so, "squad.intentInterpretationRules",
            "Maintain visibility, avoid conflict");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior",
            "Supports recon and command squads");
        S(so, "squad.parentCommandUnit", "Overseer / Insight");
        S(so, "squad.networkDependencies", "Recon Grid");
        Strings(so, "squad.subordinateUnits");
        Strings(so, "squad.relayNodes");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Drone View");
        S(so, "vrOperator.operatorHud", "Survey Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full drone flight");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.abilityControls", "Scan, Mark Terrain, Stabilize");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls", "Auto-hover");

        Strings(so, "vrOperator.operatorAbilities",
            "Wide-Area Scan",
            "Terrain Mark",
            "Thermal Search",
            "LiDAR Sweep",
            "Relay Recon Data");

        // Economy and logistics
        S(so, "economy.buildCost", "90 Metal / 80 Electronics");
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

        // Build requirements
        Requirements(so, "buildRequirements.commonComponents",
            ("Light Alloy Plates", 10),
            ("Composite Frame Rods", 6),
            ("Micro-Servo Actuators", 4),
            ("Stabilizer Fins", 2),
            ("Survey Sensor Housing", 1));

        Requirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 10),
            ("Signal Processors", 8),
            ("Drone Control Chips", 3),
            ("Survey AI Chips", 2));

        Requirements(so, "buildRequirements.power",
            ("Mid-Capacity Battery Pack", 1),
            ("Power Regulation Nodes", 2));

        Requirements(so, "buildRequirements.rareTradeable",
            ("Wide-Area Scan Core", 1));

        Strings(so, "buildRequirements.optionalUpgrades",
            "Silent Rotor Kit",
            "Extended Battery Pack",
            "High-Density Scan Grid");

        // Leave unspecified fields unchanged.
        // modelPrefab and storeImage are intentionally preserved.
        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log(
            "Survey canonical data saved: " + AssetPath +
            ". Existing model prefab and store image were preserved.");
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("Survey: serialized property not found: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else
            Debug.LogWarning("Survey: expected string property: " + path);
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
            Debug.LogWarning("Survey: expected numeric property: " + path);
    }

    private static void I(SerializedObject so, string path, int value)
    {
        SerializedProperty p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else if (p != null)
            Debug.LogWarning("Survey: expected integer property: " + path);
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else if (p != null)
            Debug.LogWarning("Survey: expected boolean property: " + path);
    }

    private static void EnumChoice(
        SerializedObject so, string path, string requested)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning(
                "Survey: category is not an enum; left unchanged.");
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
            "Survey: category enum option '" + requested +
            "' not found; left unchanged.");
    }

    private static void Strings(
        SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("Survey: expected string array: " + path);
            return;
        }

        p.arraySize = values.Length;

        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType != SerializedPropertyType.String)
            {
                Debug.LogWarning(
                    "Survey: array element is not a string: " + path);
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
            Debug.LogWarning("Survey: expected requirement array: " + path);
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
                    "Survey: requirement schema mismatch in " + path);
                continue;
            }

            component.stringValue = items[i].component;
            quantity.intValue = items[i].quantity;
        }
    }
}

using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateTerrainCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/Terrain/Terrain.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Terrain Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError("Terrain asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        S(so, "unitId", "obs-air-terrain");
        S(so, "displayName", "Terrain");
        S(so, "role", "Ground-Mapping / Elevation Analysis");
        S(so, "tier", "Recon Support");
        S(so, "status", "Active");
        EnumChoice(so, "category", "Air");

        // Physical and technical
        F(so, "physical.lengthMeters", 2.2f);
        F(so, "physical.widthMeters", 1.6f);
        F(so, "physical.heightMeters", 0.8f);
        F(so, "physical.massKg", 15f);
        S(so, "physical.powerSource", "Mid-capacity battery");
        F(so, "physical.energyCapacityWh", 1700f);
        F(so, "physical.energyConsumptionWhPerMinute", 34f);
        S(so, "physical.endurance", "40-55 min");
        F(so, "physical.payloadCapacityKg", 1.2f);
        S(so, "physical.navigationSystem", "GPS + Terrain AI");
        F(so, "physical.structuralIntegrityHp", 105f);
        F(so, "physical.armorRating", 0.12f);
        S(so, "physical.heatSignature", "Low");
        S(so, "physical.noiseSignature", "Low");
        F(so, "physical.topSpeedMetersPerSecond", 19f);
        S(so, "physical.topSpeedDescription", "19 m/s");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.climbRate", "Medium");
        S(so, "physical.stabilityRating", "High");
        Strings(so, "physical.modules",
            "Elevation scan suite + ground-mesh generator");

        // Autonomy and AI
        S(so, "autonomy.behaviorProfile",
            "Terrain Mapping / Recon Support");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.85f);
        F(so, "autonomy.persistence", 0.85f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.9f);
        F(so, "autonomy.riskTolerance", 0.25f);
        S(so, "autonomy.targetPriority",
            "Elevation features / structural terrain");
        S(so, "autonomy.formationDiscipline", "Medium");
        S(so, "autonomy.retreatBehaviorDescription", "Early");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1600f);
        F(so, "sensors.thermalRangeMeters", 500f);
        S(so, "sensors.visualRangeDescription", "1600 m");
        S(so, "sensors.thermalRangeDescription", "500 m");
        S(so, "sensors.acousticRange", "Low");
        S(so, "sensors.radarRange", "None");
        S(so, "sensors.lidarRange", "Medium (elevation-optimized)");
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
        S(so, "combat.preferredTerrain", "Open air / elevation corridors");
        S(so, "combat.preferredTargets", "None");
        S(so, "combat.vulnerabilityProfile", "Ballistic / weather");

        // Squad and command integration
        S(so, "squad.squadRole", "Terrain Mapping");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Broad recon spacing");
        S(so, "squad.intentInterpretationRules",
            "Maintain elevation visibility");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior",
            "Supports recon, survey, and command squads");
        S(so, "squad.parentCommandUnit", "Overseer / Insight");
        S(so, "squad.networkDependencies", "Recon Grid");
        Strings(so, "squad.subordinateUnits");
        Strings(so, "squad.relayNodes");

        // VR operator
        S(so, "vrOperator.operatorSeatType", "Drone View");
        S(so, "vrOperator.operatorHud", "Elevation Mapping Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full drone flight");
        S(so, "vrOperator.sensorControls", "LiDAR / thermal / zoom");
        S(so, "vrOperator.abilityControls",
            "Elevation Scan, Terrain Mesh, Stabilize");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls", "Auto-hover");

        Strings(so, "vrOperator.operatorAbilities",
            "Elevation Scan",
            "Terrain Mesh Generation",
            "Thermal Search",
            "LiDAR Sweep",
            "Relay Recon Data");

        // Economy and logistics
        S(so, "economy.buildCost", "95 Metal / 85 Electronics");
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
            ("Elevation Sensor Housing", 1));

        Requirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 10),
            ("Signal Processors", 8),
            ("Drone Control Chips", 3),
            ("Terrain AI Chips", 2));

        Requirements(so, "buildRequirements.power",
            ("Mid-Capacity Battery Pack", 1),
            ("Power Regulation Nodes", 2));

        Requirements(so, "buildRequirements.rareTradeable",
            ("Elevation Scan Core", 1));

        Strings(so, "buildRequirements.optionalUpgrades",
            "Silent Rotor Kit",
            "Extended Battery Pack",
            "High-Density Elevation Grid");

        // Leave unspecified fields unchanged.
        // modelPrefab and storeImage are intentionally preserved.
        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log(
            "Terrain canonical data saved: " + AssetPath +
            ". Existing model prefab and store image were preserved.");
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("Terrain: serialized property not found: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else
            Debug.LogWarning("Terrain: expected string property: " + path);
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
            Debug.LogWarning("Terrain: expected numeric property: " + path);
    }

    private static void I(SerializedObject so, string path, int value)
    {
        SerializedProperty p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else if (p != null)
            Debug.LogWarning("Terrain: expected integer property: " + path);
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else if (p != null)
            Debug.LogWarning("Terrain: expected boolean property: " + path);
    }

    private static void EnumChoice(
        SerializedObject so, string path, string requested)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning(
                "Terrain: category is not an enum; left unchanged.");
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
            "Terrain: category enum option '" + requested +
            "' not found; left unchanged.");
    }

    private static void Strings(
        SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("Terrain: expected string array: " + path);
            return;
        }

        p.arraySize = values.Length;

        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType != SerializedPropertyType.String)
            {
                Debug.LogWarning(
                    "Terrain: array element is not a string: " + path);
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
            Debug.LogWarning("Terrain: expected requirement array: " + path);
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
                    "Terrain: requirement schema mismatch in " + path);
                continue;
            }

            component.stringValue = items[i].component;
            quantity.intValue = items[i].quantity;
        }
    }
}

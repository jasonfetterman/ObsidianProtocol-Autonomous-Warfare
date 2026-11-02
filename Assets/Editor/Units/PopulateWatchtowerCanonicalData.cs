using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateWatchtowerCanonicalData
{
    private const string AssetPath = "Assets/Game/Units/Database/Air/Watchtower/Watchtower.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Watchtower Canonical Data")]
    public static void Populate()
    {
        var target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Watchtower] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        var so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-air-watchtower");
        S(so, "displayName", "Watchtower");
        E(so, "category", "Air");
        S(so, "role", "High-Altitude Overwatch / Strategic Visibility");
        S(so, "tier", "Advanced Recon");
        S(so, "status", "Active");

        F(so, "physical.lengthMeters", 3.8f);
        F(so, "physical.widthMeters", 2.9f);
        F(so, "physical.heightMeters", 1.3f);
        F(so, "physical.massKg", 36f);
        S(so, "physical.powerSource", "Long-endurance battery");
        F(so, "physical.energyCapacityWh", 4000f);
        F(so, "physical.energyConsumptionWhPerMinute", 55f);
        S(so, "physical.endurance", "70-100 min");
        F(so, "physical.payloadCapacityKg", 2.2f);
        F(so, "physical.structuralIntegrityHp", 200f);
        F(so, "physical.armorRating", 0.34f);
        S(so, "physical.heatSignature", "Low");
        S(so, "physical.noiseSignature", "Low");
        F(so, "physical.topSpeedMetersPerSecond", 20f);
        S(so, "physical.topSpeedDescription", "20 m/s");
        S(so, "physical.navigationSystem", "GPS + Overwatch AI");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Moderate");
        S(so, "physical.climbRate", "Extremely High");
        S(so, "physical.stabilityRating", "Maximum");
        Strings(so, "physical.modules",
            "Long-range optics",
            "Atmospheric stabilization suite");

        S(so, "autonomy.behaviorProfile", "High-Altitude Overwatch / Persistent Visibility");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.95f);
        F(so, "autonomy.persistence", 1f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.95f);
        F(so, "autonomy.riskTolerance", 0.1f);
        S(so, "autonomy.targetPriority", "Long-range visibility targets");
        S(so, "autonomy.formationDiscipline", "Very High");
        S(so, "autonomy.retreatBehaviorDescription", "Very Early");

        F(so, "sensors.visualRangeMeters", 3500f);
        F(so, "sensors.thermalRangeMeters", 1500f);
        S(so, "sensors.visualRangeDescription", "3500 m");
        S(so, "sensors.thermalRangeDescription", "1500 m");
        S(so, "sensors.acousticRange", "Low");
        S(so, "sensors.radarRange", "Medium");
        S(so, "sensors.lidarRange", "Medium");
        S(so, "sensors.signalDetectionRange", "12 km");

        F(so, "communication.meshRangeKm", 12f);
        S(so, "communication.meshRangeDescription", "12 km");
        F(so, "communication.relayCapacity", 3f);
        S(so, "communication.commandLinkType", "Recon Grid");
        S(so, "communication.networkPriority", "High");

        F(so, "combat.firepowerRating", 0f);
        F(so, "combat.suppressionValue", 0f);
        S(so, "combat.accuracy", "N/A");
        S(so, "combat.stabilization", "Extremely High");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        S(so, "combat.trackingStrength", "High");
        S(so, "combat.disruptionStrength", "Medium");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "High");
        I(so, "combat.threatLevel", 4);
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "High-altitude overwatch lanes");
        S(so, "combat.preferredTargets", "None");
        S(so, "combat.vulnerabilityProfile", "Ballistic / EMP / jamming");

        S(so, "squad.squadRole", "High-Altitude Overwatch");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "High-altitude spacing");
        S(so, "squad.intentInterpretationRules", "Maintain persistent visibility");
        S(so, "squad.retreatBehavior", "Very Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports recon and command squads");
        S(so, "squad.parentCommandUnit", "Insight / Overseer");
        Strings(so, "squad.subordinateUnits", "Horizon", "Skywatch", "Trailblazer");
        Strings(so, "squad.relayNodes", "Relay Node 1", "Relay Node 2", "Relay Node 3");
        S(so, "squad.networkDependencies", "Recon Grid");

        S(so, "vrOperator.operatorSeatType", "Overwatch Drone View");
        S(so, "vrOperator.operatorHud", "Strategic Visibility Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "High-altitude flight");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / radar / zoom");
        S(so, "vrOperator.abilityControls", "Strategic Scan, Mark Target, Stabilize");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls", "High-altitude auto-hover");
        Strings(so, "vrOperator.operatorAbilities",
            "Strategic Overwatch",
            "Long-Range Scan",
            "Predictive Movement Analysis",
            "Relay Recon Data");

        S(so, "economy.buildCost", "300 Metal / 300 Electronics");
        I(so, "economy.creditCost", 600);
        F(so, "economy.buildTimeSeconds", 90f);
        S(so, "economy.factoryType", "Drone Bay");
        S(so, "economy.techRequirements", "Recon Systems III");
        S(so, "economy.maintenanceCost", "Medium");
        F(so, "economy.repairTimeSeconds", 22f);
        S(so, "economy.supplyConsumption", "Medium");
        S(so, "economy.operationalCost", "Moderate");
        S(so, "economy.deploymentRequirements", "High-altitude drone pad");
        S(so, "economy.transportCompatibility", "All aerial racks");
        S(so, "economy.launchMethod", "Vertical takeoff");
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);

        Requirements(so, "buildRequirements.commonComponents",
            new string[] { "Medium Alloy Plates", "Composite Frame Rods", "Micro-Servo Actuators", "Stabilizer Fins", "Overwatch Sensor Housing" },
            new int[]    { 20, 14, 6, 3, 1 });

        Requirements(so, "buildRequirements.electronics",
            new string[] { "Circuit Bundles", "Signal Processors", "Drone Control Chips", "Command-Recon AI Chips" },
            new int[]    { 22, 16, 5, 4 });

        Requirements(so, "buildRequirements.power",
            new string[] { "Long-Endurance Battery Pack", "Power Regulation Nodes" },
            new int[]    { 1, 3 });

        Requirements(so, "buildRequirements.rareTradeable",
            new string[] { "Long-Range Optics Core", "Atmospheric Stability Processor" },
            new int[]    { 1, 1 });

        Strings(so, "buildRequirements.optionalUpgrades",
            "Extended Altitude Module",
            "Anti-Jamming Shielding",
            "Silent Rotor Kit",
            "Ultra-Long-Range Sensor Grid");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("[Watchtower] Canonical data population finished. Review warnings and verify the asset before marking complete.");
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        var p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Watchtower] Serialized property not found: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        var p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else if (p != null)
            Debug.LogWarning("[Watchtower] Expected string property: " + path);
    }

    private static void F(SerializedObject so, string path, float value)
    {
        var p = P(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Watchtower] Expected numeric property: " + path);
    }

    private static void I(SerializedObject so, string path, int value)
    {
        var p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else if (p != null)
            Debug.LogWarning("[Watchtower] Expected integer property: " + path);
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        var p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else if (p != null)
            Debug.LogWarning("[Watchtower] Expected boolean property: " + path);
    }

    private static void E(SerializedObject so, string path, string value)
    {
        var p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Watchtower] Expected enum property: " + path);
            return;
        }

        for (int i = 0; i < p.enumNames.Length; i++)
        {
            if (string.Equals(p.enumNames[i], value, StringComparison.OrdinalIgnoreCase))
            {
                p.enumValueIndex = i;
                return;
            }
        }

        Debug.LogWarning("[Watchtower] Enum value '" + value + "' not found for " + path);
    }

    private static void Strings(SerializedObject so, string path, params string[] values)
    {
        var p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Watchtower] Expected list property: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            var element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Watchtower] Expected string list element: " + path);
        }
    }

    private static void Requirements(SerializedObject so, string path, string[] names, int[] quantities)
    {
        var p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Watchtower] Expected requirement list: " + path);
            return;
        }

        p.arraySize = names.Length;
        for (int i = 0; i < names.Length; i++)
        {
            var element = p.GetArrayElementAtIndex(i);
            var component = element.FindPropertyRelative("component");
            var quantity = element.FindPropertyRelative("quantity");

            if (component == null || quantity == null)
            {
                Debug.LogWarning("[Watchtower] Requirement element schema mismatch: " + path);
                continue;
            }

            component.stringValue = names[i];
            quantity.intValue = quantities[i];
        }
    }
}

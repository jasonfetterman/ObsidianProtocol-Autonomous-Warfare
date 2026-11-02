using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateTrackerCanonicalData
{
    private const string AssetPath = "Assets/Game/Units/Database/Air/Tracker/Tracker.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Tracker Canonical Data")]
    public static void Populate()
    {
        var target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Tracker] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        var so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-air-tracker");
        S(so, "displayName", "Tracker");
        E(so, "category", "Air");
        S(so, "role", "Mobile Target-Tracking Recon");
        S(so, "tier", "Recon");
        S(so, "status", "Active");

        S(so, "physical.powerSource", "Mid-capacity battery");
        F(so, "physical.lengthMeters", 2.7f);
        F(so, "physical.widthMeters", 2.0f);
        F(so, "physical.heightMeters", 0.9f);
        F(so, "physical.massKg", 22f);
        F(so, "physical.energyCapacityWh", 2400f);
        F(so, "physical.energyConsumptionWhPerMinute", 40f);
        S(so, "physical.endurance", "45-65 min");
        F(so, "physical.payloadCapacityKg", 1.2f);
        F(so, "physical.structuralIntegrityHp", 130f);
        F(so, "physical.armorRating", 0.20f);
        S(so, "physical.navigationSystem", "GPS + Target-Tracking AI");
        S(so, "physical.heatSignature", "Low");
        S(so, "physical.noiseSignature", "Low");
        F(so, "physical.topSpeedMetersPerSecond", 24f);
        S(so, "physical.topSpeedDescription", "24 m/s");
        S(so, "physical.acceleration", "High");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.climbRate", "Medium");
        S(so, "physical.stabilityRating", "High");
        Strings(so, "physical.modules",
            "Motion-tracking optics", "Pursuit stabilization");

        S(so, "autonomy.behaviorProfile", "Mobile Tracking / Pursuit Recon");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.8f);
        F(so, "autonomy.persistence", 1f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.85f);
        F(so, "autonomy.riskTolerance", 0.5f);
        S(so, "autonomy.retreatBehaviorDescription", "Early");
        S(so, "autonomy.targetPriority", "Moving targets");
        S(so, "autonomy.formationDiscipline", "Medium");

        F(so, "sensors.visualRangeMeters", 2200f);
        F(so, "sensors.thermalRangeMeters", 800f);
        S(so, "sensors.visualRangeDescription", "2200 m");
        S(so, "sensors.thermalRangeDescription", "800 m");
        S(so, "sensors.acousticRange", "Medium");
        S(so, "sensors.radarRange", "None");
        S(so, "sensors.lidarRange", "Short");
        S(so, "sensors.signalDetectionRange", "Medium");

        F(so, "communication.meshRangeKm", 6f);
        S(so, "communication.meshRangeDescription", "6 km");
        F(so, "communication.relayCapacity", 0f);
        S(so, "communication.commandLinkType", "Recon Grid");
        S(so, "communication.networkPriority", "Medium");

        F(so, "combat.firepowerRating", 0f);
        F(so, "combat.suppressionValue", 0f);
        S(so, "combat.accuracy", "N/A");
        S(so, "combat.stabilization", "High");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        S(so, "combat.trackingStrength", "Very High");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Low");
        I(so, "combat.threatLevel", 2);
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "Open lanes / pursuit corridors");
        S(so, "combat.preferredTargets", "Moving units");
        S(so, "combat.vulnerabilityProfile", "Ballistic / weather");

        S(so, "squad.squadRole", "Mobile Tracking Recon");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Pursuit spacing");
        S(so, "squad.intentInterpretationRules", "Maintain lock on moving targets");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports recon and command squads");
        S(so, "squad.parentCommandUnit", "Insight / Overseer");
        Strings(so, "squad.subordinateUnits", "Spotter", "Scout Eye", "Survey");
        Strings(so, "squad.relayNodes");
        S(so, "squad.networkDependencies", "Recon Grid");

        S(so, "vrOperator.operatorSeatType", "Tracking Drone View");
        S(so, "vrOperator.operatorHud", "Motion-Tracking Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full drone flight");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.abilityControls", "Acquire Lock, Mark Target, Stabilize");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls", "Auto-hover");
        Strings(so, "vrOperator.operatorAbilities",
            "Motion Lock", "Pursuit Tracking", "Mark Moving Target", "Relay Recon Data");

        S(so, "economy.buildCost", "140 Metal / 100 Electronics");
        I(so, "economy.creditCost", 240);
        F(so, "economy.buildTimeSeconds", 45f);
        S(so, "economy.factoryType", "Drone Bay");
        S(so, "economy.techRequirements", "Recon Systems II");
        S(so, "economy.maintenanceCost", "Low");
        F(so, "economy.repairTimeSeconds", 13f);
        S(so, "economy.supplyConsumption", "Low");
        S(so, "economy.operationalCost", "Minimal");
        S(so, "economy.deploymentRequirements", "Standard drone pad");
        S(so, "economy.transportCompatibility", "All aerial racks");
        S(so, "economy.launchMethod", "Vertical takeoff");
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);

        Requirements(so, "buildRequirements.commonComponents",
            new string[] { "Light Alloy Plates", "Composite Frame Rods", "Micro-Servo Actuators", "Stabilizer Fins", "Tracking Sensor Housing" },
            new int[]    { 12, 9, 4, 2, 1 });
        Requirements(so, "buildRequirements.electronics",
            new string[] { "Circuit Bundles", "Signal Processors", "Drone Control Chips", "Recon AI Chips" },
            new int[]    { 12, 9, 3, 2 });
        Requirements(so, "buildRequirements.power",
            new string[] { "Mid-Capacity Battery Pack", "Power Regulation Nodes" },
            new int[]    { 1, 2 });
        Requirements(so, "buildRequirements.rareTradeable",
            new string[] { "Motion-Tracking Optics Core" },
            new int[]    { 1 });
        Strings(so, "buildRequirements.optionalUpgrades",
            "Extended Battery Pack", "Silent Rotor Kit", "High-Density Recon Grid");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        Debug.Log("[Tracker] Canonical data populated. Verify any missing-property warnings above before marking complete.");
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        var p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Tracker] Serialized property not found: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        var p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.String) p.stringValue = value;
        else if (p != null) Debug.LogWarning("[Tracker] Expected string property: " + path);
    }

    private static void F(SerializedObject so, string path, float value)
    {
        var p = P(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Tracker] Expected numeric property: " + path);
    }

    private static void I(SerializedObject so, string path, int value)
    {
        var p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        else if (p != null) Debug.LogWarning("[Tracker] Expected integer property: " + path);
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        var p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean) p.boolValue = value;
        else if (p != null) Debug.LogWarning("[Tracker] Expected boolean property: " + path);
    }

    private static void E(SerializedObject so, string path, string value)
    {
        var p = P(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Tracker] Expected enum property: " + path);
            return;
        }
        for (int i = 0; i < p.enumNames.Length; i++)
            if (string.Equals(p.enumNames[i], value, StringComparison.OrdinalIgnoreCase))
            {
                p.enumValueIndex = i;
                return;
            }
        Debug.LogWarning("[Tracker] Enum value '" + value + "' not found for " + path);
    }

    private static void Strings(SerializedObject so, string path, params string[] values)
    {
        var p = P(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Tracker] Expected list property: " + path);
            return;
        }
        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            var element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String) element.stringValue = values[i];
            else Debug.LogWarning("[Tracker] Expected string list element: " + path);
        }
    }

    private static void Requirements(SerializedObject so, string path, string[] names, int[] quantities)
    {
        var p = P(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Tracker] Expected requirement list: " + path);
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
                Debug.LogWarning("[Tracker] Requirement element schema mismatch: " + path);
                continue;
            }
            component.stringValue = names[i];
            quantity.intValue = quantities[i];
        }
    }
}

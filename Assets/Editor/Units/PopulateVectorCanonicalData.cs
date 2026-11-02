using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateVectorCanonicalData
{
    private const string AssetPath = "Assets/Game/Units/Database/Air/Vector/Vector.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Vector Canonical Data")]
    public static void Populate()
    {
        var target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Vector] UnitDefinition asset not found: " + AssetPath);
            return;
        }

        var so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-air-vector");
        S(so, "displayName", "Vector");
        E(so, "category", "Air");
        S(so, "role", "High-Speed Recon / Rapid Response Scouting");
        S(so, "tier", "Advanced Recon");
        S(so, "status", "Active");

        F(so, "physical.lengthMeters", 3.1f);
        F(so, "physical.widthMeters", 2.3f);
        F(so, "physical.heightMeters", 1.0f);
        F(so, "physical.massKg", 28f);
        S(so, "physical.powerSource", "High-output battery");
        F(so, "physical.energyCapacityWh", 3000f);
        F(so, "physical.energyConsumptionWhPerMinute", 58f);
        S(so, "physical.endurance", "40-55 min");
        F(so, "physical.payloadCapacityKg", 1.6f);
        F(so, "physical.structuralIntegrityHp", 160f);
        F(so, "physical.armorRating", 0.26f);
        S(so, "physical.heatSignature", "Medium");
        S(so, "physical.noiseSignature", "Medium");
        F(so, "physical.topSpeedMetersPerSecond", 32f);
        S(so, "physical.topSpeedDescription", "32 m/s");
        S(so, "physical.navigationSystem", "GPS + Rapid-Response AI");
        S(so, "physical.acceleration", "Very High");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.climbRate", "High");
        S(so, "physical.stabilityRating", "High");
        Strings(so, "physical.modules",
            "High-speed optics", "Stabilization fins");

        S(so, "autonomy.behaviorProfile", "Rapid Recon / High-Speed Sweep");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.75f);
        F(so, "autonomy.persistence", 0.85f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.8f);
        F(so, "autonomy.riskTolerance", 0.5f);
        S(so, "autonomy.targetPriority", "Fast-moving or time-critical targets");
        S(so, "autonomy.formationDiscipline", "Medium");
        S(so, "autonomy.retreatBehaviorDescription", "Early");

        F(so, "sensors.visualRangeMeters", 2600f);
        F(so, "sensors.thermalRangeMeters", 900f);
        S(so, "sensors.visualRangeDescription", "2600 m");
        S(so, "sensors.thermalRangeDescription", "900 m");
        S(so, "sensors.acousticRange", "Medium");
        S(so, "sensors.radarRange", "Short");
        S(so, "sensors.lidarRange", "Short");
        S(so, "sensors.signalDetectionRange", "Medium");

        F(so, "communication.meshRangeKm", 8f);
        S(so, "communication.meshRangeDescription", "8 km");
        F(so, "communication.relayCapacity", 1f);
        S(so, "communication.commandLinkType", "Recon Grid");
        S(so, "communication.networkPriority", "Medium");

        F(so, "combat.firepowerRating", 0f);
        F(so, "combat.suppressionValue", 0f);
        S(so, "combat.accuracy", "N/A");
        S(so, "combat.stabilization", "High");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        S(so, "combat.trackingStrength", "High");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Medium");
        I(so, "combat.threatLevel", 3);
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "Open lanes / rapid-response corridors");
        S(so, "combat.preferredTargets", "Moving / time-sensitive targets");
        S(so, "combat.vulnerabilityProfile", "Ballistic / weather");

        S(so, "squad.squadRole", "High-Speed Recon");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Rapid-response spacing");
        S(so, "squad.intentInterpretationRules", "Reach target zones quickly");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports recon and command squads");
        S(so, "squad.parentCommandUnit", "Insight / Overseer");
        Strings(so, "squad.subordinateUnits", "Tracker", "Spotter", "Scout Eye");
        Strings(so, "squad.relayNodes", "Relay Node 1");
        S(so, "squad.networkDependencies", "Recon Grid");

        S(so, "vrOperator.operatorSeatType", "High-Speed Drone View");
        S(so, "vrOperator.operatorHud", "Velocity-Tracking Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "High-speed flight");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / radar / zoom");
        S(so, "vrOperator.abilityControls", "Rapid Scan, Mark Target, Stabilize");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls", "Auto-hover");
        Strings(so, "vrOperator.operatorAbilities",
            "Rapid Recon Sweep",
            "Velocity Tracking",
            "Mark Fast Target",
            "Relay Recon Data");

        S(so, "economy.buildCost", "180 Metal / 150 Electronics");
        I(so, "economy.creditCost", 330);
        F(so, "economy.buildTimeSeconds", 60f);
        S(so, "economy.factoryType", "Drone Bay");
        S(so, "economy.techRequirements", "Recon Systems III");
        S(so, "economy.maintenanceCost", "Medium");
        F(so, "economy.repairTimeSeconds", 16f);
        S(so, "economy.supplyConsumption", "Medium");
        S(so, "economy.operationalCost", "Moderate");
        S(so, "economy.deploymentRequirements", "High-speed drone pad");
        S(so, "economy.transportCompatibility", "All aerial racks");
        S(so, "economy.launchMethod", "Vertical takeoff");
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);

        Requirements(so, "buildRequirements.commonComponents",
            new string[] { "Light Alloy Plates", "Composite Frame Rods", "Micro-Servo Actuators", "Stabilizer Fins", "High-Speed Sensor Housing" },
            new int[]    { 16, 12, 5, 2, 1 });

        Requirements(so, "buildRequirements.electronics",
            new string[] { "Circuit Bundles", "Signal Processors", "Drone Control Chips", "Recon AI Chips" },
            new int[]    { 16, 12, 4, 3 });

        Requirements(so, "buildRequirements.power",
            new string[] { "High-Output Battery Pack", "Power Regulation Nodes" },
            new int[]    { 1, 3 });

        Requirements(so, "buildRequirements.rareTradeable",
            new string[] { "High-Speed Optics Core" },
            new int[]    { 1 });

        Strings(so, "buildRequirements.optionalUpgrades",
            "Extended Battery Pack",
            "Silent Rotor Kit",
            "High-Density Recon Grid",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("[Vector] Canonical data population finished. Review warnings and verify the asset before marking complete.");
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        var p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Vector] Serialized property not found: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        var p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else if (p != null)
            Debug.LogWarning("[Vector] Expected string property: " + path);
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
            Debug.LogWarning("[Vector] Expected numeric property: " + path);
    }

    private static void I(SerializedObject so, string path, int value)
    {
        var p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else if (p != null)
            Debug.LogWarning("[Vector] Expected integer property: " + path);
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        var p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else if (p != null)
            Debug.LogWarning("[Vector] Expected boolean property: " + path);
    }

    private static void E(SerializedObject so, string path, string value)
    {
        var p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Vector] Expected enum property: " + path);
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

        Debug.LogWarning("[Vector] Enum value '" + value + "' not found for " + path);
    }

    private static void Strings(SerializedObject so, string path, params string[] values)
    {
        var p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Vector] Expected list property: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            var element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Vector] Expected string list element: " + path);
        }
    }

    private static void Requirements(SerializedObject so, string path, string[] names, int[] quantities)
    {
        var p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Vector] Expected requirement list: " + path);
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
                Debug.LogWarning("[Vector] Requirement element schema mismatch: " + path);
                continue;
            }

            component.stringValue = names[i];
            quantity.intValue = quantities[i];
        }
    }
}

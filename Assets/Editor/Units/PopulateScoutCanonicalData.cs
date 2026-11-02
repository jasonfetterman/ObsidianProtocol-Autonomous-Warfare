using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateScoutCanonicalData
{
    private const string AssetPath = "Assets/Game/Units/Database/Ground/Scout/Scout.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Scout Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Scout] Unit asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-ground-scout");
        S(so, "displayName", "Scout");
        E(so, "category", "Ground");
        S(so, "role", "Light Recon / Rapid Ground Scouting");
        S(so, "tier", "Recon");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 3.6f);
        F(so, "physical.widthMeters", 1.8f);
        F(so, "physical.heightMeters", 1.6f);
        F(so, "physical.massKg", 780f);
        S(so, "physical.powerSource", "Electric-hybrid");
        F(so, "physical.energyCapacityWh", 4600f);
        F(so, "physical.fuelCapacityLiters", 40f, true);
        S(so, "physical.endurance", "4–7 hours");
        F(so, "physical.payloadCapacityKg", 80f);
        F(so, "physical.structuralIntegrityHp", 600f);
        F(so, "physical.armorRating", 0.28f);
        F(so, "physical.topSpeedMetersPerSecond", 22f);
        S(so, "physical.navigationSystem", "GPS + Recon AI");
        S(so, "physical.heatSignature", "Low");
        S(so, "physical.noiseSignature", "Very Low");
        S(so, "physical.topSpeedDescription", "22 m/s");
        S(so, "physical.acceleration", "High");
        S(so, "physical.turningRadius", "Very Tight");
        S(so, "physical.stabilityRating", "High");
        Strings(so, "physical.modules", "Recon optics", "Silent drive kit");

        // Autonomy and AI behavior
        S(so, "autonomy.behaviorProfile", "Recon / Forward Scouting");
        F(so, "autonomy.aggression", 0.1f);
        F(so, "autonomy.caution", 0.9f);
        F(so, "autonomy.persistence", 0.8f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.85f);
        F(so, "autonomy.riskTolerance", 0.2f);
        S(so, "autonomy.retreatBehaviorDescription", "Early");
        S(so, "autonomy.targetPriority", "Movement / anomalies");
        S(so, "autonomy.formationDiscipline", "Medium");

        // Sensors
        F(so, "sensors.visualRangeMeters", 2000f);
        F(so, "sensors.thermalRangeMeters", 900f);
        S(so, "sensors.visualRangeDescription", "2000 m");
        S(so, "sensors.thermalRangeDescription", "900 m");
        S(so, "sensors.acousticRange", "High");
        S(so, "sensors.radarRange", "Short");
        S(so, "sensors.lidarRange", "Medium");
        S(so, "sensors.signalDetectionRange", "Medium");

        // Communication
        F(so, "communication.meshRangeKm", 6f);
        S(so, "communication.meshRangeDescription", "6 km");
        F(so, "communication.relayCapacity", 0f);
        S(so, "communication.commandLinkType", "Recon Grid");
        S(so, "communication.networkPriority", "High");

        // Combat and threat model
        F(so, "combat.firepowerRating", 0.15f);
        F(so, "combat.suppressionValue", 0.25f);
        S(so, "combat.firepowerDescription", "Very Low");
        S(so, "combat.accuracy", "Medium");
        S(so, "combat.stabilization", "Light turret stabilization");
        S(so, "combat.weaponMounts", "1 (light)");
        S(so, "combat.weaponTypes", "Light turret / recon suppressor");
        S(so, "combat.trackingStrength", "Medium");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Low");
        I(so, "combat.threatLevel", 3);
        S(so, "combat.engagementDistance", "200–400 m");
        S(so, "combat.preferredTerrain", "Open ground / recon lanes");
        S(so, "combat.preferredTargets", "Light infantry");
        S(so, "combat.vulnerabilityProfile", "Ballistic / explosives");

        // Squad and command integration
        S(so, "squad.squadRole", "Recon / Forward Eyes");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Forward spacing");
        S(so, "squad.intentInterpretationRules", "Identify threats early");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports security & recon squads");
        S(so, "squad.parentCommandUnit", "Command Core / Pulse");
        Strings(so, "squad.subordinateUnits");
        Strings(so, "squad.relayNodes");
        S(so, "squad.networkDependencies", "Recon Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Recon Cockpit");
        S(so, "vrOperator.operatorHud", "Recon Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full vehicle drive");
        S(so, "vrOperator.weaponControls", "Light turret");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.stabilizationControls", "Light turret stabilization");
        S(so, "vrOperator.abilityControls", "Mark Target, Scan, Stabilize");
        Strings(so, "vrOperator.operatorAbilities",
            "Recon Scan",
            "Silent Approach",
            "Threat Marking",
            "Relay Recon Data");

        // Economy and logistics
        I(so, "economy.creditCost", 460);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "300 Metal / 160 Electronics");
        F(so, "economy.buildTimeSeconds", 70f);
        S(so, "economy.factoryType", "Vehicle Bay");
        S(so, "economy.techRequirements", "Recon Systems I");
        S(so, "economy.maintenanceCost", "Low");
        F(so, "economy.repairTimeSeconds", 18f);
        S(so, "economy.supplyConsumption", "Low");
        S(so, "economy.operationalCost", "Low");
        S(so, "economy.deploymentRequirements", "Vehicle pad");
        S(so, "economy.transportCompatibility", "Light racks");
        S(so, "economy.launchMethod", "Drive-out");

        // Player collection / build requirements
        Requirements(so, "buildRequirements.commonComponents",
            ("Light Alloy Plates", 24),
            ("Reinforced Frame Rods", 12),
            ("Hydraulic Actuators", 4),
            ("Armor Panels", 2),
            ("Recon Sensor Housing", 1));

        Requirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 14),
            ("Signal Processors", 10),
            ("Vehicle Control Chips", 3),
            ("Recon AI Chips", 2));

        Requirements(so, "buildRequirements.power",
            ("Hybrid Engine Core", 1),
            ("Power Regulation Nodes", 2));

        Requirements(so, "buildRequirements.rareTradeable",
            ("Silent Drive Module", 1));

        Strings(so, "buildRequirements.optionalUpgrades",
            "Extended Battery Pack",
            "Silent Drive Kit",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Scout] Canonical data population finished. Review Console warnings for unsupported fields or enum values. Existing modelPrefab and storeImage were preserved.");
    }

    private static SerializedProperty P(SerializedObject so, string path, bool optional = false)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null && !optional)
            Debug.LogWarning("[Scout] Missing serialized property: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else if (p.propertyType == SerializedPropertyType.Enum)
        {
            int index = Array.IndexOf(p.enumNames, value);
            if (index >= 0) p.enumValueIndex = index;
            else Debug.LogWarning("[Scout] Enum value '" + value + "' unavailable for " + path);
        }
        else
            Debug.LogWarning("[Scout] Expected string/enum at " + path + ", found " + p.propertyType);
    }

    private static void F(SerializedObject so, string path, float value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Scout] Expected numeric field at " + path + ", found " + p.propertyType);
    }

    private static void I(SerializedObject so, string path, int value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else
            Debug.LogWarning("[Scout] Expected integer field at " + path + ", found " + p.propertyType);
    }

    private static void B(SerializedObject so, string path, bool value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Scout] Expected boolean field at " + path + ", found " + p.propertyType);
    }

    private static void E(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Scout] Expected enum at " + path + ", found " + p.propertyType);
            return;
        }

        int index = Array.IndexOf(p.enumNames, value);
        if (index >= 0) p.enumValueIndex = index;
        else Debug.LogWarning("[Scout] Enum value '" + value + "' unavailable for " + path);
    }

    private static void Strings(SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Scout] Expected list at " + path);
            return;
        }

        p.ClearArray();
        for (int i = 0; i < values.Length; i++)
        {
            p.InsertArrayElementAtIndex(i);
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("[Scout] Expected string list element at " + path);
        }
    }

    private static void Requirements(SerializedObject so, string path, params (string component, int quantity)[] items)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Scout] Expected requirement list at " + path);
            return;
        }

        p.ClearArray();
        for (int i = 0; i < items.Length; i++)
        {
            p.InsertArrayElementAtIndex(i);
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component != null && component.propertyType == SerializedPropertyType.String)
                component.stringValue = items[i].component;
            else
                Debug.LogWarning("[Scout] Missing string field 'component' in " + path);

            if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                quantity.intValue = items[i].quantity;
            else
                Debug.LogWarning("[Scout] Missing integer field 'quantity' in " + path);
        }
    }
}

using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulatePatrolCanonicalData
{
    private const string AssetPath = "Assets/Game/Units/Database/Ground/Patrol/Patrol.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Patrol Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Patrol] Unit asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-ground-patrol");
        S(so, "displayName", "Patrol");
        E(so, "category", "Ground");
        S(so, "role", "Light Patrol / Perimeter Sweep");
        S(so, "tier", "Security");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 3.9f);
        F(so, "physical.widthMeters", 1.9f);
        F(so, "physical.heightMeters", 1.7f);
        F(so, "physical.massKg", 900f);
        S(so, "physical.powerSource", "Diesel-electric hybrid");
        F(so, "physical.energyCapacityWh", 4200f);
        F(so, "physical.energyConsumptionWhPerMinute", 0f);
        S(so, "physical.endurance", "6–10 hours");
        F(so, "physical.payloadCapacityKg", 120f);
        F(so, "physical.structuralIntegrityHp", 700f);
        F(so, "physical.armorRating", 0.35f);
        F(so, "physical.topSpeedMetersPerSecond", 20f);
        S(so, "physical.navigationSystem", "GPS + Patrol AI");
        S(so, "physical.heatSignature", "Medium");
        S(so, "physical.noiseSignature", "Medium");
        S(so, "physical.topSpeedDescription", "20 m/s");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.stabilityRating", "High");
        Strings(so, "physical.modules",
            "Patrol optics", "Light turret mount");

        // Fuel capacity is attempted only if the current schema exposes it.
        F(so, "physical.fuelCapacityLiters", 55f, true);

        // Autonomy and AI
        S(so, "autonomy.behaviorProfile", "Perimeter Sweep / Light Security");
        F(so, "autonomy.aggression", 0.3f);
        F(so, "autonomy.caution", 0.8f);
        F(so, "autonomy.persistence", 0.7f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.7f);
        F(so, "autonomy.riskTolerance", 0.5f);
        S(so, "autonomy.retreatBehaviorDescription", "Early");
        S(so, "autonomy.targetPriority", "Movement near perimeter");
        S(so, "autonomy.formationDiscipline", "Medium");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1600f);
        F(so, "sensors.thermalRangeMeters", 700f);
        S(so, "sensors.visualRangeDescription", "1600 m");
        S(so, "sensors.thermalRangeDescription", "700 m");
        S(so, "sensors.acousticRange", "Medium");
        S(so, "sensors.radarRange", "Short");
        S(so, "sensors.lidarRange", "Short");
        S(so, "sensors.signalDetectionRange", "Short");

        // Communication
        F(so, "communication.meshRangeKm", 4f);
        S(so, "communication.meshRangeDescription", "4 km");
        F(so, "communication.relayCapacity", 0f);
        S(so, "communication.commandLinkType", "Security Grid");
        S(so, "communication.networkPriority", "Medium");

        // Combat and threat model
        F(so, "combat.firepowerRating", 0.35f);
        F(so, "combat.suppressionValue", 0.25f);
        S(so, "combat.firepowerDescription", "Low");
        S(so, "combat.accuracy", "Medium");
        S(so, "combat.stabilization", "Light turret stabilization");
        S(so, "combat.weaponMounts", "1");
        S(so, "combat.weaponTypes", "Light turret / suppression gun");
        S(so, "combat.trackingStrength", "Low");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Low");
        I(so, "combat.threatLevel", 4);
        S(so, "combat.engagementDistance", "200–500 m");
        S(so, "combat.preferredTerrain", "Perimeter roads / open ground");
        S(so, "combat.preferredTargets", "Light infantry");
        S(so, "combat.vulnerabilityProfile", "Ballistic / explosives");

        // Squad and command integration
        S(so, "squad.squadRole", "Light Patrol");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Perimeter sweep spacing");
        S(so, "squad.intentInterpretationRules", "Maintain patrol route");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports security squads");
        S(so, "squad.parentCommandUnit", "Command Core / Pulse");
        Strings(so, "squad.subordinateUnits", "Scout");
        Strings(so, "squad.relayNodes");
        S(so, "squad.networkDependencies", "Security Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Light Vehicle Cockpit");
        S(so, "vrOperator.operatorHud", "Patrol Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical + turret cam");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full vehicle drive");
        S(so, "vrOperator.weaponControls", "Light turret");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.abilityControls", "Mark Threat, Stabilize, Patrol Mode");
        Strings(so, "vrOperator.operatorAbilities",
            "Light Suppression",
            "Threat Marking",
            "Patrol Route Lock",
            "Relay Security Data");

        // Economy and logistics
        I(so, "economy.creditCost", 530);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "350 Metal / 180 Electronics");
        F(so, "economy.buildTimeSeconds", 80f);
        S(so, "economy.factoryType", "Vehicle Bay");
        S(so, "economy.techRequirements", "Security Systems I");
        S(so, "economy.maintenanceCost", "Low");
        F(so, "economy.repairTimeSeconds", 20f);
        S(so, "economy.supplyConsumption", "Low");
        S(so, "economy.operationalCost", "Low");
        S(so, "economy.deploymentRequirements", "Vehicle pad");
        S(so, "economy.transportCompatibility", "Light racks");
        S(so, "economy.launchMethod", "Drive-out");

        // Player collection / build requirements
        Requirements(so, "buildRequirements.commonComponents",
            ("Light Alloy Plates", 28),
            ("Reinforced Frame Rods", 14),
            ("Hydraulic Actuators", 4),
            ("Armor Panels", 2),
            ("Patrol Sensor Housing", 1));

        Requirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 14),
            ("Signal Processors", 10),
            ("Vehicle Control Chips", 3),
            ("Security AI Chips", 2));

        Requirements(so, "buildRequirements.power",
            ("Hybrid Engine Core", 1),
            ("Power Regulation Nodes", 2));

        Requirements(so, "buildRequirements.rareTradeable",
            ("Light Turret Stabilizer", 1));

        Strings(so, "buildRequirements.optionalUpgrades",
            "Reinforced Armor",
            "Silent Drive Kit",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Patrol] Canonical data population finished. Review Console warnings for fields not supported by the current UnitDefinition schema. Existing modelPrefab and storeImage were preserved.");
    }

    private static SerializedProperty P(SerializedObject so, string path, bool optional = false)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null && !optional)
            Debug.LogWarning("[Patrol] Missing serialized property: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.String) p.stringValue = value;
        else if (p.propertyType == SerializedPropertyType.Enum)
        {
            int index = Array.IndexOf(p.enumNames, value);
            if (index >= 0) p.enumValueIndex = index;
            else Debug.LogWarning("[Patrol] Enum value '" + value + "' is unavailable for " + path);
        }
        else Debug.LogWarning("[Patrol] Expected string/enum at " + path + ", found " + p.propertyType);
    }

    private static void F(SerializedObject so, string path, float value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Patrol] Expected numeric field at " + path + ", found " + p.propertyType);
    }

    private static void I(SerializedObject so, string path, int value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        else Debug.LogWarning("[Patrol] Expected integer field at " + path + ", found " + p.propertyType);
    }

    private static void B(SerializedObject so, string path, bool value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Boolean) p.boolValue = value;
        else Debug.LogWarning("[Patrol] Expected boolean field at " + path + ", found " + p.propertyType);
    }

    private static void E(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Patrol] Expected enum at " + path + ", found " + p.propertyType);
            return;
        }

        int index = Array.IndexOf(p.enumNames, value);
        if (index >= 0) p.enumValueIndex = index;
        else Debug.LogWarning("[Patrol] Enum value '" + value + "' is unavailable for " + path);
    }

    private static void Strings(SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Patrol] Expected list at " + path);
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
                Debug.LogWarning("[Patrol] Expected string list element at " + path);
        }
    }

    private static void Requirements(SerializedObject so, string path, params (string component, int quantity)[] items)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Patrol] Expected requirement list at " + path);
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
                Debug.LogWarning("[Patrol] Missing string field 'component' in " + path);

            if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                quantity.intValue = items[i].quantity;
            else
                Debug.LogWarning("[Patrol] Missing integer field 'quantity' in " + path);
        }
    }
}

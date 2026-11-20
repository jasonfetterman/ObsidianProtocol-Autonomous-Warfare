using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateRescueCanonicalData
{
    private const string AssetPath = "Assets/Game/Units/Database/Ground/Rescue/Rescue.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Rescue Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Rescue] Unit asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        S(so, "unitId", "obs-ground-rescue");
        S(so, "displayName", "Rescue");
        E(so, "category", "Ground");
        S(so, "role", "Emergency Extraction / Battlefield Recovery");
        S(so, "tier", "Support");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 4.3f);
        F(so, "physical.widthMeters", 2.2f);
        F(so, "physical.heightMeters", 1.8f);
        F(so, "physical.massKg", 1250f);
        S(so, "physical.powerSource", "Diesel-electric hybrid");
        F(so, "physical.energyCapacityWh", 4800f);
        F(so, "physical.fuelCapacityLiters", 65f, true);
        S(so, "physical.endurance", "5–9 hours");
        F(so, "physical.payloadCapacityKg", 300f);
        F(so, "physical.structuralIntegrityHp", 800f);
        F(so, "physical.armorRating", 0.38f);
        F(so, "physical.topSpeedMetersPerSecond", 18f);
        S(so, "physical.navigationSystem", "GPS + Rescue AI");
        S(so, "physical.heatSignature", "Medium");
        S(so, "physical.noiseSignature", "Medium");
        S(so, "physical.topSpeedDescription", "18 m/s");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Tight");
        S(so, "physical.stabilityRating", "Very High");
        Strings(so, "physical.modules",
            "Medical bay",
            "Extraction winch",
            "Stabilization cradle");

        // Autonomy and AI
        S(so, "autonomy.behaviorProfile", "Emergency Response / Extraction");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.95f);
        F(so, "autonomy.persistence", 1f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.9f);
        F(so, "autonomy.riskTolerance", 0.15f);
        S(so, "autonomy.retreatBehaviorDescription", "Early");
        S(so, "autonomy.targetPriority", "Downed units / critical injuries");
        S(so, "autonomy.formationDiscipline", "High");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1500f);
        F(so, "sensors.thermalRangeMeters", 800f);
        S(so, "sensors.visualRangeDescription", "1500 m");
        S(so, "sensors.thermalRangeDescription", "800 m");
        S(so, "sensors.acousticRange", "Medium");
        S(so, "sensors.radarRange", "None");
        S(so, "sensors.lidarRange", "Short");
        S(so, "sensors.signalDetectionRange", "Short");

        // Communication
        F(so, "communication.meshRangeKm", 6f);
        S(so, "communication.meshRangeDescription", "6 km");
        F(so, "communication.relayCapacity", 1f);
        S(so, "communication.commandLinkType", "Support Grid");
        S(so, "communication.networkPriority", "High");

        // Combat and threat model
        F(so, "combat.firepowerRating", 0f);
        F(so, "combat.suppressionValue", 0f);
        S(so, "combat.firepowerDescription", "None");
        S(so, "combat.accuracy", "N/A");
        S(so, "combat.stabilization", "Medical stabilization cradle");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        S(so, "combat.trackingStrength", "None");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Medium");
        I(so, "combat.threatLevel", 3);
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "Safe lanes / extraction corridors");
        S(so, "combat.preferredTargets", "Downed allies");
        S(so, "combat.vulnerabilityProfile", "Ballistic / explosives");

        // Squad and command integration
        S(so, "squad.squadRole", "Emergency Extraction");
        I(so, "squad.squadSizeContribution", 2);
        S(so, "squad.formationBehavior", "Rear-line support spacing");
        S(so, "squad.intentInterpretationRules", "Recover and stabilize allies");
        S(so, "squad.retreatBehavior", "Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports all squads");
        S(so, "squad.parentCommandUnit", "Command Core / Fusion");
        Strings(so, "squad.subordinateUnits", "Scout", "Mule");
        Strings(so, "squad.relayNodes", "Relay Node 1");
        S(so, "squad.networkDependencies", "Support Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Rescue Cockpit");
        S(so, "vrOperator.operatorHud", "Medical & Extraction Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical + winch cam");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Full vehicle drive");
        S(so, "vrOperator.weaponControls", "N/A - rescue and extraction equipment only");
        S(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S(so, "vrOperator.stabilizationControls", "Stabilization cradle controls");
        S(so, "vrOperator.abilityControls", "Stabilize, Extract, Mark Safe Zone");
        Strings(so, "vrOperator.operatorAbilities",
            "Emergency Extraction",
            "Stabilization Pulse",
            "Rapid Recovery",
            "Support Relay");

        // Economy and logistics
        I(so, "economy.creditCost", 710);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "450 Metal / 260 Electronics");
        F(so, "economy.buildTimeSeconds", 90f);
        S(so, "economy.factoryType", "Vehicle Bay");
        S(so, "economy.techRequirements", "Support Systems I");
        S(so, "economy.maintenanceCost", "Low");
        F(so, "economy.repairTimeSeconds", 22f);
        S(so, "economy.supplyConsumption", "Low");
        S(so, "economy.operationalCost", "Low");
        S(so, "economy.deploymentRequirements", "Vehicle pad");
        S(so, "economy.transportCompatibility", "Light racks");
        S(so, "economy.launchMethod", "Drive-out");

        // Player collection / build requirements
        Requirements(so, "buildRequirements.commonComponents",
            ("Light Alloy Plates", 32),
            ("Reinforced Frame Rods", 16),
            ("Hydraulic Actuators", 5),
            ("Armor Panels", 2),
            ("Extraction Cradle Assembly", 1));

        Requirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 16),
            ("Signal Processors", 12),
            ("Vehicle Control Chips", 4),
            ("Support AI Chips", 2));

        Requirements(so, "buildRequirements.power",
            ("Hybrid Engine Core", 1),
            ("Power Regulation Nodes", 2));

        Requirements(so, "buildRequirements.rareTradeable",
            ("Medical Stabilization Module", 1));

        Strings(so, "buildRequirements.optionalUpgrades",
            "Reinforced Cradle",
            "Silent Drive Kit",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Rescue] Canonical data population finished. Review Console warnings for fields unsupported by the current UnitDefinition schema. Existing modelPrefab and storeImage were preserved.");
    }

    private static SerializedProperty P(SerializedObject so, string path, bool optional = false)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null && !optional)
            Debug.LogWarning("[Rescue] Missing serialized property: " + path);
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
            else Debug.LogWarning("[Rescue] Enum value '" + value + "' unavailable for " + path);
        }
        else
            Debug.LogWarning("[Rescue] Expected string/enum at " + path + ", found " + p.propertyType);
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
            Debug.LogWarning("[Rescue] Expected numeric field at " + path + ", found " + p.propertyType);
    }

    private static void I(SerializedObject so, string path, int value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else
            Debug.LogWarning("[Rescue] Expected integer field at " + path + ", found " + p.propertyType);
    }

    private static void B(SerializedObject so, string path, bool value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Rescue] Expected boolean field at " + path + ", found " + p.propertyType);
    }

    private static void E(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Rescue] Expected enum at " + path + ", found " + p.propertyType);
            return;
        }

        int index = Array.IndexOf(p.enumNames, value);
        if (index >= 0) p.enumValueIndex = index;
        else Debug.LogWarning("[Rescue] Enum value '" + value + "' unavailable for " + path);
    }

    private static void Strings(SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Rescue] Expected list at " + path);
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
                Debug.LogWarning("[Rescue] Expected string list element at " + path);
        }
    }

    private static void Requirements(SerializedObject so, string path, params (string component, int quantity)[] items)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Rescue] Expected requirement list at " + path);
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
                Debug.LogWarning("[Rescue] Missing string field 'component' in " + path);

            if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                quantity.intValue = items[i].quantity;
            else
                Debug.LogWarning("[Rescue] Missing integer field 'quantity' in " + path);
        }
    }
}

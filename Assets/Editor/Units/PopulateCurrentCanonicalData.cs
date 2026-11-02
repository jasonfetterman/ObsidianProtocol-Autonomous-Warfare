using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateCurrentCanonicalData
{
    private const string SeaFolder = "Assets/Game/Units/Database/Sea";

    [MenuItem("Obsidian Protocol/Units/Populate Current Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = FindCurrentAsset();
        if (target == null)
        {
            Debug.LogError("[Current] Could not find Current.asset under " +
                           SeaFolder + ". No asset was modified.");
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(target);
        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        SetText(so, "unitId", "obs-sea-current");
        SetText(so, "displayName", "Current");
        SetText(so, "category", "Sea");
        SetText(so, "role", "Fast Marine Transport / Rapid Response");
        SetText(so, "tier", "Support");
        SetText(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 6f);
        SetFloat(so, "physical.widthMeters", 2.8f);
        SetFloat(so, "physical.heightMeters", 2f);
        SetFloat(so, "physical.massKg", 2100f);
        SetText(so, "physical.powerSource", "Marine diesel-electric hybrid");
        SetFloat(so, "physical.energyCapacityWh", 9000f);
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 140f);
        SetText(so, "physical.endurance", "5–10 hours");
        SetFloat(so, "physical.payloadCapacityKg", 300f);
        SetStrings(so, "physical.modules",
            "Stabilized hull", "Rapid-thrust propulsion");
        SetText(so, "physical.navigationSystem", "Marine GPS + Response AI");
        SetFloat(so, "physical.structuralIntegrityHp", 800f);
        SetFloat(so, "physical.armorRating", 0.30f);
        SetText(so, "physical.heatSignature", "Low");
        SetText(so, "physical.noiseSignature", "Low");
        SetFloat(so, "physical.topSpeedMetersPerSecond", 12f);
        SetText(so, "physical.topSpeedDescription", "12 m/s");
        SetText(so, "physical.acceleration", "High");
        SetText(so, "physical.turningRadius", "Tight marine turn");
        SetText(so, "physical.stabilityRating", "High");

        // Autonomy and AI
        SetText(so, "autonomy.behaviorProfile",
            "Rapid Transport / Marine Response");
        SetFloat(so, "autonomy.aggression", 0f);
        SetFloat(so, "autonomy.caution", 0.8f);
        SetFloat(so, "autonomy.persistence", 0.7f);
        SetFloat(so, "autonomy.retreatThreshold", 0.85f);
        SetFloat(so, "autonomy.riskTolerance", 0.2f);
        SetText(so, "autonomy.retreatBehaviorDescription", "Early");
        SetText(so, "autonomy.targetPriority",
            "Friendly units requesting support");
        SetText(so, "autonomy.formationDiscipline", "Medium");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1800f);
        SetFloat(so, "sensors.thermalRangeMeters", 700f);
        SetText(so, "sensors.visualRangeDescription", "1800 m");
        SetText(so, "sensors.thermalRangeDescription", "700 m");
        SetText(so, "sensors.acousticRange", "Medium");
        SetOptionalText(so, "sensors.sonarRange", "Short");
        SetText(so, "sensors.radarRange", "Short");
        SetText(so, "sensors.signalDetectionRange", "Medium");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 6f);
        SetText(so, "communication.meshRangeDescription", "6 km");
        SetFloat(so, "communication.relayCapacity", 1f);
        SetText(so, "communication.commandLinkType", "Marine Support Grid");
        SetText(so, "communication.networkPriority", "High");

        // Combat and threat
        SetFloat(so, "combat.firepowerRating", 0f);
        SetText(so, "combat.firepowerDescription", "None");
        SetText(so, "combat.weaponMounts", "None");
        SetText(so, "combat.weaponTypes", "None");
        SetFloat(so, "combat.suppressionValue", 0f);
        SetText(so, "combat.accuracy", "N/A");
        SetText(so, "combat.stabilization", "High");
        SetText(so, "combat.trackingStrength", "Low");
        SetText(so, "combat.disruptionStrength", "Low");
        SetText(so, "combat.breachCapability", "None");
        SetText(so, "combat.countermeasureRating", "Low");
        SetInt(so, "combat.threatLevel", 2);
        SetText(so, "combat.engagementDistance", "N/A");
        SetText(so, "combat.preferredTerrain", "Surface waters");
        SetText(so, "combat.preferredTargets", "None");
        SetText(so, "combat.vulnerabilityProfile",
            "Ballistic / collision / waves");

        // Squad and command integration
        SetText(so, "squad.squadRole", "Marine Transport / Support");
        SetInt(so, "squad.squadSizeContribution", 2);
        SetText(so, "squad.formationBehavior", "Support spacing");
        SetText(so, "squad.intentInterpretationRules",
            "Deliver personnel or supplies");
        SetText(so, "squad.retreatBehavior", "Early");
        SetText(so, "squad.reinforcementBehavior",
            "Supports all marine squads");
        SetText(so, "squad.parentCommandUnit", "Worldmap / Insight");
        SetStrings(so, "squad.subordinateUnits");
        SetStrings(so, "squad.relayNodes", "Relay Node 1");
        SetText(so, "squad.networkDependencies", "Marine Support Grid");

        // VR operator
        SetText(so, "vrOperator.operatorSeatType", "Marine Transport Console");
        SetText(so, "vrOperator.operatorHud", "Support Overlay");
        SetText(so, "vrOperator.cameraFeeds", "Optical");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.sonarView", true);
        SetBool(so, "vrOperator.lidarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetText(so, "vrOperator.movementControls", "Marine drive");
        SetText(so, "vrOperator.sensorControls", "Thermal / sonar / zoom");
        SetText(so, "vrOperator.abilityControls",
            "Mark Drop Zone / Stabilize / Route Lock");
        SetText(so, "vrOperator.weaponControls",
            "Cargo controls: Load / secure");
        SetText(so, "vrOperator.stabilizationControls",
            "Stabilized hull");
        SetStrings(so, "vrOperator.operatorAbilities",
            "Rapid Transport",
            "Stabilization Pulse",
            "Route Optimization",
            "Support Relay");

        // Economy and logistics
        SetText(so, "economy.buildCost", "420 Metal / 240 Electronics");
        SetInt(so, "economy.creditCost", 660);
        SetFloat(so, "economy.buildTimeSeconds", 95f);
        SetText(so, "economy.factoryType", "Marine Bay");
        SetText(so, "economy.techRequirements", "Marine Systems I");
        SetText(so, "economy.maintenanceCost", "Low");
        SetFloat(so, "economy.repairTimeSeconds", 24f);
        SetText(so, "economy.supplyConsumption", "Low");
        SetText(so, "economy.operationalCost", "Low");
        SetText(so, "economy.deploymentRequirements", "Water launch pad");
        SetText(so, "economy.transportCompatibility", "Marine racks");
        SetText(so, "economy.launchMethod", "Water drop");
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);

        // Player collection requirements
        SetRequirements(so, "buildRequirements.commonComponents",
            ("Marine Alloy Plates", 32),
            ("Reinforced Marine Rods", 18),
            ("Hydraulic Actuators", 5),
            ("Hull Stabilizer Assembly", 1));

        SetRequirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 18),
            ("Signal Processors", 12),
            ("Marine Control Chips", 3),
            ("Support AI Chips", 2));

        SetRequirements(so, "buildRequirements.power",
            ("Marine Hybrid Engine Core", 1),
            ("Power Regulation Nodes", 2));

        SetRequirements(so, "buildRequirements.rareTradeable",
            ("Rapid-Thrust Propulsion Module", 1));

        SetStrings(so, "buildRequirements.optionalUpgrades",
            "Silent Propulsion",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Current] Population pass completed for " + assetPath +
                  ". Review warnings above to confirm supported fields.");
    }

    private static UnitDefinition FindCurrentAsset()
    {
        string[] guids = AssetDatabase.FindAssets("t:UnitDefinition", new[] { SeaFolder });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.Equals(System.IO.Path.GetFileName(path),
                              "Current.asset",
                              StringComparison.OrdinalIgnoreCase))
                return AssetDatabase.LoadAssetAtPath<UnitDefinition>(path);
        }

        return null;
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Current] Serialized property not found: " + path);
        return p;
    }

    private static void SetText(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
        {
            p.stringValue = value;
            return;
        }

        if (p.propertyType == SerializedPropertyType.Enum)
        {
            int index = Array.IndexOf(p.enumNames, value);
            if (index >= 0)
                p.enumValueIndex = index;
            else
                Debug.LogWarning("[Current] Enum value '" + value +
                                 "' unavailable for " + path);
            return;
        }

        Debug.LogWarning("[Current] Expected string or enum at " + path +
                         ", found " + p.propertyType);
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Current] Expected numeric property at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetOptionalFloat(
        SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Current] Optional property has incompatible type: " +
                             path);
    }

    private static void SetOptionalText(
        SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else
            Debug.LogWarning("[Current] Optional property has incompatible type: " +
                             path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else
            Debug.LogWarning("[Current] Expected integer at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Current] Expected boolean at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetStrings(
        SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Current] Expected list at " + path);
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
                Debug.LogWarning("[Current] Expected string list element at " +
                                 path + "[" + i + "]");
        }
    }

    private static void SetRequirements(
        SerializedObject so,
        string path,
        params (string component, int quantity)[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Current] Expected requirement list at " + path);
            return;
        }

        p.ClearArray();
        for (int i = 0; i < values.Length; i++)
        {
            p.InsertArrayElementAtIndex(i);
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component == null ||
                component.propertyType != SerializedPropertyType.String ||
                quantity == null ||
                quantity.propertyType != SerializedPropertyType.Integer)
            {
                Debug.LogWarning("[Current] Requirement schema mismatch at " +
                                 path + "[" + i + "]");
                continue;
            }

            component.stringValue = values[i].component;
            quantity.intValue = values[i].quantity;
        }
    }
}

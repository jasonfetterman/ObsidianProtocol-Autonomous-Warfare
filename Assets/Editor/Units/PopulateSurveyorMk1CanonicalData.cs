using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateSurveyorMk1CanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Sea/Surveyor_Mk1/Surveyor_Mk1.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Surveyor Mk1 Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Surveyor Mk1] UnitDefinition not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        SetText(so, "unitId", "obs-sea-surveyor-mk1");
        SetText(so, "displayName", "Surveyor Mk1");
        SetText(so, "category", "Sea");
        SetText(so, "role", "Marine Recon / Surface & Shallow-Water Survey");
        SetText(so, "tier", "Recon");
        SetText(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 8f);
        SetFloat(so, "physical.widthMeters", 3.5f);
        SetFloat(so, "physical.heightMeters", 2.5f);
        SetFloat(so, "physical.massKg", 3200f);
        SetText(so, "physical.powerSource", "Marine diesel-electric hybrid");
        SetFloat(so, "physical.energyCapacityWh", 12000f);
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 180f);
        SetText(so, "physical.endurance", "6–12 hours");
        SetFloat(so, "physical.payloadCapacityKg", 200f);
        SetStrings(so, "physical.modules",
            "Forward sonar array", "Surface optics", "Mapping suite");
        SetText(so, "physical.navigationSystem", "Marine GPS + Survey AI");
        SetFloat(so, "physical.structuralIntegrityHp", 900f);
        SetFloat(so, "physical.armorRating", 0.35f);
        SetText(so, "physical.heatSignature", "Low");
        SetText(so, "physical.noiseSignature", "Low");
        SetFloat(so, "physical.topSpeedMetersPerSecond", 9f);
        SetText(so, "physical.topSpeedDescription", "9 m/s");
        SetText(so, "physical.acceleration", "Medium");
        SetText(so, "physical.turningRadius", "Tight marine turn");
        SetText(so, "physical.stabilityRating", "High (surface optimized)");

        // Autonomy
        SetText(so, "autonomy.behaviorProfile", "Recon / Mapping / Marine Survey");
        SetFloat(so, "autonomy.aggression", 0f);
        SetFloat(so, "autonomy.caution", 0.85f);
        SetFloat(so, "autonomy.persistence", 0.9f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.8f);
        SetText(so, "autonomy.retreatBehaviorDescription", "Early");
        SetText(so, "autonomy.targetPriority", "Marine anomalies / surface movement");
        SetText(so, "autonomy.formationDiscipline", "Medium");
        SetFloat(so, "autonomy.riskTolerance", 0.2f);

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 2200f);
        SetFloat(so, "sensors.thermalRangeMeters", 900f);
        SetText(so, "sensors.visualRangeDescription", "2200 m");
        SetText(so, "sensors.thermalRangeDescription", "900 m");
        SetText(so, "sensors.acousticRange", "High");
        SetText(so, "sensors.radarRange", "Short");
        SetText(so, "sensors.signalDetectionRange", "Medium");
        SetOptionalText(so, "sensors.sonarRange", "Medium (shallow optimized)");
        SetOptionalText(so, "sensors.lidarRange", "Not specified");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 6f);
        SetText(so, "communication.meshRangeDescription", "6 km");
        SetFloat(so, "communication.relayCapacity", 1f);
        SetText(so, "communication.commandLinkType", "Marine Recon Grid");
        SetText(so, "communication.networkPriority", "High");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0f);
        SetText(so, "combat.firepowerDescription", "None");
        SetText(so, "combat.weaponMounts", "None");
        SetText(so, "combat.weaponTypes", "None");
        SetFloat(so, "combat.suppressionValue", 0f);
        SetText(so, "combat.accuracy", "N/A");
        SetText(so, "combat.stabilization", "Surface-optimized stabilization");
        SetText(so, "combat.trackingStrength", "Medium (sonar only)");
        SetText(so, "combat.disruptionStrength", "Low");
        SetText(so, "combat.breachCapability", "None");
        SetText(so, "combat.countermeasureRating", "Low");
        SetInt(so, "combat.threatLevel", 2);
        SetText(so, "combat.engagementDistance", "N/A");
        SetText(so, "combat.preferredTerrain", "Surface & shallow waters");
        SetText(so, "combat.preferredTargets", "None");
        SetText(so, "combat.vulnerabilityProfile", "Ballistic / waves / collision");

        // Squad and command integration
        SetText(so, "squad.squadRole", "Marine Recon / Mapping");
        SetInt(so, "squad.squadSizeContribution", 1);
        SetText(so, "squad.formationBehavior", "Loose recon spacing");
        SetText(so, "squad.intentInterpretationRules",
            "Identify anomalies & map terrain");
        SetText(so, "squad.retreatBehavior", "Early");
        SetText(so, "squad.reinforcementBehavior",
            "Supports recon & naval squads");
        SetText(so, "squad.parentCommandUnit", "Worldmap / Insight");
        SetStrings(so, "squad.subordinateUnits");
        SetStrings(so, "squad.relayNodes", "Relay Node 1");
        SetText(so, "squad.networkDependencies", "Marine Recon Grid");

        // VR operator
        SetText(so, "vrOperator.operatorSeatType", "Marine Recon Console");
        SetText(so, "vrOperator.operatorHud", "Marine Survey Overlay");
        SetText(so, "vrOperator.cameraFeeds", "Optical + sonar visualization");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.lidarView", false);
        SetBool(so, "vrOperator.sonarView", true);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetText(so, "vrOperator.movementControls", "Marine drive");
        SetText(so, "vrOperator.sensorControls", "Sonar / thermal / zoom");
        SetText(so, "vrOperator.abilityControls", "Scan / Mark Target / Stabilize");
        SetText(so, "vrOperator.weaponControls", "N/A - non-combat survey unit");
        SetText(so, "vrOperator.stabilizationControls", "Surface stabilization");
        SetStrings(so, "vrOperator.operatorAbilities",
            "Sonar Sweep", "Surface Scan", "Hazard Detection", "Recon Relay");

        // Economy and logistics
        SetText(so, "economy.buildCost", "500 Metal / 260 Electronics");
        SetInt(so, "economy.creditCost", 760);
        SetFloat(so, "economy.buildTimeSeconds", 110f);
        SetText(so, "economy.factoryType", "Marine Bay");
        SetText(so, "economy.techRequirements", "Marine Systems I");
        SetText(so, "economy.maintenanceCost", "Low");
        SetFloat(so, "economy.repairTimeSeconds", 28f);
        SetText(so, "economy.supplyConsumption", "Low");
        SetText(so, "economy.operationalCost", "Low");
        SetText(so, "economy.deploymentRequirements", "Water launch pad");
        SetText(so, "economy.transportCompatibility", "Marine racks");
        SetText(so, "economy.launchMethod", "Water drop");
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);

        // Player collection requirements
        SetRequirements(so, "buildRequirements.commonComponents",
            ("Marine Alloy Plates", 40),
            ("Reinforced Marine Rods", 20),
            ("Hydraulic Actuators", 6),
            ("Sonar Housing", 1));

        SetRequirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 20),
            ("Signal Processors", 14),
            ("Marine Control Chips", 4),
            ("Recon AI Chips", 2));

        SetRequirements(so, "buildRequirements.power",
            ("Marine Hybrid Engine Core", 1),
            ("Power Regulation Nodes", 2));

        SetRequirements(so, "buildRequirements.rareTradeable",
            ("Medium Sonar Module", 1));

        SetStrings(so, "buildRequirements.optionalUpgrades",
            "Silent Propulsion",
            "Extended Battery Pack",
            "Anti-Jamming Shielding");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Surveyor Mk1] Population pass completed: " + AssetPath +
                  ". Review warnings for unsupported or missing serialized fields.");
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Surveyor Mk1] Serialized property not found: " + path);
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
                Debug.LogWarning("[Surveyor Mk1] Enum value '" + value +
                                 "' unavailable for " + path);
            return;
        }

        Debug.LogWarning("[Surveyor Mk1] Expected string or enum at " + path +
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
            Debug.LogWarning("[Surveyor Mk1] Expected numeric property at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetOptionalFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
        else
            Debug.LogWarning("[Surveyor Mk1] Optional property has incompatible type: " +
                             path);
    }

    private static void SetOptionalText(SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else
            Debug.LogWarning("[Surveyor Mk1] Optional property has incompatible type: " +
                             path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else
            Debug.LogWarning("[Surveyor Mk1] Expected integer at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Surveyor Mk1] Expected boolean at " + path +
                             ", found " + p.propertyType);
    }

    private static void SetStrings(SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Surveyor Mk1] Expected list at " + path);
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
                Debug.LogWarning("[Surveyor Mk1] Expected string list element at " +
                                 path + "[" + i + "]");
        }
    }

    private static void SetRequirements(
        SerializedObject so, string path, params (string component, int quantity)[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Surveyor Mk1] Expected requirement list at " + path);
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
                Debug.LogWarning("[Surveyor Mk1] Requirement schema mismatch at " +
                                 path + "[" + i + "]");
                continue;
            }

            component.stringValue = values[i].component;
            quantity.intValue = values[i].quantity;
        }
    }
}


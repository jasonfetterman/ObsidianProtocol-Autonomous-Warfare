using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateSentinelCanonicalData
{
    private const string AssetPath = "Assets/Game/Units/Database/Ground/Sentinel/Sentinel.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Sentinel Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Sentinel] Unit asset not found: " + AssetPath);
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        S(so, "unitId", "obs-ground-sentinal");
        S(so, "displayName", "Sentinal");
        E(so, "category", "Ground");
        S(so, "role", "Static Defense / Automated Turret Platform");
        S(so, "tier", "Security");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 2.1f);
        F(so, "physical.widthMeters", 2.1f);
        F(so, "physical.heightMeters", 1.4f);
        F(so, "physical.massKg", 650f);
        S(so, "physical.powerSource", "High-capacity battery + optional generator");
        F(so, "physical.energyCapacityWh", 5200f);
        F(so, "physical.fuelCapacityLiters", 0f, true);
        S(so, "physical.endurance", "8–14 hours (battery) / unlimited (generator)");
        F(so, "physical.payloadCapacityKg", 0f);
        F(so, "physical.structuralIntegrityHp", 700f);
        F(so, "physical.armorRating", 0.45f);
        F(so, "physical.topSpeedMetersPerSecond", 0f);
        S(so, "physical.navigationSystem", "Stationary (micro-adjust motors only)");
        S(so, "physical.heatSignature", "Low");
        S(so, "physical.noiseSignature", "Low");
        S(so, "physical.topSpeedDescription", "N/A (stationary)");
        S(so, "physical.acceleration", "N/A (stationary)");
        S(so, "physical.turningRadius", "Micro-adjust motors only");
        S(so, "physical.stabilityRating", "Maximum");
        Strings(so, "physical.modules", "Auto-turret", "Stabilization base");

        // Autonomy and AI
        S(so, "autonomy.behaviorProfile", "Static Defense / Automated Threat Response");
        F(so, "autonomy.aggression", 0.6f);
        F(so, "autonomy.caution", 0.9f);
        F(so, "autonomy.persistence", 1f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0f);
        F(so, "autonomy.riskTolerance", 0.5f);
        S(so, "autonomy.retreatBehaviorDescription", "None - fixed emplacement");
        S(so, "autonomy.targetPriority", "Hostile movement in line-of-sight");
        S(so, "autonomy.formationDiscipline", "Very High");

        // Sensors
        F(so, "sensors.visualRangeMeters", 2200f);
        F(so, "sensors.thermalRangeMeters", 1000f);
        S(so, "sensors.visualRangeDescription", "2200 m; nominal detection reliability 96%");
        S(so, "sensors.thermalRangeDescription", "1000 m; nominal thermal detection reliability 94%");
        S(so, "sensors.acousticRange", "Medium");
        S(so, "sensors.radarRange", "Short");
        S(so, "sensors.lidarRange", "Medium");
        S(so, "sensors.signalDetectionRange", "Medium");

        // Communication
        F(so, "communication.meshRangeKm", 5f);
        S(so, "communication.meshRangeDescription", "5 km; Security Grid range 5 km");
        F(so, "communication.relayCapacity", 0f);
        S(so, "communication.commandLinkType", "Security Grid");
        S(so, "communication.networkPriority", "High");

        // Combat
        F(so, "combat.firepowerRating", 0.5f);
        F(so, "combat.suppressionValue", 0.5f);
        S(so, "combat.firepowerDescription", "Medium");
        S(so, "combat.accuracy", "High");
        S(so, "combat.stabilization", "Very High");
        S(so, "combat.weaponMounts", "1 (auto-turret)");
        S(so, "combat.weaponTypes", "Medium machine gun / suppression cannon");
        S(so, "combat.trackingStrength", "High");
        S(so, "combat.disruptionStrength", "Medium");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Medium");
        I(so, "combat.threatLevel", 6);
        S(so, "combat.engagementDistance", "300–900 m");
        S(so, "combat.preferredTerrain", "Defensive positions / choke points");
        S(so, "combat.preferredTargets", "Infantry / light vehicles");
        S(so, "combat.vulnerabilityProfile", "EMP / explosives");

        // Squad and command integration
        S(so, "squad.squadRole", "Static Defense");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Fixed emplacement");
        S(so, "squad.intentInterpretationRules", "Hold position and fire");
        S(so, "squad.retreatBehavior", "None");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports security squads");
        S(so, "squad.parentCommandUnit", "Command Core / Pulse");
        Strings(so, "squad.subordinateUnits");
        Strings(so, "squad.relayNodes");
        S(so, "squad.networkDependencies", "Security Grid");

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Fixed Turret Control Station");
        S(so, "vrOperator.operatorHud", "Static Defense / Threat Response Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical + thermal + turret camera");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", true);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Stationary; micro-adjust motors only");
        S(so, "vrOperator.weaponControls", "Automated turret fire control");
        S(so, "vrOperator.sensorControls", "Optical / thermal / acoustic / radar / LiDAR");
        S(so, "vrOperator.stabilizationControls", "Turret stabilization");
        S(so, "vrOperator.abilityControls", "Zone Lock, Target Marking, Authorized Override");
        Strings(so, "vrOperator.operatorAbilities",
            "Automated Threat Response",
            "Defensive Zone Lock",
            "Target Classification",
            "Authorized Command Override");

        // Economy and logistics
        I(so, "economy.creditCost", 0);
        I(so, "economy.deploymentPoints", 0);
        B(so, "economy.deploymentPointsDefined", false);
        S(so, "economy.buildCost", "Metal / Electronics cost not specified");
        F(so, "economy.buildTimeSeconds", 0f);
        S(so, "economy.factoryType", "Static Defense Fabricator");
        S(so, "economy.techRequirements", "Security Systems I");
        S(so, "economy.maintenanceCost", "Tier I; MTBF 2400 h");
        F(so, "economy.repairTimeSeconds", 20f);
        S(so, "economy.supplyConsumption", "Resource drain 0.30% per hour");
        S(so, "economy.operationalCost", "5 Metal / 4 Electronics per hour");
        S(so, "economy.deploymentRequirements", "Fixed Emplacement Pad");
        S(so, "economy.transportCompatibility", "Fixed emplacement; transport not specified");
        S(so, "economy.launchMethod", "Fixed emplacement");

        // Build requirements and upgrade options were not supplied as collection quantities.
        // Preserve existing build requirement lists rather than replacing them with invented costs.
        Strings(so, "buildRequirements.optionalUpgrades",
            "Reinforced Armor",
            "Extended Battery Pack",
            "Anti-Jamming Shielding",
            "Thermal-Enhanced Optics",
            "Advanced Fire-Control Core",
            "Hardened Turret Stabilizer");

        // Save only supported serialized fields.
        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Supplemental metrics are applied only if the asset schema already contains matching fields.
        SetOptional(so, "operationalParameters.missionType", "Static Defense / Automated Threat Response");
        SetOptional(so, "operationalParameters.optimalDeploymentZone", "Defensive positions / choke points");
        SetOptional(so, "operationalParameters.commandLatencySeconds", 0.20f);
        SetOptional(so, "operationalParameters.relayLoadPercent", 36f);
        SetOptional(so, "operationalParameters.signalIntegrityPercent", 98f);
        SetOptional(so, "operationalParameters.aiSyncRatePercent", 99f);
        SetOptional(so, "operationalParameters.tacticalRefreshIntervalSeconds", 0.5f);
        SetOptional(so, "operationalParameters.strategicRefreshIntervalSeconds", 2f);
        SetOptional(so, "operationalParameters.autonomousDecisionDepth", "Tier III");
        SetOptional(so, "operationalParameters.overridePriority", "Security Grid > AI > Manual");
        SetOptional(so, "operationalParameters.commandAuthority", "Tactical");
        SetOptional(so, "operationalParameters.commandRangeKm", 5f);
        SetOptional(so, "operationalParameters.processingCapacity", "High");
        SetOptional(so, "operationalParameters.networkCapacity", "High");
        SetOptional(so, "operationalParameters.autonomousOperation", true);

        Debug.Log("[Sentinel] Population routine completed. Check preceding warnings. Supplemental metrics not represented in UnitDefinition require schema support before they can be saved.");
    }

    private static SerializedProperty P(SerializedObject so, string path, bool optional = false)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null && !optional)
            Debug.LogWarning("[Sentinel] Missing serialized property: " + path);
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
            else Debug.LogWarning("[Sentinel] Enum value '" + value + "' unavailable for " + path);
        }
        else
            Debug.LogWarning("[Sentinel] Expected string/enum at " + path + ", found " + p.propertyType);
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
            Debug.LogWarning("[Sentinel] Expected numeric field at " + path + ", found " + p.propertyType);
    }

    private static void I(SerializedObject so, string path, int value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else
            Debug.LogWarning("[Sentinel] Expected integer field at " + path + ", found " + p.propertyType);
    }

    private static void B(SerializedObject so, string path, bool value, bool optional = false)
    {
        SerializedProperty p = P(so, path, optional);
        if (p == null) return;

        if (p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else
            Debug.LogWarning("[Sentinel] Expected boolean field at " + path + ", found " + p.propertyType);
    }

    private static void E(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Sentinel] Expected enum at " + path + ", found " + p.propertyType);
            return;
        }

        int index = Array.IndexOf(p.enumNames, value);
        if (index >= 0) p.enumValueIndex = index;
        else Debug.LogWarning("[Sentinel] Enum value '" + value + "' unavailable for " + path);
    }

    private static void Strings(SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Sentinel] Expected list at " + path);
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
                Debug.LogWarning("[Sentinel] Expected string list element at " + path);
        }
    }

    private static void SetOptional(SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.String) p.stringValue = value;
        else if (p.propertyType == SerializedPropertyType.Enum)
        {
            int index = Array.IndexOf(p.enumNames, value);
            if (index >= 0) p.enumValueIndex = index;
        }
    }

    private static void SetOptional(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
    }

    private static void SetOptional(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
    }
}

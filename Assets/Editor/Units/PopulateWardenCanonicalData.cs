using UnityEditor;
using UnityEngine;

public static class PopulateWardenCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/Warden/Warden.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Warden Canonical Data")]
    public static void Populate()
    {
        var asset = AssetDatabase.LoadMainAssetAtPath(AssetPath);
        if (asset == null)
        {
            Debug.LogError("Warden asset could not be loaded: " + AssetPath);
            return;
        }

        var so = new SerializedObject(asset);
        so.Update();

        SetString(so, "unitId", "obs-air-warden");
        SetString(so, "displayName", "Warden");
        SetString(so, "role", "Aerial Surveillance / Recon Intelligence");
        SetString(so, "tier", "Operational");
        SetString(so, "status", "Active");

        SetFloat(so, "physical.lengthMeters", 3.0f);
        SetFloat(so, "physical.widthMeters", 2.2f);
        SetFloat(so, "physical.heightMeters", 0.8f);
        SetFloat(so, "physical.massKg", 18f);
        SetFloat(so, "physical.energyCapacityWh", 2200f);
        SetFloat(so, "physical.energyConsumptionWhPerMinute", 45f);
        SetFloat(so, "physical.payloadCapacityKg", 2.5f);
        SetFloat(so, "physical.structuralIntegrityHp", 120f);
        SetFloat(so, "physical.armorRating", 0.15f);
        SetFloat(so, "physical.topSpeedMetersPerSecond", 22f);
        SetString(so, "physical.powerSource", "High-density battery");
        SetString(so, "physical.endurance", "30–50 minutes");
        SetString(so, "physical.navigationSystem", "GPS + AI Stabilization");
        SetString(so, "physical.heatSignature", "Low");
        SetString(so, "physical.noiseSignature", "Low");
        SetString(so, "physical.topSpeedDescription", "22 m/s");
        SetString(so, "physical.acceleration", "Medium");
        SetString(so, "physical.turningRadius", "Tight");
        SetString(so, "physical.climbRate", "High");
        SetString(so, "physical.stabilityRating", "High");
        SetStrings(so, "physical.modules",
            "Camera array", "Stabilization frame");

        SetString(so, "autonomy.behaviorProfile", "Surveillance");
        SetFloat(so, "autonomy.aggression", 0.1f);
        SetFloat(so, "autonomy.caution", 0.8f);
        SetFloat(so, "autonomy.persistence", 0.6f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.7f);
        SetFloat(so, "autonomy.riskTolerance", 0.1f);
        SetString(so, "autonomy.retreatBehaviorDescription",
            "Retreat under threat or when battery is low.");
        SetString(so, "autonomy.targetPriority", "Reconnaissance");
        SetString(so, "autonomy.formationDiscipline", "High");

        SetFloat(so, "sensors.visualRangeMeters", 1800f);
        SetFloat(so, "sensors.thermalRangeMeters", 900f);
        SetString(so, "sensors.visualRangeDescription", "High-resolution camera");
        SetString(so, "sensors.thermalRangeDescription", "Integrated microthermal sensor");
        SetString(so, "sensors.acousticRange", "Low");
        SetString(so, "sensors.radarRange", "None");
        SetString(so, "sensors.lidarRange", "None");
        SetString(so, "sensors.signalDetectionRange", "Medium");

        SetFloat(so, "communication.meshRangeKm", 5f);
        SetString(so, "communication.meshRangeDescription", "5 km");
        SetFloat(so, "communication.relayCapacity", 0f);
        SetString(so, "communication.commandLinkType", "Standard");
        SetString(so, "communication.networkPriority", "Medium");

        SetFloat(so, "combat.firepowerRating", 0f);
        SetFloat(so, "combat.suppressionValue", 0f);
        SetString(so, "combat.firepowerDescription", "Noncombat reconnaissance unit");
        SetString(so, "combat.accuracy", "Not applicable");
        SetString(so, "combat.stabilization", "High");
        SetString(so, "combat.weaponMounts", "None");
        SetString(so, "combat.weaponTypes", "None");
        SetString(so, "combat.trackingStrength", "High");
        SetString(so, "combat.disruptionStrength", "Low");
        SetString(so, "combat.breachCapability", "None");
        SetString(so, "combat.countermeasureRating", "Low");
        SetInt(so, "combat.threatLevel", 2);
        SetString(so, "combat.engagementDistance", "N/A");
        SetString(so, "combat.preferredTerrain", "Open air");
        SetString(so, "combat.preferredTargets", "High-value reconnaissance targets");
        SetString(so, "combat.vulnerabilityProfile",
            "Ballistic damage, adverse weather, and jamming");

        SetString(so, "squad.squadRole", "Recon");
        SetInt(so, "squad.squadSizeContribution", 1);
        SetString(so, "squad.formationBehavior", "High-discipline recon spacing");
        SetString(so, "squad.intentInterpretationRules",
            "Maintain distance; avoid threats; prioritize visibility.");
        SetString(so, "squad.retreatBehavior", "Retreat early when threatened.");
        SetString(so, "squad.breachBehavior", "No breach behavior");
        SetString(so, "squad.flankingBehavior", "No flanking behavior");
        SetString(so, "squad.reinforcementBehavior", "Supports reconnaissance squads");
        SetString(so, "squad.parentCommandUnit", "Sentinel / Overseer");
        SetString(so, "squad.networkDependencies", "Standard mesh network");
        SetStrings(so, "squad.subordinateUnits");
        SetStrings(so, "squad.relayNodes");

        SetString(so, "vrOperator.operatorSeatType", "Drone view");
        SetString(so, "vrOperator.operatorHud", "Recon overlay");
        SetString(so, "vrOperator.cameraFeeds", "Optical camera");
        SetString(so, "vrOperator.movementControls", "Full flight controls");
        SetString(so, "vrOperator.sensorControls", "Zoom, thermal, tracking");
        SetString(so, "vrOperator.abilityControls", "Scan, mark, stabilize");
        SetString(so, "vrOperator.weaponControls", "None");
        SetString(so, "vrOperator.stabilizationControls", "Auto-hover and auto-level");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.lidarView", false);
        SetBool(so, "vrOperator.sonarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetStrings(so, "vrOperator.operatorAbilities",
            "Scan", "Mark Target", "Stabilize", "Relay Recon Data");

        SetInt(so, "economy.creditCost", 800);
        SetString(so, "economy.buildCost", "120 Metal / 80 Electronics");
        SetFloat(so, "economy.buildTimeSeconds", 40f);
        SetString(so, "economy.factoryType", "Drone Bay");
        SetString(so, "economy.techRequirements", "Basic Recon");
        SetString(so, "economy.maintenanceCost", "Low");
        SetFloat(so, "economy.repairTimeSeconds", 10f);
        SetString(so, "economy.supplyConsumption", "Low");
        SetString(so, "economy.operationalCost", "Minimal");
        SetString(so, "economy.deploymentRequirements", "Standard drone pad");
        SetString(so, "economy.transportCompatibility", "All aerial racks");
        SetString(so, "economy.launchMethod", "Vertical takeoff");

        SetStrings(so, "buildRequirements.optionalUpgrades",
            "Thermal Lens Upgrade",
            "Extended Battery Module",
            "Silent Rotor Kit");

        SetString(so, "environment.terrainAdaptationCoefficients",
            "Preferred operating terrain: open air.");
        SetString(so, "environment.visibilityModifiers",
            "Recon effectiveness depends on visibility and sensor conditions.");
        SetString(so, "environment.signalPropagationCurve",
            "Mesh communications nominal range: 5 km.");
        SetFloat(so, "environment.weatherSensitivityIndex", 0.5f);

        SetFloat(so, "cognitive.decisionConfidence", 0f);
        SetFloat(so, "cognitive.threatRecognitionAccuracy", 0f);
        SetFloat(so, "cognitive.adaptiveLearningRatePerMission", 0f);
        SetFloat(so, "cognitive.commandOverrideResistance", 0f);
        SetFloat(so, "cognitive.cooperativeBehaviorIndex", 0f);

        SetString(so, "tactical.reinforcementTriggerConditions",
            "Request support when reconnaissance objectives exceed current capability.");
        SetString(so, "tactical.retreatLogicThresholds",
            "Retreat threshold: 0.7.");

        SetString(so, "maintenance.maintenancePriorityLevel", "Low");
        SetString(so, "maintenance.operationalCostPerHour", "Minimal");

        // Do not overwrite storeImage, modelPrefab, category, or unknown fields.
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();

        Debug.Log("Warden canonical data population completed. Review any warnings above; existing asset references were preserved.");
    }

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        var p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("Warden field not found; skipped: " + path);
        return p;
    }

    private static void SetString(SerializedObject so, string path, string value)
    {
        var p = Find(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else if (p != null)
            Debug.LogWarning("Warden field is not a string; skipped: " + path);
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        var p = Find(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p != null)
            Debug.LogWarning("Warden field is not a float; skipped: " + path);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        var p = Find(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else if (p != null)
            Debug.LogWarning("Warden field is not an integer; skipped: " + path);
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        var p = Find(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else if (p != null)
            Debug.LogWarning("Warden field is not a Boolean; skipped: " + path);
    }

    private static void SetStrings(SerializedObject so, string path, params string[] values)
    {
        var p = Find(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("Warden field is not a list; skipped: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            var element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("List element is not a string; skipped: " + path);
        }
    }
}

using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateSARCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/SAR/SAR.asset";

    [MenuItem("Obsidian Protocol/Units/Populate SAR Canonical Data")]
    public static void Populate()
    {
        var unit = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (unit == null)
        {
            Debug.LogError("SAR population failed: UnitDefinition not found at " + AssetPath);
            return;
        }

        Undo.RecordObject(unit, "Populate SAR Canonical Data");
        var so = new SerializedObject(unit);
        so.Update();

        SetString(so, "unitId", "obs-air-sar");
        SetString(so, "displayName", "SAR");
        SetString(so, "role", "Search & Rescue / Survival Extraction");
        SetString(so, "tier", "Advanced Support");
        SetString(so, "status", "Active");

        // Physical and technical
        SetString(so, "physical.powerSource", "High-capacity rescue battery");
        SetFloat(so, "physical.lengthMeters", 3.3f);
        SetFloat(so, "physical.widthMeters", 2.4f);
        SetFloat(so, "physical.heightMeters", 1.2f);
        SetFloat(so, "physical.massKg", 30f);
        SetFloat(so, "physical.energyCapacityWh", 3000f);
        SetFloat(so, "physical.energyConsumptionWhPerMinute", 70f);
        SetString(so, "physical.endurance", "30–45 minutes");
        SetFloat(so, "physical.payloadCapacityKg", 3f);
        SetFloat(so, "physical.structuralIntegrityHp", 160f);
        SetFloat(so, "physical.armorRating", 0.26f);
        SetFloat(so, "physical.topSpeedMetersPerSecond", 24f);
        SetString(so, "physical.topSpeedDescription", "24 m/s");
        SetString(so, "physical.acceleration", "Medium");
        SetString(so, "physical.turningRadius", "Tight");
        SetString(so, "physical.climbRate", "High");
        SetString(so, "physical.stabilityRating", "Very High (hover-lock for extraction)");
        SetString(so, "physical.navigationSystem", "GPS + Rescue Pathfinding AI");
        SetString(so, "physical.heatSignature", "Medium");
        SetString(so, "physical.noiseSignature", "Medium");
        SetStrings(so, "physical.modules", new[]
        {
            "SAR harness", "Thermal search suite"
        });

        // Autonomy and AI
        SetString(so, "autonomy.behaviorProfile", "Search & Rescue / Extraction");
        SetFloat(so, "autonomy.aggression", 0f);
        SetFloat(so, "autonomy.caution", 0.9f);
        SetFloat(so, "autonomy.persistence", 0.95f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.85f);
        SetFloat(so, "autonomy.riskTolerance", 0.5f);
        SetString(so, "autonomy.targetPriority", "Survivors / distress signals");
        SetString(so, "autonomy.formationDiscipline", "Medium");
        SetString(so, "autonomy.retreatBehaviorDescription", "Medium; disengage when extraction safety is compromised.");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1600f);
        SetFloat(so, "sensors.thermalRangeMeters", 1800f);
        SetString(so, "sensors.visualRangeDescription", "Visual survivor search range: 1600 m");
        SetString(so, "sensors.thermalRangeDescription", "High-resolution thermal survivor detection: 1800 m");
        SetString(so, "sensors.acousticRange", "High (survivor detection)");
        SetString(so, "sensors.radarRange", "None");
        SetString(so, "sensors.lidarRange", "Short-range");
        SetString(so, "sensors.signalDetectionRange", "High");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 6f);
        SetString(so, "communication.meshRangeDescription", "6 km");
        SetFloat(so, "communication.relayCapacity", 0f);
        SetString(so, "communication.commandLinkType", "Rescue Grid");
        SetString(so, "communication.networkPriority", "High");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0f);
        SetFloat(so, "combat.suppressionValue", 0f);
        SetInt(so, "combat.threatLevel", 3);
        SetString(so, "combat.accuracy", "N/A");
        SetString(so, "combat.stabilization", "Very High");
        SetString(so, "combat.weaponMounts", "None");
        SetString(so, "combat.weaponTypes", "None");
        SetString(so, "combat.trackingStrength", "High");
        SetString(so, "combat.disruptionStrength", "Low");
        SetString(so, "combat.breachCapability", "None");
        SetString(so, "combat.countermeasureRating", "Medium");
        SetString(so, "combat.engagementDistance", "N/A");
        SetString(so, "combat.preferredTerrain", "Rescue corridors / disaster zones");
        SetString(so, "combat.preferredTargets", "None");
        SetString(so, "combat.vulnerabilityProfile", "Ballistic / weather");
        SetString(so, "combat.firepowerDescription", "Unarmed rescue aircraft; no offensive weapons.");

        // Squad and command integration
        SetString(so, "squad.squadRole", "Search & Rescue");
        SetInt(so, "squad.squadSizeContribution", 1);
        SetString(so, "squad.formationBehavior", "SAR spacing");
        SetString(so, "squad.intentInterpretationRules", "Prioritize survivor extraction");
        SetString(so, "squad.retreatBehavior", "Medium");
        SetString(so, "squad.breachBehavior", "None");
        SetString(so, "squad.flankingBehavior", "None");
        SetString(so, "squad.reinforcementBehavior", "Supports rescue, logistics, and command squads");
        SetString(so, "squad.parentCommandUnit", "Rescue / Command Core");
        SetString(so, "squad.networkDependencies", "Rescue Grid");
        SetStrings(so, "squad.subordinateUnits", new[] { "Lifeline", "Response" });
        SetStrings(so, "squad.relayNodes", new string[0]);

        // VR operator mode
        SetString(so, "vrOperator.operatorSeatType", "Drone View");
        SetString(so, "vrOperator.operatorHud", "SAR Overlay");
        SetString(so, "vrOperator.cameraFeeds", "Optical");
        SetString(so, "vrOperator.movementControls", "Full drone flight");
        SetString(so, "vrOperator.sensorControls", "Thermal / LiDAR / sonar / zoom");
        SetString(so, "vrOperator.abilityControls", "Deploy Harness, Scan, Stabilize");
        SetString(so, "vrOperator.weaponControls", "None");
        SetString(so, "vrOperator.stabilizationControls", "Auto-hover + extraction lock");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.lidarView", true);
        SetBool(so, "vrOperator.sonarView", true);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetStrings(so, "vrOperator.operatorAbilities", new[]
        {
            "Survivor Extraction Harness",
            "Thermal Search",
            "Sonar Sweep",
            "LiDAR Scan",
            "Relay Rescue Data"
        });

        // Economy and logistics
        SetInt(so, "economy.creditCost", 0);
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);
        SetString(so, "economy.buildCost", "220 Metal / 230 Electronics");
        SetFloat(so, "economy.buildTimeSeconds", 70f);
        SetString(so, "economy.factoryType", "Drone Bay");
        SetString(so, "economy.techRequirements", "Rescue Systems III");
        SetString(so, "economy.maintenanceCost", "Medium");
        SetFloat(so, "economy.repairTimeSeconds", 18f);
        SetString(so, "economy.supplyConsumption", "Medium");
        SetString(so, "economy.operationalCost", "Moderate");
        SetString(so, "economy.deploymentRequirements", "Standard drone pad");
        SetString(so, "economy.transportCompatibility", "All aerial racks");
        SetString(so, "economy.launchMethod", "Vertical takeoff");

        // Player collection build requirements
        SetRequirements(so, "buildRequirements.commonComponents", new[]
        {
            new Requirement("Medium Alloy Plates", 16),
            new Requirement("Composite Frame Rods", 12),
            new Requirement("Micro-Servo Actuators", 6),
            new Requirement("Stabilizer Fins", 3),
            new Requirement("SAR Harness Housing", 1)
        });
        SetRequirements(so, "buildRequirements.electronics", new[]
        {
            new Requirement("Circuit Bundles", 16),
            new Requirement("Signal Processors", 12),
            new Requirement("Drone Control Chips", 4),
            new Requirement("Rescue AI Chips", 4)
        });
        SetRequirements(so, "buildRequirements.power", new[]
        {
            new Requirement("High-Capacity Rescue Battery", 1),
            new Requirement("Power Regulation Nodes", 3)
        });
        SetRequirements(so, "buildRequirements.rareTradeable", new[]
        {
            new Requirement("Survivor Detection Core", 1),
            new Requirement("Rescue Pathfinding Processor", 1)
        });
        SetStrings(so, "buildRequirements.optionalUpgrades", new[]
        {
            "Extended Battery Module",
            "Silent Rotor Kit",
            "High-Capacity Harness",
            "Anti-Jamming Shielding"
        });

        // Values not supplied by the SAR specification are intentionally
        // left unchanged. Existing prefab and store-image references are
        // never assigned or cleared by this script.

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(unit);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("SAR canonical data saved: " + AssetPath +
            ". Existing model prefab and store image were preserved.");
    }

    private struct Requirement
    {
        public string component;
        public int quantity;

        public Requirement(string component, int quantity)
        {
            this.component = component;
            this.quantity = quantity;
        }
    }

    private static SerializedProperty Property(SerializedObject so, string path)
    {
        var p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("SAR: serialized property not found: " + path);
        return p;
    }

    private static void SetString(SerializedObject so, string path, string value)
    {
        var p = Property(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        var p = Property(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Float)
            p.floatValue = value;
        else if (p != null && p.propertyType == SerializedPropertyType.Integer)
            p.intValue = Mathf.RoundToInt(value);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        var p = Property(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        var p = Property(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
    }

    private static void SetStrings(SerializedObject so, string path, string[] values)
    {
        var p = Property(so, path);
        if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String)
            return;

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            var element = p.GetArrayElementAtIndex(i);
            if (element.propertyType == SerializedPropertyType.String)
                element.stringValue = values[i];
            else
                Debug.LogWarning("SAR: " + path + " element is not a string.");
        }
    }

    private static void SetRequirements(
        SerializedObject so, string path, Requirement[] values)
    {
        var p = Property(so, path);
        if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String)
            return;

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            var element = p.GetArrayElementAtIndex(i);
            var component = element.FindPropertyRelative("component");
            var quantity = element.FindPropertyRelative("quantity");

            if (component != null)
                component.stringValue = values[i].component;
            if (quantity != null)
                quantity.intValue = values[i].quantity;
        }
    }
}

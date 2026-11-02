using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateCrusherCanonicalData
{
    private const string AssetPath = "Assets/Game/Units/Database/Ground/Crusher/Crusher.asset";

    [MenuItem("Obsidian Protocol/Units/Populate Crusher Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);
        if (target == null)
        {
            Debug.LogError("[Crusher] Asset not found: " + AssetPath +
                ". Verify the actual asset path; no asset was modified.");
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        SetString(so, "unitId", "obs-ground-crusher");
        SetString(so, "displayName", "Crusher");
        SetEnum(so, "category", "Ground");
        SetString(so, "role", "Heavy Demolition / Terrain Clearing");
        SetString(so, "tier", "Assault Engineering");
        SetString(so, "status", "Active");

        // Physical and technical
        SetFloat(so, "physical.lengthMeters", 5.1f);
        SetFloat(so, "physical.widthMeters", 2.8f);
        SetFloat(so, "physical.heightMeters", 2.3f);
        SetFloat(so, "physical.massKg", 2600f);
        SetString(so, "physical.powerSource", "Industrial diesel-electric hybrid");
        SetFloat(so, "physical.energyCapacityWh", 9000f);
        SetOptionalFloat(so, "physical.fuelCapacityLiters", 110f);
        SetString(so, "physical.endurance", "4–6 hours");
        SetFloat(so, "physical.payloadCapacityKg", 500f);
        SetStrings(so, "physical.modules", new[]
        {
            "Dual crushing arms",
            "Reinforced demolition chassis"
        });
        SetString(so, "physical.navigationSystem", "GPS + Demolition AI");
        SetFloat(so, "physical.structuralIntegrityHp", 1800f);
        SetFloat(so, "physical.armorRating", 0.90f);
        SetString(so, "physical.heatSignature", "High");
        SetString(so, "physical.noiseSignature", "Very High");
        SetFloat(so, "physical.topSpeedMetersPerSecond", 9f);
        SetString(so, "physical.topSpeedDescription", "9 m/s");
        SetString(so, "physical.acceleration", "Low");
        SetString(so, "physical.turningRadius", "Heavy pivot");
        SetString(so, "physical.stabilityRating", "Maximum");

        // Autonomy
        SetString(so, "autonomy.behaviorProfile", "Demolition / Terrain Clearing");
        SetFloat(so, "autonomy.aggression", 0.7f);
        SetFloat(so, "autonomy.caution", 0.4f);
        SetFloat(so, "autonomy.persistence", 1.0f);
        SetFloat(so, "autonomy.flankingLikelihood", 0f);
        SetFloat(so, "autonomy.retreatThreshold", 0.3f);
        SetFloat(so, "autonomy.riskTolerance", 0.8f);
        SetString(so, "autonomy.retreatBehaviorDescription", "Late; maintain demolition task until retreat threshold is reached");
        SetString(so, "autonomy.targetPriority", "Obstacles / structures / debris");
        SetString(so, "autonomy.formationDiscipline", "High");

        // Sensors
        SetFloat(so, "sensors.visualRangeMeters", 1200f);
        SetFloat(so, "sensors.thermalRangeMeters", 600f);
        SetString(so, "sensors.visualRangeDescription", "1200 m visual range");
        SetString(so, "sensors.thermalRangeDescription", "600 m thermal range");
        SetString(so, "sensors.acousticRange", "Medium");
        SetString(so, "sensors.radarRange", "None");
        SetString(so, "sensors.lidarRange", "Medium");
        SetString(so, "sensors.signalDetectionRange", "Short");

        // Communication
        SetFloat(so, "communication.meshRangeKm", 4f);
        SetString(so, "communication.meshRangeDescription", "4 km");
        SetFloat(so, "communication.relayCapacity", 0f);
        SetString(so, "communication.commandLinkType", "Engineering Grid");
        SetString(so, "communication.networkPriority", "Medium");

        // Combat and threat model
        SetFloat(so, "combat.firepowerRating", 0.25f);
        SetString(so, "combat.firepowerDescription", "Low; crushing arms only");
        SetString(so, "combat.accuracy", "N/A");
        SetString(so, "combat.stabilization", "High; reinforced demolition stance");
        SetString(so, "combat.weaponMounts", "None");
        SetString(so, "combat.weaponTypes", "Hydraulic crushing arms");
        SetFloat(so, "combat.suppressionValue", 0.25f);
        SetString(so, "combat.trackingStrength", "None");
        SetString(so, "combat.disruptionStrength", "High");
        SetString(so, "combat.breachCapability", "Very High");
        SetString(so, "combat.countermeasureRating", "Medium");
        SetInt(so, "combat.threatLevel", 7);
        SetString(so, "combat.engagementDistance", "Contact");
        SetString(so, "combat.preferredTerrain", "Urban rubble / breach zones");
        SetString(so, "combat.preferredTargets", "Structures / debris / fortifications");
        SetString(so, "combat.vulnerabilityProfile", "Ballistic / explosives");

        // Squad and command integration
        SetString(so, "squad.squadRole", "Heavy Demolition");
        SetInt(so, "squad.squadSizeContribution", 4);
        SetString(so, "squad.formationBehavior", "Front-line demolition spacing");
        SetString(so, "squad.intentInterpretationRules", "Clear obstacles and open paths");
        SetString(so, "squad.retreatBehavior", "Late");
        SetString(so, "squad.breachBehavior", "Primary");
        SetString(so, "squad.flankingBehavior", "None");
        SetString(so, "squad.reinforcementBehavior", "Supports assault and engineering squads");
        SetString(so, "squad.parentCommandUnit", "Command Core / Fusion");
        SetStrings(so, "squad.subordinateUnits", new[] { "Hammer", "Ironwalker" });
        SetStrings(so, "squad.relayNodes", new string[0]);
        SetString(so, "squad.networkDependencies", "Engineering Grid");

        // VR operator
        SetString(so, "vrOperator.operatorSeatType", "Heavy Demolition Cockpit");
        SetString(so, "vrOperator.operatorHud", "Demolition Overlay");
        SetString(so, "vrOperator.cameraFeeds", "Optical + arm cams");
        SetBool(so, "vrOperator.thermalView", true);
        SetBool(so, "vrOperator.lidarView", true);
        SetBool(so, "vrOperator.sonarView", false);
        SetBool(so, "vrOperator.tacticalOverlay", true);
        SetString(so, "vrOperator.movementControls", "Heavy vehicle drive");
        SetString(so, "vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        SetString(so, "vrOperator.abilityControls", "Crush / lift / break / clear; Demolition Mode / Stabilize / Structural Scan");
        SetString(so, "vrOperator.weaponControls", "None; hydraulic crushing tools only");
        SetString(so, "vrOperator.stabilizationControls", "Reinforced demolition stance");
        SetStrings(so, "vrOperator.operatorAbilities", new[]
        {
            "Hydraulic Crush",
            "Debris Clearing",
            "Structural Weak-Point Scan",
            "Reinforced Stance"
        });

        // Economy and logistics
        SetInt(so, "economy.creditCost", 2000);
        SetString(so, "economy.buildCost", "1300 Metal / 700 Electronics");
        SetFloat(so, "economy.buildTimeSeconds", 190f);
        SetString(so, "economy.factoryType", "Heavy Vehicle Bay");
        SetString(so, "economy.techRequirements", "Engineering Systems II");
        SetString(so, "economy.maintenanceCost", "High");
        SetFloat(so, "economy.repairTimeSeconds", 48f);
        SetString(so, "economy.supplyConsumption", "High");
        SetString(so, "economy.operationalCost", "High");
        SetString(so, "economy.deploymentRequirements", "Heavy vehicle pad");
        SetString(so, "economy.transportCompatibility", "Heavy racks");
        SetString(so, "economy.launchMethod", "Drive-out");
        SetInt(so, "economy.deploymentPoints", 0);
        SetBool(so, "economy.deploymentPointsDefined", false);

        // Player collection build requirements
        SetRequirements(so, "buildRequirements.commonComponents", new[]
        {
            new Requirement("Heavy Alloy Plates", 80),
            new Requirement("Reinforced Frame Rods", 40),
            new Requirement("Hydraulic Actuators", 14),
            new Requirement("Armor Panels", 8),
            new Requirement("Crushing Arm Assembly", 1)
        });
        SetRequirements(so, "buildRequirements.electronics", new[]
        {
            new Requirement("Circuit Bundles", 36),
            new Requirement("Signal Processors", 22),
            new Requirement("Vehicle Control Chips", 7),
            new Requirement("Engineering AI Chips", 4)
        });
        SetRequirements(so, "buildRequirements.power", new[]
        {
            new Requirement("Industrial Hybrid Engine Core", 1),
            new Requirement("Power Regulation Nodes", 3)
        });
        SetRequirements(so, "buildRequirements.rareTradeable", new[]
        {
            new Requirement("Hydraulic Crushing Core", 1)
        });
        SetStrings(so, "buildRequirements.optionalUpgrades", new[]
        {
            "Reinforced Arms",
            "Shock-Dampened Chassis",
            "Extended Fuel Tank",
            "Anti-Jamming Shielding"
        });

        // Optional fields: populated only if matching serialized properties exist.
        SetOptionalString(so, "operationalParameters.missionType", "Heavy Demolition / Terrain Clearing");
        SetOptionalString(so, "operationalParameters.optimalDeploymentZone", "Urban rubble / breach zones");
        SetOptionalFloat(so, "operationalParameters.commandRangeKm", 4f);
        SetOptionalString(so, "operationalParameters.commandAuthority", "Tactical");
        SetOptionalBool(so, "operationalParameters.autonomousOperation", true);

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Crusher] Population pass completed for " + AssetPath +
            ". Review Console warnings and verify serialized values. " +
            "Existing modelPrefab and storeImage were not changed.");
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

    private static SerializedProperty Find(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("[Crusher] Serialized property not found: " + path);
        return p;
    }

    private static void SetString(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.String)
        {
            Debug.LogWarning("[Crusher] Expected string property: " + path);
            return;
        }
        p.stringValue = value;
    }

    private static void SetOptionalString(SerializedObject so, string path, string value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p != null && p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
    }

    private static void SetFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        else Debug.LogWarning("[Crusher] Expected numeric property: " + path);
    }

    private static void SetOptionalFloat(SerializedObject so, string path, float value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
    }

    private static void SetInt(SerializedObject so, string path, int value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Integer)
        {
            Debug.LogWarning("[Crusher] Expected integer property: " + path);
            return;
        }
        p.intValue = value;
    }

    private static void SetBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Boolean)
        {
            Debug.LogWarning("[Crusher] Expected boolean property: " + path);
            return;
        }
        p.boolValue = value;
    }

    private static void SetOptionalBool(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
    }

    private static void SetEnum(SerializedObject so, string path, string value)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning("[Crusher] Expected enum property: " + path);
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

        Debug.LogWarning("[Crusher] Enum value '" + value + "' is not defined for " + path);
    }

    private static void SetStrings(SerializedObject so, string path, string[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Crusher] Expected array/list property: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType != SerializedPropertyType.String)
            {
                Debug.LogWarning("[Crusher] Expected string list element: " + path);
                return;
            }
            element.stringValue = values[i];
        }
    }

    private static void SetRequirements(SerializedObject so, string path, Requirement[] values)
    {
        SerializedProperty p = Find(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("[Crusher] Expected requirement list: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component == null || quantity == null)
            {
                Debug.LogWarning("[Crusher] Requirement entry schema mismatch: " + path);
                return;
            }

            component.stringValue = values[i].component;
            quantity.intValue = values[i].quantity;
        }
    }
}

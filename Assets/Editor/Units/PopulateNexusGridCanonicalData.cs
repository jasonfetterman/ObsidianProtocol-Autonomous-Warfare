using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateNexusGridCanonicalData
{
    private const string AssetPath =
        "Assets/Game/Units/Database/Air/NexusGrid/NexusGrid.asset";

    [MenuItem("Obsidian Protocol/Units/Populate NexusGrid Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

        if (target == null)
        {
            Debug.LogError(
                "NexusGrid asset not found at " + AssetPath +
                ". No changes were made.");
            return;
        }

        SerializedObject so = new SerializedObject(target);
        so.Update();

        // Identity
        S(so, "unitId", "obs-air-nexusgrid");
        S(so, "displayName", "NexusGrid");
        S(so, "role", "Orbital Space Station / Strategic Mesh Coordination Hub");
        S(so, "tier", "Orbital Infrastructure");
        S(so, "status", "Active");
        EnumChoice(so, "category", "Air");

        // Physical and technical
        F(so, "physical.lengthMeters", 34f);
        F(so, "physical.widthMeters", 26f);
        F(so, "physical.heightMeters", 14f);
        F(so, "physical.massKg", 1200000f);
        S(so, "physical.powerSource", "250 kW peak solar photovoltaic generation with 2,000 kWh usable battery storage");
        F(so, "physical.energyCapacityWh", 2000000f);
        F(so, "physical.energyConsumptionWhPerMinute", 1333.3334f);
        S(so, "physical.endurance", "25 hours equivalent at 80 kW average load; actual endurance depends on solar input and operating load");
        F(so, "physical.payloadCapacityKg", 20000f);
        S(so, "physical.navigationSystem", "GPS + Network Optimization AI");
        F(so, "physical.structuralIntegrityHp", 200f);
        F(so, "physical.armorRating", 0.32f);
        S(so, "physical.heatSignature", "Medium");
        S(so, "physical.noiseSignature", "Medium");
        F(so, "physical.topSpeedMetersPerSecond", 0f);
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Moderate");
        S(so, "physical.climbRate", "Medium");
        S(so, "physical.stabilityRating", "Very High");
        S(so, "physical.topSpeedDescription", "Orbital station; no atmospheric speed rating");

        Strings(so, "physical.modules",
            "Multi-node mesh anchor array");

        // Autonomy and AI
        S(so, "autonomy.behaviorProfile",
            "Strategic Network Coordination");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.9f);
        F(so, "autonomy.persistence", 1f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.9f);
        F(so, "autonomy.riskTolerance", 0.1f);
        S(so, "autonomy.targetPriority", "Mesh gaps / command nodes");
        S(so, "autonomy.formationDiscipline", "Extremely High");
        S(so, "autonomy.retreatBehaviorDescription", "Very Early");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1200f);
        F(so, "sensors.thermalRangeMeters", 500f);
        S(so, "sensors.visualRangeDescription", "1200 m");
        S(so, "sensors.thermalRangeDescription", "500 m");
        S(so, "sensors.acousticRange", "Low");
        S(so, "sensors.radarRange", "None");
        S(so, "sensors.lidarRange", "None");
        S(so, "sensors.signalDetectionRange", "Extremely High");

        // Communication and network
        F(so, "communication.meshRangeKm", 22f);
        S(so, "communication.meshRangeDescription", "22 km");
        F(so, "communication.relayCapacity", 16f);
        S(so, "communication.commandLinkType", "NexusGrid Protocol");
        S(so, "communication.networkPriority", "Maximum");

        // Combat and threat model
        F(so, "combat.firepowerRating", 0f);
        F(so, "combat.suppressionValue", 0f);
        I(so, "combat.threatLevel", 4);
        S(so, "combat.firepowerDescription", "0");
        S(so, "combat.accuracy", "N/A");
        S(so, "combat.stabilization", "Very High");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        S(so, "combat.trackingStrength", "Medium");
        S(so, "combat.disruptionStrength", "High");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "High");
        S(so, "combat.engagementDistance", "N/A");
        S(so, "combat.preferredTerrain", "Orbital operating environment");
        S(so, "combat.preferredTargets", "None");
        S(so, "combat.vulnerabilityProfile", "Ballistic / EMP / jamming");

        // Squad and command integration
        S(so, "squad.squadRole", "Strategic Mesh Anchor");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "High-discipline mesh lattice");
        S(so, "squad.intentInterpretationRules",
            "Maintain network stability above all");
        S(so, "squad.retreatBehavior", "Very Early");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior",
            "Supports command, mesh, and recon squads");
        S(so, "squad.parentCommandUnit", "Command Core");
        S(so, "squad.networkDependencies", "NexusGrid Protocol");

        Strings(so, "squad.subordinateUnits",
            "Mesh", "Relay", "Link");

        // The specification gives a count of 16 relay nodes, not
        // individual deployed node identities. Do not fabricate entries
        // in the relayNodes list; the capacity is set above.

        // VR operator mode
        S(so, "vrOperator.operatorSeatType", "Orbital Station Operations View");
        S(so, "vrOperator.operatorHud", "NexusGrid Network Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", false);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        S(so, "vrOperator.movementControls", "Station systems and network management");
        S(so, "vrOperator.sensorControls", "Signal scan / mesh diagnostics");
        S(so, "vrOperator.abilityControls",
            "Deploy Anchor, Boost Mesh, Stabilize Network");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls", "Orbital station-keeping");

        Strings(so, "vrOperator.operatorAbilities",
            "Deploy Mesh Anchor",
            "Boost Mesh",
            "Network Stabilization",
            "Mesh Diagnostics",
            "Relay Command Data");

        // Economy and logistics
        S(so, "economy.buildCost", "18000 Metal / 14000 Electronics / 6000 Advanced Components (provisional game-balance values)");
        F(so, "economy.buildTimeSeconds", 21600f);
        S(so, "economy.factoryType", "Orbital Construction Facility");
        S(so, "economy.techRequirements", "Orbital Infrastructure III");
        S(so, "economy.maintenanceCost", "Medium");
        F(so, "economy.repairTimeSeconds", 3600f);
        S(so, "economy.supplyConsumption", "Medium");
        S(so, "economy.operationalCost", "Moderate");
        S(so, "economy.deploymentRequirements", "Orbital deployment infrastructure");
        S(so, "economy.transportCompatibility", "Orbital transport and docking systems");
        S(so, "economy.launchMethod", "Orbital deployment");

        // Build requirements: preserve the collection schema.
        Requirements(so, "buildRequirements.commonComponents",
            ("Medium Alloy Plates", 18),
            ("Composite Frame Rods", 12),
            ("Robotic Maintenance Arms", 8),
            ("Orbital Attitude-Control Assemblies", 4),
            ("Mesh Anchor Housing", 1));

        Requirements(so, "buildRequirements.electronics",
            ("Circuit Bundles", 20),
            ("Signal Processors", 14),
            ("Station Flight-Critical Computers", 6),
            ("Mesh AI Chips", 6));

        Requirements(so, "buildRequirements.power",
            ("Command-Grade Mesh Battery", 1),
            ("Power Regulation Nodes", 3));

        Requirements(so, "buildRequirements.rareTradeable",
            ("NexusGrid Core", 1),
            ("Encrypted Mesh Processor", 1));

        Strings(so, "buildRequirements.optionalUpgrades",
            "Extended Mesh Range Module",
            "Anti-Jamming Shielding",
            "Station Acoustic and Vibration Damping",
            "High-Density Mesh Grid");

        // Deliberately leave unspecified fields unchanged.
        // In particular, modelPrefab and storeImage are not modified.
        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log(
            "NexusGrid canonical data saved: " + AssetPath +
            ". Existing model prefab and store image were preserved.");
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        SerializedProperty p = so.FindProperty(path);
        if (p == null)
            Debug.LogWarning("NexusGrid: serialized property not found: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        SerializedProperty p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.String)
            p.stringValue = value;
        else if (p != null)
            Debug.LogWarning("NexusGrid: expected string property: " + path);
    }

    private static void F(SerializedObject so, string path, float value)
    {
        SerializedProperty p = P(so, path);
        if (p != null)
        {
            if (p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
            else if (p.propertyType == SerializedPropertyType.Integer)
                p.intValue = Mathf.RoundToInt(value);
            else
                Debug.LogWarning("NexusGrid: expected numeric property: " + path);
        }
    }

    private static void I(SerializedObject so, string path, int value)
    {
        SerializedProperty p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Integer)
            p.intValue = value;
        else if (p != null)
            Debug.LogWarning("NexusGrid: expected integer property: " + path);
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        SerializedProperty p = P(so, path);
        if (p != null && p.propertyType == SerializedPropertyType.Boolean)
            p.boolValue = value;
        else if (p != null)
            Debug.LogWarning("NexusGrid: expected boolean property: " + path);
    }

    private static void EnumChoice(
        SerializedObject so, string path, string requested)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (p.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning(
                "NexusGrid: category is not an enum; left unchanged.");
            return;
        }

        string[] names = p.enumNames;
        for (int i = 0; i < names.Length; i++)
        {
            if (string.Equals(names[i], requested,
                StringComparison.OrdinalIgnoreCase))
            {
                p.enumValueIndex = i;
                return;
            }
        }

        Debug.LogWarning(
            "NexusGrid: category enum option '" + requested +
            "' not found; left unchanged.");
    }

    private static void Strings(
        SerializedObject so, string path, params string[] values)
    {
        SerializedProperty p = P(so, path);
        if (p == null) return;

        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("NexusGrid: expected string array: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            SerializedProperty element = p.GetArrayElementAtIndex(i);
            if (element.propertyType != SerializedPropertyType.String)
            {
                Debug.LogWarning(
                    "NexusGrid: array element is not a string: " + path);
                return;
            }
            element.stringValue = values[i];
        }
    }

    private static void Requirements(
        SerializedObject so,
        string path,
        params (string component, int quantity)[] items)
    {
        SerializedProperty array = P(so, path);
        if (array == null) return;

        if (!array.isArray ||
            array.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning(
                "NexusGrid: expected requirement array: " + path);
            return;
        }

        array.arraySize = items.Length;

        for (int i = 0; i < items.Length; i++)
        {
            SerializedProperty element = array.GetArrayElementAtIndex(i);
            SerializedProperty component = element.FindPropertyRelative("component");
            SerializedProperty quantity = element.FindPropertyRelative("quantity");

            if (component == null || quantity == null)
            {
                Debug.LogWarning(
                    "NexusGrid: requirement schema mismatch in " + path);
                continue;
            }

            component.stringValue = items[i].component;
            quantity.intValue = items[i].quantity;
        }
    }
}


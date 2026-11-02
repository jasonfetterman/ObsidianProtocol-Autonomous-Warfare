using UnityEditor;
using UnityEngine;

public static class PopulateMeshCanonicalData
{
    private const string UnitId = "obs-air-mesh";

    [MenuItem("Obsidian Protocol/Units/Populate Mesh Canonical Data")]
    public static void Populate()
    {
        string assetPath = null;
        SerializedObject so = null;

        foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition"))
        {
            string candidate = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadMainAssetAtPath(candidate);
            if (asset == null) continue;

            var serialized = new SerializedObject(asset);
            var id = serialized.FindProperty("unitId");
            if (id != null && id.stringValue == UnitId)
            {
                assetPath = candidate;
                so = serialized;
                break;
            }
        }

        if (so == null)
        {
            Debug.LogError("Mesh population failed: UnitDefinition not found for " + UnitId);
            return;
        }

        so.Update();

        void S(string path, string value)
        {
            var p = so.FindProperty(path);
            if (p == null) { Debug.LogWarning("Mesh: unsupported field " + path); return; }
            if (p.propertyType == SerializedPropertyType.String) p.stringValue = value;
        }
        void F(string path, float value)
        {
            var p = so.FindProperty(path);
            if (p == null) { Debug.LogWarning("Mesh: unsupported field " + path); return; }
            if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
            else if (p.propertyType == SerializedPropertyType.Integer) p.intValue = Mathf.RoundToInt(value);
        }
        void I(string path, int value)
        {
            var p = so.FindProperty(path);
            if (p == null) { Debug.LogWarning("Mesh: unsupported field " + path); return; }
            if (p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
            else if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        }
        void B(string path, bool value)
        {
            var p = so.FindProperty(path);
            if (p == null) { Debug.LogWarning("Mesh: unsupported field " + path); return; }
            if (p.propertyType == SerializedPropertyType.Boolean) p.boolValue = value;
        }
        void A(string path, params string[] values)
        {
            var p = so.FindProperty(path);
            if (p == null || !p.isArray) { Debug.LogWarning("Mesh: unsupported list " + path); return; }
            p.arraySize = 0;
            foreach (string value in values)
            {
                int index = p.arraySize++;
                var e = p.GetArrayElementAtIndex(index);
                if (e.propertyType == SerializedPropertyType.String) e.stringValue = value;
                else Debug.LogWarning("Mesh: expected string list at " + path);
            }
        }
        void R(string path, (string component, int quantity)[] values)
        {
            var p = so.FindProperty(path);
            if (p == null || !p.isArray) { Debug.LogWarning("Mesh: unsupported list " + path); return; }
            p.arraySize = 0;
            foreach (var value in values)
            {
                int index = p.arraySize++;
                var e = p.GetArrayElementAtIndex(index);
                var c = e.FindPropertyRelative("component");
                var q = e.FindPropertyRelative("quantity");
                if (c != null) c.stringValue = value.component;
                if (q != null) q.intValue = value.quantity;
            }
        }

        S("unitId", UnitId);
        S("displayName", "Mesh");
        S("role", "High-Capacity Mesh Relay / Network Backbone");
        S("tier", "Advanced Support");
        S("status", "Active");

        var category = so.FindProperty("category");
        if (category != null && category.propertyType == SerializedPropertyType.Enum)
        {
            for (int i = 0; i < category.enumNames.Length; i++)
                if (category.enumNames[i] == "Air") { category.enumValueIndex = i; break; }
        }

        S("physical.powerSource", "High-efficiency battery");
        F("physical.lengthMeters", 2.4f);
        F("physical.widthMeters", 1.8f);
        F("physical.heightMeters", 0.9f);
        F("physical.massKg", 20f);
        F("physical.energyCapacityWh", 2000f);
        F("physical.energyConsumptionWhPerMinute", 45f);
        S("physical.endurance", "35–55 min");
        F("physical.payloadCapacityKg", 1f);
        A("physical.modules", "Multi-antenna mesh array");
        S("physical.navigationSystem", "GPS + Mesh Optimization AI");
        F("physical.structuralIntegrityHp", 115f);
        F("physical.armorRating", 0.15f);
        S("physical.heatSignature", "Low");
        S("physical.noiseSignature", "Low");
        F("physical.topSpeedMetersPerSecond", 19f);
        S("physical.topSpeedDescription", "19 m/s");
        S("physical.acceleration", "Medium");
        S("physical.turningRadius", "Tight");
        S("physical.climbRate", "Medium");
        S("physical.stabilityRating", "High");

        S("autonomy.behaviorProfile", "Mesh Relay / Network Backbone");
        F("autonomy.aggression", 0f);
        F("autonomy.caution", 0.9f);
        F("autonomy.persistence", 0.9f);
        F("autonomy.flankingLikelihood", 0f);
        F("autonomy.retreatThreshold", 0.85f);
        F("autonomy.riskTolerance", 0.1f);
        S("autonomy.targetPriority", "Network gaps / weak mesh zones");
        S("autonomy.formationDiscipline", "Very High");
        S("autonomy.retreatBehaviorDescription", "Early");

        F("sensors.visualRangeMeters", 900f);
        F("sensors.thermalRangeMeters", 400f);
        S("sensors.visualRangeDescription", "900 m");
        S("sensors.thermalRangeDescription", "400 m");
        S("sensors.acousticRange", "Low");
        S("sensors.radarRange", "None");
        S("sensors.lidarRange", "None");
        S("sensors.signalDetectionRange", "Very High");

        F("communication.meshRangeKm", 18f);
        S("communication.meshRangeDescription", "18 km");
        F("communication.relayCapacity", 12f);
        S("communication.commandLinkType", "Mesh-Optimized");
        S("communication.networkPriority", "Very High");

        F("combat.firepowerRating", 0f);
        F("combat.suppressionValue", 0f);
        S("combat.firepowerDescription", "Unarmed mesh relay platform");
        S("combat.accuracy", "N/A");
        S("combat.stabilization", "High");
        S("combat.weaponMounts", "None");
        S("combat.weaponTypes", "None");
        S("combat.trackingStrength", "Low");
        S("combat.disruptionStrength", "Medium");
        S("combat.breachCapability", "None");
        S("combat.countermeasureRating", "Medium");
        I("combat.threatLevel", 2);
        S("combat.engagementDistance", "N/A");
        S("combat.preferredTerrain", "High-altitude mesh corridors");
        S("combat.preferredTargets", "None");
        S("combat.vulnerabilityProfile", "Ballistic / EMP / jamming");

        S("squad.squadRole", "Mesh Relay");
        I("squad.squadSizeContribution", 1);
        S("squad.formationBehavior", "High-discipline mesh spacing");
        S("squad.intentInterpretationRules", "Maintain coverage, avoid conflict");
        S("squad.retreatBehavior", "Early");
        S("squad.breachBehavior", "None");
        S("squad.flankingBehavior", "None");
        S("squad.reinforcementBehavior", "Supports communication and command squads");
        S("squad.parentCommandUnit", "NexusGrid / Overseer");
        S("squad.networkDependencies", "Warden mesh grid");
        A("squad.subordinateUnits");
        A("squad.relayNodes", "Relay Node 1", "Relay Node 2", "Relay Node 3",
            "Relay Node 4", "Relay Node 5", "Relay Node 6", "Relay Node 7",
            "Relay Node 8", "Relay Node 9", "Relay Node 10", "Relay Node 11",
            "Relay Node 12");

        S("vrOperator.operatorSeatType", "Drone View");
        S("vrOperator.operatorHud", "Mesh Relay Overlay");
        S("vrOperator.cameraFeeds", "Optical");
        B("vrOperator.thermalView", true);
        B("vrOperator.lidarView", false);
        B("vrOperator.sonarView", false);
        B("vrOperator.tacticalOverlay", true);
        S("vrOperator.movementControls", "Full drone flight");
        S("vrOperator.sensorControls", "Signal scan / relay diagnostics");
        S("vrOperator.abilityControls", "Deploy Relay, Boost Signal, Mesh Stabilize");
        S("vrOperator.weaponControls", "None");
        S("vrOperator.stabilizationControls", "Auto-hover");
        A("vrOperator.operatorAbilities", "Deploy Relay Node", "Boost Signal",
            "Mesh Stabilization", "Network Scan", "Relay Diagnostics");

        S("economy.buildCost", "160 Metal / 160 Electronics");
        F("economy.buildTimeSeconds", 55f);
        S("economy.factoryType", "Drone Bay");
        S("economy.techRequirements", "Mesh Networking II");
        S("economy.maintenanceCost", "Low");
        F("economy.repairTimeSeconds", 12f);
        S("economy.supplyConsumption", "Low");
        S("economy.operationalCost", "Minimal");
        S("economy.deploymentRequirements", "Standard drone pad");
        S("economy.transportCompatibility", "All aerial racks");
        S("economy.launchMethod", "Vertical takeoff");

        R("buildRequirements.commonComponents", new[] {
            ("Light Alloy Plates", 12), ("Composite Frame Rods", 8),
            ("Micro-Servo Actuators", 6), ("Stabilizer Fins", 2),
            ("Mesh Antenna Housing", 1)
        });
        R("buildRequirements.electronics", new[] {
            ("Circuit Bundles", 14), ("Signal Processors", 10),
            ("Drone Control Chips", 4), ("Mesh Routing Chips", 4)
        });
        R("buildRequirements.power", new[] {
            ("High-Efficiency Battery Pack", 1), ("Power Regulation Nodes", 2)
        });
        R("buildRequirements.rareTradeable", new[] {
            ("High-Grade Mesh Antenna Array", 1), ("Encrypted Mesh Core", 1)
        });
        A("buildRequirements.optionalUpgrades", "Extended Mesh Range Module",
            "Signal Boost Amplifier", "Anti-Jamming Shielding", "Silent Rotor Kit");

        S("environment.terrainAdaptationCoefficients", "High-altitude mesh corridors");
        S("environment.visibilityModifiers", "Not specified");
        S("environment.signalPropagationCurve", "Mesh-optimized; 18 km mesh range");
        S("network.packetLossRate", "Not specified");
        S("network.meshSelfHealingThreshold", "Not specified");
        S("network.relaySaturationCurve", "12-node relay capacity");

        S("tactical.reinforcementTriggerConditions", "Supports communication and command squads");
        S("tactical.retreatLogicThresholds", "Retreat threshold 0.85; very low risk tolerance");

        A("behaviorNodes");
        var behaviors = so.FindProperty("behaviorNodes");
        if (behaviors != null && behaviors.isArray)
        {
            void AddBehavior(string node, float weight, string description)
            {
                int index = behaviors.arraySize++;
                var e = behaviors.GetArrayElementAtIndex(index);
                e.FindPropertyRelative("behaviorNode").stringValue = node;
                e.FindPropertyRelative("weight").floatValue = weight;
                e.FindPropertyRelative("description").stringValue = description;
            }
            AddBehavior("Relay Coverage", 1f, "Maintain mesh coverage and bridge network gaps");
            AddBehavior("Signal Boost", 0.9f, "Improve signal strength in weak mesh zones");
            AddBehavior("Mesh Stabilization", 0.9f, "Maintain stable mesh connectivity");
        }

        A("missionBehaviors");
        var missions = so.FindProperty("missionBehaviors");
        if (missions != null && missions.isArray)
        {
            void AddMission(string type, string focus, float efficiency)
            {
                int index = missions.arraySize++;
                var e = missions.GetArrayElementAtIndex(index);
                e.FindPropertyRelative("missionType").stringValue = type;
                e.FindPropertyRelative("behaviorFocus").stringValue = focus;
                e.FindPropertyRelative("efficiency").floatValue = efficiency;
            }
            AddMission("Network Relay", "Extend and stabilize mesh coverage", 0.95f);
            AddMission("Command Support", "Maintain communication for command squads", 0.9f);
        }

        A("unitSynergies");
        var synergies = so.FindProperty("unitSynergies");
        if (synergies != null && synergies.isArray)
        {
            void AddSynergy(string partner, float rating, string function)
            {
                int index = synergies.arraySize++;
                var e = synergies.GetArrayElementAtIndex(index);
                e.FindPropertyRelative("partnerUnit").stringValue = partner;
                e.FindPropertyRelative("synergyRating").floatValue = rating;
                e.FindPropertyRelative("function").stringValue = function;
            }
            AddSynergy("Warden", 0.95f, "Extends and stabilizes the Warden mesh grid");
            AddSynergy("Command units", 0.9f, "Improves command-network connectivity");
        }

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(AssetDatabase.LoadMainAssetAtPath(assetPath));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Mesh population saved: " + assetPath);
    }
}

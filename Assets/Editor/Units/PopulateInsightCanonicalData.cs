using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.EditorTools
{
    public static class PopulateInsightCanonicalData
    {
        private const string UnitId = "obs-air-insight";

        [MenuItem("Obsidian Protocol/Units/Populate Insight Canonical Data")]
        public static void Populate()
        {
            UnitDefinition unit = null;

            foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var candidate = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);
                if (candidate == null) continue;

                var so = new SerializedObject(candidate);
                var id = so.FindProperty("unitId");
                var name = so.FindProperty("displayName");

                if ((id != null && id.stringValue == UnitId) ||
                    (name != null && name.stringValue == "Insight"))
                {
                    unit = candidate;
                    break;
                }
            }

            if (unit == null)
            {
                Debug.LogError("Insight population failed: no existing UnitDefinition found for " + UnitId);
                return;
            }

            var s = new SerializedObject(unit);
            s.Update();

            SetString(s, "unitId", UnitId);
            SetString(s, "displayName", "Insight");
            SetEnum(s, "category", "Air");
            SetString(s, "role", "Strategic Intelligence / Multi-Spectrum Recon");
            SetString(s, "tier", "Command");
            SetString(s, "status", "Active");

            F(s, "physical.lengthMeters", 3.6f);
            F(s, "physical.widthMeters", 2.8f);
            F(s, "physical.heightMeters", 1.3f);
            F(s, "physical.massKg", 34f);
            S(s, "physical.powerSource", "Command-grade battery");
            F(s, "physical.energyCapacityWh", 3200f);
            F(s, "physical.energyConsumptionWhPerMinute", 75f);
            F(s, "physical.payloadCapacityKg", 3.5f);
            S(s, "physical.endurance", "35–50 min");
            S(s, "physical.navigationSystem", "GPS + Strategic Recon AI");
            F(s, "physical.structuralIntegrityHp", 190f);
            F(s, "physical.armorRating", 0.30f);
            S(s, "physical.heatSignature", "Medium");
            S(s, "physical.noiseSignature", "Medium");
            F(s, "physical.topSpeedMetersPerSecond", 23f);
            S(s, "physical.topSpeedDescription", "23 m/s");
            S(s, "physical.acceleration", "Medium");
            S(s, "physical.turningRadius", "Moderate");
            S(s, "physical.climbRate", "Medium");
            S(s, "physical.stabilityRating", "Very High");
            StringArray(s, "physical.modules", "Multi-spectrum intelligence suite");

            S(s, "autonomy.behaviorProfile", "Strategic Recon / Intelligence");
            F(s, "autonomy.aggression", 0.1f);
            F(s, "autonomy.caution", 0.8f);
            F(s, "autonomy.persistence", 0.9f);
            F(s, "autonomy.flankingLikelihood", 0.2f);
            F(s, "autonomy.retreatThreshold", 0.65f);
            S(s, "autonomy.targetPriority", "High-value intelligence targets");
            S(s, "autonomy.formationDiscipline", "Very High");
            S(s, "autonomy.riskTolerance", "Medium");

            F(s, "sensors.visualRangeMeters", 2400f);
            F(s, "sensors.thermalRangeMeters", 1500f);
            S(s, "sensors.visualRangeDescription", "2400 m");
            S(s, "sensors.thermalRangeDescription", "1500 m");
            S(s, "sensors.acousticRange", "Medium");
            S(s, "sensors.radarRange", "Medium");
            S(s, "sensors.lidarRange", "Medium");
            S(s, "sensors.signalDetectionRange", "Very High");

            F(s, "communication.meshRangeKm", 10f);
            S(s, "communication.meshRangeDescription", "10 km");
            F(s, "communication.relayCapacity", 4f);
            S(s, "communication.commandLinkType", "Intelligence Grid");
            S(s, "communication.networkPriority", "Very High");

            F(s, "combat.firepowerRating", 0.25f);
            F(s, "combat.suppressionValue", 0.5f);
            S(s, "combat.firepowerDescription", "Low");
            S(s, "combat.accuracy", "Medium");
            S(s, "combat.stabilization", "Very High");
            S(s, "combat.weaponMounts", "1 (marker beam / deterrent pulse)");
            S(s, "combat.weaponTypes", "Marker beam / deterrent pulse");
            S(s, "combat.trackingStrength", "Very High");
            S(s, "combat.disruptionStrength", "Medium");
            S(s, "combat.breachCapability", "None");
            S(s, "combat.countermeasureRating", "Medium");
            I(s, "combat.threatLevel", 5);
            S(s, "combat.engagementDistance", "200–400 m");
            S(s, "combat.preferredTerrain", "High-visibility recon airspace");
            S(s, "combat.preferredTargets", "High-value targets / command nodes");
            S(s, "combat.vulnerabilityProfile", "Ballistic / EMP / jamming");

            S(s, "squad.squadRole", "Strategic Recon");
            I(s, "squad.squadSizeContribution", 1);
            S(s, "squad.formationBehavior", "Command-grade spacing");
            S(s, "squad.intentInterpretationRules", "Maintain oversight, coordinate recon units");
            S(s, "squad.retreatBehavior", "Medium");
            S(s, "squad.breachBehavior", "None");
            S(s, "squad.flankingBehavior", "Light");
            S(s, "squad.reinforcementBehavior", "Supports recon, mesh, and command squads");
            S(s, "squad.parentCommandUnit", "Command Core");
            S(s, "squad.networkDependencies", "Intelligence Grid");
            StringArray(s, "squad.subordinateUnits", "Iris", "Locator", "Nightguard", "ScoutEye");
            StringArray(s, "squad.relayNodes", "Relay Node 1", "Relay Node 2", "Relay Node 3", "Relay Node 4");

            S(s, "vrOperator.operatorSeatType", "Command Drone View");
            S(s, "vrOperator.operatorHud", "Multi-Spectrum Intelligence Overlay");
            S(s, "vrOperator.cameraFeeds", "Optical");
            B(s, "vrOperator.thermalView", true);
            B(s, "vrOperator.lidarView", true);
            B(s, "vrOperator.tacticalOverlay", true);
            S(s, "vrOperator.movementControls", "Full drone flight");
            S(s, "vrOperator.sensorControls", "Multi-sensor switching");
            S(s, "vrOperator.abilityControls", "Mark Target, Intelligence Ping, Scan");
            S(s, "vrOperator.weaponControls", "Marker beam / deterrent pulse");
            S(s, "vrOperator.stabilizationControls", "Auto-hover");
            StringArray(s, "vrOperator.operatorAbilities",
                "Intelligence Ping", "Multi-Spectrum Scan", "Target Mark",
                "Squad Coordination", "Relay Intelligence Data");

            S(s, "economy.buildCost", "280 Metal / 260 Electronics");
            F(s, "economy.buildTimeSeconds", 80f);
            S(s, "economy.factoryType", "Drone Bay");
            S(s, "economy.techRequirements", "Intelligence Systems I");
            S(s, "economy.maintenanceCost", "Medium");
            F(s, "economy.repairTimeSeconds", 20f);
            S(s, "economy.supplyConsumption", "Medium");
            S(s, "economy.operationalCost", "Moderate");
            S(s, "economy.deploymentRequirements", "Command drone pad");
            S(s, "economy.transportCompatibility", "All aerial racks");
            S(s, "economy.launchMethod", "Vertical takeoff");

            Resources(s, "buildRequirements.commonComponents",
                ("Medium Alloy Plates", 20),
                ("Composite Frame Rods", 12),
                ("Micro-Servo Actuators", 8),
                ("Stabilizer Fins", 4),
                ("Intelligence Sensor Housing", 1));

            Resources(s, "buildRequirements.electronics",
                ("Circuit Bundles", 20),
                ("Signal Processors", 14),
                ("Drone Control Chips", 6),
                ("Intelligence AI Chips", 4));

            Resources(s, "buildRequirements.power",
                ("Command-Grade Battery Pack", 1),
                ("Power Regulation Nodes", 3));

            Resources(s, "buildRequirements.rareTradeable",
                ("Multi-Spectrum Intelligence Core", 1),
                ("Encrypted Intelligence Processor", 1));

            StringArray(s, "buildRequirements.optionalUpgrades",
                "Extended Intelligence Range Module",
                "Anti-Jamming Shielding",
                "Silent Rotor Kit",
                "High-Density Sensor Grid");

            s.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(unit);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Insight canonical data saved: " +
                AssetDatabase.GetAssetPath(unit) +
                ". Existing modelPrefab and storeImage were preserved. Radar View is not represented by the current UnitDefinition schema.");
        }

        private static void SetString(SerializedObject s, string path, string value)
        {
            var p = s.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.String) p.stringValue = value;
        }

        private static void S(SerializedObject s, string path, string value) => SetString(s, path, value);

        private static void F(SerializedObject s, string path, float value)
        {
            var p = s.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        }

        private static void I(SerializedObject s, string path, int value)
        {
            var p = s.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        }

        private static void B(SerializedObject s, string path, bool value)
        {
            var p = s.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.Boolean) p.boolValue = value;
        }

        private static void SetEnum(SerializedObject s, string path, string value)
        {
            var p = s.FindProperty(path);
            if (p == null || p.propertyType != SerializedPropertyType.Enum) return;
            for (int i = 0; i < p.enumNames.Length; i++)
                if (string.Equals(p.enumNames[i], value, System.StringComparison.OrdinalIgnoreCase))
                {
                    p.enumValueIndex = i;
                    return;
                }
        }

        private static void StringArray(SerializedObject s, string path, params string[] values)
        {
            var p = s.FindProperty(path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String) return;
            p.arraySize = 0;

            foreach (string value in values)
            {
                int index = p.arraySize;
                p.arraySize++;
                var e = p.GetArrayElementAtIndex(index);

                if (e.propertyType == SerializedPropertyType.String)
                    e.stringValue = value;
                else
                {
                    SetChildString(e, "moduleName", value);
                    SetChildString(e, "name", value);
                    SetChildString(e, "value", value);
                    SetChildString(e, "abilityName", value);
                }
            }
        }

        private static void Resources(SerializedObject s, string path, params (string name, int quantity)[] values)
        {
            var p = s.FindProperty(path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String) return;
            p.arraySize = 0;

            foreach (var value in values)
            {
                int index = p.arraySize;
                p.arraySize++;
                var e = p.GetArrayElementAtIndex(index);
                SetChildString(e, "component", value.name);
                SetChildString(e, "name", value.name);
                SetChildInt(e, "quantity", value.quantity);
            }
        }

        private static void SetChildString(SerializedProperty parent, string childName, string value)
        {
            var p = parent.FindPropertyRelative(childName);
            if (p != null && p.propertyType == SerializedPropertyType.String) p.stringValue = value;
        }

        private static void SetChildInt(SerializedProperty parent, string childName, int value)
        {
            var p = parent.FindPropertyRelative(childName);
            if (p != null && p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        }
    }
}


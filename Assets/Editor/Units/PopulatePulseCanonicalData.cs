using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.EditorTools
{
    public static class PopulatePulseCanonicalData
    {
        private const string UnitId = "obs-air-pulse";

        [MenuItem("Obsidian Protocol/Units/Populate Pulse Canonical Data")]
        public static void Populate()
        {
            UnitDefinition unit = null;

            foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var candidate = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);
                if (candidate == null) continue;

                var check = new SerializedObject(candidate);
                var id = check.FindProperty("unitId");
                var name = check.FindProperty("displayName");

                if ((id != null && id.stringValue == UnitId) ||
                    (name != null && name.stringValue == "Pulse"))
                {
                    unit = candidate;
                    break;
                }
            }

            if (unit == null)
            {
                Debug.LogError("Pulse population failed: existing UnitDefinition not found for " + UnitId);
                return;
            }

            var s = new SerializedObject(unit);
            s.Update();

            S(s, "unitId", UnitId);
            S(s, "displayName", "Pulse");
            E(s, "category", "Air");
            S(s, "role", "Electronic Pulse / Area Disruption Drone");
            S(s, "tier", "Advanced Support");
            S(s, "status", "Active");

            F(s, "physical.lengthMeters", 2.7f);
            F(s, "physical.widthMeters", 2.0f);
            F(s, "physical.heightMeters", 1.0f);
            F(s, "physical.massKg", 24f);
            S(s, "physical.powerSource", "High-discharge battery");
            F(s, "physical.energyCapacityWh", 2600f);
            F(s, "physical.energyConsumptionWhPerMinute", 65f);
            S(s, "physical.endurance", "25–40 min");
            F(s, "physical.payloadCapacityKg", 1.5f);
            S(s, "physical.navigationSystem", "GPS + Disruption AI");
            F(s, "physical.structuralIntegrityHp", 140f);
            F(s, "physical.armorRating", 0.22f);
            S(s, "physical.heatSignature", "Medium");
            S(s, "physical.noiseSignature", "Medium");
            F(s, "physical.topSpeedMetersPerSecond", 20f);
            S(s, "physical.topSpeedDescription", "20 m/s");
            S(s, "physical.acceleration", "Medium");
            S(s, "physical.turningRadius", "Tight");
            S(s, "physical.climbRate", "Medium");
            S(s, "physical.stabilityRating", "High");
            A(s, "physical.modules", "Pulse emitter array");

            S(s, "autonomy.behaviorProfile", "Electronic Disruption / Support");
            F(s, "autonomy.aggression", 0.3f);
            F(s, "autonomy.caution", 0.7f);
            F(s, "autonomy.persistence", 0.8f);
            F(s, "autonomy.flankingLikelihood", 0.2f);
            F(s, "autonomy.retreatThreshold", 0.6f);
            S(s, "autonomy.targetPriority", "Electronic systems / drones");
            S(s, "autonomy.formationDiscipline", "Medium");
            S(s, "autonomy.riskTolerance", "Medium");

            F(s, "sensors.visualRangeMeters", 1200f);
            F(s, "sensors.thermalRangeMeters", 600f);
            S(s, "sensors.visualRangeDescription", "1200 m");
            S(s, "sensors.thermalRangeDescription", "600 m");
            S(s, "sensors.acousticRange", "Medium");
            S(s, "sensors.radarRange", "None");
            S(s, "sensors.lidarRange", "None");
            S(s, "sensors.signalDetectionRange", "High");

            F(s, "communication.meshRangeKm", 6f);
            S(s, "communication.meshRangeDescription", "6 km");
            F(s, "communication.relayCapacity", 0f);
            S(s, "communication.commandLinkType", "Disruption Grid");
            S(s, "communication.networkPriority", "High");

            F(s, "combat.firepowerRating", 0.5f);
            F(s, "combat.suppressionValue", 0.8f);
            S(s, "combat.firepowerDescription", "Medium (non-lethal electronic disruption)");
            S(s, "combat.accuracy", "Medium");
            S(s, "combat.stabilization", "High");
            S(s, "combat.weaponMounts", "1 (pulse emitter)");
            S(s, "combat.weaponTypes", "EMP pulse / disruption wave");
            S(s, "combat.trackingStrength", "Medium");
            S(s, "combat.disruptionStrength", "Very High");
            S(s, "combat.breachCapability", "None");
            S(s, "combat.countermeasureRating", "Medium");
            I(s, "combat.threatLevel", 5);
            S(s, "combat.engagementDistance", "100–300 m");
            S(s, "combat.preferredTerrain", "Electronic warfare corridors");
            S(s, "combat.preferredTargets", "Drones / sensors / electronics");
            S(s, "combat.vulnerabilityProfile", "Ballistic / weather");

            S(s, "squad.squadRole", "Electronic Warfare Support");
            I(s, "squad.squadSizeContribution", 1);
            S(s, "squad.formationBehavior", "Disruption spacing");
            S(s, "squad.intentInterpretationRules", "Prioritize electronic suppression");
            S(s, "squad.retreatBehavior", "Medium");
            S(s, "squad.breachBehavior", "None");
            S(s, "squad.flankingBehavior", "Light");
            S(s, "squad.reinforcementBehavior", "Supports recon, mesh, and command squads");
            S(s, "squad.parentCommandUnit", "Overseer / Insight");
            S(s, "squad.networkDependencies", "Disruption Grid");
            A(s, "squad.subordinateUnits");
            A(s, "squad.relayNodes");

            S(s, "vrOperator.operatorSeatType", "Drone View");
            S(s, "vrOperator.operatorHud", "Electronic Warfare Overlay");
            S(s, "vrOperator.cameraFeeds", "Optical");
            B(s, "vrOperator.thermalView", true);
            B(s, "vrOperator.lidarView", false);
            B(s, "vrOperator.tacticalOverlay", true);
            S(s, "vrOperator.movementControls", "Full drone flight");
            S(s, "vrOperator.sensorControls", "Signal scan / disruption diagnostics");
            S(s, "vrOperator.abilityControls", "Pulse, Wave, Stabilize");
            S(s, "vrOperator.weaponControls", "EMP pulse");
            S(s, "vrOperator.stabilizationControls", "Auto-hover");
            A(s, "vrOperator.operatorAbilities",
                "EMP Pulse", "Disruption Wave", "Signal Scan", "Stabilize", "Relay EW Data");

            S(s, "economy.buildCost", "200 Metal / 210 Electronics");
            F(s, "economy.buildTimeSeconds", 65f);
            S(s, "economy.factoryType", "Drone Bay");
            S(s, "economy.techRequirements", "Electronic Warfare I");
            S(s, "economy.maintenanceCost", "Medium");
            F(s, "economy.repairTimeSeconds", 16f);
            S(s, "economy.supplyConsumption", "Medium");
            S(s, "economy.operationalCost", "Moderate");
            S(s, "economy.deploymentRequirements", "Standard drone pad");
            S(s, "economy.transportCompatibility", "All aerial racks");
            S(s, "economy.launchMethod", "Vertical takeoff");

            R(s, "buildRequirements.commonComponents",
                ("Medium Alloy Plates", 14),
                ("Composite Frame Rods", 10),
                ("Micro-Servo Actuators", 6),
                ("Stabilizer Fins", 3),
                ("Pulse Emitter Housing", 1));

            R(s, "buildRequirements.electronics",
                ("Circuit Bundles", 16),
                ("Signal Processors", 12),
                ("Drone Control Chips", 4),
                ("Disruption AI Chips", 4));

            R(s, "buildRequirements.power",
                ("High-Discharge Battery Pack", 1),
                ("Power Regulation Nodes", 3));

            R(s, "buildRequirements.rareTradeable",
                ("EMP Pulse Core", 1),
                ("Electronic Disruption Processor", 1));

            A(s, "buildRequirements.optionalUpgrades",
                "Extended Pulse Range Module",
                "Anti-Jamming Shielding",
                "Silent Rotor Kit",
                "High-Density Disruption Grid");

            s.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(unit);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Pulse canonical data saved: " + AssetDatabase.GetAssetPath(unit) +
                ". Existing modelPrefab and storeImage preserved. Radar View is not a dedicated field in the current schema.");
        }

        private static void S(SerializedObject s, string path, string value)
        {
            var p = s.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.String) p.stringValue = value;
        }

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

        private static void E(SerializedObject s, string path, string value)
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

        private static void A(SerializedObject s, string path, params string[] values)
        {
            var p = s.FindProperty(path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String) return;
            p.arraySize = 0;
            foreach (string value in values)
            {
                int index = p.arraySize++;
                var e = p.GetArrayElementAtIndex(index);
                if (e.propertyType == SerializedPropertyType.String) e.stringValue = value;
                else
                {
                    ChildString(e, "moduleName", value);
                    ChildString(e, "name", value);
                    ChildString(e, "value", value);
                    ChildString(e, "abilityName", value);
                }
            }
        }

        private static void R(SerializedObject s, string path, params (string name, int quantity)[] values)
        {
            var p = s.FindProperty(path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String) return;
            p.arraySize = 0;
            foreach (var value in values)
            {
                int index = p.arraySize++;
                var e = p.GetArrayElementAtIndex(index);
                ChildString(e, "component", value.name);
                ChildString(e, "name", value.name);
                ChildInt(e, "quantity", value.quantity);
            }
        }

        private static void ChildString(SerializedProperty parent, string child, string value)
        {
            var p = parent.FindPropertyRelative(child);
            if (p != null && p.propertyType == SerializedPropertyType.String) p.stringValue = value;
        }

        private static void ChildInt(SerializedProperty parent, string child, int value)
        {
            var p = parent.FindPropertyRelative(child);
            if (p != null && p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        }
    }
}

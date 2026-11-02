using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.EditorTools
{
    public static class PopulateRelayCanonicalData
    {
        private const string UnitId = "obs-air-relay";

        [MenuItem("Obsidian Protocol/Units/Populate Relay Canonical Data")]
        public static void Populate()
        {
            UnitDefinition unit = null;

            foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var candidate = AssetDatabase.LoadAssetAtPath<UnitDefinition>(path);
                if (candidate == null) continue;

                var check = new SerializedObject(candidate);
                var id = check.FindProperty("unitId");
                var name = check.FindProperty("displayName");

                if ((id != null && id.stringValue == UnitId) ||
                    (name != null && name.stringValue == "Relay"))
                {
                    unit = candidate;
                    break;
                }
            }

            if (unit == null)
            {
                Debug.LogError("Relay population failed: existing UnitDefinition not found for " + UnitId);
                return;
            }

            var s = new SerializedObject(unit);
            s.Update();

            S(s, "unitId", UnitId);
            S(s, "displayName", "Relay");
            E(s, "category", "Air");
            S(s, "role", "Tactical Signal Relay / Battlefield Communications");
            S(s, "tier", "Operational Support");
            S(s, "status", "Active");

            F(s, "physical.lengthMeters", 2.1f);
            F(s, "physical.widthMeters", 1.6f);
            F(s, "physical.heightMeters", 0.8f);
            F(s, "physical.massKg", 16f);
            S(s, "physical.powerSource", "High-efficiency battery");
            F(s, "physical.energyCapacityWh", 1800f);
            F(s, "physical.energyConsumptionWhPerMinute", 35f);
            S(s, "physical.endurance", "40–60 min");
            F(s, "physical.payloadCapacityKg", 1f);
            S(s, "physical.navigationSystem", "GPS + Signal Routing AI");
            F(s, "physical.structuralIntegrityHp", 110f);
            F(s, "physical.armorRating", 0.14f);
            S(s, "physical.heatSignature", "Low");
            S(s, "physical.noiseSignature", "Low");
            F(s, "physical.topSpeedMetersPerSecond", 18f);
            S(s, "physical.topSpeedDescription", "18 m/s");
            S(s, "physical.acceleration", "Medium");
            S(s, "physical.turningRadius", "Tight");
            S(s, "physical.climbRate", "Medium");
            S(s, "physical.stabilityRating", "High");
            A(s, "physical.modules", "Tactical relay antenna");

            S(s, "autonomy.behaviorProfile", "Tactical Communications / Relay");
            F(s, "autonomy.aggression", 0f);
            F(s, "autonomy.caution", 0.85f);
            F(s, "autonomy.persistence", 0.8f);
            F(s, "autonomy.flankingLikelihood", 0f);
            F(s, "autonomy.retreatThreshold", 0.8f);
            S(s, "autonomy.targetPriority", "Signal gaps / comms dead zones");
            S(s, "autonomy.formationDiscipline", "High");
            S(s, "autonomy.riskTolerance", "Low");

            F(s, "sensors.visualRangeMeters", 900f);
            F(s, "sensors.thermalRangeMeters", 400f);
            S(s, "sensors.visualRangeDescription", "900 m");
            S(s, "sensors.thermalRangeDescription", "400 m");
            S(s, "sensors.acousticRange", "Low");
            S(s, "sensors.radarRange", "None");
            S(s, "sensors.lidarRange", "None");
            S(s, "sensors.signalDetectionRange", "High");

            F(s, "communication.meshRangeKm", 12f);
            S(s, "communication.meshRangeDescription", "12 km");
            F(s, "communication.relayCapacity", 8f);
            S(s, "communication.commandLinkType", "Tactical Grid");
            S(s, "communication.networkPriority", "High");

            F(s, "combat.firepowerRating", 0f);
            F(s, "combat.suppressionValue", 0f);
            S(s, "combat.firepowerDescription", "0");
            S(s, "combat.accuracy", "N/A");
            S(s, "combat.stabilization", "High");
            S(s, "combat.weaponMounts", "None");
            S(s, "combat.weaponTypes", "None");
            S(s, "combat.trackingStrength", "Low");
            S(s, "combat.disruptionStrength", "Medium");
            S(s, "combat.breachCapability", "None");
            S(s, "combat.countermeasureRating", "Medium");
            I(s, "combat.threatLevel", 2);
            S(s, "combat.engagementDistance", "N/A");
            S(s, "combat.preferredTerrain", "Mid-altitude comms corridors");
            S(s, "combat.preferredTargets", "None");
            S(s, "combat.vulnerabilityProfile", "Ballistic / EMP / jamming");

            S(s, "squad.squadRole", "Tactical Relay");
            I(s, "squad.squadSizeContribution", 1);
            S(s, "squad.formationBehavior", "Relay-optimized spacing");
            S(s, "squad.intentInterpretationRules", "Maintain coverage, avoid conflict");
            S(s, "squad.retreatBehavior", "Early");
            S(s, "squad.breachBehavior", "None");
            S(s, "squad.flankingBehavior", "None");
            S(s, "squad.reinforcementBehavior", "Supports mesh, command, and recon squads");
            S(s, "squad.parentCommandUnit", "NexusGrid / Overseer");
            S(s, "squad.networkDependencies", "Tactical Grid");
            A(s, "squad.subordinateUnits");
            A(s, "squad.relayNodes",
                "Relay Node 1", "Relay Node 2", "Relay Node 3", "Relay Node 4",
                "Relay Node 5", "Relay Node 6", "Relay Node 7", "Relay Node 8");

            S(s, "vrOperator.operatorSeatType", "Drone View");
            S(s, "vrOperator.operatorHud", "Tactical Relay Overlay");
            S(s, "vrOperator.cameraFeeds", "Optical");
            B(s, "vrOperator.thermalView", true);
            B(s, "vrOperator.lidarView", false);
            B(s, "vrOperator.sonarView", false);
            B(s, "vrOperator.tacticalOverlay", true);
            S(s, "vrOperator.movementControls", "Full drone flight");
            S(s, "vrOperator.sensorControls", "Signal scan / relay diagnostics");
            S(s, "vrOperator.abilityControls", "Deploy Relay, Boost Signal");
            S(s, "vrOperator.weaponControls", "None");
            S(s, "vrOperator.stabilizationControls", "Auto-hover");
            A(s, "vrOperator.operatorAbilities",
                "Deploy Relay Node", "Boost Signal", "Tactical Scan",
                "Relay Diagnostics", "Network Stabilization");

            S(s, "economy.buildCost", "140 Metal / 130 Electronics");
            F(s, "economy.buildTimeSeconds", 45f);
            S(s, "economy.factoryType", "Drone Bay");
            S(s, "economy.techRequirements", "Tactical Networking I");
            S(s, "economy.maintenanceCost", "Low");
            F(s, "economy.repairTimeSeconds", 10f);
            S(s, "economy.supplyConsumption", "Low");
            S(s, "economy.operationalCost", "Minimal");
            S(s, "economy.deploymentRequirements", "Standard drone pad");
            S(s, "economy.transportCompatibility", "All aerial racks");
            S(s, "economy.launchMethod", "Vertical takeoff");

            R(s, "buildRequirements.commonComponents",
                ("Light Alloy Plates", 10),
                ("Composite Frame Rods", 6),
                ("Micro-Servo Actuators", 4),
                ("Stabilizer Fins", 2),
                ("Relay Antenna Housing", 1));

            R(s, "buildRequirements.electronics",
                ("Circuit Bundles", 12),
                ("Signal Processors", 8),
                ("Drone Control Chips", 4),
                ("Relay AI Chips", 2));

            R(s, "buildRequirements.power",
                ("High-Efficiency Battery Pack", 1),
                ("Power Regulation Nodes", 2));

            R(s, "buildRequirements.rareTradeable",
                ("Tactical Relay Core", 1),
                ("Signal Routing Processor", 1));

            A(s, "buildRequirements.optionalUpgrades",
                "Extended Relay Range Module",
                "Anti-Jamming Shielding",
                "Silent Rotor Kit",
                "Signal Boost Amplifier");

            s.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(unit);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Relay canonical data saved: " + AssetDatabase.GetAssetPath(unit) +
                ". Existing modelPrefab and storeImage preserved.");
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
                int index = p.arraySize;
                p.arraySize++;
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
                int index = p.arraySize;
                p.arraySize++;
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

using UnityEngine;
using UnityEditor;

namespace ObsidianProtocol.EditorTools
{
    public static class PopulateOverseerCanonicalData
    {
        private const string UnitId = "obs-air-overseer";

        [MenuItem("Obsidian Protocol/Units/Populate Overseer Canonical Data")]
        public static void Populate()
        {
            string[] guids = AssetDatabase.FindAssets(
                "t:UnitDefinition",
                new[] { "Assets/Game/Units/Database" });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadMainAssetAtPath(path);
                if (asset == null) continue;

                var so = new SerializedObject(asset);
                var id = so.FindProperty("unitId");
                if (id == null || id.stringValue != UnitId) continue;

                so.Update();

                S(so, "displayName", "Overseer");
                E(so, "category", "Air");
                S(so, "role", "Command-Grade Recon / Multi-Sensor Coordination");
                S(so, "tier", "Command");
                S(so, "status", "Active");

                F(so, "physical.lengthMeters", 3.4f);
                F(so, "physical.widthMeters", 2.6f);
                F(so, "physical.heightMeters", 1.2f);
                F(so, "physical.massKg", 32f);
                S(so, "physical.powerSource", "High-capacity command battery");
                F(so, "physical.energyCapacityWh", 3000f);
                F(so, "physical.energyConsumptionWhPerMinute", 70f);
                S(so, "physical.endurance", "35–50 min");
                F(so, "physical.payloadCapacityKg", 3f);
                Strings(so, "physical.modules", "Multi-sensor command suite");
                S(so, "physical.navigationSystem", "GPS + Command-AI Pathing");
                F(so, "physical.structuralIntegrityHp", 180f);
                F(so, "physical.armorRating", 0.28f);
                S(so, "physical.heatSignature", "Medium");
                S(so, "physical.noiseSignature", "Medium");
                F(so, "physical.topSpeedMetersPerSecond", 22f);
                S(so, "physical.topSpeedDescription", "22 m/s");
                S(so, "physical.acceleration", "Medium");
                S(so, "physical.turningRadius", "Moderate");
                S(so, "physical.climbRate", "Medium");
                S(so, "physical.stabilityRating", "Very High");

                S(so, "autonomy.behaviorProfile", "Command Recon / Coordination");
                F(so, "autonomy.aggression", 0.2f);
                F(so, "autonomy.caution", 0.7f);
                F(so, "autonomy.persistence", 0.9f);
                F(so, "autonomy.flankingLikelihood", 0.2f);
                F(so, "autonomy.retreatThreshold", 0.6f);
                S(so, "autonomy.targetPriority", "High-value targets");
                S(so, "autonomy.formationDiscipline", "Very High");
                F(so, "autonomy.riskTolerance", 0.5f);
                S(so, "autonomy.retreatBehaviorDescription", "Medium");

                F(so, "sensors.visualRangeMeters", 2200f);
                F(so, "sensors.thermalRangeMeters", 1400f);
                S(so, "sensors.visualRangeDescription", "2200 m");
                S(so, "sensors.thermalRangeDescription", "1400 m");
                S(so, "sensors.acousticRange", "Medium");
                S(so, "sensors.radarRange", "Short-range");
                S(so, "sensors.lidarRange", "Medium");
                S(so, "sensors.signalDetectionRange", "Very High");

                F(so, "communication.meshRangeKm", 10f);
                S(so, "communication.meshRangeDescription", "10 km");
                F(so, "communication.relayCapacity", 4f);
                S(so, "communication.commandLinkType", "Command Grid");
                S(so, "communication.networkPriority", "Very High");

                F(so, "combat.firepowerRating", 0.25f);
                S(so, "combat.firepowerDescription", "Low; command-grade deterrent");
                S(so, "combat.accuracy", "Medium");
                S(so, "combat.stabilization", "Very High");
                S(so, "combat.weaponMounts", "1 (non-lethal deterrent or marker beam)");
                S(so, "combat.weaponTypes", "Marker beam / deterrent pulse");
                F(so, "combat.suppressionValue", 0.5f);
                S(so, "combat.trackingStrength", "Very High");
                S(so, "combat.disruptionStrength", "Medium");
                S(so, "combat.breachCapability", "None");
                S(so, "combat.countermeasureRating", "Medium");
                I(so, "combat.threatLevel", 5);
                S(so, "combat.engagementDistance", "200–400 m");
                S(so, "combat.preferredTerrain", "High-visibility command airspace");
                S(so, "combat.preferredTargets", "High-value targets / command nodes");
                S(so, "combat.vulnerabilityProfile", "Ballistic / EMP / jamming");

                S(so, "squad.squadRole", "Air Command Recon");
                I(so, "squad.squadSizeContribution", 1);
                S(so, "squad.formationBehavior", "Command-grade spacing");
                S(so, "squad.intentInterpretationRules", "Maintain oversight, coordinate recon units");
                S(so, "squad.retreatBehavior", "Medium");
                S(so, "squad.breachBehavior", "None");
                S(so, "squad.flankingBehavior", "Light");
                S(so, "squad.reinforcementBehavior", "Supports recon, mesh, and targeting squads");
                S(so, "squad.parentCommandUnit", "Command Core");
                Strings(so, "squad.subordinateUnits", "Warden", "Iris", "Locator", "Nightguard");
                Strings(so, "squad.relayNodes", "Relay Node 1", "Relay Node 2", "Relay Node 3", "Relay Node 4");
                S(so, "squad.networkDependencies", "Command Grid");

                S(so, "vrOperator.operatorSeatType", "Command Drone View");
                S(so, "vrOperator.operatorHud", "Multi-Sensor Command Overlay");
                S(so, "vrOperator.cameraFeeds", "Optical");
                B(so, "vrOperator.thermalView", true);
                B(so, "vrOperator.lidarView", true);
                B(so, "vrOperator.sonarView", false);
                B(so, "vrOperator.tacticalOverlay", true);
                S(so, "vrOperator.movementControls", "Full drone flight");
                S(so, "vrOperator.sensorControls", "Multi-sensor switching");
                S(so, "vrOperator.abilityControls", "Mark Target, Command Ping, Scan");
                S(so, "vrOperator.weaponControls", "Marker beam / deterrent pulse");
                S(so, "vrOperator.stabilizationControls", "Auto-hover");
                Strings(so, "vrOperator.operatorAbilities",
                    "Command Ping", "Multi-Sensor Scan", "Target Mark",
                    "Squad Coordination", "Relay Command Data");

                S(so, "economy.buildCost", "260 Metal / 240 Electronics");
                F(so, "economy.buildTimeSeconds", 75f);
                S(so, "economy.factoryType", "Drone Bay");
                S(so, "economy.techRequirements", "Command Systems I");
                S(so, "economy.maintenanceCost", "Medium");
                F(so, "economy.repairTimeSeconds", 20f);
                S(so, "economy.supplyConsumption", "Medium");
                S(so, "economy.operationalCost", "Moderate");
                S(so, "economy.deploymentRequirements", "Command drone pad");
                S(so, "economy.transportCompatibility", "All aerial racks");
                S(so, "economy.launchMethod", "Vertical takeoff");

                Resources(so, "buildRequirements.commonComponents",
                    new[] { "Medium Alloy Plates", "Composite Frame Rods",
                        "Micro-Servo Actuators", "Stabilizer Fins",
                        "Command Sensor Housing" },
                    new[] { 18, 12, 8, 4, 1 });

                Resources(so, "buildRequirements.electronics",
                    new[] { "Circuit Bundles", "Signal Processors",
                        "Drone Control Chips", "Command AI Chips" },
                    new[] { 18, 12, 6, 4 });

                Resources(so, "buildRequirements.power",
                    new[] { "High-Capacity Command Battery", "Power Regulation Nodes" },
                    new[] { 1, 3 });

                Resources(so, "buildRequirements.rareTradeable",
                    new[] { "Command-Grade Multi-Sensor Core", "Encrypted Command Processor" },
                    new[] { 1, 1 });

                Strings(so, "buildRequirements.optionalUpgrades",
                    "Extended Command Range Module", "Anti-Jamming Shielding",
                    "Silent Rotor Kit", "High-Density Sensor Grid");

                so.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();

                Debug.Log("Overseer population saved: " + path +
                    ". Existing model prefab and store image preserved.", asset);
                return;
            }

            Debug.LogError("Overseer population failed: UnitDefinition ID '" +
                UnitId + "' not found under Assets/Game/Units/Database.");
        }

        private static SerializedProperty P(SerializedObject so, string path) =>
            so.FindProperty(path);

        private static void S(SerializedObject so, string path, string value)
        {
            var p = P(so, path);
            if (p != null && p.propertyType == SerializedPropertyType.String)
                p.stringValue = value;
        }

        private static void F(SerializedObject so, string path, float value)
        {
            var p = P(so, path);
            if (p != null && p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
        }

        private static void I(SerializedObject so, string path, int value)
        {
            var p = P(so, path);
            if (p != null && p.propertyType == SerializedPropertyType.Integer)
                p.intValue = value;
        }

        private static void B(SerializedObject so, string path, bool value)
        {
            var p = P(so, path);
            if (p != null && p.propertyType == SerializedPropertyType.Boolean)
                p.boolValue = value;
        }

        private static void E(SerializedObject so, string path, string value)
        {
            var p = P(so, path);
            if (p == null || p.propertyType != SerializedPropertyType.Enum) return;

            for (int i = 0; i < p.enumNames.Length; i++)
                if (string.Equals(p.enumNames[i], value,
                    System.StringComparison.OrdinalIgnoreCase))
                {
                    p.enumValueIndex = i;
                    return;
                }
        }

        private static void Strings(SerializedObject so, string path, params string[] values)
        {
            var p = P(so, path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String)
                return;

            p.arraySize = 0;
            foreach (string value in values)
            {
                int index = p.arraySize++;
                var element = p.GetArrayElementAtIndex(index);
                if (element.propertyType == SerializedPropertyType.String)
                    element.stringValue = value;
                else if (element.propertyType == SerializedPropertyType.Generic)
                {
                    var named = element.FindPropertyRelative("module")
                        ?? element.FindPropertyRelative("moduleName")
                        ?? element.FindPropertyRelative("name");
                    if (named != null && named.propertyType == SerializedPropertyType.String)
                        named.stringValue = value;
                }
            }
        }

        private static void Resources(SerializedObject so, string path,
            string[] components, int[] quantities)
        {
            var p = P(so, path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String)
                return;

            p.arraySize = 0;
            for (int i = 0; i < components.Length; i++)
            {
                int index = p.arraySize++;
                var element = p.GetArrayElementAtIndex(index);
                var component = element.FindPropertyRelative("component");
                var quantity = element.FindPropertyRelative("quantity");

                if (component != null && component.propertyType == SerializedPropertyType.String)
                    component.stringValue = components[i];
                if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                    quantity.intValue = quantities[i];
            }
        }
    }
}

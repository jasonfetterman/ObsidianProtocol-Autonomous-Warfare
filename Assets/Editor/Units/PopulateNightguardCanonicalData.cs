using UnityEngine;
using UnityEditor;

namespace ObsidianProtocol.EditorTools
{
    public static class PopulateNightguardCanonicalData
    {
        private const string UnitId = "obs-air-nightguard";

        [MenuItem("Obsidian Protocol/Units/Populate Nightguard Canonical Data")]
        public static void Populate()
        {
            string[] guids = AssetDatabase.FindAssets(
                "t:UnitDefinition",
                new[] { "Assets/Game/Units/Database" });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
                if (asset == null) continue;

                var so = new SerializedObject(asset);
                var id = so.FindProperty("unitId");
                if (id == null || id.stringValue != UnitId) continue;

                so.Update();

                S(so, "displayName", "Nightguard");
                E(so, "category", "Air");
                S(so, "role", "Night Surveillance / Thermal Recon");
                S(so, "tier", "Advanced Recon");
                S(so, "status", "Active");

                F(so, "physical.lengthMeters", 2.6f);
                F(so, "physical.widthMeters", 2.0f);
                F(so, "physical.heightMeters", 0.9f);
                F(so, "physical.massKg", 22f);
                S(so, "physical.powerSource", "High-density battery");
                F(so, "physical.energyCapacityWh", 2400f);
                F(so, "physical.energyConsumptionWhPerMinute", 50f);
                S(so, "physical.endurance", "35–55 min");
                F(so, "physical.payloadCapacityKg", 1.5f);
                Strings(so, "physical.modules", "Thermal imaging suite");
                S(so, "physical.navigationSystem", "GPS + Night-Ops AI");
                F(so, "physical.structuralIntegrityHp", 130f);
                F(so, "physical.armorRating", 0.18f);
                S(so, "physical.heatSignature", "Very Low");
                S(so, "physical.noiseSignature", "Very Low");
                F(so, "physical.topSpeedMetersPerSecond", 21f);
                S(so, "physical.topSpeedDescription", "21 m/s");
                S(so, "physical.acceleration", "Medium");
                S(so, "physical.turningRadius", "Tight");
                S(so, "physical.climbRate", "Medium");
                S(so, "physical.stabilityRating", "Very High (night hover stability)");

                S(so, "autonomy.behaviorProfile", "Night Surveillance / Thermal Recon");
                F(so, "autonomy.aggression", 0.1f);
                F(so, "autonomy.caution", 0.9f);
                F(so, "autonomy.persistence", 0.8f);
                F(so, "autonomy.flankingLikelihood", 0.1f);
                F(so, "autonomy.retreatThreshold", 0.75f);
                S(so, "autonomy.targetPriority", "Heat signatures / movement");
                S(so, "autonomy.formationDiscipline", "High");
                F(so, "autonomy.riskTolerance", 0.1f);
                S(so, "autonomy.retreatBehaviorDescription", "Medium");

                F(so, "sensors.visualRangeMeters", 1200f);
                F(so, "sensors.thermalRangeMeters", 1800f);
                S(so, "sensors.visualRangeDescription", "1200 m");
                S(so, "sensors.thermalRangeDescription", "1800 m (very high)");
                S(so, "sensors.acousticRange", "Medium");
                S(so, "sensors.radarRange", "None");
                S(so, "sensors.lidarRange", "Short-range");
                S(so, "sensors.signalDetectionRange", "Medium");

                F(so, "communication.meshRangeKm", 6f);
                S(so, "communication.meshRangeDescription", "6 km");
                F(so, "communication.relayCapacity", 0f);
                S(so, "communication.commandLinkType", "Night-Ops Grid");
                S(so, "communication.networkPriority", "High");

                F(so, "combat.firepowerRating", 0f);
                S(so, "combat.accuracy", "N/A");
                S(so, "combat.stabilization", "Very High");
                S(so, "combat.weaponMounts", "None");
                S(so, "combat.weaponTypes", "None");
                F(so, "combat.suppressionValue", 0f);
                S(so, "combat.trackingStrength", "High");
                S(so, "combat.disruptionStrength", "Low");
                S(so, "combat.breachCapability", "None");
                S(so, "combat.countermeasureRating", "Medium");
                I(so, "combat.threatLevel", 3);
                S(so, "combat.engagementDistance", "N/A");
                S(so, "combat.preferredTerrain", "Low-light airspace");
                S(so, "combat.preferredTargets", "Heat signatures / moving targets");
                S(so, "combat.vulnerabilityProfile", "Ballistic / weather / jamming");
                S(so, "combat.firepowerDescription", "No weapons; surveillance and recon platform");

                S(so, "squad.squadRole", "Night Surveillance");
                I(so, "squad.squadSizeContribution", 1);
                S(so, "squad.formationBehavior", "Night-ops spacing");
                S(so, "squad.intentInterpretationRules", "Maintain thermal lock, avoid conflict");
                S(so, "squad.retreatBehavior", "Medium");
                S(so, "squad.breachBehavior", "None");
                S(so, "squad.flankingBehavior", "Minimal");
                S(so, "squad.reinforcementBehavior", "Supports recon and security squads");
                S(so, "squad.parentCommandUnit", "Overseer / Insight");
                Strings(so, "squad.subordinateUnits");
                Strings(so, "squad.relayNodes");
                S(so, "squad.networkDependencies", "Night-Ops Grid");

                S(so, "vrOperator.operatorSeatType", "Drone View");
                S(so, "vrOperator.operatorHud", "Night-Ops Thermal Overlay");
                S(so, "vrOperator.cameraFeeds", "Optical");
                B(so, "vrOperator.thermalView", true);
                B(so, "vrOperator.lidarView", true);
                B(so, "vrOperator.sonarView", false);
                B(so, "vrOperator.tacticalOverlay", true);
                S(so, "vrOperator.movementControls", "Full drone flight");
                S(so, "vrOperator.sensorControls", "Thermal / zoom / LiDAR");
                S(so, "vrOperator.abilityControls", "Thermal Lock, Scan, Stabilize");
                S(so, "vrOperator.weaponControls", "None");
                S(so, "vrOperator.stabilizationControls", "Auto-hover");
                Strings(so, "vrOperator.operatorAbilities",
                    "Thermal Lock", "Night Scan", "LiDAR Sweep",
                    "Stabilize", "Relay Thermal Data");

                S(so, "economy.buildCost", "180 Metal / 170 Electronics");
                F(so, "economy.buildTimeSeconds", 55f);
                S(so, "economy.factoryType", "Drone Bay");
                S(so, "economy.techRequirements", "Night-Ops Systems I");
                S(so, "economy.maintenanceCost", "Medium");
                F(so, "economy.repairTimeSeconds", 14f);
                S(so, "economy.supplyConsumption", "Medium");
                S(so, "economy.operationalCost", "Moderate");
                S(so, "economy.deploymentRequirements", "Standard drone pad");
                S(so, "economy.transportCompatibility", "All aerial racks");
                S(so, "economy.launchMethod", "Vertical takeoff");

                Resources(so, "buildRequirements.commonComponents",
                    new[] { "Medium Alloy Plates", "Composite Frame Rods",
                        "Micro-Servo Actuators", "Stabilizer Fins",
                        "Thermal Sensor Housing" },
                    new[] { 14, 10, 6, 3, 1 });

                Resources(so, "buildRequirements.electronics",
                    new[] { "Circuit Bundles", "Signal Processors",
                        "Drone Control Chips", "Thermal AI Chips" },
                    new[] { 14, 10, 4, 3 });

                Resources(so, "buildRequirements.power",
                    new[] { "High-Density Battery Pack", "Power Regulation Nodes" },
                    new[] { 1, 2 });

                Resources(so, "buildRequirements.rareTradeable",
                    new[] { "High-Resolution Thermal Core", "Night-Ops Microprocessor" },
                    new[] { 1, 1 });

                Strings(so, "buildRequirements.optionalUpgrades",
                    "Ultra-Thermal Lens", "Anti-Jamming Shielding",
                    "Silent Rotor Kit", "Extended Battery Module");

                so.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();

                Debug.Log(
                    "Nightguard population saved: " + assetPath +
                    ". Existing model prefab and store image were preserved.", asset);
                return;
            }

            Debug.LogError("Nightguard population failed: UnitDefinition with ID '" +
                           UnitId + "' was not found under Assets/Game/Units/Database.");
        }

        private static SerializedProperty P(SerializedObject so, string path)
        {
            return so.FindProperty(path);
        }

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
            {
                if (string.Equals(p.enumNames[i], value,
                                  System.StringComparison.OrdinalIgnoreCase))
                {
                    p.enumValueIndex = i;
                    return;
                }
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
                int index = p.arraySize;
                p.arraySize++;
                var element = p.GetArrayElementAtIndex(index);

                if (element.propertyType == SerializedPropertyType.String)
                {
                    element.stringValue = value;
                }
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

        private static void Resources(
            SerializedObject so, string path, string[] components, int[] quantities)
        {
            var p = P(so, path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String)
                return;

            p.arraySize = 0;

            for (int i = 0; i < components.Length; i++)
            {
                int index = p.arraySize;
                p.arraySize++;
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

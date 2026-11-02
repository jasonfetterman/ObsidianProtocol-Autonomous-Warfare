using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateSeaTidebreakerCanonicalData
    {
        [MenuItem("Obsidian Protocol/Units/Populate Sea Tidebreaker Canonical Data")]
        public static void Populate()
        {
            const string searchRoot = "Assets/Game/Units/Database/Sea";

            string[] guids = AssetDatabase.FindAssets("t:UnitDefinition", new[] { searchRoot });
            var matches = new List<string>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = Path.GetFileName(path);

                if (string.Equals(fileName, "Tidebreaker.asset", StringComparison.OrdinalIgnoreCase))
                    matches.Add(path);
            }

            if (matches.Count != 1)
            {
                Debug.LogError(
                    "[Tidebreaker] Expected exactly one Tidebreaker.asset under " +
                    searchRoot + ", but found " + matches.Count + ". Matches: " +
                    string.Join(", ", matches));
                return;
            }

            string assetPath = matches[0];
            UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);

            if (target == null)
            {
                Debug.LogError("[Tidebreaker] Could not load UnitDefinition at " + assetPath);
                return;
            }

            var so = new SerializedObject(target);
            so.Update();

            SetText(so, "unitId", "obs-sea-tidebreaker");
            SetText(so, "displayName", "Tidebreaker");
            SetText(so, "category", "Sea");
            SetText(so, "role", "Heavy Marine Assault / Wave-Stabilized Breaker Vessel");
            SetText(so, "tier", "Heavy");
            SetText(so, "status", "Active");

            SetFloat(so, "physical.lengthMeters", 14.0f);
            SetFloat(so, "physical.widthMeters", 5.5f);
            SetFloat(so, "physical.heightMeters", 4.0f);
            SetFloat(so, "physical.massKg", 12000f);
            SetFloat(so, "physical.energyCapacityWh", 40000f);
            SetFloat(so, "physical.payloadCapacityKg", 800f);
            SetFloat(so, "physical.structuralIntegrityHp", 2400f);
            SetFloat(so, "physical.armorRating", 0.75f);
            SetFloat(so, "physical.topSpeedMetersPerSecond", 9f);
            SetText(so, "physical.powerSource", "Dual marine diesel-electric turbines");
            SetText(so, "physical.endurance", "16–30 hours");
            SetText(so, "physical.navigationSystem", "Marine GPS + Assault AI");
            SetText(so, "physical.heatSignature", "Medium");
            SetText(so, "physical.noiseSignature", "High");
            SetText(so, "physical.topSpeedDescription", "9 m/s");
            SetText(so, "physical.acceleration", "Low");
            SetText(so, "physical.turningRadius", "Heavy marine turn");
            SetText(so, "physical.stabilityRating", "Maximum (storm-grade)");
            SetStrings(so, "physical.modules", new[]
            {
                "Reinforced breaker hull",
                "Heavy stabilization fins",
                "Forward impact prow",
                "Marine assault suite"
            });

            SetText(so, "autonomy.behaviorProfile",
                "Heavy assault / wave breaking / path clearing");
            SetFloat(so, "autonomy.aggression", 0.7f);
            SetFloat(so, "autonomy.caution", 0.6f);
            SetFloat(so, "autonomy.persistence", 1.0f);
            SetFloat(so, "autonomy.flankingLikelihood", 0.15f);
            SetFloat(so, "autonomy.retreatThreshold", 0.4f);
            SetFloat(so, "autonomy.riskTolerance", 0.8f);
            SetText(so, "autonomy.targetPriority",
                "Obstacles / hostile marine positions");
            SetText(so, "autonomy.formationDiscipline", "High");
            SetText(so, "autonomy.retreatBehaviorDescription", "Late");

            SetFloat(so, "sensors.visualRangeMeters", 2400f);
            SetFloat(so, "sensors.thermalRangeMeters", 900f);
            SetText(so, "sensors.visualRangeDescription", "2400 m");
            SetText(so, "sensors.thermalRangeDescription", "900 m");
            SetText(so, "sensors.acousticRange", "Medium");
            SetText(so, "sensors.radarRange", "Medium");
            SetText(so, "sensors.signalDetectionRange", "Medium");

            SetFloat(so, "communication.meshRangeKm", 8f);
            SetText(so, "communication.meshRangeDescription", "8 km");
            SetFloat(so, "communication.relayCapacity", 2f);
            SetText(so, "communication.commandLinkType", "Marine Assault Grid");
            SetText(so, "communication.networkPriority", "High");

            SetFloat(so, "combat.firepowerRating", 0.5f);
            SetFloat(so, "combat.suppressionValue", 0.5f);
            SetText(so, "combat.firepowerDescription", "Medium");
            SetText(so, "combat.accuracy", "Medium");
            SetText(so, "combat.stabilization", "Very High");
            SetText(so, "combat.weaponMounts", "2 (medium)");
            SetText(so, "combat.weaponTypes",
                "Marine autocannon / suppression turret");
            SetText(so, "combat.trackingStrength", "Medium");
            SetText(so, "combat.disruptionStrength", "Medium");
            SetText(so, "combat.breachCapability", "Very High (breaker prow)");
            SetText(so, "combat.countermeasureRating", "Medium");
            SetInt(so, "combat.threatLevel", 7);
            SetText(so, "combat.engagementDistance", "300–1200 m");
            SetText(so, "combat.preferredTerrain",
                "Rough waters / storm zones / assault lanes");
            SetText(so, "combat.preferredTargets",
                "Marine obstacles / hostile marine positions / light-to-medium surface threats");
            SetText(so, "combat.vulnerabilityProfile",
                "Ballistic / torpedoes / heavy waves");

            SetText(so, "squad.squadRole", "Heavy Marine Assault / Path Clearing");
            SetInt(so, "squad.squadSizeContribution", 4);
            SetText(so, "squad.formationBehavior", "Front-line assault spacing");
            SetText(so, "squad.intentInterpretationRules",
                "Break waves, clear paths, push forward");
            SetText(so, "squad.retreatBehavior", "Late");
            SetText(so, "squad.breachBehavior", "Aggressive obstacle breach");
            SetText(so, "squad.flankingBehavior", "Limited");
            SetText(so, "squad.reinforcementBehavior",
                "Supports assault & marine squads");
            SetText(so, "squad.parentCommandUnit", "Fusion / Command Core");
            SetStrings(so, "squad.subordinateUnits", new[] { "Harbor", "Current" });
            SetStrings(so, "squad.relayNodes", new[] { "Relay Node 1", "Relay Node 2" });
            SetText(so, "squad.networkDependencies", "Marine Assault Grid");

            SetText(so, "vrOperator.operatorSeatType", "Heavy Assault Console");
            SetText(so, "vrOperator.operatorHud", "Marine Assault Overlay");
            SetText(so, "vrOperator.cameraFeeds", "Optical + prow cam");
            SetText(so, "vrOperator.movementControls", "Heavy marine drive");
            SetText(so, "vrOperator.sensorControls", "Thermal / sonar / radar / zoom");
            SetText(so, "vrOperator.abilityControls", "Break Wave, Stabilize, Breach Mode");
            SetText(so, "vrOperator.weaponControls", "Turrets");
            SetText(so, "vrOperator.stabilizationControls",
                "Automatic storm stabilization");
            SetBool(so, "vrOperator.thermalView", true);
            SetBool(so, "vrOperator.sonarView", true);
            SetBool(so, "vrOperator.lidarView", false);
            SetBool(so, "vrOperator.tacticalOverlay", true);
            SetStrings(so, "vrOperator.operatorAbilities", new[]
            {
                "Wave Break",
                "Hull Ram",
                "Suppression Burst",
                "Assault Relay",
                "Breach Mode"
            });

            SetInt(so, "economy.creditCost", 2100);
            SetInt(so, "economy.deploymentPoints", 0);
            SetBool(so, "economy.deploymentPointsDefined", false);
            SetText(so, "economy.buildCost", "1,400 Metal / 700 Electronics");
            SetText(so, "economy.factoryType", "Marine Bay");
            SetText(so, "economy.techRequirements", "Marine Systems III");
            SetText(so, "economy.maintenanceCost", "High");
            SetText(so, "economy.supplyConsumption", "High");
            SetText(so, "economy.operationalCost",
                "22 Metal / 12 Electronics per hour");
            SetText(so, "economy.deploymentRequirements", "Heavy water launch pad");
            SetText(so, "economy.transportCompatibility", "Heavy marine racks");
            SetText(so, "economy.launchMethod", "Water drop");
            SetFloat(so, "economy.buildTimeSeconds", 200f);
            SetFloat(so, "economy.repairTimeSeconds", 55f);

            SetRequirements(so, "buildRequirements.commonComponents", new[]
            {
                ("Marine Alloy Plates", 80),
                ("Reinforced Marine Rods", 40),
                ("Hydraulic Actuators", 12),
                ("Armor Panels", 4),
                ("Breaker Hull Assembly", 1)
            });

            SetRequirements(so, "buildRequirements.electronics", new[]
            {
                ("Circuit Bundles", 36),
                ("Signal Processors", 22),
                ("Marine Control Chips", 6),
                ("Assault AI Chips", 3)
            });

            SetRequirements(so, "buildRequirements.power", new[]
            {
                ("Dual Marine Turbine Core", 1),
                ("Power Regulation Nodes", 3)
            });

            SetRequirements(so, "buildRequirements.rareTradeable", new[]
            {
                ("Breaker Prow Module", 1)
            });

            SetStrings(so, "buildRequirements.optionalUpgrades", new[]
            {
                "Reinforced Hull",
                "Extended Fuel Tank",
                "Anti-Jamming Shielding"
            });

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                "[Tidebreaker] Canonical data populated. Asset: " + assetPath +
                ". Existing modelPrefab and storeImage references were not changed. " +
                "Only fields supported by the current UnitDefinition schema were written.");
        }

        private static SerializedProperty Find(SerializedObject so, string path)
        {
            SerializedProperty property = so.FindProperty(path);
            if (property == null)
                Debug.LogWarning("[Tidebreaker] Serialized property not found: " + path);
            return property;
        }

        private static void SetText(SerializedObject so, string path, string value)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.String)
                p.stringValue = value;
            else if (p.propertyType == SerializedPropertyType.Enum)
            {
                int index = Array.IndexOf(p.enumNames, value);
                if (index >= 0)
                    p.enumValueIndex = index;
                else
                    Debug.LogWarning("[Tidebreaker] Enum value '" + value +
                                     "' is not defined for " + path +
                                     ". Existing value was preserved.");
            }
            else
                Debug.LogWarning("[Tidebreaker] " + path +
                                 " is not a string or enum; value was not written.");
        }

        private static void SetFloat(SerializedObject so, string path, float value)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
            else if (p.propertyType == SerializedPropertyType.Integer)
                p.intValue = Mathf.RoundToInt(value);
            else
                Debug.LogWarning("[Tidebreaker] " + path +
                                 " is not numeric; value was not written.");
        }

        private static void SetInt(SerializedObject so, string path, int value)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.Integer)
                p.intValue = value;
            else
                Debug.LogWarning("[Tidebreaker] " + path +
                                 " is not an integer; value was not written.");
        }

        private static void SetBool(SerializedObject so, string path, bool value)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.Boolean)
                p.boolValue = value;
            else
                Debug.LogWarning("[Tidebreaker] " + path +
                                 " is not a boolean; value was not written.");
        }

        private static void SetStrings(
            SerializedObject so, string path, string[] values)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (!p.isArray || p.propertyType == SerializedPropertyType.String)
            {
                Debug.LogWarning("[Tidebreaker] " + path +
                                 " is not a serialized list; values were not written.");
                return;
            }

            p.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                SerializedProperty element = p.GetArrayElementAtIndex(i);
                if (element.propertyType == SerializedPropertyType.String)
                    element.stringValue = values[i];
                else
                    Debug.LogWarning("[Tidebreaker] " + path +
                                     " contains non-string elements; list was not fully written.");
            }
        }

        private static void SetRequirements(
            SerializedObject so,
            string path,
            (string component, int quantity)[] requirements)
        {
            SerializedProperty list = Find(so, path);
            if (list == null) return;

            if (!list.isArray || list.propertyType == SerializedPropertyType.String)
            {
                Debug.LogWarning("[Tidebreaker] " + path +
                                 " is not a serialized requirements list.");
                return;
            }

            list.arraySize = requirements.Length;

            for (int i = 0; i < requirements.Length; i++)
            {
                SerializedProperty element = list.GetArrayElementAtIndex(i);
                SerializedProperty component = element.FindPropertyRelative("component");
                SerializedProperty quantity = element.FindPropertyRelative("quantity");

                if (component != null &&
                    component.propertyType == SerializedPropertyType.String)
                    component.stringValue = requirements[i].component;
                else
                    Debug.LogWarning("[Tidebreaker] Missing component field in " + path);

                if (quantity != null &&
                    quantity.propertyType == SerializedPropertyType.Integer)
                    quantity.intValue = requirements[i].quantity;
                else
                    Debug.LogWarning("[Tidebreaker] Missing quantity field in " + path);
            }
        }
    }
}

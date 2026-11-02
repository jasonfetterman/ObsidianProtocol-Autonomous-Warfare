using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateCommandWorldmapCanonicalData
    {
        [MenuItem("Obsidian Protocol/Units/Populate Command Worldmap Canonical Data")]
        public static void Populate()
        {
            const string root = "Assets/Game/Units/Database";
            string[] guids = AssetDatabase.FindAssets("t:UnitDefinition", new[] { root });
            var matches = new List<string>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string name = Path.GetFileName(path);

                if (string.Equals(name, "Worldmap.asset", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, "WorldMap.asset", StringComparison.OrdinalIgnoreCase))
                    matches.Add(path);
            }

            if (matches.Count != 1)
            {
                Debug.LogError("[Worldmap] Expected exactly one Worldmap asset under " +
                    root + "; found " + matches.Count + ": " + string.Join(", ", matches));
                return;
            }

            string assetPath = matches[0];
            UnitDefinition target = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);

            if (target == null)
            {
                Debug.LogError("[Worldmap] Failed to load UnitDefinition: " + assetPath);
                return;
            }

            var so = new SerializedObject(target);
            so.Update();

            SetText(so, "unitId", "obs-command-worldmap");
            SetText(so, "displayName", "Worldmap");
            SetText(so, "category", "Command");
            SetText(so, "role", "Global Navigation Core / Strategic Terrain Intelligence");
            SetText(so, "tier", "Command");
            SetText(so, "status", "Active");

            SetFloat(so, "physical.lengthMeters", 12f);
            SetFloat(so, "physical.widthMeters", 8f);
            SetFloat(so, "physical.heightMeters", 5f);
            SetFloat(so, "physical.massKg", 22000f);
            SetFloat(so, "physical.energyCapacityWh", 150000f);
            SetFloat(so, "physical.payloadCapacityKg", 0f);
            SetFloat(so, "physical.structuralIntegrityHp", 4400f);
            SetFloat(so, "physical.armorRating", 1.25f);
            SetText(so, "physical.powerSource", "Dual command-grade reactors");
            SetText(so, "physical.endurance", "Continuous");
            SetText(so, "physical.navigationSystem", "Strategic geospatial AI");
            SetText(so, "physical.heatSignature", "Medium");
            SetText(so, "physical.noiseSignature", "Low");
            SetText(so, "physical.topSpeedDescription", "N/A");
            SetText(so, "physical.stabilityRating", "Maximum");
            SetStrings(so, "physical.modules", new[]
            {
                "Global terrain processor",
                "Satellite uplink array",
                "Multi-node command mesh",
                "Redundant cooling towers"
            });

            SetText(so, "autonomy.behaviorProfile",
                "Global navigation / strategic terrain intelligence");
            SetFloat(so, "autonomy.aggression", 0f);
            SetFloat(so, "autonomy.caution", 1f);
            SetFloat(so, "autonomy.persistence", 1f);
            SetFloat(so, "autonomy.flankingLikelihood", 0f);
            SetFloat(so, "autonomy.retreatThreshold", 0f);
            SetFloat(so, "autonomy.riskTolerance", 0.1f);
            SetText(so, "autonomy.targetPriority", "None — non-combat strategic unit");
            SetText(so, "autonomy.formationDiscipline", "Maximum");
            SetText(so, "autonomy.retreatBehaviorDescription", "None");

            SetText(so, "sensors.visualRangeDescription", "Strategic global terrain data");
            SetText(so, "sensors.thermalRangeDescription", "Strategic environmental data");
            SetText(so, "sensors.acousticRange", "Short");
            SetText(so, "sensors.radarRange", "Medium");
            SetText(so, "sensors.signalDetectionRange", "Very High");

            SetFloat(so, "communication.relayCapacity", 10f);
            SetText(so, "communication.meshRangeDescription",
                "Unlimited within connected strategic network");
            SetText(so, "communication.commandLinkType", "Strategic Command Mesh");
            SetText(so, "communication.networkPriority", "Maximum");

            SetFloat(so, "combat.firepowerRating", 0f);
            SetFloat(so, "combat.suppressionValue", 0f);
            SetText(so, "combat.firepowerDescription", "None — non-combat command installation");
            SetText(so, "combat.weaponMounts", "None");
            SetText(so, "combat.weaponTypes", "None");
            SetText(so, "combat.accuracy", "N/A");
            SetText(so, "combat.stabilization", "N/A");
            SetText(so, "combat.trackingStrength", "High — signal and threat mapping");
            SetText(so, "combat.disruptionStrength", "High — strategic data routing");
            SetText(so, "combat.breachCapability", "None");
            SetText(so, "combat.countermeasureRating", "Very High");
            SetInt(so, "combat.threatLevel", 5);
            SetText(so, "combat.engagementDistance", "N/A");
            SetText(so, "combat.preferredTerrain", "Secure command facility");
            SetText(so, "combat.preferredTargets", "None");
            SetText(so, "combat.vulnerabilityProfile",
                "EMP / cyber intrusion / sabotage / facility attack / satellite disruption");

            SetText(so, "squad.squadRole", "Strategic navigation and terrain intelligence");
            SetInt(so, "squad.squadSizeContribution", 6);
            SetText(so, "squad.formationBehavior", "Central command anchor");
            SetText(so, "squad.intentInterpretationRules",
                "Analyze terrain, generate routes, map threats, route logistics, relay validated intelligence");
            SetText(so, "squad.retreatBehavior", "None");
            SetText(so, "squad.breachBehavior", "None");
            SetText(so, "squad.flankingBehavior", "None");
            SetText(so, "squad.reinforcementBehavior",
                "Provides strategic navigation and terrain intelligence");
            SetText(so, "squad.parentCommandUnit", "Command Core");
            SetStrings(so, "squad.subordinateUnits", new[] { "Archive", "Insight" });
            SetStrings(so, "squad.relayNodes", new[]
            {
                "Relay Node 1", "Relay Node 2", "Relay Node 3", "Relay Node 4",
                "Relay Node 5", "Relay Node 6", "Relay Node 7", "Relay Node 8",
                "Relay Node 9", "Relay Node 10"
            });
            SetText(so, "squad.networkDependencies",
                "Strategic Command Mesh / Satellite Network");

            SetText(so, "vrOperator.operatorSeatType", "Strategic Navigation Console");
            SetText(so, "vrOperator.operatorHud", "Terrain and strategic data overlay");
            SetText(so, "vrOperator.cameraFeeds", "Satellite / terrain / command network feeds");
            SetText(so, "vrOperator.movementControls", "Strategic route generation and modification");
            SetText(so, "vrOperator.sensorControls", "Terrain / satellite / network monitoring");
            SetText(so, "vrOperator.abilityControls",
                "Global Route Sync / Terrain Forecast / Mesh Stabilization / Command Relay");
            SetText(so, "vrOperator.weaponControls", "None — non-combat unit");
            SetText(so, "vrOperator.stabilizationControls", "Network and system diagnostics");
            SetBool(so, "vrOperator.thermalView", false);
            SetBool(so, "vrOperator.sonarView", false);
            SetBool(so, "vrOperator.lidarView", false);
            SetBool(so, "vrOperator.tacticalOverlay", true);
            SetStrings(so, "vrOperator.operatorAbilities", new[]
            {
                "Global Route Sync",
                "Terrain Forecast",
                "Mesh Stabilization",
                "Command Relay",
                "Satellite Uplink Management",
                "System Diagnostics"
            });

            SetInt(so, "economy.creditCost", 20000);
            SetInt(so, "economy.deploymentPoints", 0);
            SetBool(so, "economy.deploymentPointsDefined", false);
            SetText(so, "economy.buildCost",
                "8,000 Metal / 12,000 Electronics / 2,000 Rare Components");
            SetText(so, "economy.factoryType", "Strategic Command Facility");
            SetText(so, "economy.techRequirements", "Advanced Strategic Command");
            SetText(so, "economy.maintenanceCost", "Very High");
            SetText(so, "economy.supplyConsumption", "Low");
            SetText(so, "economy.operationalCost", "18 Metal / 24 Electronics per hour");
            SetText(so, "economy.deploymentRequirements", "Strategic Command Zone");
            SetText(so, "economy.transportCompatibility", "Fixed Installation");
            SetText(so, "economy.launchMethod", "Facility-Integrated");
            SetFloat(so, "economy.buildTimeSeconds", 900f);
            SetFloat(so, "economy.repairTimeSeconds", 180f);

            SetRequirements(so, "buildRequirements.commonComponents", new[]
            {
                ("Heavy Structural Frames", 80),
                ("Command-Grade Armor Sections", 40),
                ("Redundant Cooling Towers", 4),
                ("Hardened Data Vaults", 4)
            });

            SetRequirements(so, "buildRequirements.electronics", new[]
            {
                ("Strategic Processing Cores", 6),
                ("Satellite Uplink Arrays", 4),
                ("Quantum Navigation Processors", 2),
                ("Command Mesh Controllers", 10),
                ("Encrypted Strategic Transceivers", 8)
            });

            SetRequirements(so, "buildRequirements.power", new[]
            {
                ("Dual Command Reactors", 2)
            });

            SetRequirements(so, "buildRequirements.rareTradeable", new[]
            {
                ("Rare Components", 1)
            });

            SetStrings(so, "buildRequirements.optionalUpgrades", new[]
            {
                "Quantum Terrain Processor",
                "Deep-Space Uplink Array",
                "Hardened Command Mesh",
                "Predictive Terrain AI",
                "Redundant Reactor Core",
                "Strategic Data Vault"
            });

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[Worldmap] Canonical data populated: " + assetPath +
                ". Existing modelPrefab and storeImage references were not modified. " +
                "Fields unsupported by the current UnitDefinition schema were not stored.");
        }

        private static SerializedProperty Find(SerializedObject so, string path)
        {
            SerializedProperty p = so.FindProperty(path);
            if (p == null)
                Debug.LogWarning("[Worldmap] Serialized property not found: " + path);
            return p;
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
                    Debug.LogWarning("[Worldmap] Enum value '" + value +
                        "' is unavailable for " + path + "; preserved existing value.");
            }
            else
                Debug.LogWarning("[Worldmap] " + path +
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
                Debug.LogWarning("[Worldmap] " + path +
                    " is not numeric; value was not written.");
        }

        private static void SetInt(SerializedObject so, string path, int value)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.Integer)
                p.intValue = value;
            else
                Debug.LogWarning("[Worldmap] " + path +
                    " is not an integer; value was not written.");
        }

        private static void SetBool(SerializedObject so, string path, bool value)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.Boolean)
                p.boolValue = value;
            else
                Debug.LogWarning("[Worldmap] " + path +
                    " is not a boolean; value was not written.");
        }

        private static void SetStrings(SerializedObject so, string path, string[] values)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (!p.isArray || p.propertyType == SerializedPropertyType.String)
            {
                Debug.LogWarning("[Worldmap] " + path + " is not a serialized list.");
                return;
            }

            p.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                SerializedProperty element = p.GetArrayElementAtIndex(i);
                if (element.propertyType == SerializedPropertyType.String)
                    element.stringValue = values[i];
                else
                    Debug.LogWarning("[Worldmap] " + path +
                        " contains non-string entries; skipped entry " + i);
            }
        }

        private static void SetRequirements(
            SerializedObject so, string path, (string component, int quantity)[] values)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (!p.isArray || p.propertyType == SerializedPropertyType.String)
            {
                Debug.LogWarning("[Worldmap] " + path + " is not a requirements list.");
                return;
            }

            p.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                SerializedProperty element = p.GetArrayElementAtIndex(i);
                SerializedProperty component = element.FindPropertyRelative("component");
                SerializedProperty quantity = element.FindPropertyRelative("quantity");

                if (component != null && component.propertyType == SerializedPropertyType.String)
                    component.stringValue = values[i].component;
                else
                    Debug.LogWarning("[Worldmap] Missing component field in " + path);

                if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                    quantity.intValue = values[i].quantity;
                else
                    Debug.LogWarning("[Worldmap] Missing quantity field in " + path);
            }
        }
    }
}

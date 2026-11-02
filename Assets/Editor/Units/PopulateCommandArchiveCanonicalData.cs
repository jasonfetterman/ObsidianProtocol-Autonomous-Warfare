using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateCommandArchiveCanonicalData
    {
        [MenuItem("Obsidian Protocol/Units/Populate Command Archive Canonical Data")]
        public static void Populate()
        {
            string[] guids = AssetDatabase.FindAssets(
                "t:UnitDefinition", new[] { "Assets/Game/Units/Database" });

            string assetPath = null;

            foreach (string guid in guids)
            {
                string candidate = AssetDatabase.GUIDToAssetPath(guid);

                if (!string.Equals(Path.GetFileName(candidate), "Archive.asset",
                    StringComparison.OrdinalIgnoreCase))
                    continue;

                if (assetPath != null)
                {
                    Debug.LogError("Archive population stopped: multiple matching assets found: "
                        + assetPath + " and " + candidate);
                    return;
                }

                assetPath = candidate;
            }

            if (assetPath == null)
            {
                Debug.LogError("Archive population failed: Archive.asset was not found under Assets/Game/Units/Database.");
                return;
            }

            UnityEngine.Object target = AssetDatabase.LoadMainAssetAtPath(assetPath);

            if (target == null)
            {
                Debug.LogError("Archive population failed to load: " + assetPath);
                return;
            }

            var so = new SerializedObject(target);
            so.Update();

            SetText(so, "unitId", "obs-command-archive");
            SetText(so, "displayName", "Archive");
            SetText(so, "category", "Command");
            SetText(so, "role", "Strategic Data Core / Global Intelligence Storage");
            SetText(so, "tier", "Command");
            SetText(so, "status", "Active");

            SetFloat(so, "physical.lengthMeters", 10f);
            SetFloat(so, "physical.widthMeters", 7f);
            SetFloat(so, "physical.heightMeters", 5f);
            SetFloat(so, "physical.massKg", 18000f);
            SetText(so, "physical.powerSource", "Dual redundant command-grade reactors");
            SetFloat(so, "physical.energyCapacityWh", 120000f);
            SetFloat(so, "physical.payloadCapacityKg", 0f);
            SetFloat(so, "physical.structuralIntegrityHp", 4000f);
            SetFloat(so, "physical.armorRating", 1.20f);
            SetText(so, "physical.endurance", "Continuous");
            SetText(so, "physical.navigationSystem", "Stationary (anchored command node)");
            SetText(so, "physical.heatSignature", "Medium");
            SetText(so, "physical.noiseSignature", "Low");
            SetText(so, "physical.topSpeedDescription", "N/A — stationary command installation");
            SetText(so, "physical.stabilityRating", "Maximum");

            SetStrings(so, "physical.modules", new[]
            {
                "Strategic data vault",
                "Encryption lattice",
                "Multi-node command mesh",
                "Redundant cooling towers"
            });

            SetText(so, "autonomy.behaviorProfile", "Strategic data management / command relay");
            SetFloat(so, "autonomy.aggression", 0f);
            SetFloat(so, "autonomy.caution", 1f);
            SetFloat(so, "autonomy.persistence", 1f);
            SetFloat(so, "autonomy.flankingLikelihood", 0f);
            SetFloat(so, "autonomy.retreatThreshold", 0f);
            SetFloat(so, "autonomy.riskTolerance", 0.1f);
            SetText(so, "autonomy.retreatBehaviorDescription", "None — anchored command node; preserve data and transfer services to redundant systems");
            SetText(so, "autonomy.targetPriority", "None (non-combat)");
            SetText(so, "autonomy.formationDiscipline", "Maximum");

            SetText(so, "sensors.visualRangeDescription", "Short");
            SetText(so, "sensors.thermalRangeDescription", "Short");
            SetText(so, "sensors.acousticRange", "Short");
            SetText(so, "sensors.radarRange", "Medium");
            SetText(so, "sensors.signalDetectionRange", "Very High");

            SetText(so, "communication.meshRangeDescription", "Global (command-tier)");
            SetFloat(so, "communication.relayCapacity", 8f);
            SetText(so, "communication.commandLinkType", "Strategic Command Mesh");
            SetText(so, "communication.networkPriority", "Maximum");

            SetFloat(so, "combat.firepowerRating", 0f);
            SetFloat(so, "combat.suppressionValue", 0f);
            SetText(so, "combat.firepowerDescription", "None — non-combat strategic data core");
            SetText(so, "combat.weaponMounts", "None");
            SetText(so, "combat.weaponTypes", "None");
            SetText(so, "combat.trackingStrength", "High (signals only)");
            SetText(so, "combat.disruptionStrength", "High (data routing)");
            SetText(so, "combat.countermeasureRating", "Very High");
            SetInt(so, "combat.threatLevel", 5);
            SetText(so, "combat.preferredTerrain", "Secure command zones");
            SetText(so, "combat.vulnerabilityProfile", "EMP / sabotage");

            SetText(so, "squad.squadRole", "Strategic Data Core");
            SetInt(so, "squad.squadSizeContribution", 6);
            SetText(so, "squad.formationBehavior", "Central command anchor");
            SetText(so, "squad.intentInterpretationRules", "Store, route, and secure global data");
            SetText(so, "squad.retreatBehavior", "None");
            SetText(so, "squad.breachBehavior", "None");
            SetText(so, "squad.flankingBehavior", "None");
            SetText(so, "squad.reinforcementBehavior", "Supports all units");
            SetText(so, "squad.parentCommandUnit", "Command Core");
            SetText(so, "squad.networkDependencies", "Strategic Command Mesh");
            SetStrings(so, "squad.subordinateUnits", new[] { "Insight", "Pulse" });
            SetStrings(so, "squad.relayNodes", new[]
            {
                "Relay Node 1", "Relay Node 2", "Relay Node 3", "Relay Node 4",
                "Relay Node 5", "Relay Node 6", "Relay Node 7", "Relay Node 8"
            });

            SetText(so, "vrOperator.operatorSeatType", "Command Data Console");
            SetText(so, "vrOperator.operatorHud", "Strategic Data Overlay");
            SetText(so, "vrOperator.cameraFeeds", "Internal systems only");
            SetText(so, "vrOperator.movementControls", "Stationary anchor installation; no movement controls");
            SetText(so, "vrOperator.sensorControls", "Data routing / encryption lattice management / mesh node control / system diagnostics");
            SetText(so, "vrOperator.abilityControls", "Global Data Sync / Mesh Stabilization / Encryption Pulse / Command Relay");
            SetText(so, "vrOperator.weaponControls", "None");
            SetText(so, "vrOperator.stabilizationControls", "Mesh health and command-network continuity");
            SetBool(so, "vrOperator.thermalView", true);
            SetBool(so, "vrOperator.sonarView", false);
            SetBool(so, "vrOperator.lidarView", false);
            SetBool(so, "vrOperator.tacticalOverlay", true);
            SetStrings(so, "vrOperator.operatorAbilities", new[]
            {
                "Global Data Sync",
                "Mesh Stabilization",
                "Encryption Pulse",
                "Command Relay"
            });

            SetInt(so, "economy.creditCost", 3200);
            SetInt(so, "economy.deploymentPoints", 0);
            SetBool(so, "economy.deploymentPointsDefined", false);
            SetText(so, "economy.buildCost", "2,000 Metal / 1,200 Electronics");
            SetText(so, "economy.factoryType", "Command Foundry");
            SetText(so, "economy.techRequirements", "Command Systems I");
            SetText(so, "economy.maintenanceCost", "High");
            SetText(so, "economy.supplyConsumption", "Medium");
            SetText(so, "economy.operationalCost", "Medium");
            SetText(so, "economy.deploymentRequirements", "Command zone");
            SetText(so, "economy.transportCompatibility", "Heavy command carriers");
            SetText(so, "economy.launchMethod", "Anchor-install");
            SetFloat(so, "economy.buildTimeSeconds", 240f);
            SetFloat(so, "economy.repairTimeSeconds", 70f);

            SetRequirements(so, "buildRequirements.commonComponents", new[]
            {
                ("Command Alloy Plates", 120),
                ("Reinforced Command Rods", 60),
                ("Hydraulic Actuators", 10),
                ("Armor Panels", 4),
                ("Data Vault Assembly", 1)
            });

            SetRequirements(so, "buildRequirements.electronics", new[]
            {
                ("Circuit Bundles", 40),
                ("Signal Processors", 30),
                ("Command Control Chips", 10),
                ("Command AI Chips", 4)
            });

            SetRequirements(so, "buildRequirements.power", new[]
            {
                ("Command Reactor Core", 1),
                ("Power Regulation Nodes", 3)
            });

            SetRequirements(so, "buildRequirements.rareTradeable", new[]
            {
                ("Encryption Lattice Module", 1)
            });

            SetStrings(so, "buildRequirements.optionalUpgrades", new[]
            {
                "Reinforced Vault",
                "Quantum Encryption",
                "Anti-Jamming Shielding"
            });

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Archive canonical data populated. Asset: " + assetPath +
                      ". Existing modelPrefab and storeImage references were left unchanged.");
        }

        private static SerializedProperty Find(SerializedObject so, string path)
        {
            SerializedProperty p = so.FindProperty(path);
            if (p == null)
                Debug.LogWarning("Archive: serialized property not found; skipped: " + path);
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
                    Debug.LogWarning("Archive: enum value '" + value + "' is unavailable for " + path);
            }
            else
                Debug.LogWarning("Archive: incompatible text property type for " + path);
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
                Debug.LogWarning("Archive: incompatible numeric property type for " + path);
        }

        private static void SetInt(SerializedObject so, string path, int value)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.Integer)
                p.intValue = value;
            else
                Debug.LogWarning("Archive: incompatible integer property type for " + path);
        }

        private static void SetBool(SerializedObject so, string path, bool value)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.Boolean)
                p.boolValue = value;
            else
                Debug.LogWarning("Archive: incompatible boolean property type for " + path);
        }

        private static void SetStrings(SerializedObject so, string path, string[] values)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (!p.isArray || p.propertyType == SerializedPropertyType.String)
            {
                Debug.LogWarning("Archive: expected a list property for " + path);
                return;
            }

            p.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                SerializedProperty element = p.GetArrayElementAtIndex(i);
                if (element.propertyType == SerializedPropertyType.String)
                    element.stringValue = values[i];
                else
                    Debug.LogWarning("Archive: list element is not a string for " + path);
            }
        }

        private static void SetRequirements(
            SerializedObject so, string path, (string component, int quantity)[] values)
        {
            SerializedProperty p = Find(so, path);
            if (p == null) return;

            if (!p.isArray || p.propertyType == SerializedPropertyType.String)
            {
                Debug.LogWarning("Archive: expected a requirement list for " + path);
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
                    Debug.LogWarning("Archive: missing/incompatible component field in " + path);

                if (quantity != null && quantity.propertyType == SerializedPropertyType.Integer)
                    quantity.intValue = values[i].quantity;
                else
                    Debug.LogWarning("Archive: missing/incompatible quantity field in " + path);
            }
        }
    }
}

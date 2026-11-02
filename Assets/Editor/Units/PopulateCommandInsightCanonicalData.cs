using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateCommandInsightCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Command/Insight/Insight.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Command Insight Canonical Data")]
        public static void Populate()
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(AssetPath);

            if (asset == null)
            {
                Debug.LogError("[Insight] Unit asset not found: " + AssetPath +
                    ". Locate the actual Insight.asset path and update AssetPath.");
                return;
            }

            var so = new SerializedObject(asset);
            so.Update();

            SetString(so, "unitId", "obs-command-insight");
            SetString(so, "id", "obs-command-insight");
            SetString(so, "displayName", "Insight");
            SetString(so, "unitName", "Insight");
            SetString(so, "role", "Strategic Analysis Core / Pattern Recognition & Intelligence Processing");
            SetString(so, "unitRole", "Strategic Analysis Core / Pattern Recognition & Intelligence Processing");
            SetString(so, "tier", "Command");
            SetString(so, "status", "Active");

            SetFloat(so, "length", 9f);
            SetFloat(so, "width", 7f);
            SetFloat(so, "height", 5f);
            SetFloat(so, "mass", 17000f);
            SetString(so, "powerSource", "Dual command-grade reactors");
            SetFloat(so, "energyCapacity", 130000f);
            SetString(so, "fuelCapacity", "N/A (reactor-based)");
            SetString(so, "endurance", "Continuous");
            SetString(so, "payloadCapacity", "Internal systems only");
            SetString(so, "modules",
                "Strategic analysis chamber; Pattern recognition lattice; Multi-node mesh interface; Redundant cooling towers");
            SetString(so, "navigationSystem", "Stationary (anchored command node)");
            SetFloat(so, "structuralIntegrity", 3800f);
            SetFloat(so, "armorRating", 1.10f);
            SetString(so, "heatSignature", "Medium");
            SetString(so, "noiseSignature", "Low");
            SetString(so, "topSpeed", "N/A");
            SetString(so, "stabilityRating", "Maximum");

            SetString(so, "behaviorProfile",
                "Strategic analysis / anomaly detection / intelligence synthesis");
            SetFloat(so, "aggression", 0f);
            SetFloat(so, "caution", 1f);
            SetFloat(so, "persistence", 1f);
            SetString(so, "retreatThreshold", "N/A");
            SetString(so, "targetPriority", "None (non-combat)");
            SetString(so, "formationDiscipline", "Maximum");
            SetString(so, "riskTolerance", "Very Low");

            SetString(so, "visualRange", "Short");
            SetString(so, "thermalRange", "Short");
            SetString(so, "acousticRange", "Short");
            SetString(so, "radarRange", "Medium");
            SetString(so, "signalDetectionRange", "Very High");
            SetString(so, "meshRange", "Global");
            SetInt(so, "relayCapacity", 8);
            SetString(so, "commandLinkType", "Strategic Command Mesh");
            SetString(so, "networkPriority", "Maximum");

            SetString(so, "firepowerRating", "None");
            SetString(so, "weaponMounts", "None");
            SetString(so, "trackingStrength", "Very High (intelligence-grade signal analysis)");
            SetString(so, "disruptionStrength", "High");
            SetString(so, "countermeasureRating", "Very High");
            SetInt(so, "threatLevel", 6);
            SetString(so, "preferredTerrain", "Secure command zones");
            SetString(so, "vulnerabilityProfile", "EMP / sabotage");

            SetString(so, "squadRole", "Strategic Intelligence Processor");
            SetInt(so, "squadSizeContribution", 6);
            SetString(so, "formationBehavior", "Central anchor");
            SetString(so, "intentInterpretationRules",
                "Analyze patterns, detect anomalies, synthesize intelligence");
            SetString(so, "retreatBehavior", "None");
            SetString(so, "reinforcementBehavior", "Supports all units");
            SetString(so, "parentCommandUnit", "Nexus / Fusion");
            SetString(so, "subordinateUnits", "Pulse");
            SetInt(so, "relayNodes", 8);
            SetString(so, "networkDependencies", "Strategic Command Mesh");

            SetString(so, "operatorSeatType", "Intelligence Console");
            SetString(so, "operatorHud", "Strategic Analysis Overlay");
            SetString(so, "cameraFeeds", "Internal systems only");
            SetBool(so, "thermalView", true);
            SetString(so, "tacticalOverlay",
                "Pattern maps / anomaly markers / intelligence routes");
            SetString(so, "operatorControls",
                "Pattern analysis; Mesh intelligence routing; Lattice control; System diagnostics");
            SetString(so, "operatorAbilities",
                "Pattern Sync; Anomaly Detection Pulse; Intelligence Broadcast; Mesh Stabilization");

            SetString(so, "buildCost", "2200 Metal / 1300 Electronics");
            SetInt(so, "metalCost", 2200);
            SetInt(so, "electronicsCost", 1300);
            SetFloat(so, "buildTime", 230f);
            SetString(so, "factoryType", "Command Foundry");
            SetString(so, "techRequirements", "Command Systems I");
            SetString(so, "maintenanceCost", "Medium");
            SetFloat(so, "repairTime", 70f);
            SetString(so, "supplyConsumption", "Medium");
            SetString(so, "operationalCost", "Medium");
            SetString(so, "deploymentRequirements", "Command zone");
            SetString(so, "transportCompatibility", "Heavy command carriers");
            SetString(so, "launchMethod", "Anchor-install");

            SetString(so, "commonComponents",
                "120x Command Alloy Plates; 60x Reinforced Command Rods; 10x Hydraulic Actuators; 4x Armor Panels; 1x Analysis Chamber Assembly");
            SetString(so, "electronicsRequirements",
                "44x Circuit Bundles; 30x Signal Processors; 10x Command Control Chips; 4x Command AI Chips");
            SetString(so, "powerRequirements",
                "1x Command Reactor Core; 3x Power Regulation Nodes");
            SetString(so, "rareTradeableRequirements",
                "1x Pattern Recognition Lattice");
            SetString(so, "optionalUpgrades",
                "Quantum Encryption; Reinforced Hull; Anti-Jamming Shielding");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log("[Insight] Canonical-data population pass finished for " +
                AssetPath + ". Verify serialized fields and the unit ID in Unity.");
        }

        private static SerializedProperty Find(SerializedObject so, string name)
        {
            return so.FindProperty(name);
        }

        private static void SetString(SerializedObject so, string name, string value)
        {
            var p = Find(so, name);
            if (p != null && p.propertyType == SerializedPropertyType.String)
                p.stringValue = value;
        }

        private static void SetFloat(SerializedObject so, string name, float value)
        {
            var p = Find(so, name);
            if (p != null && p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
            else if (p != null && p.propertyType == SerializedPropertyType.Integer)
                p.intValue = Mathf.RoundToInt(value);
        }

        private static void SetInt(SerializedObject so, string name, int value)
        {
            var p = Find(so, name);
            if (p != null && p.propertyType == SerializedPropertyType.Integer)
                p.intValue = value;
        }

        private static void SetBool(SerializedObject so, string name, bool value)
        {
            var p = Find(so, name);
            if (p != null && p.propertyType == SerializedPropertyType.Boolean)
                p.boolValue = value;
        }
    }
}

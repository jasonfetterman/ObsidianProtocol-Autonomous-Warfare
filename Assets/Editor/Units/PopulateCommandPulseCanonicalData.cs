using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateCommandPulseCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Command/Pulse/Pulse.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Command Pulse Canonical Data")]
        public static void Populate()
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(AssetPath);

            if (asset == null)
            {
                Debug.LogError("[Pulse] Unit asset not found: " + AssetPath +
                    ". Locate the actual Pulse.asset path and update AssetPath.");
                return;
            }

            var so = new SerializedObject(asset);
            so.Update();

            SetString(so, "unitId", "obs-command-pulse");
            SetString(so, "id", "obs-command-pulse");
            SetString(so, "displayName", "Pulse");
            SetString(so, "unitName", "Pulse");
            SetString(so, "role", "Tactical Signal Hub / Real-Time Mesh Stabilizer");
            SetString(so, "unitRole", "Tactical Signal Hub / Real-Time Mesh Stabilizer");
            SetString(so, "tier", "Command");
            SetString(so, "status", "Active");

            SetFloat(so, "length", 8f);
            SetFloat(so, "width", 6f);
            SetFloat(so, "height", 4f);
            SetFloat(so, "mass", 12000f);
            SetString(so, "powerSource", "Dual tactical-grade reactors");
            SetFloat(so, "energyCapacity", 90000f);
            SetString(so, "fuelCapacity", "N/A (reactor-based)");
            SetString(so, "endurance", "Continuous");
            SetString(so, "payloadCapacity", "Internal systems only");
            SetString(so, "modules",
                "Tactical signal chamber; Mesh stabilization lattice; Multi-node relay interface; Redundant cooling towers");
            SetString(so, "navigationSystem", "Stationary (anchored command node)");
            SetFloat(so, "structuralIntegrity", 3000f);
            SetFloat(so, "armorRating", 1.00f);
            SetString(so, "heatSignature", "Low");
            SetString(so, "noiseSignature", "Low");
            SetString(so, "topSpeed", "N/A");
            SetString(so, "stabilityRating", "Maximum");

            SetString(so, "behaviorProfile",
                "Tactical signal routing / mesh stabilization");
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
            SetInt(so, "relayCapacity", 6);
            SetString(so, "commandLinkType", "Strategic Command Mesh");
            SetString(so, "networkPriority", "Maximum");

            SetString(so, "firepowerRating", "None");
            SetString(so, "weaponMounts", "None");
            SetString(so, "trackingStrength", "Very High (tactical signal intelligence)");
            SetString(so, "disruptionStrength", "High");
            SetString(so, "countermeasureRating", "Very High");
            SetInt(so, "threatLevel", 5);
            SetString(so, "preferredTerrain", "Secure command zones");
            SetString(so, "vulnerabilityProfile", "EMP / sabotage");

            SetString(so, "squadRole", "Tactical Mesh Stabilizer");
            SetInt(so, "squadSizeContribution", 5);
            SetString(so, "formationBehavior", "Central anchor");
            SetString(so, "intentInterpretationRules",
                "Maintain mesh stability and tactical routing");
            SetString(so, "retreatBehavior", "None");
            SetString(so, "reinforcementBehavior", "Supports all units");
            SetString(so, "parentCommandUnit", "Insight / Nexus");
            SetString(so, "subordinateUnits", "None");
            SetInt(so, "relayNodes", 6);
            SetString(so, "networkDependencies", "Strategic Command Mesh");

            SetString(so, "operatorSeatType", "Tactical Signal Console");
            SetString(so, "operatorHud", "Mesh Stability Overlay");
            SetString(so, "cameraFeeds", "Internal systems only");
            SetBool(so, "thermalView", true);
            SetString(so, "tacticalOverlay",
                "Mesh health / routing paths / signal load");
            SetString(so, "operatorControls",
                "Mesh routing; Stabilization lattice control; Tactical diagnostics; Signal balancing");
            SetString(so, "operatorAbilities",
                "Mesh Pulse; Stabilization Burst; Tactical Sync; Encryption Surge");

            SetString(so, "buildCost", "1800 Metal / 1000 Electronics");
            SetInt(so, "metalCost", 1800);
            SetInt(so, "electronicsCost", 1000);
            SetFloat(so, "buildTime", 200f);
            SetString(so, "factoryType", "Command Foundry");
            SetString(so, "techRequirements", "Command Systems I");
            SetString(so, "maintenanceCost", "Medium");
            SetFloat(so, "repairTime", 60f);
            SetString(so, "supplyConsumption", "Medium");
            SetString(so, "operationalCost", "Medium");
            SetString(so, "deploymentRequirements", "Command zone");
            SetString(so, "transportCompatibility", "Heavy command carriers");
            SetString(so, "launchMethod", "Anchor-install");

            SetString(so, "commonComponents",
                "100x Command Alloy Plates; 50x Reinforced Command Rods; 8x Hydraulic Actuators; 3x Armor Panels; 1x Signal Chamber Assembly");
            SetString(so, "electronicsRequirements",
                "36x Circuit Bundles; 24x Signal Processors; 8x Command Control Chips; 3x Command AI Chips");
            SetString(so, "powerRequirements",
                "1x Tactical Reactor Core; 2x Power Regulation Nodes");
            SetString(so, "rareTradeableRequirements",
                "1x Mesh Stabilization Lattice");
            SetString(so, "optionalUpgrades",
                "Quantum Encryption; Reinforced Hull; Anti-Jamming Shielding");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log("[Pulse] Canonical-data population pass finished for " +
                AssetPath + ". Verify serialized fields and unit ID in Unity.");
        }

        private static void SetString(SerializedObject so, string name, string value)
        {
            var p = so.FindProperty(name);
            if (p != null && p.propertyType == SerializedPropertyType.String)
                p.stringValue = value;
        }

        private static void SetFloat(SerializedObject so, string name, float value)
        {
            var p = so.FindProperty(name);
            if (p != null && p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
            else if (p != null && p.propertyType == SerializedPropertyType.Integer)
                p.intValue = Mathf.RoundToInt(value);
        }

        private static void SetInt(SerializedObject so, string name, int value)
        {
            var p = so.FindProperty(name);
            if (p != null && p.propertyType == SerializedPropertyType.Integer)
                p.intValue = value;
        }

        private static void SetBool(SerializedObject so, string name, bool value)
        {
            var p = so.FindProperty(name);
            if (p != null && p.propertyType == SerializedPropertyType.Boolean)
                p.boolValue = value;
        }
    }
}

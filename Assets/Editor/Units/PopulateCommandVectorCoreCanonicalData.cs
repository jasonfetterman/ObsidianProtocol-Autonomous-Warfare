using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateCommandVectorCoreCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Command/Vector_Core/Vector_Core.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Command Vector Core Canonical Data")]
        public static void Populate()
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(AssetPath);

            if (asset == null)
            {
                Debug.LogError("[Vector Core] Unit asset not found: " + AssetPath +
                    ". Locate the actual Vector Core asset path and update AssetPath.");
                return;
            }

            var so = new SerializedObject(asset);
            so.Update();

            SetString(so, "unitId", "obs-command-vectorcore");
            SetString(so, "id", "obs-command-vectorcore");
            SetString(so, "displayName", "Vector Core");
            SetString(so, "unitName", "Vector Core");
            SetString(so, "role", "Tactical Vector Routing / Real-Time Unit Pathing");
            SetString(so, "unitRole", "Tactical Vector Routing / Real-Time Unit Pathing");
            SetString(so, "tier", "Command");
            SetString(so, "status", "Active");

            SetFloat(so, "length", 12f);
            SetFloat(so, "width", 9f);
            SetFloat(so, "height", 6f);
            SetFloat(so, "mass", 24000f);
            SetString(so, "powerSource", "Dual tactical-grade reactors");
            SetFloat(so, "energyCapacity", 140000f);
            SetString(so, "fuelCapacity", "N/A (reactor-based)");
            SetString(so, "endurance", "Continuous");
            SetString(so, "payloadCapacity", "Internal systems only");
            SetString(so, "modules",
                "Vector routing chamber; Pathing lattice; Multi-node relay interface; Redundant cooling towers; Stabilization pylons");
            SetString(so, "navigationSystem", "Stationary (anchored command node)");
            SetFloat(so, "structuralIntegrity", 4200f);
            SetFloat(so, "armorRating", 1.20f);
            SetString(so, "heatSignature", "Low");
            SetString(so, "noiseSignature", "Low");
            SetString(so, "topSpeed", "N/A");
            SetString(so, "stabilityRating", "Maximum");

            SetString(so, "behaviorProfile",
                "Tactical pathing / vector routing / movement optimization");
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
            SetInt(so, "relayCapacity", 10);
            SetString(so, "commandLinkType", "Strategic Command Mesh");
            SetString(so, "networkPriority", "Maximum");

            SetString(so, "firepowerRating", "None");
            SetString(so, "weaponMounts", "None");
            SetString(so, "trackingStrength", "Very High (movement intelligence)");
            SetString(so, "disruptionStrength", "High");
            SetString(so, "countermeasureRating", "Very High");
            SetInt(so, "threatLevel", 6);
            SetString(so, "preferredTerrain", "Secure command zones");
            SetString(so, "vulnerabilityProfile", "EMP / sabotage");

            SetString(so, "squadRole", "Tactical Movement Coordinator");
            SetInt(so, "squadSizeContribution", 7);
            SetString(so, "formationBehavior", "Central anchor");
            SetString(so, "intentInterpretationRules",
                "Optimize movement vectors for all units");
            SetString(so, "retreatBehavior", "None");
            SetString(so, "reinforcementBehavior", "Supports all units");
            SetString(so, "parentCommandUnit", "Nexus / Fusion");
            SetString(so, "subordinateUnits", "None");
            SetInt(so, "relayNodes", 10);
            SetString(so, "networkDependencies", "Strategic Command Mesh");

            SetString(so, "operatorSeatType", "Tactical Pathing Console");
            SetString(so, "operatorHud", "Vector Routing Overlay");
            SetString(so, "cameraFeeds", "Internal systems only");
            SetBool(so, "thermalView", true);
            SetString(so, "tacticalOverlay",
                "Movement vectors / pathing lanes / congestion maps");
            SetString(so, "operatorControls",
                "Vector routing; Pathing lattice control; Tactical diagnostics; Movement optimization");
            SetString(so, "operatorAbilities",
                "Vector Sync; Pathing Pulse; Tactical Flow Optimization; Mesh Stabilization");

            SetString(so, "buildCost", "2400 Metal / 1500 Electronics");
            SetInt(so, "metalCost", 2400);
            SetInt(so, "electronicsCost", 1500);
            SetFloat(so, "buildTime", 260f);
            SetString(so, "factoryType", "Command Foundry");
            SetString(so, "techRequirements", "Command Systems II");
            SetString(so, "maintenanceCost", "Medium");
            SetFloat(so, "repairTime", 75f);
            SetString(so, "supplyConsumption", "Medium");
            SetString(so, "operationalCost", "Medium");
            SetString(so, "deploymentRequirements", "Command zone");
            SetString(so, "transportCompatibility", "Heavy command carriers");
            SetString(so, "launchMethod", "Anchor-install");

            SetString(so, "commonComponents",
                "140x Command Alloy Plates; 70x Reinforced Command Rods; 10x Hydraulic Actuators; 4x Armor Panels; 1x Vector Routing Chamber Assembly");
            SetString(so, "electronicsRequirements",
                "48x Circuit Bundles; 32x Signal Processors; 10x Command Control Chips; 4x Command AI Chips");
            SetString(so, "powerRequirements",
                "1x Tactical Reactor Core; 3x Power Regulation Nodes");
            SetString(so, "rareTradeableRequirements",
                "1x Vector Lattice Module");
            SetString(so, "optionalUpgrades",
                "Quantum Encryption; Reinforced Hull; Anti-Jamming Shielding");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log("[Vector Core] Canonical-data population pass finished for " +
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


using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateSatelliteSkylinkCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Satellite/Skylink/Skylink.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Satellite Skylink Canonical Data")]
        public static void Populate()
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(AssetPath);

            if (asset == null)
            {
                Debug.LogError("[Skylink] Unit asset not found: " + AssetPath +
                    ". Locate the actual Skylink.asset path and update AssetPath.");
                return;
            }

            var so = new SerializedObject(asset);
            so.Update();

            SetString(so, "unitId", "obs-sat-skylink");
            SetString(so, "id", "obs-sat-skylink");
            SetString(so, "displayName", "Skylink");
            SetString(so, "unitName", "Skylink");
            SetString(so, "role", "Mid-Orbit Communications / High-Bandwidth Relay");
            SetString(so, "unitRole", "Mid-Orbit Communications / High-Bandwidth Relay");
            SetString(so, "tier", "Satellite");
            SetString(so, "status", "Active");

            SetFloat(so, "length", 8f);
            SetFloat(so, "width", 5f);
            SetFloat(so, "height", 3f);
            SetFloat(so, "mass", 5600f);
            SetString(so, "powerSource", "Solar array + micro-reactor backup");
            SetFloat(so, "energyCapacity", 24000f);
            SetString(so, "fuelCapacity", "60 L (orbital correction thrusters)");
            SetString(so, "endurance", "4–7 years");
            SetString(so, "payloadCapacity", "Internal systems only");
            SetString(so, "modules",
                "High-bandwidth relay dish; Mid-orbit stabilization gyros; Multi-node mesh router; Micro-thruster array");
            SetString(so, "navigationSystem", "Orbital AI + star tracker");
            SetFloat(so, "structuralIntegrity", 1400f);
            SetFloat(so, "armorRating", 0.45f);
            SetString(so, "heatSignature", "Very Low");
            SetString(so, "noiseSignature", "None");
            SetString(so, "topSpeed", "Orbital velocity");
            SetString(so, "stabilityRating", "Maximum");

            SetString(so, "behaviorProfile", "Communications relay / mesh routing");
            SetFloat(so, "aggression", 0f);
            SetFloat(so, "caution", 1f);
            SetFloat(so, "persistence", 1f);
            SetString(so, "retreatThreshold", "N/A");
            SetString(so, "targetPriority", "None (non-combat)");
            SetString(so, "formationDiscipline", "Maximum");
            SetString(so, "riskTolerance", "Very Low");

            SetString(so, "opticalRange", "Global");
            SetString(so, "visualRange", "Global");
            SetString(so, "thermalRange", "Medium");
            SetString(so, "radarRange", "Medium");
            SetString(so, "signalDetectionRange", "Extremely High");
            SetString(so, "meshRange", "Global");
            SetInt(so, "relayCapacity", 6);
            SetString(so, "commandLinkType", "Satellite Mesh");
            SetString(so, "networkPriority", "Maximum");

            SetString(so, "firepowerRating", "None");
            SetString(so, "weaponMounts", "None");
            SetString(so, "trackingStrength", "Medium");
            SetString(so, "disruptionStrength", "High");
            SetString(so, "countermeasureRating", "Medium");
            SetInt(so, "threatLevel", 5);
            SetString(so, "preferredTerrain", "Mid orbit");
            SetString(so, "vulnerabilityProfile", "Debris / EMP / solar storms");

            SetString(so, "squadRole", "High-Bandwidth Relay");
            SetInt(so, "squadSizeContribution", 3);
            SetString(so, "formationBehavior", "Orbital spacing");
            SetString(so, "intentInterpretationRules", "Maintain communications coverage");
            SetString(so, "retreatBehavior", "None");
            SetString(so, "reinforcementBehavior", "Supports all grids");
            SetString(so, "parentCommandUnit", "Fusion / Insight");
            SetString(so, "subordinateUnits", "None");
            SetInt(so, "relayNodes", 6);
            SetString(so, "networkDependencies", "Satellite Mesh");

            SetString(so, "operatorSeatType", "Communications Console");
            SetString(so, "operatorHud", "Relay Network Overlay");
            SetString(so, "cameraFeeds", "Optical + thermal");
            SetBool(so, "thermalView", true);
            SetString(so, "tacticalOverlay", "Mesh routes / signal load / relay health");
            SetString(so, "operatorControls",
                "Relay routing; Signal balancing; Thermal view; Orbital diagnostics");
            SetString(so, "operatorAbilities",
                "Relay Boost; Mesh Stabilization; Signal Sync; Thermal Sweep");

            SetString(so, "buildCost", "1400 Metal / 900 Electronics");
            SetInt(so, "metalCost", 1400);
            SetInt(so, "electronicsCost", 900);
            SetFloat(so, "buildTime", 200f);
            SetString(so, "factoryType", "Satellite Bay");
            SetString(so, "techRequirements", "Satellite Systems I");
            SetString(so, "maintenanceCost", "Low");
            SetFloat(so, "repairTime", 45f);
            SetString(so, "supplyConsumption", "Very Low");
            SetString(so, "operationalCost", "Very Low");
            SetString(so, "deploymentRequirements", "Launch pad");
            SetString(so, "transportCompatibility", "Satellite carrier");
            SetString(so, "launchMethod", "Orbital insertion");

            SetString(so, "commonComponents",
                "70x Satellite Alloy Plates; 34x Reinforced Satellite Rods; 6x Hydraulic Actuators; 1x Relay Dish Assembly");
            SetString(so, "electronicsRequirements",
                "32x Circuit Bundles; 22x Signal Processors; 6x Satellite Control Chips; 3x Relay AI Chips");
            SetString(so, "powerRequirements",
                "1x Solar Array Assembly; 1x Micro-Reactor Core; 2x Power Regulation Nodes");
            SetString(so, "rareTradeableRequirements",
                "1x High-Bandwidth Relay Module");
            SetString(so, "optionalUpgrades",
                "Enhanced Relay Dish; Thermal Boost; Anti-Jamming Shielding");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log("[Skylink] Canonical-data population pass finished for " +
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

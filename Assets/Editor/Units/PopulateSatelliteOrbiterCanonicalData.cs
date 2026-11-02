using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateSatelliteOrbiterCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Satellite/Orbiter/Orbiter.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Satellite Orbiter Canonical Data")]
        public static void Populate()
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(AssetPath);

            if (asset == null)
            {
                Debug.LogError("[Orbiter] Unit asset not found: " + AssetPath +
                    ". Locate the actual Orbiter.asset path and update AssetPath.");
                return;
            }

            var so = new SerializedObject(asset);
            so.Update();

            SetString(so, "unitId", "obs-sat-orbiter");
            SetString(so, "id", "obs-sat-orbiter");
            SetString(so, "displayName", "Orbiter");
            SetString(so, "unitName", "Orbiter");
            SetString(so, "role", "Low-Orbit Recon / Signal Relay");
            SetString(so, "unitRole", "Low-Orbit Recon / Signal Relay");
            SetString(so, "tier", "Satellite");
            SetString(so, "status", "Active");

            SetFloat(so, "length", 6f);
            SetFloat(so, "width", 4f);
            SetFloat(so, "height", 3f);
            SetFloat(so, "mass", 4200f);
            SetString(so, "powerSource", "Solar array + micro-reactor backup");
            SetFloat(so, "energyCapacity", 18000f);
            SetString(so, "fuelCapacity", "40 L (orbital correction thrusters)");
            SetString(so, "endurance", "3–5 years");
            SetString(so, "payloadCapacity", "Internal systems only");
            SetString(so, "modules",
                "Low-orbit recon optics; Signal relay dish; Stabilization gyros; Micro-thruster array");
            SetString(so, "navigationSystem", "Orbital AI + star tracker");
            SetFloat(so, "structuralIntegrity", 1200f);
            SetFloat(so, "armorRating", 0.40f);
            SetString(so, "heatSignature", "Very Low");
            SetString(so, "noiseSignature", "None");
            SetString(so, "topSpeed", "Orbital velocity");
            SetString(so, "stabilityRating", "Maximum");

            SetString(so, "behaviorProfile", "Recon / relay / orbital stabilization");
            SetFloat(so, "aggression", 0f);
            SetFloat(so, "caution", 1f);
            SetFloat(so, "persistence", 1f);
            SetString(so, "retreatThreshold", "N/A");
            SetString(so, "targetPriority", "None (non-combat)");
            SetString(so, "formationDiscipline", "Maximum");
            SetString(so, "riskTolerance", "Very Low");

            SetString(so, "opticalRange", "Global");
            SetString(so, "visualRange", "Global");
            SetString(so, "thermalRange", "High");
            SetString(so, "radarRange", "Medium");
            SetString(so, "signalDetectionRange", "Very High");
            SetString(so, "meshRange", "Global");
            SetInt(so, "relayCapacity", 4);
            SetString(so, "commandLinkType", "Satellite Mesh");
            SetString(so, "networkPriority", "High");

            SetString(so, "firepowerRating", "None");
            SetString(so, "weaponMounts", "None");
            SetString(so, "trackingStrength", "High (optical + thermal)");
            SetString(so, "disruptionStrength", "Medium");
            SetString(so, "countermeasureRating", "Medium");
            SetInt(so, "threatLevel", 4);
            SetString(so, "preferredTerrain", "Low orbit");
            SetString(so, "vulnerabilityProfile", "Debris / EMP / solar storms");

            SetString(so, "squadRole", "Recon & Relay");
            SetInt(so, "squadSizeContribution", 2);
            SetString(so, "formationBehavior", "Orbital spacing");
            SetString(so, "intentInterpretationRules", "Maintain recon coverage");
            SetString(so, "retreatBehavior", "None");
            SetString(so, "reinforcementBehavior", "Supports all grids");
            SetString(so, "parentCommandUnit", "Worldmap / Insight");
            SetString(so, "subordinateUnits", "None");
            SetInt(so, "relayNodes", 4);
            SetString(so, "networkDependencies", "Satellite Mesh");

            SetString(so, "operatorSeatType", "Orbital Recon Console");
            SetString(so, "operatorHud", "Satellite Recon Overlay");
            SetString(so, "cameraFeeds", "Optical + thermal");
            SetBool(so, "thermalView", true);
            SetString(so, "tacticalOverlay", "Global recon grid");
            SetString(so, "operatorControls",
                "Recon optics; Thermal view; Signal relay; Orbital diagnostics");
            SetString(so, "operatorAbilities",
                "Global Recon Sweep; Thermal Scan; Relay Boost; Stabilization Pulse");

            SetString(so, "buildCost", "1200 Metal / 700 Electronics");
            SetInt(so, "metalCost", 1200);
            SetInt(so, "electronicsCost", 700);
            SetFloat(so, "buildTime", 180f);
            SetString(so, "factoryType", "Satellite Bay");
            SetString(so, "techRequirements", "Satellite Systems I");
            SetString(so, "maintenanceCost", "Low");
            SetFloat(so, "repairTime", 40f);
            SetString(so, "supplyConsumption", "Very Low");
            SetString(so, "operationalCost", "Very Low");
            SetString(so, "deploymentRequirements", "Launch pad");
            SetString(so, "transportCompatibility", "Satellite carrier");
            SetString(so, "launchMethod", "Orbital insertion");

            SetString(so, "commonComponents",
                "60x Satellite Alloy Plates; 30x Reinforced Satellite Rods; 6x Hydraulic Actuators; 1x Optics Housing");
            SetString(so, "electronicsRequirements",
                "28x Circuit Bundles; 20x Signal Processors; 6x Satellite Control Chips; 3x Recon AI Chips");
            SetString(so, "powerRequirements",
                "1x Solar Array Assembly; 1x Micro-Reactor Core; 2x Power Regulation Nodes");
            SetString(so, "rareTradeableRequirements",
                "1x Orbital Stabilization Module");
            SetString(so, "optionalUpgrades",
                "Enhanced Optics; Thermal Boost; Anti-Jamming Shielding");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log("[Orbiter] Canonical-data population pass finished for " +
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

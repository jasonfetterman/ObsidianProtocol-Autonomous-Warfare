using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.EditorTools
{
    public static class PopulateSatelliteCosmosCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Satellite/Cosmos/Cosmos.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Satellite Cosmos Canonical Data")]
        public static void Populate()
        {
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetPath);
            if (asset == null)
            {
                Debug.LogError("[COSMOS] Unit asset not found: " + AssetPath +
                    ". Locate the actual Cosmos asset path before rerunning.");
                return;
            }

            var so = new SerializedObject(asset);
            so.Update();

            // Identity
            S(so, "obs-sat-cosmos", "unitId", "id", "unitID");
            S(so, "Cosmos", "displayName", "unitName");
            S(so, "Deep-Orbit Strategic Relay / Long-Range Signal Projection", "role", "unitRole");
            S(so, "Satellite", "tier", "unitTier");
            S(so, "Active", "status", "unitStatus");

            // Physical and technical
            F(so, 14f, "length", "lengthMeters");
            F(so, 8f, "width", "widthMeters");
            F(so, 5f, "height", "heightMeters");
            F(so, 10200f, "mass", "massKg");
            S(so, "Expanded solar array + dual micro-reactor backups", "powerSource");
            F(so, 55000f, "energyCapacity", "energyCapacityWh");
            F(so, 120f, "fuelCapacity", "fuelCapacityLiters");
            S(so, "120 L (orbital correction thrusters)", "fuelDescription", "fuelType");
            S(so, "8–12 years", "endurance");
            S(so, "Internal systems only", "payloadCapacity");
            S(so, "Deep-orbit relay dish; Long-range signal amplifier; Stabilization gyros; Micro-thruster array", "modules", "moduleDescription");
            S(so, "Orbital AI + star tracker", "navigationSystem");
            F(so, 2200f, "structuralIntegrity", "maxHealth", "health");
            F(so, 0.60f, "armorRating", "armor");
            S(so, "Very Low", "heatSignature");
            S(so, "None", "noiseSignature");
            S(so, "Orbital velocity", "topSpeed", "speedDescription");
            S(so, "Maximum", "stabilityRating");

            // Autonomy and AI behavior
            S(so, "Long-range relay / deep-orbit stabilization", "behaviorProfile", "aiBehaviorProfile");
            F(so, 0f, "aggression");
            F(so, 1f, "caution");
            F(so, 1f, "persistence");
            S(so, "N/A", "retreatThreshold");
            S(so, "None", "targetPriority");
            S(so, "Maximum", "formationDiscipline");
            S(so, "Very Low", "riskTolerance");

            // Sensors
            S(so, "Global", "opticalRange");
            S(so, "Medium", "thermalRange");
            S(so, "Medium", "radarRange");
            S(so, "Extremely High", "signalDetectionRange");

            // Communications
            S(so, "Global", "meshRange");
            I(so, 10, "relayCapacity", "relayNodes");
            S(so, "Satellite Mesh", "commandLinkType");
            S(so, "Maximum", "networkPriority");

            // Combat and threat model
            S(so, "None", "firepowerRating", "firepower");
            S(so, "None", "weaponMounts");
            S(so, "Medium", "trackingStrength");
            S(so, "High", "disruptionStrength");
            S(so, "Medium", "countermeasureRating");
            I(so, 7, "threatLevel");
            S(so, "Deep orbit", "preferredTerrain");
            S(so, "Debris / EMP / solar storms", "vulnerabilityProfile");

            // Squad and command integration
            S(so, "Strategic Long-Range Relay", "squadRole");
            I(so, 5, "squadSizeContribution");
            S(so, "Orbital spacing", "formationBehavior");
            S(so, "Maintain long-range communications coverage", "intentInterpretationRules");
            S(so, "None", "retreatBehavior");
            S(so, "Supports all grids", "reinforcementBehavior");
            S(so, "Nexus / Fusion", "parentCommandUnit");
            S(so, "None", "subordinateUnits");
            S(so, "Satellite Mesh", "networkDependencies");

            // VR operator mode
            S(so, "Deep-Orbit Relay Console", "operatorSeatType");
            S(so, "Long-Range Signal Overlay", "operatorHUD", "operatorHud");
            S(so, "Optical + thermal", "cameraFeeds");
            S(so, "Relay routes / signal load / mesh health", "tacticalOverlay");
            S(so, "Relay routing; Signal amplification; Thermal view; Orbital diagnostics", "operatorControls");
            S(so, "Long-Range Boost; Mesh Stabilization; Signal Sync; Thermal Sweep", "operatorAbilities");

            // Economy and logistics
            S(so, "2,000 Metal / 1,300 Electronics", "buildCost");
            I(so, 2000, "metalCost", "buildCostMetal");
            I(so, 1300, "electronicsCost", "buildCostElectronics");
            F(so, 260f, "buildTime", "buildTimeSeconds");
            S(so, "Satellite Bay", "factoryType");
            S(so, "Satellite Systems II", "techRequirements", "technologyRequirements");
            S(so, "Low", "maintenanceCost");
            F(so, 60f, "repairTime", "repairTimeSeconds");
            S(so, "Very Low", "supplyConsumption");
            S(so, "Very Low", "operationalCost");
            S(so, "Launch pad", "deploymentRequirements");
            S(so, "Satellite carrier", "transportCompatibility");
            S(so, "Orbital insertion", "launchMethod");

            // Player collection build requirements
            S(so, "110x Satellite Alloy Plates; 55x Reinforced Satellite Rods; 10x Hydraulic Actuators; 1x Deep-Orbit Relay Assembly", "commonComponents");
            S(so, "44x Circuit Bundles; 30x Signal Processors; 10x Satellite Control Chips; 4x Relay AI Chips", "electronicsComponents");
            S(so, "1x Expanded Solar Array Assembly; 2x Micro-Reactor Cores; 3x Power Regulation Nodes", "powerComponents");
            S(so, "1x Long-Range Amplifier Module", "rareComponents", "tradeableComponents");
            S(so, "Enhanced Relay Dish; Thermal Boost; Anti-Jamming Shielding", "optionalUpgrades");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log("[COSMOS] Population pass finished for " + AssetPath +
                ". Confirm the serialized fields in the Inspector before marking complete.");
        }

        private static void S(SerializedObject so, string value, params string[] names)
        {
            foreach (var name in names)
            {
                var p = so.FindProperty(name);
                if (p == null || p.propertyType != SerializedPropertyType.String) continue;
                p.stringValue = value;
                return;
            }
        }

        private static void I(SerializedObject so, int value, params string[] names)
        {
            foreach (var name in names)
            {
                var p = so.FindProperty(name);
                if (p == null) continue;
                if (p.propertyType == SerializedPropertyType.Integer)
                {
                    p.intValue = value;
                    return;
                }
                if (p.propertyType == SerializedPropertyType.Float)
                {
                    p.floatValue = value;
                    return;
                }
            }
        }

        private static void F(SerializedObject so, float value, params string[] names)
        {
            foreach (var name in names)
            {
                var p = so.FindProperty(name);
                if (p == null) continue;
                if (p.propertyType == SerializedPropertyType.Float)
                {
                    p.floatValue = value;
                    return;
                }
                if (p.propertyType == SerializedPropertyType.Integer)
                {
                    p.intValue = Mathf.RoundToInt(value);
                    return;
                }
            }
        }
    }
}

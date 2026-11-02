using UnityEditor;
using UnityEngine;
using System;

namespace ObsidianProtocol.EditorTools
{
    public static class PopulateSatelliteAstralCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Satellite/Astral/Astral.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Satellite Astral Canonical Data")]
        public static void Populate()
        {
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetPath);

            if (asset == null)
            {
                Debug.LogError(
                    "[ASTRAL] Unit asset not found: " + AssetPath +
                    ". Locate the actual Astral asset path before rerunning.");
                return;
            }

            var so = new SerializedObject(asset);
            so.Update();

            // Sets the first matching serialized field. Missing aliases are skipped.
            Set(so, new[] { "unitId", "id", "unitID" }, "obs-sat-astral");
            Set(so, new[] { "displayName", "unitName", "name" }, "Astral");
            Set(so, new[] { "role", "unitRole" }, "High-Resolution Recon / Deep-Scan Imaging");
            Set(so, new[] { "tier", "unitTier" }, "Satellite");
            Set(so, new[] { "status", "unitStatus" }, "Active");

            Set(so, new[] { "length", "lengthMeters" }, 12f);
            Set(so, new[] { "width", "widthMeters" }, 7f);
            Set(so, new[] { "height", "heightMeters" }, 4f);
            Set(so, new[] { "mass", "massKg" }, 8400f);
            Set(so, new[] { "powerSource" }, "Expanded solar array + micro-reactor backup");
            Set(so, new[] { "energyCapacity", "energyCapacityWh" }, 40000f);
            Set(so, new[] { "fuelCapacity", "fuelCapacityLiters" }, 100f);
            Set(so, new[] { "fuelDescription", "fuelType" }, "100 L (orbital correction thrusters)");
            Set(so, new[] { "endurance" }, "6–10 years");
            Set(so, new[] { "payloadCapacity" }, "Internal systems only");
            Set(so, new[] { "modules", "moduleDescription" },
                "High-resolution imaging suite; Deep-scan thermal array; Stabilization gyros; Micro-thruster array");
            Set(so, new[] { "navigationSystem" }, "Orbital AI + star tracker");
            Set(so, new[] { "structuralIntegrity", "maxHealth", "health" }, 1800f);
            Set(so, new[] { "armorRating", "armor" }, 0.55f);
            Set(so, new[] { "heatSignature" }, "Very Low");
            Set(so, new[] { "noiseSignature" }, "None");
            Set(so, new[] { "topSpeed", "speedDescription" }, "Orbital velocity");
            Set(so, new[] { "stabilityRating" }, "Maximum");

            Set(so, new[] { "behaviorProfile", "aiBehaviorProfile" },
                "Recon / deep-scan imaging / orbital stabilization");
            Set(so, new[] { "aggression" }, 0f);
            Set(so, new[] { "caution" }, 1f);
            Set(so, new[] { "persistence" }, 1f);
            Set(so, new[] { "retreatThreshold" }, "N/A");
            Set(so, new[] { "targetPriority" }, "None (non-combat)");
            Set(so, new[] { "formationDiscipline" }, "Maximum");
            Set(so, new[] { "riskTolerance" }, "Very Low");

            Set(so, new[] { "opticalRange" }, "Global (high-resolution)");
            Set(so, new[] { "thermalRange" }, "High");
            Set(so, new[] { "radarRange" }, "Medium");
            Set(so, new[] { "signalDetectionRange" }, "High");
            Set(so, new[] { "meshRange" }, "Global");
            Set(so, new[] { "relayCapacity" }, 6);
            Set(so, new[] { "commandLinkType" }, "Satellite Mesh");
            Set(so, new[] { "networkPriority" }, "High");

            Set(so, new[] { "firepowerRating", "firepower" }, "None");
            Set(so, new[] { "weaponMounts" }, "None");
            Set(so, new[] { "trackingStrength" }, "Very High (optical + thermal)");
            Set(so, new[] { "disruptionStrength" }, "Medium");
            Set(so, new[] { "countermeasureRating" }, "Medium");
            Set(so, new[] { "threatLevel" }, 6);
            Set(so, new[] { "preferredTerrain" }, "High orbit");
            Set(so, new[] { "vulnerabilityProfile" }, "Debris / EMP / solar storms");

            Set(so, new[] { "squadRole" }, "High-Resolution Recon");
            Set(so, new[] { "squadSizeContribution" }, 4);
            Set(so, new[] { "formationBehavior" }, "Orbital spacing");
            Set(so, new[] { "intentInterpretationRules" }, "Maintain deep-scan recon coverage");
            Set(so, new[] { "retreatBehavior" }, "None");
            Set(so, new[] { "reinforcementBehavior" }, "Supports all grids");
            Set(so, new[] { "parentCommandUnit" }, "Insight / Worldmap");
            Set(so, new[] { "subordinateUnits" }, "None");
            Set(so, new[] { "relayNodes" }, 6);
            Set(so, new[] { "networkDependencies" }, "Satellite Mesh");

            Set(so, new[] { "operatorSeatType" }, "Deep-Scan Recon Console");
            Set(so, new[] { "operatorHUD", "operatorHud" }, "High-Resolution Recon Overlay");
            Set(so, new[] { "cameraFeeds" }, "Optical + thermal");
            Set(so, new[] { "tacticalOverlay" }, "Recon grid / deep-scan markers");
            Set(so, new[] { "operatorControls" },
                "Recon optics; Deep-scan thermal; Signal relay; Orbital diagnostics");
            Set(so, new[] { "operatorAbilities" },
                "Deep-Scan Sweep; Thermal Pulse; Relay Boost; Stabilization Burst");

            Set(so, new[] { "buildCost" }, "1,800 Metal / 1,200 Electronics");
            Set(so, new[] { "metalCost", "buildCostMetal" }, 1800);
            Set(so, new[] { "electronicsCost", "buildCostElectronics" }, 1200);
            Set(so, new[] { "buildTime", "buildTimeSeconds" }, 240f);
            Set(so, new[] { "factoryType" }, "Satellite Bay");
            Set(so, new[] { "techRequirements", "technologyRequirements" }, "Satellite Systems II");
            Set(so, new[] { "maintenanceCost" }, "Low");
            Set(so, new[] { "repairTime", "repairTimeSeconds" }, 55f);
            Set(so, new[] { "supplyConsumption" }, "Very Low");
            Set(so, new[] { "operationalCost" }, "Very Low");
            Set(so, new[] { "deploymentRequirements" }, "Launch pad");
            Set(so, new[] { "transportCompatibility" }, "Satellite carrier");
            Set(so, new[] { "launchMethod" }, "Orbital insertion");

            Set(so, new[] { "commonComponents" },
                "90x Satellite Alloy Plates; 45x Reinforced Satellite Rods; 8x Hydraulic Actuators; 1x Imaging Suite Assembly");
            Set(so, new[] { "electronicsComponents" },
                "40x Circuit Bundles; 28x Signal Processors; 8x Satellite Control Chips; 4x Recon AI Chips");
            Set(so, new[] { "powerComponents" },
                "1x Expanded Solar Array Assembly; 1x Micro-Reactor Core; 2x Power Regulation Nodes");
            Set(so, new[] { "rareComponents", "tradeableComponents" },
                "1x Deep-Scan Thermal Module");
            Set(so, new[] { "optionalUpgrades" },
                "Enhanced Optics; Thermal Boost; Anti-Jamming Shielding");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log(
                "[ASTRAL] Population pass finished for " + AssetPath +
                ". This confirms the asset was loaded and the pass ran; " +
                "it does not verify that every requested field matched a serialized property.");
        }

        private static void Set(SerializedObject so, string[] names, string value)
        {
            foreach (var name in names)
            {
                var p = so.FindProperty(name);
                if (p == null || p.propertyType != SerializedPropertyType.String)
                    continue;
                p.stringValue = value;
                return;
            }
        }

        private static void Set(SerializedObject so, string[] names, int value)
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

        private static void Set(SerializedObject so, string[] names, float value)
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

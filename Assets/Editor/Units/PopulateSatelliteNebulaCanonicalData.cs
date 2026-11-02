using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateSatelliteNebulaCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Satellite/Nebula/Nebula.asset";

        private static bool SetString(SerializedObject so, string value, params string[] names)
        {
            foreach (string name in names)
            {
                SerializedProperty p = so.FindProperty(name);
                if (p != null && p.propertyType == SerializedPropertyType.String)
                {
                    p.stringValue = value;
                    return true;
                }
            }
            return false;
        }

        private static bool SetNumber(SerializedObject so, float value, params string[] names)
        {
            foreach (string name in names)
            {
                SerializedProperty p = so.FindProperty(name);
                if (p == null) continue;

                if (p.propertyType == SerializedPropertyType.Float)
                {
                    p.floatValue = value;
                    return true;
                }

                if (p.propertyType == SerializedPropertyType.Integer)
                {
                    p.intValue = Mathf.RoundToInt(value);
                    return true;
                }
            }
            return false;
        }

        [MenuItem("Obsidian Protocol/Units/Populate Satellite Nebula Canonical Data")]
        public static void Populate()
        {
            ScriptableObject asset =
                AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetPath);

            if (asset == null)
            {
                Debug.LogError(
                    "[NEBULA] Asset not found at: " + AssetPath +
                    ". Locate Nebula.asset and correct AssetPath in this script.");
                return;
            }

            SerializedObject so = new SerializedObject(asset);
            so.Update();

            // Identity
            SetString(so, "obs-sat-nebula", "unitID", "unitId", "UnitID", "id", "ID");
            SetString(so, "NEBULA", "displayName", "unitName", "UnitName", "unitDisplayName");
            SetString(so, "Deep-Space Recon / Cosmic Signal Analysis",
                "role", "unitRole", "description", "roleDescription");
            SetString(so, "Satellite", "tier", "unitTier", "category", "unitCategory");
            SetString(so, "Active", "status", "unitStatus");

            // Physical and technical
            SetNumber(so, 16f, "length", "Length");
            SetNumber(so, 9f, "width", "Width");
            SetNumber(so, 5f, "height", "Height");
            SetNumber(so, 12800f, "mass", "Mass", "massKg");
            SetString(so, "Expanded solar array + dual micro-reactor backups",
                "powerSource", "energySource");
            SetNumber(so, 70000f, "energyCapacity", "energyCapacityWh", "powerCapacity");
            SetNumber(so, 160f, "fuelCapacity", "fuelCapacityLiters");
            SetString(so, "10–15 years", "endurance", "operationalEndurance");
            SetString(so, "Internal systems only", "payloadCapacity", "payload");
            SetString(so,
                "Deep-space imaging suite; Cosmic signal analyzer; Stabilization gyros; Micro-thruster array",
                "modules", "moduleList", "installedModules");
            SetString(so, "Orbital AI + star tracker", "navigationSystem", "navigation");
            SetNumber(so, 2600f, "structuralIntegrity", "integrity", "hitPoints", "maxHealth");
            SetNumber(so, 0.65f, "armorRating", "armor");
            SetString(so, "Very Low", "heatSignature", "heatSignatureLevel");
            SetString(so, "None", "noiseSignature", "noiseSignatureLevel");
            SetString(so, "Orbital velocity", "topSpeed", "maximumSpeed");
            SetString(so, "Maximum", "stabilityRating", "stability");

            // Autonomy and AI
            SetString(so, "Deep-space recon / cosmic signal analysis",
                "behaviorProfile", "aiBehaviorProfile", "autonomyProfile");
            SetNumber(so, 0f, "aggression", "aggressionLevel");
            SetNumber(so, 1f, "caution", "cautionLevel");
            SetNumber(so, 1f, "persistence", "persistenceLevel");
            SetString(so, "N/A", "retreatThreshold", "aiRetreatThreshold");
            SetString(so, "None", "targetPriority", "aiTargetPriority");
            SetString(so, "Maximum", "formationDiscipline", "aiFormationDiscipline");
            SetString(so, "Very Low", "riskTolerance", "aiRiskTolerance");

            // Sensors
            SetString(so, "Global (deep-space)", "opticalRange", "opticalSensorRange");
            SetString(so, "High", "thermalRange", "thermalSensorRange");
            SetString(so, "Medium", "radarRange", "radarSensorRange");
            SetString(so, "Extremely High", "signalDetectionRange", "signalRange");

            // Communications
            SetString(so, "Global", "meshRange", "communicationMeshRange");
            SetNumber(so, 12f, "relayCapacity", "communicationRelayCapacity");
            SetString(so, "Satellite Mesh", "commandLinkType", "networkType");
            SetString(so, "Maximum", "networkPriority", "communicationPriority");

            // Combat and threat model
            SetString(so, "None", "firepowerRating", "firepower");
            SetString(so, "None", "weaponMounts", "weapons");
            SetString(so, "Very High", "trackingStrength", "tracking");
            SetString(so, "Medium", "disruptionStrength", "disruption");
            SetString(so, "Medium", "countermeasureRating", "countermeasures");
            SetNumber(so, 7f, "threatLevel", "threat");
            SetString(so, "Deep orbit / high orbit", "preferredTerrain", "preferredOrbit");
            SetString(so, "Debris / EMP / solar storms",
                "vulnerabilityProfile", "vulnerabilities");

            // Squad and command integration
            SetString(so, "Deep-Space Recon & Analysis", "squadRole", "unitSquadRole");
            SetNumber(so, 6f, "squadSizeContribution", "squadContribution");
            SetString(so, "Orbital spacing", "formationBehavior", "formation");
            SetString(so, "Maintain cosmic recon coverage",
                "intentInterpretationRules", "squadIntent", "intentRules");
            SetString(so, "None", "retreatBehavior", "squadRetreatBehavior");
            SetString(so, "Supports all grids", "reinforcementBehavior", "reinforcement");
            SetString(so, "Insight / Worldmap", "parentCommandUnit", "parentCommand", "parentUnit");
            SetString(so, "None", "subordinateUnits", "subordinates");
            SetNumber(so, 12f, "relayNodes", "relayNodeCount");
            SetString(so, "Satellite Mesh", "networkDependencies", "networkDependency");

            // VR operator mode
            SetString(so, "Deep-Space Recon Console", "operatorSeatType", "operatorSeat");
            SetString(so, "Cosmic Recon Overlay", "operatorHUD", "operatorHud", "hudName");
            SetString(so, "Optical + thermal", "cameraFeeds", "operatorCameraFeeds");
            SetString(so, "Deep-space recon grid / cosmic signal markers",
                "tacticalOverlay", "operatorTacticalOverlay");
            SetString(so,
                "Deep-space optics; Cosmic signal analysis; Thermal view; Orbital diagnostics",
                "operatorControls", "controls");
            SetString(so,
                "Cosmic Scan; Deep-Space Sweep; Relay Boost; Stabilization Burst",
                "operatorAbilities", "abilities");

            // Economy and logistics
            SetNumber(so, 2400f, "metalCost", "buildMetalCost", "costMetal");
            SetNumber(so, 1600f, "electronicsCost", "buildElectronicsCost", "costElectronics");
            SetNumber(so, 300f, "buildTime", "buildTimeSeconds");
            SetString(so, "Satellite Bay", "factoryType", "productionBuilding");
            SetString(so, "Satellite Systems III", "techRequirements", "technologyRequirement");
            SetString(so, "Low", "maintenanceCost", "maintenanceLevel");
            SetNumber(so, 70f, "repairTime", "repairTimeSeconds");
            SetString(so, "Very Low", "supplyConsumption", "supplyConsumptionLevel");
            SetString(so, "Very Low", "operationalCost", "operationalCostLevel");
            SetString(so, "Launch pad", "deploymentRequirements", "deploymentRequirement");
            SetString(so, "Satellite carrier", "transportCompatibility", "transportType");
            SetString(so, "Orbital insertion", "launchMethod", "deploymentMethod");

            // Player collection build requirements
            SetString(so,
                "Common: 130x Satellite Alloy Plates; 65x Reinforced Satellite Rods; 12x Hydraulic Actuators; 1x Cosmic Imaging Assembly",
                "commonComponents", "commonBuildRequirements");
            SetString(so,
                "Electronics: 50x Circuit Bundles; 34x Signal Processors; 12x Satellite Control Chips; 4x Recon AI Chips",
                "electronicsComponents", "electronicsBuildRequirements");
            SetString(so,
                "Power: 1x Expanded Solar Array Assembly; 2x Micro-Reactor Cores; 3x Power Regulation Nodes",
                "powerComponents", "powerBuildRequirements");
            SetString(so, "1x Cosmic Signal Analysis Module",
                "rareComponents", "rareBuildRequirements");
            SetString(so, "Enhanced Optics; Thermal Boost; Anti-Jamming Shielding",
                "optionalUpgrades", "upgradeOptions");

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log("[NEBULA] Canonical population pass finished. Verify serialized fields in the Inspector; unmatched field names are left unchanged.", asset);
        }
    }
}

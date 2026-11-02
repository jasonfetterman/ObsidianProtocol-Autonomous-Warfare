using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateCommandFusionCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Command/Fusion/Fusion.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Command Fusion Canonical Data")]
        public static void Populate()
        {
            UnitDefinition target =
                AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

            if (target == null)
            {
                Debug.LogError("[Fusion] UnitDefinition not found: " + AssetPath);
                return;
            }

            SerializedObject so = new SerializedObject(target);
            so.Update();

            SetString(so, "unitId", "obs-command-fusion");
            SetString(so, "id", "obs-command-fusion");
            SetString(so, "displayName", "Fusion");
            SetString(so, "role",
                "Multi-Node Command Integrator / Cross-Domain Coordinator");
            SetString(so, "tier", "Command");
            SetString(so, "status", "Active");

            SetFloat(so, "length", 12f);
            SetFloat(so, "width", 9f);
            SetFloat(so, "height", 6f);
            SetFloat(so, "mass", 26000f);
            SetFloat(so, "energyCapacity", 160000f);
            SetFloat(so, "structuralIntegrity", 4600f);
            SetFloat(so, "armorRating", 1.30f);
            SetFloat(so, "buildTime", 280f);
            SetFloat(so, "repairTime", 85f);
            SetFloat(so, "aggression", 0f);
            SetFloat(so, "caution", 1f);
            SetFloat(so, "persistence", 1f);

            SetInt(so, "relayCapacity", 10);
            SetInt(so, "relayNodes", 10);
            SetInt(so, "threatLevel", 7);
            SetInt(so, "squadSizeContribution", 8);

            SetString(so, "powerSource", "Dual command-grade reactors");
            SetString(so, "fuelCapacity", "N/A (reactor-based)");
            SetString(so, "endurance", "Continuous");
            SetString(so, "payloadCapacity", "Internal systems only");
            SetString(so, "navigationSystem",
                "Stationary (anchored command node)");
            SetString(so, "heatSignature", "Medium");
            SetString(so, "noiseSignature", "Low");
            SetString(so, "topSpeed", "N/A");
            SetString(so, "stabilityRating", "Maximum");

            SetString(so, "behaviorProfile",
                "Cross-domain coordination / mesh integration");
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
            SetString(so, "commandLinkType", "Strategic Command Mesh");
            SetString(so, "networkPriority", "Maximum");

            SetString(so, "firepowerRating", "None");
            SetString(so, "weaponMounts", "None");
            SetString(so, "trackingStrength",
                "Very High (cross-domain signal intelligence)");
            SetString(so, "disruptionStrength", "Very High");
            SetString(so, "countermeasureRating", "Extremely High");
            SetString(so, "preferredTerrain", "Secure command zones");
            SetString(so, "vulnerabilityProfile", "EMP / sabotage");

            SetString(so, "squadRole", "Multi-Domain Coordinator");
            SetString(so, "formationBehavior", "Central anchor");
            SetString(so, "intentInterpretationRules",
                "Integrate AIR, GROUND, SEA, and COMMAND grids");
            SetString(so, "retreatBehavior", "None");
            SetString(so, "reinforcementBehavior", "Supports all units");
            SetString(so, "parentCommandUnit", "Command Core");
            SetString(so, "subordinateUnits", "Nexus, Pulse");
            SetString(so, "networkDependencies", "Strategic Command Mesh");

            SetString(so, "operatorSeatType", "Multi-Domain Command Console");
            SetString(so, "operatorHUD", "Cross-Grid Overlay");
            SetString(so, "cameraFeeds", "Internal systems only");
            SetBool(so, "thermalView", true);
            SetString(so, "tacticalOverlay",
                "AIR / GROUND / SEA / COMMAND mesh visualization");

            SetString(so, "buildCost", "2600 Metal / 1600 Electronics");
            SetString(so, "factoryType", "Command Foundry");
            SetString(so, "techRequirements", "Command Systems II");
            SetString(so, "maintenanceCost", "High");
            SetString(so, "supplyConsumption", "Medium");
            SetString(so, "operationalCost", "Medium");
            SetString(so, "deploymentRequirements", "Command zone");
            SetString(so, "transportCompatibility", "Heavy command carriers");
            SetString(so, "launchMethod", "Anchor-install");

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                "[Fusion] Canonical population pass finished. " +
                "Only matching serialized fields were changed; " +
                "modelPrefab and storeImage were not modified.",
                target);
        }

        private static void SetString(SerializedObject so, string path, string value)
        {
            SerializedProperty p = so.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.String)
                p.stringValue = value;
        }

        private static void SetFloat(SerializedObject so, string path, float value)
        {
            SerializedProperty p = so.FindProperty(path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
            else if (p.propertyType == SerializedPropertyType.Integer)
                p.intValue = Mathf.RoundToInt(value);
        }

        private static void SetInt(SerializedObject so, string path, int value)
        {
            SerializedProperty p = so.FindProperty(path);
            if (p == null) return;

            if (p.propertyType == SerializedPropertyType.Integer)
                p.intValue = value;
            else if (p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
        }

        private static void SetBool(SerializedObject so, string path, bool value)
        {
            SerializedProperty p = so.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.Boolean)
                p.boolValue = value;
        }
    }
}


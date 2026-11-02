using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateCommandNexusCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Command/Nexus/Nexus.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Command Nexus Canonical Data")]
        public static void Populate()
        {
            UnitDefinition target =
                AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

            if (target == null)
            {
                Debug.LogError("[Nexus] UnitDefinition not found: " + AssetPath);
                return;
            }

            SerializedObject so = new SerializedObject(target);
            so.Update();

            SetString(so, "unitId", "obs-command-nexus");
            SetString(so, "id", "obs-command-nexus");
            SetString(so, "displayName", "Nexus");
            SetString(so, "role",
                "High-Capacity Strategic Router / Multi-Grid Synchronizer");
            SetString(so, "tier", "Command (Major Node)");
            SetString(so, "status", "Active");

            SetFloat(so, "length", 16f);
            SetFloat(so, "width", 11f);
            SetFloat(so, "height", 8f);
            SetFloat(so, "mass", 38000f);
            SetFloat(so, "energyCapacity", 260000f);
            SetFloat(so, "structuralIntegrity", 6000f);
            SetFloat(so, "armorRating", 1.50f);
            SetFloat(so, "buildTime", 340f);
            SetFloat(so, "repairTime", 100f);
            SetFloat(so, "aggression", 0f);
            SetFloat(so, "caution", 1f);
            SetFloat(so, "persistence", 1f);

            SetInt(so, "relayCapacity", 14);
            SetInt(so, "relayNodes", 14);
            SetInt(so, "threatLevel", 9);
            SetInt(so, "squadSizeContribution", 12);

            SetString(so, "powerSource", "Triple command-grade reactors");
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
                "Global synchronization / command routing");
            SetString(so, "retreatThreshold", "N/A");
            SetString(so, "targetPriority", "None (non-combat)");
            SetString(so, "formationDiscipline", "Maximum");
            SetString(so, "riskTolerance", "Very Low");

            SetString(so, "visualRange", "Short");
            SetString(so, "thermalRange", "Short");
            SetString(so, "acousticRange", "Short");
            SetString(so, "radarRange", "Medium");
            SetString(so, "signalDetectionRange", "Extremely High");

            SetString(so, "meshRange", "Global");
            SetString(so, "commandLinkType", "Strategic Command Mesh");
            SetString(so, "networkPriority", "Maximum");

            SetString(so, "firepowerRating", "None");
            SetString(so, "weaponMounts", "None");
            SetString(so, "trackingStrength",
                "Extremely High (command-grade signal intelligence)");
            SetString(so, "disruptionStrength", "Extremely High");
            SetString(so, "countermeasureRating", "Maximum");
            SetString(so, "preferredTerrain", "Secure command zones");
            SetString(so, "vulnerabilityProfile",
                "EMP / sabotage / coordinated strikes");

            SetString(so, "squadRole", "High-Capacity Synchronizer");
            SetString(so, "formationBehavior", "Central anchor");
            SetString(so, "intentInterpretationRules",
                "Synchronize AIR / GROUND / SEA / COMMAND grids");
            SetString(so, "retreatBehavior", "None");
            SetString(so, "reinforcementBehavior", "Supports all units");
            SetString(so, "parentCommandUnit", "Fusion / Command Core");
            SetString(so, "subordinateUnits", "Insight, Pulse, Vector Core");
            SetString(so, "networkDependencies", "Strategic Command Mesh");

            SetString(so, "operatorSeatType", "High-Capacity Command Console");
            SetString(so, "operatorHUD", "Multi-Grid Synchronization Overlay");
            SetString(so, "cameraFeeds", "Internal systems only");
            SetBool(so, "thermalView", true);
            SetString(so, "tacticalOverlay", "Full global mesh visualization");

            SetString(so, "buildCost", "3400 Metal / 2000 Electronics");
            SetString(so, "factoryType", "Command Foundry");
            SetString(so, "techRequirements", "Command Systems III");
            SetString(so, "maintenanceCost", "Very High");
            SetString(so, "supplyConsumption", "High");
            SetString(so, "operationalCost", "High");
            SetString(so, "deploymentRequirements", "Command zone");
            SetString(so, "transportCompatibility", "Heavy command carriers");
            SetString(so, "launchMethod", "Anchor-install");

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                "[Nexus] Canonical population pass finished. " +
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

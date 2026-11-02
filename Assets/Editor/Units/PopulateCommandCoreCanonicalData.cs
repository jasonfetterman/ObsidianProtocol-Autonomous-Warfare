using System;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Editor.Units
{
    public static class PopulateCommandCoreCanonicalData
    {
        private const string AssetPath =
            "Assets/Game/Units/Database/Command/Command_Core/Command_Core.asset";

        [MenuItem("Obsidian Protocol/Units/Populate Command Core Canonical Data")]
        public static void Populate()
        {
            UnitDefinition target =
                AssetDatabase.LoadAssetAtPath<UnitDefinition>(AssetPath);

            if (target == null)
            {
                Debug.LogError("[Command Core] UnitDefinition not found: " + AssetPath);
                return;
            }

            SerializedObject so = new SerializedObject(target);
            so.Update();

            SetString(so, "unitId", "obs-command-core");
            SetString(so, "id", "obs-command-core");
            SetString(so, "displayName", "Command Core");
            SetString(so, "role",
                "Primary Strategic Command / Global Coordination Hub");
            SetString(so, "tier", "Command (Top-Tier)");
            SetString(so, "description",
                "Primary strategic command node for global coordination, " +
                "command distribution, network control, and autonomous " +
                "force synchronization. Stationary, non-combat installation.");

            SetFloat(so, "length", 14f);
            SetFloat(so, "width", 10f);
            SetFloat(so, "height", 7f);
            SetFloat(so, "mass", 30000f);
            SetFloat(so, "energyCapacity", 200000f);
            SetFloat(so, "structuralIntegrity", 5000f);
            SetFloat(so, "armorRating", 1.4f);
            SetFloat(so, "commandLatency", 0.10f);
            SetFloat(so, "relayLoad", 38f);
            SetFloat(so, "signalIntegrity", 99.90f);
            SetFloat(so, "aiSyncRate", 99.95f);
            SetFloat(so, "tacticalRefreshInterval", 0.5f);
            SetFloat(so, "strategicRefreshInterval", 0.10f);
            SetInt(so, "relayCapacity", 12);
            SetInt(so, "relayNodes", 12);
            SetInt(so, "threatLevel", 8);
            SetInt(so, "squadSizeContribution", 10);

            SetBool(so, "autonomousOperation", true);
            SetBool(so, "autonomousAssignment", true);
            SetBool(so, "directCombatAssignment", false);
            SetBool(so, "directUnitMicromanagement", false);

            SetString(so, "missionType",
                "Primary Strategic Command / Global Coordination");
            SetString(so, "optimalDeploymentZone",
                "Secure command zone / hardened strategic facility");
            SetString(so, "navigationSystem",
                "Stationary (anchored command node)");
            SetString(so, "powerSource",
                "Dual high-capacity command reactors");
            SetString(so, "fuelCapacity", "N/A (reactor-based)");
            SetString(so, "endurance", "Continuous");
            SetString(so, "payloadCapacity", "Internal systems only");
            SetString(so, "commandRange", "Global");
            SetString(so, "meshRange", "Global");
            SetString(so, "commandLinkType", "Strategic Command Mesh");
            SetString(so, "networkPriority", "Maximum");
            SetString(so, "squadRole", "Primary Command Node");
            SetString(so, "formationBehavior", "Central anchor");
            SetString(so, "parentCommandUnit", "None (top of hierarchy)");

            SetString(so, "visualRange", "Short");
            SetString(so, "thermalRange", "Short");
            SetString(so, "acousticRange", "Short");
            SetString(so, "radarRange", "Medium");
            SetString(so, "lidarRange", "None");
            SetString(so, "signalDetectionRange", "Very High");
            SetString(so, "strategicDataDetection", "Global via network");

            SetString(so, "behaviorProfile",
                "Global command / coordination / strategic routing");
            SetFloat(so, "aggression", 0f);
            SetFloat(so, "caution", 1f);
            SetFloat(so, "persistence", 1f);
            SetFloat(so, "flankingLikelihood", 0f);
            SetFloat(so, "riskTolerance", 0f);
            SetString(so, "targetPriority", "None (non-combat)");
            SetString(so, "firepowerRating", "None");
            SetString(so, "weaponMounts", "None");
            SetString(so, "weaponTypes", "None");

            SetString(so, "buildCost",
                "3000 Metal / 1800 Electronics");
            SetFloat(so, "buildTime", 300f);
            SetString(so, "factoryType", "Command Foundry");
            SetString(so, "techRequirements", "Command Systems II");
            SetString(so, "maintenanceCost", "Very High");
            SetString(so, "deploymentRequirements", "Command zone");
            SetString(so, "launchMethod", "Anchor-install");
            SetString(so, "infrastructureRequirement",
                "Hardened Command Facility");

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                "[Command Core] Canonical population pass finished. " +
                "Unsupported or differently named fields were skipped; " +
                "modelPrefab and storeImage were not modified.",
                target);
        }

        private static void SetString(SerializedObject so, string path, string value)
        {
            SerializedProperty p = so.FindProperty(path);
            if (p == null || p.propertyType != SerializedPropertyType.String)
                return;
            p.stringValue = value;
        }

        private static void SetFloat(SerializedObject so, string path, float value)
        {
            SerializedProperty p = so.FindProperty(path);
            if (p == null)
                return;

            if (p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
            else if (p.propertyType == SerializedPropertyType.Integer)
                p.intValue = Mathf.RoundToInt(value);
        }

        private static void SetInt(SerializedObject so, string path, int value)
        {
            SerializedProperty p = so.FindProperty(path);
            if (p == null)
                return;

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

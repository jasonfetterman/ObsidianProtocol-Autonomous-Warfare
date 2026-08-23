#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianGarage25Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 25 - Garage")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform garage = GetOrCreate("Garage", hud.transform);

            // =================================================
            // GARAGE MAIN
            // =================================================

            Transform main = GetOrCreate("Main", garage);

            CreateNode("GAR-001 Garage Entry Screen", main);
            CreateNode("GAR-002 Garage Overview", main);
            CreateNode("GAR-003 Fleet Overview", main);
            CreateNode("GAR-004 Available Slots", main);
            CreateNode("GAR-005 Facility Status", main);

            // =================================================
            // UNIT INSPECTION
            // =================================================

            Transform inspection = GetOrCreate("Unit Inspection", garage);

            CreateNode("GAR-006 Unit Inspection", inspection);
            CreateNode("GAR-007 Rotate Unit", inspection);
            CreateNode("GAR-008 Zoom Unit", inspection);
            CreateNode("GAR-009 Unit Information", inspection);
            CreateNode("GAR-010 Unit Statistics", inspection);
            CreateNode("GAR-011 Unit Condition", inspection);

            // =================================================
            // CONFIGURATION
            // =================================================

            Transform configuration = GetOrCreate("Configuration", garage);

            CreateNode("GAR-012 Configuration Screen", configuration);
            CreateNode("GAR-013 Equipment", configuration);
            CreateNode("GAR-014 Weapons", configuration);
            CreateNode("GAR-015 Armor", configuration);
            CreateNode("GAR-016 Sensors", configuration);
            CreateNode("GAR-017 Modules", configuration);
            CreateNode("GAR-018 AI Personality", configuration);
            CreateNode("GAR-019 AI Behavior", configuration);
            CreateNode("GAR-020 Loadout Save", configuration);

            // =================================================
            // MAINTENANCE
            // =================================================

            Transform maintenance = GetOrCreate("Maintenance", garage);

            CreateNode("GAR-021 Repair Screen", maintenance);
            CreateNode("GAR-022 Repair Condition", maintenance);
            CreateNode("GAR-023 Repair Cost", maintenance);
            CreateNode("GAR-024 Repair Time", maintenance);
            CreateNode("GAR-025 Repair Confirmation", maintenance);

            // =================================================
            // CUSTOMIZATION
            // =================================================

            Transform customization = GetOrCreate("Customization", garage);

            CreateNode("GAR-026 Paint", customization);
            CreateNode("GAR-027 Color Selection", customization);
            CreateNode("GAR-028 Decals", customization);
            CreateNode("GAR-029 Emblems", customization);
            CreateNode("GAR-030 Unit Name", customization);
            CreateNode("GAR-031 Customization Preview", customization);
            CreateNode("GAR-032 Save Customization", customization);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject = garage.gameObject;

            Debug.Log(
                "[UI] PHASE 25 - GARAGE hierarchy built successfully."
            );
        }

        private static Transform GetOrCreate(
            string objectName,
            Transform parent
        )
        {
            Transform existing = parent.Find(objectName);

            if (existing != null)
            {
                return existing;
            }

            GameObject obj = new GameObject(objectName);
            obj.transform.SetParent(parent, false);

            return obj.transform;
        }

        private static void CreateNode(
            string objectName,
            Transform parent
        )
        {
            if (parent.Find(objectName) != null)
            {
                return;
            }

            GameObject obj = new GameObject(objectName);
            obj.transform.SetParent(parent, false);
        }
    }
}
#endif

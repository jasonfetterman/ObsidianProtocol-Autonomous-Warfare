#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianNotifications29Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 29 - Notifications")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform notifications =
                GetOrCreate("Notifications", hud.transform);

            // =================================================
            // RESOURCES
            // =================================================

            Transform resources =
                GetOrCreate("Resources", notifications);

            CreateNode("NOTIF-001 Resource Gained", resources);
            CreateNode("NOTIF-002 Resource Depleted", resources);

            // =================================================
            // UNITS
            // =================================================

            Transform units =
                GetOrCreate("Units", notifications);

            CreateNode("NOTIF-003 Unit Produced", units);
            CreateNode("NOTIF-004 Unit Deployed", units);
            CreateNode("NOTIF-005 Unit Destroyed", units);

            // =================================================
            // SQUADS
            // =================================================

            Transform squads =
                GetOrCreate("Squads", notifications);

            CreateNode("NOTIF-006 Squad Created", squads);
            CreateNode("NOTIF-007 Squad Destroyed", squads);

            // =================================================
            // OBJECTIVES
            // =================================================

            Transform objectives =
                GetOrCreate("Objectives", notifications);

            CreateNode("NOTIF-008 Objective Completed", objectives);
            CreateNode("NOTIF-009 Objective Failed", objectives);

            // =================================================
            // UNLOCKS
            // =================================================

            Transform unlocks =
                GetOrCreate("Unlocks", notifications);

            CreateNode("NOTIF-010 Technology Unlocked", unlocks);
            CreateNode("NOTIF-011 Unit Unlocked", unlocks);
            CreateNode("NOTIF-012 Equipment Unlocked", unlocks);
            CreateNode("NOTIF-013 Mission Unlocked", unlocks);
            CreateNode("NOTIF-014 Achievement Unlocked", unlocks);

            // =================================================
            // OPERATIONS
            // =================================================

            Transform operations =
                GetOrCreate("Operations", notifications);

            CreateNode("NOTIF-015 Construction Complete", operations);
            CreateNode("NOTIF-016 Production Complete", operations);
            CreateNode("NOTIF-017 Repair Complete", operations);
            CreateNode("NOTIF-018 Research Complete", operations);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject =
                notifications.gameObject;

            Debug.Log(
                "[UI] PHASE 29 - NOTIFICATIONS hierarchy built successfully."
            );
        }

        private static Transform GetOrCreate(
            string objectName,
            Transform parent
        )
        {
            Transform existing =
                parent.Find(objectName);

            if (existing != null)
            {
                return existing;
            }

            GameObject obj =
                new GameObject(objectName);

            obj.transform.SetParent(
                parent,
                false
            );

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

            GameObject obj =
                new GameObject(objectName);

            obj.transform.SetParent(
                parent,
                false
            );
        }
    }
}
#endif

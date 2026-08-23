#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianTooltips32Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 32 - Tooltips")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform tooltips =
                GetOrCreate("Tooltips", hud.transform);

            // =================================================
            // UNITS
            // =================================================

            Transform units =
                GetOrCreate("Units", tooltips);

            CreateNode(
                "TIP-001 Unit Tooltip",
                units
            );

            // =================================================
            // EQUIPMENT
            // =================================================

            Transform equipment =
                GetOrCreate("Equipment", tooltips);

            CreateNode(
                "TIP-002 Weapon Tooltip",
                equipment
            );

            CreateNode(
                "TIP-003 Equipment Tooltip",
                equipment
            );

            // =================================================
            // ABILITIES
            // =================================================

            Transform abilities =
                GetOrCreate("Abilities", tooltips);

            CreateNode(
                "TIP-004 Ability Tooltip",
                abilities
            );

            // =================================================
            // RESOURCES
            // =================================================

            Transform resources =
                GetOrCreate("Resources", tooltips);

            CreateNode(
                "TIP-005 Resource Tooltip",
                resources
            );

            // =================================================
            // STRUCTURES
            // =================================================

            Transform structures =
                GetOrCreate("Structures", tooltips);

            CreateNode(
                "TIP-006 Building Tooltip",
                structures
            );

            // =================================================
            // TECHNOLOGY
            // =================================================

            Transform technology =
                GetOrCreate("Technology", tooltips);

            CreateNode(
                "TIP-007 Technology Tooltip",
                technology
            );

            // =================================================
            // AUTONOMY
            // =================================================

            Transform autonomy =
                GetOrCreate("Autonomy", tooltips);

            CreateNode(
                "TIP-008 Intent Tooltip",
                autonomy
            );

            CreateNode(
                "TIP-009 AI State Tooltip",
                autonomy
            );

            // =================================================
            // DEPLOYMENT
            // =================================================

            Transform deployment =
                GetOrCreate("Deployment", tooltips);

            CreateNode(
                "TIP-010 Deployment Point Tooltip",
                deployment
            );

            // =================================================
            // OBJECTIVES
            // =================================================

            Transform objectives =
                GetOrCreate("Objectives", tooltips);

            CreateNode(
                "TIP-011 Objective Tooltip",
                objectives
            );

            // =================================================
            // STATUS
            // =================================================

            Transform status =
                GetOrCreate("Status", tooltips);

            CreateNode(
                "TIP-012 Status Effect Tooltip",
                status
            );

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject =
                tooltips.gameObject;

            Debug.Log(
                "[UI] PHASE 32 - TOOLTIP hierarchy built successfully."
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

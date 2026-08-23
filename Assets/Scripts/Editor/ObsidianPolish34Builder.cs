#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianPolish34Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 34 - HUD Polish")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform polish =
                GetOrCreate("Polish", hud.transform);

            // =================================================
            // LAYOUT
            // =================================================

            Transform layout =
                GetOrCreate("Layout", polish);

            CreateNode(
                "POLISH-001 HUD Spacing",
                layout
            );

            CreateNode(
                "POLISH-002 Alignment",
                layout
            );

            // =================================================
            // VISUAL
            // =================================================

            Transform visual =
                GetOrCreate("Visual", polish);

            CreateNode(
                "POLISH-003 Typography",
                visual
            );

            CreateNode(
                "POLISH-004 Icon Consistency",
                visual
            );

            CreateNode(
                "POLISH-005 Panel Hierarchy",
                visual
            );

            // =================================================
            // ANIMATION
            // =================================================

            Transform animation =
                GetOrCreate("Animation", polish);

            CreateNode(
                "POLISH-006 Selection Animations",
                animation
            );

            CreateNode(
                "POLISH-007 Alert Animations",
                animation
            );

            CreateNode(
                "POLISH-008 Popup Animations",
                animation
            );

            CreateNode(
                "POLISH-009 Window Transitions",
                animation
            );

            CreateNode(
                "POLISH-010 Map Transitions",
                animation
            );

            // =================================================
            // INTERACTION
            // =================================================

            Transform interaction =
                GetOrCreate("Interaction", polish);

            CreateNode(
                "POLISH-011 Hover Feedback",
                interaction
            );

            CreateNode(
                "POLISH-012 Click Feedback",
                interaction
            );

            CreateNode(
                "POLISH-013 Error Feedback",
                interaction
            );

            // =================================================
            // FEEDBACK
            // =================================================

            Transform feedback =
                GetOrCreate("Feedback", polish);

            CreateNode(
                "POLISH-014 Audio Feedback",
                feedback
            );

            CreateNode(
                "POLISH-015 Controller Feedback",
                feedback
            );

            CreateNode(
                "POLISH-016 VR Feedback",
                feedback
            );

            // =================================================
            // TESTING
            // =================================================

            Transform testing =
                GetOrCreate("Testing", polish);

            CreateNode(
                "POLISH-017 Resolution Testing",
                testing
            );

            CreateNode(
                "POLISH-018 Ultrawide Testing",
                testing
            );

            CreateNode(
                "POLISH-019 VR Readability",
                testing
            );

            // =================================================
            // PERFORMANCE
            // =================================================

            Transform performance =
                GetOrCreate("Performance", polish);

            CreateNode(
                "POLISH-020 Performance Optimization",
                performance
            );

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject =
                polish.gameObject;

            Debug.Log(
                "[UI] PHASE 34 - HUD POLISH hierarchy built successfully."
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

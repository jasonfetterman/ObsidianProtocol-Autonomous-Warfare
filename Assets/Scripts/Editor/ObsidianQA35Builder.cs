#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianQA35Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 35 - Final UI QA")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform qa =
                GetOrCreate("QA", hud.transform);

            // =================================================
            // COVERAGE
            // =================================================

            Transform coverage =
                GetOrCreate("Coverage", qa);

            CreateNode("QA-001 HUD Elements Test", coverage);
            CreateNode("QA-002 Popups Test", coverage);
            CreateNode("QA-003 Windows Test", coverage);
            CreateNode("QA-004 Menus Test", coverage);
            CreateNode("QA-005 Screens Test", coverage);
            CreateNode("QA-006 Overlays Test", coverage);
            CreateNode("QA-007 Buttons Test", coverage);
            CreateNode("QA-008 Input Test", coverage);

            // =================================================
            // INPUT
            // =================================================

            Transform input =
                GetOrCreate("Input", qa);

            CreateNode("QA-009 Keyboard Test", input);
            CreateNode("QA-010 Mouse Test", input);

            // =================================================
            // PLATFORM
            // =================================================

            Transform platform =
                GetOrCreate("Platform", qa);

            CreateNode("QA-011 Controller Test", platform);
            CreateNode("QA-012 VR Test", platform);

            // =================================================
            // GAME MODES
            // =================================================

            Transform gameModes =
                GetOrCreate("Game Modes", qa);

            CreateNode("QA-013 Multiplayer Test", gameModes);
            CreateNode("QA-014 Campaign Test", gameModes);
            CreateNode("QA-015 Garage Test", gameModes);
            CreateNode("QA-016 Store Test", gameModes);
            CreateNode("QA-017 Deployment Test", gameModes);

            // =================================================
            // PERSISTENCE
            // =================================================

            Transform persistence =
                GetOrCreate("Persistence", qa);

            CreateNode("QA-018 Save Load Test", persistence);

            // =================================================
            // QUALITY
            // =================================================

            Transform quality =
                GetOrCreate("Quality", qa);

            CreateNode("QA-019 Resolution Test", quality);
            CreateNode("QA-020 Performance Test", quality);
            CreateNode("QA-021 Accessibility Test", quality);

            // =================================================
            // ASSETS
            // =================================================

            Transform assets =
                GetOrCreate("Assets", qa);

            CreateNode("QA-022 UI Asset Audit", assets);
            CreateNode("QA-023 Missing Image Audit", assets);
            CreateNode("QA-024 Missing Icon Audit", assets);

            // =================================================
            // INTEGRATION
            // =================================================

            Transform integration =
                GetOrCreate("Integration", qa);

            CreateNode(
                "QA-025 Final UI Integration Test",
                integration
            );

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject =
                qa.gameObject;

            Debug.Log(
                "[UI] PHASE 35 - FINAL UI QA hierarchy built successfully."
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

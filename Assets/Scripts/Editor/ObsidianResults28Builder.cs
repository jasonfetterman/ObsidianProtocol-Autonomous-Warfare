#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianResults28Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 28 - Results")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform results = GetOrCreate("Results", hud.transform);

            // =================================================
            // OUTCOME
            // =================================================

            Transform outcome = GetOrCreate("Outcome", results);

            CreateNode("RESULT-001 Victory Screen", outcome);
            CreateNode("RESULT-002 Defeat Screen", outcome);
            CreateNode("RESULT-003 Draw Screen", outcome);

            // =================================================
            // STATISTICS
            // =================================================

            Transform statistics = GetOrCreate("Statistics", results);

            CreateNode("RESULT-004 Match Statistics", statistics);
            CreateNode("RESULT-005 Units Deployed", statistics);
            CreateNode("RESULT-006 Units Lost", statistics);
            CreateNode("RESULT-007 Damage Dealt", statistics);
            CreateNode("RESULT-008 Damage Received", statistics);
            CreateNode("RESULT-009 Objectives Completed", statistics);
            CreateNode("RESULT-010 Resources Collected", statistics);
            CreateNode("RESULT-011 Economy Statistics", statistics);

            // =================================================
            // PERFORMANCE
            // =================================================

            Transform performance = GetOrCreate("Performance", results);

            CreateNode("RESULT-012 AI Autonomy Statistics", performance);
            CreateNode("RESULT-013 Player Performance", performance);

            // =================================================
            // REWARDS
            // =================================================

            Transform rewards = GetOrCreate("Rewards", results);

            CreateNode("RESULT-014 Rewards", rewards);
            CreateNode("RESULT-015 Progression", rewards);

            // =================================================
            // NAVIGATION
            // =================================================

            Transform navigation = GetOrCreate("Navigation", results);

            CreateNode("RESULT-016 Continue", navigation);
            CreateNode("RESULT-017 Return to Garage", navigation);
            CreateNode("RESULT-018 Return to Multiplayer", navigation);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject = results.gameObject;

            Debug.Log(
                "[UI] PHASE 28 - RESULTS hierarchy built successfully."
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

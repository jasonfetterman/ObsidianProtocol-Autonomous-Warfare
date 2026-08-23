#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianCampaign27Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 27 - Campaign")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform campaign = GetOrCreate("Campaign", hud.transform);

            // =================================================
            // CAMPAIGN MAIN
            // =================================================

            Transform main = GetOrCreate("Main", campaign);

            CreateNode("CAM-001 Campaign Home", main);

            // =================================================
            // CAMPAIGN MAP
            // =================================================

            Transform map = GetOrCreate("Map", campaign);

            CreateNode("CAM-002 Campaign Map", map);
            CreateNode("CAM-003 Region Selection", map);

            // =================================================
            // MISSIONS
            // =================================================

            Transform missions = GetOrCreate("Missions", campaign);

            CreateNode("CAM-004 Mission Selection", missions);
            CreateNode("CAM-005 Mission Briefing", missions);
            CreateNode("CAM-006 Mission Objectives", missions);
            CreateNode("CAM-007 Mission Requirements", missions);

            // =================================================
            // DEPLOYMENT
            // =================================================

            Transform deployment = GetOrCreate("Deployment", campaign);

            CreateNode("CAM-008 Deployment Preparation", deployment);
            CreateNode("CAM-009 Mission Loading", deployment);
            CreateNode("CAM-010 Mission HUD", deployment);

            // =================================================
            // RESULTS
            // =================================================

            Transform results = GetOrCreate("Results", campaign);

            CreateNode("CAM-011 Mission Progress", results);
            CreateNode("CAM-012 Mission Complete", results);
            CreateNode("CAM-013 Mission Failed", results);
            CreateNode("CAM-014 Mission Rewards", results);

            // =================================================
            // PROGRESSION
            // =================================================

            Transform progression = GetOrCreate("Progression", campaign);

            CreateNode("CAM-015 Campaign Progression", progression);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject = campaign.gameObject;

            Debug.Log(
                "[UI] PHASE 27 - CAMPAIGN hierarchy built successfully."
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

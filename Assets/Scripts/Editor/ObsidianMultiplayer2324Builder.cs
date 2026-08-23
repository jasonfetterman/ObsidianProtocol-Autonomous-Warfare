#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianMultiplayer2324Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 23-24")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform menus = GetOrCreate("Menus", hud.transform);
            Transform multiplayer = GetOrCreate("Multiplayer", menus);

            // =================================================
            // PHASE 23 — MULTIPLAYER MENUS
            // =================================================

            Transform home = GetOrCreate("Home", multiplayer);

            Transform homeCore = GetOrCreate("Core", home);
            Transform matchSetup = GetOrCreate("MatchSetup", home);
            Transform playerSetup = GetOrCreate("PlayerSetup", home);
            Transform readiness = GetOrCreate("Readiness", home);
            Transform transition = GetOrCreate("Transition", home);

            CreateNode("MP-001 Multiplayer Home", homeCore);
            CreateNode("MP-002 Play", homeCore);

            CreateNode("MP-003 Create Match", matchSetup);
            CreateNode("MP-004 Join Match", matchSetup);
            CreateNode("MP-005 Server Browser", matchSetup);
            CreateNode("MP-006 Match Type", matchSetup);
            CreateNode("MP-007 Map Selection", matchSetup);
            CreateNode("MP-011 Match Settings", matchSetup);
            CreateNode("MP-012 Deployment Budget", matchSetup);

            CreateNode("MP-008 Player Count", playerSetup);
            CreateNode("MP-009 Team Selection", playerSetup);
            CreateNode("MP-010 Faction Selection", playerSetup);

            CreateNode("MP-013 Ready State", readiness);
            CreateNode("MP-014 Player List", readiness);
            CreateNode("MP-015 Team List", readiness);
            CreateNode("MP-016 Match Countdown", readiness);

            CreateNode("MP-017 Loading Screen", transition);

            // =================================================
            // PHASE 24 — MULTIPLAYER IN-MATCH UI
            // =================================================

            Transform match = GetOrCreate("Match", multiplayer);

            Transform status = GetOrCreate("Status", match);
            Transform objectives = GetOrCreate("Objectives", match);
            Transform resources = GetOrCreate("Resources", match);
            Transform deployment = GetOrCreate("Deployment", match);
            Transform scoring = GetOrCreate("Scoring", match);
            Transform communication = GetOrCreate("Communication", match);
            Transform playerInfo = GetOrCreate("PlayerInfo", match);
            Transform spectator = GetOrCreate("Spectator", match);

            CreateNode("MP-018 Player Status", status);
            CreateNode("MP-019 Team Status", status);

            CreateNode("MP-020 Team Objectives", objectives);

            CreateNode("MP-021 Team Resources", resources);

            CreateNode("MP-022 Team Deployment", deployment);

            CreateNode("MP-023 Score", scoring);
            CreateNode("MP-024 Match Timer", scoring);
            CreateNode("MP-028 Scoreboard", scoring);

            CreateNode("MP-025 Player Communication", communication);
            CreateNode("MP-026 Team Communication", communication);

            CreateNode("MP-027 Player List", playerInfo);

            CreateNode("MP-029 Spectator UI", spectator);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject = multiplayer.gameObject;

            Debug.Log(
                "[UI] PHASE 23-24 hierarchy built successfully."
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

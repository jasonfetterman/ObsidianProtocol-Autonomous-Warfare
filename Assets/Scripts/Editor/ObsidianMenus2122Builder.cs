#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianMenus2122Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 21-22")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found.");
                return;
            }

            Transform menus = GetOrCreate("Menus", hud.transform);

            // =================================================
            // PHASE 21 — PAUSE MENU
            // =================================================

            Transform pause = GetOrCreate("Pause", menus);

            Transform pauseCore = GetOrCreate("Core", pause);
            Transform pauseActions = GetOrCreate("Actions", pause);
            Transform pauseNavigation = GetOrCreate("Navigation", pause);

            CreateNode("MENU-001 PauseOverlay", pauseCore);

            CreateNode("MENU-002 Resume", pauseActions);
            CreateNode("MENU-003 Settings", pauseActions);
            CreateNode("MENU-004 Controls", pauseActions);
            CreateNode("MENU-005 Objectives", pauseActions);
            CreateNode("MENU-006 Map", pauseActions);
            CreateNode("MENU-007 Restart", pauseActions);
            CreateNode("MENU-008 ExitMatch", pauseNavigation);
            CreateNode("MENU-009 QuitGame", pauseNavigation);

            // =================================================
            // PHASE 22 — SETTINGS
            // =================================================

            Transform settings = GetOrCreate("Settings", menus);

            Transform settingsCore = GetOrCreate("Core", settings);
            Transform settingsDisplay = GetOrCreate("Display", settings);
            Transform settingsAudio = GetOrCreate("Audio", settings);
            Transform settingsGameplay = GetOrCreate("Gameplay", settings);
            Transform settingsInput = GetOrCreate("Input", settings);
            Transform settingsAccessibility = GetOrCreate("Accessibility", settings);
            Transform settingsActions = GetOrCreate("Actions", settings);

            CreateNode("MENU-010 SettingsHome", settingsCore);

            CreateNode("MENU-011 Video", settingsDisplay);
            CreateNode("MENU-012 Graphics", settingsDisplay);
            CreateNode("MENU-013 Display", settingsDisplay);

            CreateNode("MENU-014 Audio", settingsAudio);

            CreateNode("MENU-015 Gameplay", settingsGameplay);

            CreateNode("MENU-016 Controls", settingsInput);
            CreateNode("MENU-017 Mouse", settingsInput);
            CreateNode("MENU-018 Keyboard", settingsInput);
            CreateNode("MENU-019 Controller", settingsInput);
            CreateNode("MENU-020 VR", settingsInput);

            CreateNode("MENU-021 Accessibility", settingsAccessibility);
            CreateNode("MENU-022 UI Scale", settingsAccessibility);
            CreateNode("MENU-023 HUD Customization", settingsAccessibility);
            CreateNode("MENU-024 Color Display Options", settingsAccessibility);

            CreateNode("MENU-025 Apply", settingsActions);
            CreateNode("MENU-026 Reset", settingsActions);
            CreateNode("MENU-027 Back", settingsActions);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject = menus.gameObject;

            Debug.Log(
                "[UI] PHASE 21-22 hierarchy built successfully."
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

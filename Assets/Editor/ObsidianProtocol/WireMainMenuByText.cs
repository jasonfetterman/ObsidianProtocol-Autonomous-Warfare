using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ObsidianProtocol.Editor
{
    public static class WireMainMenuByText
    {
        [MenuItem("Tools/Obsidian Protocol/WIRE MAIN MENU BY TEXT")]
        public static void Wire()
        {
            Scene scene = SceneManager.GetActiveScene();

            List<Button> buttons = new List<Button>();

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                buttons.AddRange(root.GetComponentsInChildren<Button>(true));
            }

            WireButton(buttons, "Continue", "Command_Center");
            WireButton(buttons, "Campaign", "Command_Center");
            WireButton(buttons, "Multiplayer", "MultiplayerHUD");
            WireButton(buttons, "Garage", "GARAGE");
            WireButton(buttons, "Store", "Store");
            WireButton(buttons, "VR Operator", "VR_Command");
            WireButton(buttons, "Settings", "Settings");

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Debug.Log("[Obsidian Protocol] Main Menu persistent wiring complete.");

            EditorUtility.DisplayDialog(
                "Obsidian Protocol",
                "Main Menu buttons wired and saved.",
                "OK");
        }

        private static void WireButton(
            List<Button> buttons,
            string label,
            string sceneName)
        {
            Button target = null;

            foreach (Button button in buttons)
            {
                TMPro.TMP_Text tmp = button.GetComponentInChildren<TMPro.TMP_Text>(true);

                if (tmp != null && tmp.text.Trim() == label)
                {
                    target = button;
                    break;
                }

                UnityEngine.UI.Text legacy =
                    button.GetComponentInChildren<UnityEngine.UI.Text>(true);

                if (legacy != null && legacy.text.Trim() == label)
                {
                    target = button;
                    break;
                }
            }

            if (target == null)
            {
                Debug.LogWarning(
                    "[Obsidian Protocol] Could not find button text: " + label);
                return;
            }

            for (int i = target.onClick.GetPersistentEventCount() - 1; i >= 0; i--)
            {
                UnityEventTools.RemovePersistentListener(target.onClick, i);
            }

            UnityEventTools.AddPersistentListener(
                target.onClick,
                CreateSceneLoader(sceneName));

            EditorUtility.SetDirty(target);

            Debug.Log(
                "[Obsidian Protocol] Wired: " +
                label +
                " -> " +
                sceneName);
        }

        private static UnityAction CreateSceneLoader(string sceneName)
        {
            return () => SceneManager.LoadScene(sceneName);
        }
    }
}

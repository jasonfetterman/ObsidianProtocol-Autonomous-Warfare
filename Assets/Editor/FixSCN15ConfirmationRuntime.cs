using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FixSCN15ConfirmationRuntime
{
    [MenuItem("Obsidian Protocol/SCN-15/FIX CONFIRMATION PANEL RUNTIME")]
    public static void Fix()
    {
        string[] guids = AssetDatabase.FindAssets("Exit_Confirmation t:Scene");

        string scenePath = null;

        foreach (string guid in guids)
        {
            string candidate = AssetDatabase.GUIDToAssetPath(guid);

            if (candidate.Contains("EXIT CONFIRMATION"))
            {
                scenePath = candidate;
                break;
            }
        }

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("[SCN-15] Exit Confirmation scene not found.");
            return;
        }

        Debug.Log("[SCN-15] Opening: " + scenePath);

        Scene scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject popup = GameObject.Find("POP-024 EXIT CONFIRMATION");

        if (popup == null)
        {
            Debug.LogError(
                "[SCN-15] Could not find POP-024 EXIT CONFIRMATION."
            );

            return;
        }

        // Remove the generic UIConfirmationSystem.
        // SCN-15 uses POP-024 directly.
        Component confirmationSystem =
            popup.GetComponent(
                System.Type.GetType(
                    "ObsidianProtocol.UI.UIConfirmationSystem, Assembly-CSharp"
                )
            );

        if (confirmationSystem != null)
        {
            Object.DestroyImmediate(confirmationSystem);

            Debug.Log(
                "[SCN-15] Removed UIConfirmationSystem. " +
                "POP-024 now controls the confirmation screen directly."
            );
        }

        GameObject window =
            GameObject.Find("EXIT CONFIRMATION WINDOW");

        if (window != null)
        {
            window.SetActive(true);

            Debug.Log(
                "[SCN-15] EXIT CONFIRMATION WINDOW forced ACTIVE."
            );
        }
        else
        {
            Debug.LogError(
                "[SCN-15] EXIT CONFIRMATION WINDOW not found."
            );
        }

        Canvas canvas =
            GameObject.Find("EXIT CONFIRMATION HUD")
                ?.GetComponent<Canvas>();

        if (canvas != null)
        {
            canvas.enabled = true;
            canvas.gameObject.SetActive(true);

            Debug.Log(
                "[SCN-15] EXIT CONFIRMATION HUD forced ACTIVE."
            );
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[SCN-15] FIX COMPLETE. " +
            "Test SCN-15 in Play Mode."
        );
    }
}

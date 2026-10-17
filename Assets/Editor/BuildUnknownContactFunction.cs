using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Intelligence;

public static class BuildUnknownContactFunction
{
    [MenuItem("Obsidian Protocol/Fix/Build UNKNOWN Contact Function")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError("[INTELLIGENCE] INTELLIGENCE HUD not found.");
            return;
        }

        IntelligenceContactFunctionController controller =
            hud.GetComponent<IntelligenceContactFunctionController>();

        if (controller == null)
            controller = hud.AddComponent<IntelligenceContactFunctionController>();

        Transform buttonTransform = FindChild(hud.transform, "BUTTON - UNKNOWN");

        if (buttonTransform == null)
        {
            Debug.LogError("[INTELLIGENCE] BUTTON - UNKNOWN not found.");
            return;
        }

        Button button = buttonTransform.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("[INTELLIGENCE] BUTTON - UNKNOWN has no Button component.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(controller.ShowUnknown);

        EditorSceneManager.MarkSceneDirty(hud.scene);
        EditorSceneManager.SaveScene(hud.scene);

        Debug.Log("[INTELLIGENCE] UNKNOWN contact function built and wired.");
    }

    private static Transform FindChild(Transform parent, string objectName)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == objectName)
                return child;
        }

        return null;
    }
}

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class SCN17AddSceneRoster
{
    [MenuItem("Obsidian Protocol/SCN17/Add Scene Roster")]
    public static void AddSceneRoster()
    {
        const string scenePath =
            "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath, OpenSceneMode.Single);

        GameObject hud = null;
        foreach (var root in scene.GetRootGameObjects())
        {
            if (root.name == "PERSISTENT MILITARY HUD")
            {
                hud = root;
                break;
            }
        }

        if (hud == null || hud.GetComponent<Canvas>() == null)
        {
            Debug.LogError("SCN17 roster: HUD Canvas not found. Scene unchanged.");
            return;
        }

        Transform existing = hud.transform.Find("RUNTIME UNIT SELECTOR");
        GameObject panel;

        if (existing != null)
        {
            panel = existing.gameObject;
        }
        else
        {
            panel = new GameObject(
                "RUNTIME UNIT SELECTOR",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image));

            Undo.RegisterCreatedObjectUndo(panel, "Create SCN17 Scene Roster");
            panel.transform.SetParent(hud.transform, false);
        }

        var rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.anchoredPosition = new Vector2(-24f, -24f);
        rect.sizeDelta = new Vector2(300f, 220f);

        var image = panel.GetComponent<Image>();
        image.color = new Color(0.035f, 0.055f, 0.075f, 0.96f);
        image.raycastTarget = false;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Selection.activeGameObject = panel;
        Debug.Log("SCN17 roster created and scene saved. Select RUNTIME UNIT SELECTOR to adjust its RectTransform.");
    }
}

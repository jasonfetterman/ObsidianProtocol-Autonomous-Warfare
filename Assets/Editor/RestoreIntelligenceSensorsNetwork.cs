using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RestoreIntelligenceSensorsNetwork
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Restore Sensors Network")]
    public static void Restore()
    {
        Scene scene =
            EditorSceneManager.OpenScene(
                ScenePath,
                OpenSceneMode.Single);

        GameObject hud =
            GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError("[INTELLIGENCE] HUD not found.");
            return;
        }

        RestorePanel(
            hud,
            "SENSORS",
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(585f, 215f),
            new Vector2(330f, 370f));

        RestorePanel(
            hud,
            "NETWORK",
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(585f, -190f),
            new Vector2(330f, 370f));

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[INTELLIGENCE] SENSORS AND NETWORK RESTORED.");
    }

    private static void RestorePanel(
        GameObject hud,
        string name,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 position,
        Vector2 size)
    {
        GameObject panel = Find(hud, name);

        if (panel == null)
        {
            Debug.LogError(
                "[INTELLIGENCE] Missing panel: " + name);
            return;
        }

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        if (rect == null)
            return;

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    private static GameObject Find(
        GameObject root,
        string name)
    {
        foreach (
            Transform child
            in root.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
                return child.gameObject;
        }

        return null;
    }
}

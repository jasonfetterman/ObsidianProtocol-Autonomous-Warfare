using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FixIntelligenceHeaderOverlap
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Fix Header Overlap")]
    public static void Fix()
    {
        Scene scene =
            EditorSceneManager.OpenScene(
                ScenePath,
                OpenSceneMode.Single);

        GameObject hud = GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError("[INTELLIGENCE] HUD not found.");
            return;
        }

        Move(hud, "CONTACTS", 20f);
        Move(hud, "SENSORS", -190f);
        Move(hud, "NETWORK", -570f);

        Move(hud, "THREAT ANALYSIS", 195f);
        Move(hud, "INTELLIGENCE REPORTS", -195f);

        Move(hud, "SURVEILLANCE", 195f);
        Move(hud, "ANALYSIS", -195f);

        Move(hud, "OPERATIONS LINK", 70f);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[INTELLIGENCE] HEADER OVERLAP FIXED.");
    }

    private static void Move(
        GameObject hud,
        string name,
        float y)
    {
        GameObject obj = Find(hud, name);

        if (obj == null)
            return;

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        if (rect == null)
            return;

        Vector2 pos = rect.anchoredPosition;
        pos.y = y;
        rect.anchoredPosition = pos;
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

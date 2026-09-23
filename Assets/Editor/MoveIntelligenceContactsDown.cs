using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MoveIntelligenceContactsDown
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Move Contacts Down")]
    public static void Fix()
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

        GameObject contacts = Find(
            hud,
            "CONTACTS");

        if (contacts == null)
        {
            Debug.LogError(
                "[INTELLIGENCE] CONTACTS not found.");
            return;
        }

        RectTransform rect =
            contacts.GetComponent<RectTransform>();

        rect.anchoredPosition =
            new Vector2(195f, -45f);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[INTELLIGENCE] CONTACTS MOVED DOWN TO Y=-45.");
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

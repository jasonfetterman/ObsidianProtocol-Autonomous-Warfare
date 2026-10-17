using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AcheronRemoveDuplicateDataRows
{
    private const string ScenePath =
        "Assets/Scenes/SCN30_ACHERON/Acheron.unity";

    [MenuItem("Obsidian Protocol/Acheron/REMOVE DUPLICATE DATA ROWS")]
    public static void Remove()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath);

        GameObject duplicate =
            GameObject.Find("INTERACTIVE_PLANETARY_ROWS");

        if (duplicate != null)
        {
            Object.DestroyImmediate(duplicate);
            Debug.Log("[ACHERON DATA] Duplicate interactive rows removed.");
        }
        else
        {
            Debug.Log("[ACHERON DATA] No duplicate interactive rows found.");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }
}

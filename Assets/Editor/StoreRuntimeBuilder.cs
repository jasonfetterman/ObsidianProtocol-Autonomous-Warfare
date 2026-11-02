using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class StoreRuntimeBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    [MenuItem("Obsidian Protocol/Store/REMOVE Runtime Overlay")]
    public static void RemoveRuntimeOverlay()
    {
        EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        GameObject existing =
            GameObject.Find("STORE_RUNTIME_CONTROLLER");

        if (existing != null)
            Object.DestroyImmediate(existing);

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        AssetDatabase.SaveAssets();

        Debug.Log("[STORE] Runtime overlay removed.");
    }
}

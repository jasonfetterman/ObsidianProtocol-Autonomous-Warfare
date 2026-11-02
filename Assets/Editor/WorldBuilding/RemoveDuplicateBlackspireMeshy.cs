using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RemoveDuplicateBlackspireMeshy
{
    private const string ScenePath =
        "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";

    private const string ModelPath =
        "Assets/Art/Models/Map designs/Blackspire Expanse/Meshy_AI_Realistic_sci_fi_mili_1015104137_texture.fbx";

    private const string DuplicateName =
        "Meshy_AI_Realistic_sci_fi_mili_1015104137_texture";

    [MenuItem("Obsidian Protocol/World Building/Remove Duplicate Meshy Terrain")]
    public static void RemoveDuplicate()
    {
        Scene scene = SceneManager.GetSceneByPath(ScenePath);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);

        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogError("[BLACKSPIRE CLEANUP] Could not open target scene.");
            return;
        }

        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(ModelPath);
        if (model == null)
        {
            Debug.LogError("[BLACKSPIRE CLEANUP] Source FBX not found. Nothing removed.");
            if (openedHere) EditorSceneManager.CloseScene(scene, true);
            return;
        }

        int removed = 0;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name != DuplicateName)
                continue;

            Object source = PrefabUtility.GetCorrespondingObjectFromSource(root);
            if (source == null || AssetDatabase.GetAssetPath(source) != ModelPath)
            {
                Debug.LogWarning(
                    "[BLACKSPIRE CLEANUP] Name matched, but source verification failed. Preserved object.");
                continue;
            }

            Undo.DestroyObjectImmediate(root);
            removed++;
        }

        if (removed > 0)
        {
            EditorSceneManager.MarkSceneDirty(scene);
            if (EditorSceneManager.SaveScene(scene))
                Debug.Log("[BLACKSPIRE CLEANUP] Removed " + removed +
                    " verified duplicate Meshy instance(s). Intended terrain preserved.");
            else
                Debug.LogError("[BLACKSPIRE CLEANUP] Save failed. Check the scene in Unity.");
        }
        else
        {
            Debug.LogWarning(
                "[BLACKSPIRE CLEANUP] No matching standalone duplicate found. Nothing removed.");
        }

        if (openedHere && scene.IsValid() && scene.isLoaded)
            EditorSceneManager.CloseScene(scene, true);
    }
}

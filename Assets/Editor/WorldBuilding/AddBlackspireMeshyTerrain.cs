using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AddBlackspireMeshyTerrain
{
    private const string ScenePath =
        "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";

    private const string ModelPath =
        "Assets/Art/Models/Map designs/Blackspire Expanse/Meshy_AI_Realistic_sci_fi_mili_1015104137_texture.fbx";

    private const string RootName = "BLACKSPIRE MESHY TERRAIN";

    [MenuItem("Obsidian Protocol/World Building/Add Meshy Terrain to Blackspire")]
    public static void AddTerrain()
    {
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(ModelPath);

        if (model == null)
        {
            Debug.LogError("[BLACKSPIRE MESHY] FBX not found: " + ModelPath);
            return;
        }

        if (!AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath))
        {
            Debug.LogError("[BLACKSPIRE MESHY] Target scene not found: " + ScenePath);
            return;
        }

        Scene scene = SceneManager.GetSceneByPath(ScenePath);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);

        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogError("[BLACKSPIRE MESHY] Could not load target scene.");
            return;
        }

        foreach (GameObject existing in scene.GetRootGameObjects())
        {
            if (existing.name == RootName)
            {
                Debug.LogError(
                    "[BLACKSPIRE MESHY] Terrain root already exists. No duplicate created.");
                return;
            }
        }

        GameObject root = new GameObject(RootName);
        SceneManager.MoveGameObjectToScene(root, scene);
        root.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        root.transform.localScale = Vector3.one * 0.125f;

        GameObject instance = null;

        try
        {
            instance = (GameObject)PrefabUtility.InstantiatePrefab(model, scene);

            if (instance == null)
                throw new System.InvalidOperationException("FBX instantiation failed.");

            instance.name = "MESHY BLACKSPIRE TERRAIN MODEL";
            instance.transform.SetParent(root.transform, false);

            MeshFilter filter = instance.GetComponentInChildren<MeshFilter>(true);
            if (filter == null || filter.sharedMesh == null)
                throw new System.InvalidOperationException("No imported terrain mesh found.");

            MeshCollider collider = filter.GetComponent<MeshCollider>();
            if (collider == null)
                collider = Undo.AddComponent<MeshCollider>(filter.gameObject);

            collider.sharedMesh = filter.sharedMesh;
            collider.convex = false;

            EditorSceneManager.MarkSceneDirty(scene);

            if (!EditorSceneManager.SaveScene(scene))
                throw new System.InvalidOperationException("Scene save failed.");

            Selection.activeGameObject = root;

            Debug.Log(
                "[BLACKSPIRE MESHY] ADDED SUCCESSFULLY | Scene: " + ScenePath +
                " | Parent scale: 0.125 | FBX root scale retained | MeshCollider assigned" +
                " | Expected footprint approximately 997 x 999 m. " +
                "Existing blockout and landmarks retained.");
        }
        catch (System.Exception exception)
        {
            Debug.LogError("[BLACKSPIRE MESHY] Integration failed: " + exception);
            if (root != null)
                Object.DestroyImmediate(root);
        }
        finally
        {
            if (openedHere && scene.IsValid() && scene.isLoaded)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}

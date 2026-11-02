using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BuildBlackspireCitadel
{
    const string ScenePath = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
    const string ModelPath = "Assets/Art/Models/ClownSyndicate/Crimson_Clown_Citadel/Meshy_AI_Crimson_Clown_Citadel_20261018_texture.fbx";
    const string RootName = "CRIMSON_CLOWN_CITADEL_HEADQUARTERS";

    [MenuItem("Obsidian Protocol/World Building/Build Crimson Clown Citadel")]
    public static void Build()
    {
        Scene scene = SceneManager.GetSceneByPath(ScenePath);
        if (!scene.IsValid() || !scene.isLoaded)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name == RootName)
            {
                Debug.Log("[CITADEL BUILD] Headquarters already exists; no duplicate created.", root);
                return;
            }
        }

        GameObject terrain = null;
        GameObject marker = null;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name == "BLACKSPIRE MESHY TERRAIN") terrain = root;
            if (root.name == "Crimson_Clowns") marker = root;
        }

        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(ModelPath);
        if (terrain == null || model == null)
        {
            Debug.LogError("[CITADEL BUILD] Terrain or citadel FBX missing. No changes made.");
            return;
        }

        Vector3 target = marker != null
            ? marker.transform.position
            : new Vector3(-430f, 0f, -450f);

        Vector3 rayStart = new Vector3(target.x, 500f, target.z);
        float groundY = 0f;

        Collider terrainCollider = terrain.GetComponentInChildren<Collider>(true);
        if (terrainCollider != null &&
            terrainCollider.Raycast(new Ray(rayStart, Vector3.down), out RaycastHit hit, 1000f))
        {
            groundY = hit.point.y;
        }
        else
        {
            Debug.LogWarning("[CITADEL BUILD] Ground raycast missed; using Y=0.");
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(model, scene);
        if (instance == null)
        {
            Debug.LogError("[CITADEL BUILD] Could not instantiate citadel.");
            return;
        }

        Undo.RegisterCreatedObjectUndo(instance, "Build Crimson Clown Citadel");
        instance.name = RootName;
        instance.transform.position = new Vector3(target.x, groundY, target.z);

        // Preserve the FBX's imported scale while increasing the headquarters footprint.
        instance.transform.localScale = model.transform.localScale * 50f;

        EditorSceneManager.MarkSceneDirty(scene);
        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("[CITADEL BUILD] Scene save failed.");
            return;
        }

        Bounds bounds = new Bounds(instance.transform.position, Vector3.zero);
        bool hasBounds = false;
        foreach (Renderer r in instance.GetComponentsInChildren<Renderer>(true))
        {
            if (!hasBounds) { bounds = r.bounds; hasBounds = true; }
            else bounds.Encapsulate(r.bounds);
        }

        Debug.Log("[CITADEL BUILD] Headquarters placed at " + instance.transform.position, instance);
        Debug.Log("[CITADEL BUILD] World size: " +
            (hasBounds ? bounds.size.ToString("F1") : "No renderer bounds"));
        Debug.Log("[CITADEL BUILD] Scene saved.");
        Selection.activeGameObject = instance;
    }
}

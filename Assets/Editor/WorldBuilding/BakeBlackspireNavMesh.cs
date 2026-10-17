using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Unity.AI.Navigation;

public static class BakeBlackspireNavMesh
{
    [MenuItem("Obsidian Protocol/World Building/Bake Blackspire Walkable Map")]
    private static void Bake()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        var scene = SceneManager.GetSceneByPath(path);
        bool opened = !scene.IsValid() || !scene.isLoaded;

        if (opened) scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        try
        {
            GameObject root = null;
            foreach (var go in scene.GetRootGameObjects())
                if (go.name == "BLACKSPIRE MESHY TERRAIN") root = go;

            if (!root)
            {
                Debug.LogError("[BLACKSPIRE NAV] Meshy terrain root not found.");
                return;
            }

            var surface = root.GetComponent<NavMeshSurface>();
            if (!surface) surface = Undo.AddComponent<NavMeshSurface>(root);

            surface.collectObjects = CollectObjects.Children;
            surface.layerMask = ~0;
            surface.BuildNavMesh();

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("[BLACKSPIRE NAV] Bake complete. Test walking in Play mode.");
        }
        finally
        {
            if (opened) EditorSceneManager.CloseScene(scene, true);
        }
    }
}


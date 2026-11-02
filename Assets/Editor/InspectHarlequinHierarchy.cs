using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InspectHarlequinHierarchy
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Harlequin Hierarchy")]
    private static void Run()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        var scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;
        if (openedHere) scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        foreach (var root in scene.GetRootGameObjects())
        foreach (var t in root.GetComponentsInChildren<Transform>(true))
        {
            var smr = t.GetComponent<SkinnedMeshRenderer>();
            if (!smr || !AssetDatabase.GetAssetPath(smr.sharedMesh).Contains("autorig-7e7ecd87-v4-glb"))
                continue;

            var unitRoot = t;
            while (unitRoot.parent != null && unitRoot.parent != root.transform)
                unitRoot = unitRoot.parent;

            Debug.Log("[HARLEQUIN] Root: " + unitRoot.name +
                " | Root position: " + unitRoot.position +
                " | Renderer bounds: " + smr.bounds, unitRoot.gameObject);

            foreach (var child in unitRoot.GetComponentsInChildren<Transform>(true))
            {
                var components = child.GetComponents<Component>();
                string names = "";
                foreach (var c in components)
                    names += (c ? c.GetType().Name : "MissingScript") + ", ";
                Debug.Log("[HARLEQUIN CHILD] " + child.name +
                    " | Local: " + child.localPosition +
                    " | Components: " + names, child.gameObject);
            }
        }

        if (openedHere) EditorSceneManager.CloseScene(scene, true);
    }
}

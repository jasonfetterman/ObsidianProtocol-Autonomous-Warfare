using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BlackspireMapSizeAudit
{
    [MenuItem("Obsidian Protocol/World Building/Audit Blackspire Map Size")]
    public static void Audit()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        Scene scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        try
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                if (!root.name.Contains("BLACKSPIRE MESHY TERRAIN")) continue;

                Debug.Log("[BLACKSPIRE SIZE] Root: " + root.name +
                    " | Position: " + root.transform.position +
                    " | Rotation: " + root.transform.eulerAngles +
                    " | Scale: " + root.transform.localScale, root);

                foreach (Renderer r in root.GetComponentsInChildren<Renderer>(true))
                    Debug.Log("[BLACKSPIRE SIZE] Renderer: " + r.name +
                        " | World bounds: " + r.bounds.size.ToString("F2") +
                        " | Center: " + r.bounds.center.ToString("F2"), r);

                foreach (Collider c in root.GetComponentsInChildren<Collider>(true))
                    Debug.Log("[BLACKSPIRE SIZE] Collider: " + c.name +
                        " | Bounds: " + c.bounds.size.ToString("F2"), c);
            }
        }
        finally
        {
            if (openedHere && scene.IsValid() && scene.isLoaded)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}

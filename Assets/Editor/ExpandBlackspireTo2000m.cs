using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ExpandBlackspireTo2000m
{
    [MenuItem("Obsidian Protocol/World Building/Expand Blackspire to 2000m")]
    public static void Expand()
    {
        const string path =
            "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";

        Scene scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

        GameObject terrain = null;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name == "BLACKSPIRE MESHY TERRAIN")
            {
                terrain = root;
                break;
            }
        }

        if (terrain == null)
        {
            Debug.LogError("[BLACKSPIRE EXPAND] Terrain root not found. No changes made.");
            return;
        }

        Undo.RecordObject(terrain.transform, "Expand Blackspire to 2000m");

        Vector3 scale = terrain.transform.localScale;
        terrain.transform.localScale = new Vector3(0.25f, scale.y, 0.25f);

        EditorUtility.SetDirty(terrain.transform);

        if (EditorSceneManager.SaveScene(scene))
        {
            Bounds bounds = new Bounds();
            bool found = false;

            foreach (Renderer r in terrain.GetComponentsInChildren<Renderer>(true))
            {
                if (!found) { bounds = r.bounds; found = true; }
                else bounds.Encapsulate(r.bounds);
            }

            Debug.Log("[BLACKSPIRE EXPAND] Saved. Terrain bounds: " +
                (found ? bounds.size.ToString("F2") : "No renderer found"));
        }
        else
        {
            Debug.LogError("[BLACKSPIRE EXPAND] Scene save failed.");
        }
    }
}

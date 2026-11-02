using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SCN17ApplyTronWalls
{
    [MenuItem("Obsidian Protocol/SCN17/Apply TRON Texture to Solid Walls")]
    public static void Apply()
    {
        const string scenePath = "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";
        const string matPath = "Assets/Materials/SCN17/SCN17_TRON_Wall.mat";

        if (EditorSceneManager.GetActiveScene().path != scenePath)
        {
            EditorUtility.DisplayDialog("SCN17 TRON", "Open SCN17_PERSISTENT_MILITARY first. Nothing changed.", "OK");
            return;
        }

        Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        if (!mat)
        {
            EditorUtility.DisplayDialog("SCN17 TRON", "TRON material not found. Nothing changed.", "OK");
            return;
        }

        string[] names =
        {
            "NORTH LOWER WALL", "NORTH UPPER WALL",
            "EAST LOWER WALL", "EAST UPPER WALL",
            "WEST LOWER WALL", "WEST UPPER WALL",
            "ENTRANCE WALL LEFT", "ENTRANCE WALL RIGHT", "ENTRANCE WALL HEADER"
        };

        int changed = 0;
        foreach (string name in names)
        {
            GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject go in objects)
            {
                if (!go.scene.IsValid() || go.scene.path != scenePath || go.name != name)
                    continue;

                Renderer r = go.GetComponent<Renderer>();
                if (!r) continue;

                Undo.RecordObject(r, "Apply TRON Wall Material");
                r.sharedMaterial = mat;
                EditorUtility.SetDirty(r);
                changed++;
            }
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        EditorUtility.DisplayDialog("SCN17 TRON", "Applied TRON material to " + changed + " solid wall objects. Glass, frames, and lights were not changed.", "OK");
    }
}

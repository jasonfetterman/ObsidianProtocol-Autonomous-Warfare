using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ClearStoreUI
{
    [MenuItem("Obsidian Protocol/Command Center/CLEAR STORE UI")]
    public static void Clear()
    {
        const string path = "Assets/Scenes/SCN02_COMMAND_CENTER/COMMAND CENTER HUD/Command_Center.unity";
        var scene = SceneManager.GetActiveScene();
        if (scene.path != path) scene = EditorSceneManager.OpenScene(path);

        foreach (string name in new[] { "STORE_REAL_PANEL", "STORE_PANEL" })
        {
            var obj = System.Array.Find(Resources.FindObjectsOfTypeAll<GameObject>(), go => go.scene == scene && go.name == name);
            if (obj == null) continue;
            for (int i = obj.transform.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(obj.transform.GetChild(i).gameObject);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[STORE] Both Store panels cleared.");
    }
}
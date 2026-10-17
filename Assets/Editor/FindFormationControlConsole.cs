using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class FindFormationControlConsole
{
    [MenuItem("Obsidian Protocol/Deployment/Find Formation Control Console")]
    public static void Find()
    {
        var scene = EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity",
            OpenSceneMode.Single
        );

        Transform[] all =
            Resources.FindObjectsOfTypeAll<Transform>();

        foreach (Transform t in all)
        {
            if (t.name.ToLower().Contains("formation"))
            {
                if (t.gameObject.scene == scene)
                {
                    Debug.Log(
                        "[FORMATION FIND] " +
                        GetPath(t)
                    );
                }
            }
        }
    }

    static string GetPath(Transform t)
    {
        string path = t.name;

        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }

        return path;
    }
}

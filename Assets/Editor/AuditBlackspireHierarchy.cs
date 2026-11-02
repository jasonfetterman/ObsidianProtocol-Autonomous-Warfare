using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AuditBlackspireHierarchy
{
    [MenuItem("Obsidian Protocol/Diagnostics/Audit Blackspire Hierarchy")]
    public static void Run()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        var scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;
        if (openedHere) scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        try
        {
            foreach (var root in scene.GetRootGameObjects())
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                if (t.name != "CRIMSON_CLOWN_CITADEL_HEADQUARTERS" &&
                    t.name != "Crimson_Clowns") continue;

                Debug.Log("[BLACKSPIRE HIERARCHY] --- " + t.name + " ---");
                for (var p = t; p != null; p = p.parent)
                    Debug.Log("[BLACKSPIRE HIERARCHY] " + p.name +
                        " | LocalPos=" + p.localPosition.ToString("F2") +
                        " | WorldPos=" + p.position.ToString("F2") +
                        " | LocalScale=" + p.localScale.ToString("F3") +
                        " | WorldScale=" + p.lossyScale.ToString("F3") +
                        " | LocalRot=" + p.localEulerAngles.ToString("F1"));
            }
        }
        finally
        {
            if (openedHere) EditorSceneManager.CloseScene(scene, true);
        }
    }
}

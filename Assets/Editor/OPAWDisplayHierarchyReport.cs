using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class OPAWDisplayHierarchyReport
{
    [MenuItem("Obsidian Protocol/Garage/Report Display Hierarchy")]
    private static void Report()
    {
        var scene = EditorSceneManager.GetActiveScene();
        if (scene.name != "SCN04_GARAGE")
        {
            Debug.LogError("[DisplayReport] Open SCN04_GARAGE first. Active scene: " + scene.name);
            return;
        }

        int count = 0;
        foreach (var root in scene.GetRootGameObjects())
        {
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                if (t.name != "DISPLAY") continue;

                count++;
                string chain = t.name;
                Transform p = t.parent;
                int depth = 0;

                while (p != null && depth++ < 8)
                {
                    chain += " <- " + p.name;
                    p = p.parent;
                }

                var renderer = t.GetComponent<Renderer>();
                string materialNames = "(no Renderer)";
                if (renderer != null && renderer.sharedMaterials != null)
                {
                    materialNames = "";
                    foreach (var mat in renderer.sharedMaterials)
                        materialNames += (mat ? mat.name : "(null)") + "; ";
                }

                Debug.Log("[DisplayReport] " + chain + " | Materials: " + materialNames, t.gameObject);
            }
        }

        Debug.Log("[DisplayReport] Finished. DISPLAY objects found: " + count);
    }
}

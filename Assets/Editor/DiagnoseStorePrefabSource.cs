using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public static class DiagnoseStorePrefabSource
{
    [MenuItem("Obsidian Protocol/Store/Compare Scene Store To Prefab")]
    private static void Run()
    {
        var scene = SceneManager.GetActiveScene();
        Debug.Log("[STORE COMPARE] Scene: " + scene.path);

        foreach (var go in Object.FindObjectsByType<GameObject>(
            FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (go.scene != scene || go.name != "STORE_AAA_REBUILD")
                continue;

            var source = PrefabUtility.GetCorrespondingObjectFromSource(go);
            string sourcePath = source
                ? AssetDatabase.GetAssetPath(source)
                : "(no prefab source found)";

            int renderers = 0, slots = 0, empty = 0;
            var missingNames = new List<string>();

            foreach (var r in go.GetComponentsInChildren<Renderer>(true))
            {
                renderers++;
                foreach (var m in r.sharedMaterials)
                {
                    slots++;
                    if (!m)
                    {
                        empty++;
                        if (missingNames.Count < 20)
                            missingNames.Add(r.name);
                    }
                }
            }

            Debug.Log("[STORE COMPARE] Instance source: " + sourcePath);
            Debug.Log("[STORE COMPARE] Scene instance renderers=" + renderers +
                ", material slots=" + slots + ", empty slots=" + empty);
            Debug.Log("[STORE COMPARE] First empty-material renderers: " +
                string.Join(", ", missingNames));

            if (source)
            {
                var prefabContents = PrefabUtility.LoadPrefabContents(sourcePath);
                try
                {
                    int pr = 0, ps = 0, pe = 0;
                    foreach (var r in prefabContents.GetComponentsInChildren<Renderer>(true))
                    {
                        pr++;
                        foreach (var m in r.sharedMaterials)
                        {
                            ps++;
                            if (!m) pe++;
                        }
                    }
                    Debug.Log("[STORE COMPARE] SOURCE PREFAB renderers=" + pr +
                        ", material slots=" + ps + ", empty slots=" + pe);
                }
                finally
                {
                    PrefabUtility.UnloadPrefabContents(prefabContents);
                }
            }
        }
    }
}

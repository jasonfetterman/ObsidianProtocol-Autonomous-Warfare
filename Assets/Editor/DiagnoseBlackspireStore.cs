using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DiagnoseBlackspireStore
{
    [MenuItem("Obsidian Protocol/Store/Diagnose Blackspire Store")]
    private static void Run()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity")
        {
            Debug.LogError("Open Blackspire_Continent first.");
            return;
        }

        GameObject root = null;
        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
            if (go.scene == scene && go.name == "STORE_AAA_REBUILD")
            {
                root = go;
                break;
            }

        if (!root)
        {
            Debug.LogError("[STORE CHECK] STORE_AAA_REBUILD not found.");
            return;
        }

        int matches = 0;
        foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
        {
            string n = t.name.ToLowerInvariant();
            if (!n.Contains("console") && !n.Contains("terminal") &&
                !n.Contains("screen") && !n.Contains("display"))
                continue;

            matches++;
            var go = t.gameObject;
            var r = go.GetComponent<Renderer>();
            var c = go.GetComponent<Collider>();
            var interaction = go.GetComponent<StoreTerminalInteraction>();

            Debug.Log($"[STORE CHECK] {t.name} | active={go.activeInHierarchy} | " +
                $"renderer={(r != null)} | collider={(c != null)} | " +
                $"StoreTerminalInteraction={(interaction != null)} | path={Path(t)}");
        }

        Debug.Log($"[STORE CHECK] Finished. Matching objects: {matches}");
    }

    private static string Path(Transform t)
    {
        string p = t.name;
        while (t.parent != null) { t = t.parent; p = t.name + "/" + p; }
        return p;
    }
}

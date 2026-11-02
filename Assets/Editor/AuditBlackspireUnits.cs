using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AuditBlackspireUnits
{
    [MenuItem("Obsidian Protocol/Diagnostics/Audit Existing Blackspire Units")]
    public static void Run()
    {
        var scene = SceneManager.GetActiveScene();
        Debug.Log("[BLACKSPIRE UNITS] Scene: " + scene.path);

        foreach (var go in scene.GetRootGameObjects())
            Inspect(go);

        Debug.Log("[BLACKSPIRE UNITS] Audit complete. No scene changes made.");
    }

    static void Inspect(GameObject go)
    {
        if (go.name == "MESHY BLACKSPIRE TERRAIN MODEL" ||
            go.name == "BLACKSPIRE MESHY TERRAIN")
        {
            Debug.Log("[BLACKSPIRE EXISTING] TERRAIN " + go.name);
        }
        else if (go.name == "CRIMSON_CLOWN_CITADEL_HEADQUARTERS")
        {
            Debug.Log("[BLACKSPIRE EXISTING] CITADEL " + go.name +
                " | Position=" + go.transform.position.ToString("F1"));
        }
        else if (go.GetComponentInChildren<Renderer>(true))
        {
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            var bounds = renderers[0].bounds;
            foreach (var r in renderers)
                bounds.Encapsulate(r.bounds);

            Debug.Log("[BLACKSPIRE EXISTING] " + go.name +
                " | Position=" + go.transform.position.ToString("F1") +
                " | Bounds=" + bounds.size.ToString("F1"));
        }

        foreach (Transform child in go.transform)
            Inspect(child.gameObject);
    }
}

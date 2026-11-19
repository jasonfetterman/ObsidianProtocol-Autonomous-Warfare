using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FixBattlefieldTerrainCollider
{
    [MenuItem("OPAW/Battlefield/Fix Terrain Collision")]
    private static void Fix()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity")
        {
            Debug.LogError("Open SCN03_BATTLEFIELD_AAA first.");
            return;
        }

        var terrain = GameObject.Find("Battlefield_Terrain");
        if (terrain == null)
        {
            Debug.LogError("Battlefield_Terrain not found.");
            return;
        }

        var filter = terrain.GetComponent<MeshFilter>();
        if (filter == null || filter.sharedMesh == null)
        {
            Debug.LogError("Terrain MeshFilter has no imported mesh.");
            return;
        }

        var collider = terrain.GetComponent<MeshCollider>();
        if (collider == null) collider = Undo.AddComponent<MeshCollider>(terrain);
        collider.sharedMesh = null;
        collider.convex = false;
        collider.sharedMesh = filter.sharedMesh;
        collider.enabled = true;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Terrain collision assigned: " + filter.sharedMesh.name);
    }
}

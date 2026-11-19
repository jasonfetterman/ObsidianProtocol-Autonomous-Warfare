using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class BattlefieldTerrainColliderRepair
{
    private const string TargetScene =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";

    static BattlefieldTerrainColliderRepair()
    {
        EditorApplication.delayCall += Repair;
    }

    static void Repair()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != TargetScene)
            return;

        int added = 0;

        foreach (Terrain terrain in Object.FindObjectsByType<Terrain>(
            FindObjectsSortMode.None))
        {
            TerrainCollider collider =
                terrain.GetComponent<TerrainCollider>();

            if (collider == null)
            {
                collider = terrain.gameObject.AddComponent<TerrainCollider>();
                added++;
            }

            collider.terrainData = terrain.terrainData;
        }

        if (added > 0)
        {
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        Debug.Log("[OPAW] Terrain collider repair complete. Added: " + added);
    }
}

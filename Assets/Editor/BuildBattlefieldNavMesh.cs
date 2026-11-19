using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.IO;

public static class BuildBattlefieldNavMesh
{
    [MenuItem("OPAW/Battlefield/Build Unit Navigation Mesh")]
    public static void Build()
    {
        const string scenePath = "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";
        const string assetFolder = "Assets/Game/Battlefield/Navigation";
        const string assetPath = assetFolder + "/BattlefieldNavMesh.asset";
        if (EditorSceneManager.GetActiveScene().path != scenePath)
        {
            Debug.LogError("Open SCN03_BATTLEFIELD_AAA before building navigation.");
            return;
        }
        var terrain = GameObject.Find("Battlefield_Terrain");
        if (terrain == null)
        {
            Debug.LogError("Battlefield_Terrain was not found.");
            return;
        }
        var filter = terrain.GetComponent<MeshFilter>();
        var collider = terrain.GetComponent<MeshCollider>();
        if (filter == null || filter.sharedMesh == null || collider == null || collider.sharedMesh == null)
        {
            Debug.LogError("Terrain mesh or MeshCollider is missing. Navigation was not built.");
            return;
        }
        Directory.CreateDirectory(assetFolder);
        var surface = terrain.GetComponent<NavMeshSurface>();
        if (surface == null) surface = terrain.AddComponent<NavMeshSurface>();
        var existing = AssetDatabase.LoadAssetAtPath<NavMeshData>(assetPath);
        if (existing != null) surface.navMeshData = existing;
        surface.collectObjects = CollectObjects.All;
        surface.layerMask = 1 << terrain.layer;
        surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        surface.BuildNavMesh();
        if (surface.navMeshData == null)
        {
            Debug.LogError("NavMesh build returned no navigation data.");
            return;
        }
        if (existing == null) AssetDatabase.CreateAsset(surface.navMeshData, assetPath);
        EditorUtility.SetDirty(surface);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        AssetDatabase.SaveAssets();
        Debug.Log("Battlefield NavMesh built and saved: " + assetPath);
    }
}

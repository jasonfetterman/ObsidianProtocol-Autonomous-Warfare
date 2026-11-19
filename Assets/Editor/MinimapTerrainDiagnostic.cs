using UnityEditor;
using UnityEngine;

public static class MinimapTerrainDiagnostic
{
    [MenuItem("Obsidian Protocol/Battlefield/Diagnose Minimap Terrain")]
    public static void Run()
    {
        var cam = GameObject.Find("Minimap_Camera")?.GetComponent<Camera>();
        var terrain = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

        if (!cam || !terrain) { Debug.LogError("Camera or terrain not found."); return; }

        var mesh = terrain.GetComponent<MeshFilter>()?.sharedMesh;
        var renderer = terrain.GetComponent<MeshRenderer>();

        Debug.Log("MINIMAP CAMERA position=" + cam.transform.position +
            " rotation=" + cam.transform.eulerAngles +
            " forward=" + cam.transform.forward +
            " size=" + cam.orthographicSize +
            " cullingMask=" + cam.cullingMask);

        Debug.Log("TERRAIN layer=" + terrain.layer +
            " active=" + terrain.activeInHierarchy +
            " rendererEnabled=" + renderer.enabled +
            " mesh=" + (mesh ? mesh.name : "MISSING") +
            " localBounds=" + (mesh ? mesh.bounds.ToString() : "NONE") +
            " worldBounds=" + renderer.bounds);

        Debug.Log("TERRAIN CENTER IN CAMERA VIEW=" +
            cam.WorldToViewportPoint(renderer.bounds.center));
    }
}

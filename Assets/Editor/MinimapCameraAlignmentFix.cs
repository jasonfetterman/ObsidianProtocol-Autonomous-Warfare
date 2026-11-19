using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MinimapCameraAlignmentFix
{
    [MenuItem("Obsidian Protocol/Battlefield/FIX MINIMAP CAMERA POSITION")]
    public static void Fix()
    {
        var cameraObject = GameObject.Find("Minimap_Camera");
        var terrainObject = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

        if (!cameraObject || !terrainObject)
        {
            Debug.LogError("[MINIMAP FIX] Camera or terrain not found.");
            return;
        }

        var renderer = terrainObject.GetComponent<Renderer>();
        if (!renderer)
        {
            Debug.LogError("[MINIMAP FIX] Terrain Renderer not found.");
            return;
        }

        Undo.RecordObject(cameraObject.transform, "Align Minimap Camera");

        Vector3 position = cameraObject.transform.position;
        position.x = renderer.bounds.center.x;
        position.z = renderer.bounds.center.z;
        cameraObject.transform.position = position;

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("[MINIMAP FIX] Camera aligned to terrain center: " + position);
    }
}

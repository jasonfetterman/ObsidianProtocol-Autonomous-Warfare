using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ObsidianProtocol.Game.Intelligence;

public static class AddSensorArrayControl
{
    [MenuItem("Obsidian Protocol/Fix/Add Sensor Array Control")]
    public static void Add()
    {
        GameObject camera = GameObject.Find("INTELLIGENCE CAMERA");

        if (camera == null)
        {
            Debug.LogError("[INTELLIGENCE] INTELLIGENCE CAMERA not found.");
            return;
        }

        if (camera.GetComponent<IntelligenceSensorArrayControl>() == null)
            camera.AddComponent<IntelligenceSensorArrayControl>();

        EditorSceneManager.MarkSceneDirty(camera.scene);
        EditorSceneManager.SaveScene(camera.scene);

        Debug.Log("[INTELLIGENCE] Sensor Array Control added to INTELLIGENCE CAMERA.");
    }
}


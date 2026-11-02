using System.Collections.Generic;
using ObsidianProtocol.Game.Units;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InstallUnitMountedCameras
{
    private const string CameraObjectName =
        "UNIT CAMERA - ADJUST IN SCENE VIEW";

    [MenuItem("Obsidian Protocol/Unit Cameras/Create or Repair Cameras in Open Scene")]
    private static void Install()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("[UNIT CAMERAS] Exit Play mode first.");
            return;
        }

        MonoBehaviour[] behaviours =
            Resources.FindObjectsOfTypeAll<MonoBehaviour>();

        HashSet<Transform> processed = new HashSet<Transform>();
        HashSet<Scene> changedScenes = new HashSet<Scene>();

        int added = 0;
        int reused = 0;

        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour == null ||
                !behaviour.gameObject.scene.IsValid() ||
                !behaviour.gameObject.scene.isLoaded ||
                !(behaviour is IUnitManualControl))
                continue;

            Transform unit = behaviour.transform;

            if (!processed.Add(unit))
                continue;

            Camera unitCamera = FindUnitCamera(unit);

            if (unitCamera == null)
            {
                GameObject cameraObject = new GameObject(CameraObjectName);
                Undo.RegisterCreatedObjectUndo(
                    cameraObject, "Create Unit-Mounted Camera");

                cameraObject.transform.SetParent(unit, false);
                cameraObject.transform.localPosition =
                    new Vector3(0f, 1.25f, -2.5f);
                cameraObject.transform.localRotation =
                    Quaternion.Euler(12f, 0f, 0f);
                cameraObject.transform.localScale = Vector3.one;

                unitCamera = Undo.AddComponent<Camera>(cameraObject);
                unitCamera.fieldOfView = 70f;
                unitCamera.nearClipPlane = 0.03f;
                unitCamera.farClipPlane = 10000f;
                added++;
            }
            else
            {
                // Repair the automatically-created camera's old placement,
                // but preserve cameras the developer positioned manually.
                if (unitCamera.gameObject.name == CameraObjectName)
                {
                    Undo.RecordObject(
                        unitCamera.transform, "Position Unit Camera");

                    unitCamera.transform.localPosition =
                        new Vector3(0f, 1.25f, -2.5f);
                    unitCamera.transform.localRotation =
                        Quaternion.Euler(12f, 0f, 0f);
                }

                reused++;
            }

            Undo.RecordObject(unitCamera, "Configure Unit Camera");
            unitCamera.enabled = false;
            EditorUtility.SetDirty(unitCamera);
            EditorUtility.SetDirty(unitCamera.gameObject);
            changedScenes.Add(unit.gameObject.scene);
        }

        foreach (Scene scene in changedScenes)
            EditorSceneManager.MarkSceneDirty(scene);

        string message =
            "Cameras created: " + added +
            "\nExisting cameras reused: " + reused +
            "\n\nSave the open scene. Expand a unit in the Hierarchy and " +
            "select its Camera child to adjust its Transform.";

        Debug.Log("[UNIT CAMERAS] " + message.Replace("\n", " "));
        EditorUtility.DisplayDialog("Unit Cameras", message, "OK");
    }

    private static Camera FindUnitCamera(Transform unit)
    {
        Camera[] cameras = unit.GetComponentsInChildren<Camera>(true);

        foreach (Camera candidate in cameras)
        {
            if (candidate != null &&
                candidate.gameObject.name == CameraObjectName)
                return candidate;
        }

        foreach (Camera candidate in cameras)
        {
            if (candidate != null)
                return candidate;
        }

        return null;
    }
}

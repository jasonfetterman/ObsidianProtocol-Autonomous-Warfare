using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public static class InstallVRCommandController
{
    private const string ControllerName = "[SYSTEM] VR COMMAND CONTROLLER";

    [MenuItem("Tools/Obsidian Protocol/VR Command/Install Runtime Controller")]
    public static void Install()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid())
        {
            Debug.LogError("[VR COMMAND] No valid active scene.");
            return;
        }

        if (!scene.path.Contains("VR_Command.unity"))
        {
            Debug.LogWarning("[VR COMMAND] Active scene does not appear to be SCN-20 VR COMMAND.");
        }

        GameObject controllerObject = GameObject.Find(ControllerName);

        if (controllerObject == null)
        {
            controllerObject = new GameObject(ControllerName);
            Undo.RegisterCreatedObjectUndo(controllerObject, "Create VR Command Controller");
        }

        VRCommandController controller =
            controllerObject.GetComponent<VRCommandController>();

        if (controller == null)
            controller = Undo.AddComponent<VRCommandController>(controllerObject);

        SerializedObject so = new SerializedObject(controller);

        SetObject(so, "vrCommandHUD", Find("[HUD] VR COMMAND HUD"));

        SetButton(so, "commandButton", "[BUTTON] COMMAND");
        SetButton(so, "fleetButton", "[BUTTON] FLEET");
        SetButton(so, "intelButton", "[BUTTON] INTEL");
        SetButton(so, "deployButton", "[BUTTON] DEPLOY");
        SetButton(so, "garageButton", "[BUTTON] GARAGE");

        SetObject(so, "commandPanel", Find("[PANEL] COMMAND CENTER"));
        SetObject(so, "fleetPanel", Find("[PANEL] FLEET INSPECTION"));
        SetObject(so, "intelligencePanel", Find("[PANEL] INTELLIGENCE SYSTEMS"));
        SetObject(so, "deploymentPanel", Find("[PANEL] DEPLOYMENT SYSTEMS"));
        SetObject(so, "garagePanel", Find("[PANEL] PHYSICAL GARAGE"));

        SetObject(so, "maintenancePanel", Find("[PANEL] MAINTENANCE INTERACTION"));
        SetObject(so, "unitInspectionPanel", Find("[PANEL] UNIT INSPECTION"));
        SetObject(so, "battlefieldPanel", Find("[PANEL] FULL VR BATTLEFIELD COMMAND"));
        SetObject(so, "holographicPanel", Find("[PANEL] HOLOGRAPHIC DISPLAYS"));
        SetObject(so, "physicalGaragePanel", Find("[PANEL] PHYSICAL GARAGE"));

        so.ApplyModifiedPropertiesWithoutUndo();

        EditorUtility.SetDirty(controller);
        EditorSceneManager.MarkSceneDirty(scene);

        Debug.Log("[VR COMMAND] Controller installed and references assigned.");
    }

    private static GameObject Find(string objectName)
    {
        GameObject[] all = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in all)
        {
            if (go.scene.IsValid() && go.name == objectName)
                return go;
        }

        Debug.LogWarning("[VR COMMAND] Could not find: " + objectName);
        return null;
    }

    private static void SetObject(
        SerializedObject so,
        string propertyName,
        GameObject value)
    {
        SerializedProperty p = so.FindProperty(propertyName);

        if (p != null)
            p.objectReferenceValue = value;
    }

    private static void SetButton(
        SerializedObject so,
        string propertyName,
        string objectName)
    {
        GameObject go = Find(objectName);

        if (go == null)
            return;

        Button button = go.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning(
                "[VR COMMAND] " + objectName +
                " exists but has no Unity UI Button component.");
            return;
        }

        SerializedProperty p = so.FindProperty(propertyName);

        if (p != null)
            p.objectReferenceValue = button;
    }
}


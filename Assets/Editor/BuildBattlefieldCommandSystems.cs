using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildBattlefieldCommandSystems
{
    private const string ScenePath =
        "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

    [MenuItem("Obsidian Protocol/Battlefield/BUILD COMMAND SYSTEMS")]
    public static void Build()
    {
        EditorSceneManager.OpenScene(ScenePath);

        GameObject battlefieldCamera =
            GameObject.Find("BATTLEFIELD CAMERA");

        if (battlefieldCamera == null)
        {
            Debug.LogError(
                "[BATTLEFIELD COMMAND] BATTLEFIELD CAMERA NOT FOUND.");
            return;
        }

        Camera camera =
            battlefieldCamera.GetComponent<Camera>();

        if (camera == null)
        {
            Debug.LogError(
                "[BATTLEFIELD COMMAND] BATTLEFIELD CAMERA HAS NO CAMERA COMPONENT.");
            return;
        }

        UnitSelectionManager selectionManager =
            battlefieldCamera.GetComponent<UnitSelectionManager>();

        if (selectionManager == null)
        {
            selectionManager =
                battlefieldCamera.AddComponent<UnitSelectionManager>();
        }

        CommandIntentManager intentManager =
            battlefieldCamera.GetComponent<CommandIntentManager>();

        if (intentManager == null)
        {
            intentManager =
                battlefieldCamera.AddComponent<CommandIntentManager>();
        }

        SerializedObject selectionSerialized =
            new SerializedObject(selectionManager);

        SerializedProperty cameraProperty =
            selectionSerialized.FindProperty("battlefieldCamera");

        if (cameraProperty == null)
        {
            Debug.LogError(
                "[BATTLEFIELD COMMAND] battlefieldCamera SERIALIZED FIELD NOT FOUND.");
            return;
        }

        cameraProperty.objectReferenceValue = camera;
        selectionSerialized.ApplyModifiedPropertiesWithoutUndo();

        SerializedObject intentSerialized =
            new SerializedObject(intentManager);

        SerializedProperty selectionProperty =
            intentSerialized.FindProperty("selectionManager");

        if (selectionProperty == null)
        {
            Debug.LogError(
                "[BATTLEFIELD COMMAND] selectionManager SERIALIZED FIELD NOT FOUND.");
            return;
        }

        selectionProperty.objectReferenceValue =
            selectionManager;

        intentSerialized.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "[BATTLEFIELD COMMAND] COMMAND SYSTEMS WIRED.");

        Debug.Log(
            "[BATTLEFIELD COMMAND] SELECTION MANAGER = " +
            selectionManager.gameObject.name);

        Debug.Log(
            "[BATTLEFIELD COMMAND] BATTLEFIELD CAMERA = " +
            camera.gameObject.name);

        Debug.Log(
            "[BATTLEFIELD COMMAND] INTENT MANAGER = " +
            intentManager.gameObject.name);
    }
}

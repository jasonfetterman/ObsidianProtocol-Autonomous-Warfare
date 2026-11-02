using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ImportPhysicalGarageIntoVRCommand
{
    private const string SourcePath =
        "Assets/Scenes/SCN04_GARAGE/SCN04_GARAGE.unity";

    private const string TargetPath =
        "Assets/Scenes/SCN20_VR_COMMAND/SCN20_VR_COMMAND.unity";

    [MenuItem("Obsidian Protocol/Scene Editing/Import Physical Garage Into VR Command")]
    public static void ImportGarage()
    {
        if (EditorSceneManager.GetActiveScene().path != TargetPath)
        {
            Debug.LogError("Open SCN20_VR_COMMAND.unity first. Nothing imported.");
            return;
        }

        if (GameObject.Find("IMPORTED PHYSICAL GARAGE") != null)
        {
            Debug.LogWarning("Imported Garage already exists. No duplicate created.");
            return;
        }

        Scene source = EditorSceneManager.OpenScene(
            SourcePath, OpenSceneMode.Additive);

        string[] rootsToCopy =
        {
            "GARAGE_WORLD",
            "GARAGE_LIGHTING",
            "GARAGE_STORAGE",
            "GARAGE_TERMINALS",
            "GARAGE_UNITS"
        };

        GameObject container = new GameObject("IMPORTED PHYSICAL GARAGE");
        Undo.RegisterCreatedObjectUndo(container, "Import Physical Garage");

        int copied = 0;

        foreach (string rootName in rootsToCopy)
        {
            GameObject[] roots = source.GetRootGameObjects();

            foreach (GameObject root in roots)
            {
                if (root.name != rootName)
                    continue;

                GameObject copy = Object.Instantiate(root);
                copy.name = root.name;
                SceneManager.MoveGameObjectToScene(copy,
                    EditorSceneManager.GetActiveScene());
                Undo.RegisterCreatedObjectUndo(copy, "Import " + rootName);
                copy.transform.SetParent(container.transform, true);
                copied++;
            }
        }

        EditorSceneManager.CloseScene(source, true);

        if (copied == 0)
        {
            Object.DestroyImmediate(container);
            Debug.LogError("No Garage roots copied. Check source scene hierarchy.");
            return;
        }

        Selection.activeGameObject = container;
        SceneView.lastActiveSceneView?.FrameSelected();
        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        Debug.Log("Imported " + copied +
            " physical Garage roots. HUD was not copied. Review placement before saving.");
    }
}

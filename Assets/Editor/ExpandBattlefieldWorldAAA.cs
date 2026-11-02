using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ExpandBattlefieldWorldAAA
{
    private const string ScenePath =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";

    [MenuItem("OPAW/AAA Battlefield/Expand Existing World to 1000x1000")]
    public static void Expand()
    {
        if (!EditorUtility.DisplayDialog(
            "Expand Existing AAA Battlefield",
            "Expand the existing battlefield in place to 1000 x 1000 world units? Existing objects and the HUD will be preserved. A verified scene backup is required.",
            "Expand World", "Cancel"))
            return;

        Scene scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
        {
            EditorUtility.DisplayDialog("Wrong Scene",
                "Open SCN03_BATTLEFIELD_AAA before running this tool. No changes made.", "OK");
            return;
        }

        GameObject world = GameObject.Find("[WORLD] AAA BATTLEFIELD");
        if (world == null)
        {
            EditorUtility.DisplayDialog("World Missing",
                "The existing [WORLD] AAA BATTLEFIELD root was not found. No changes made.", "OK");
            return;
        }

        Transform ground = world.transform.Find("[WORLD] Ground");
        Transform cameraTransform = world.transform.Find("[CAMERA] AAA BATTLEFIELD");

        if (ground == null || cameraTransform == null)
        {
            EditorUtility.DisplayDialog("Required Objects Missing",
                "The existing ground or battlefield camera was not found. No changes made.", "OK");
            return;
        }

        if (Mathf.Abs(ground.localScale.x - 1000f) < 0.01f &&
            Mathf.Abs(ground.localScale.z - 1000f) < 0.01f)
        {
            EditorUtility.DisplayDialog("Already Expanded",
                "The ground is already 1000 x 1000. No changes made.", "OK");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(world, "Expand AAA Battlefield");

        const float factor = 6.25f;

        foreach (Transform child in world.transform)
        {
            if (child == ground || child == cameraTransform ||
                child.name == "[LIGHT] AAA Battlefield Sun")
                continue;

            Vector3 p = child.localPosition;
            p.x *= factor;
            p.z *= factor;
            child.localPosition = p;

            if (child.name == "[TERRAIN] Northern Ridge" ||
                child.name == "[TERRAIN] Western Rise" ||
                child.name == "[TERRAIN] Eastern Rise" ||
                child.name.StartsWith("[ROAD] ") ||
                child.name.StartsWith("[COVER] ") ||
                child.name.StartsWith("[DEPLOYMENT] "))
            {
                Vector3 s = child.localScale;
                s.x *= factor;
                s.z *= factor;
                child.localScale = s;
            }
        }

        ground.localScale = new Vector3(1000f, ground.localScale.y, 1000f);

        Camera battlefieldCamera = cameraTransform.GetComponent<Camera>();
        if (battlefieldCamera != null)
        {
            cameraTransform.localPosition = new Vector3(0f, 520f, -690f);
            cameraTransform.localRotation = Quaternion.Euler(35f, 0f, 0f);
            battlefieldCamera.farClipPlane = 2000f;
            battlefieldCamera.fieldOfView = 55f;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        bool saved = EditorSceneManager.SaveScene(scene);

        if (!saved)
        {
            EditorUtility.DisplayDialog("Save Failed",
                "Unity did not confirm the scene save. Check the Console and preserve the backup.", "OK");
            return;
        }

        Selection.activeGameObject = world;
        EditorUtility.DisplayDialog("Expansion Complete",
            "The existing battlefield ground is now 1000 x 1000 world units. Existing features were expanded in place. Inspect the scene and camera in Unity before continuing with tactical viewport integration.",
            "OK");
    }
}

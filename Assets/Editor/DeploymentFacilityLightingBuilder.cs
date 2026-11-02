using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DeploymentFacilityLightingBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    private const string RootName = "[FACILITY] GENERATED LIGHTING";

    [MenuItem("Obsidian Protocol/Deployment/Build Industrial Lighting")]
    private static void BuildLighting()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog(
                "Facility Lighting",
                "Exit Play Mode before building lighting.",
                "OK");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            EditorUtility.DisplayDialog(
                "Wrong Scene",
                "Open SCN12_DEPLOYMENT before running this builder.",
                "OK");
            return;
        }

        if (FindSceneObject(scene, RootName) != null)
        {
            EditorUtility.DisplayDialog(
                "Already Built",
                "Generated facility lighting already exists. No changes made.",
                "OK");
            return;
        }

        GameObject building = FindSceneObject(scene, "BUILDING");

        if (building == null)
        {
            EditorUtility.DisplayDialog(
                "Missing BUILDING",
                "Could not find BUILDING. No changes made.",
                "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Build Industrial Lighting",
            "Add warm overhead lights and cool perimeter lights inside the deployment facility?",
            "Build",
            "Cancel"))
            return;

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Facility Lighting");
        Undo.SetTransformParent(
            root.transform, building.transform, "Parent Facility Lighting");

        // Warm overhead lighting down the center of the facility.
        for (int i = -1; i <= 1; i++)
        {
            CreateLight(
                root.transform,
                "WARM CEILING LIGHT " + (i + 2),
                new Vector3(0f, 12.5f, 5f + i * 18f),
                new Color(1f, 0.76f, 0.48f),
                900f,
                18f,
                1.2f);
        }

        // Cool industrial lights along the left and right sides.
        for (int i = -1; i <= 1; i++)
        {
            float z = 5f + i * 18f;

            CreateLight(
                root.transform,
                "LEFT PERIMETER LIGHT " + (i + 2),
                new Vector3(-30f, 10f, z),
                new Color(0.42f, 0.72f, 1f),
                550f,
                12f,
                0.8f);

            CreateLight(
                root.transform,
                "RIGHT PERIMETER LIGHT " + (i + 2),
                new Vector3(30f, 10f, z),
                new Color(0.42f, 0.72f, 1f),
                550f,
                12f,
                0.8f);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        Selection.activeGameObject = root;

        EditorUtility.DisplayDialog(
            "Lighting Created",
            "Nine industrial point lights have been added. Inspect the facility before saving.",
            "OK");
    }

    private static void CreateLight(
        Transform parent,
        string objectName,
        Vector3 localPosition,
        Color color,
        float intensity,
        float range,
        float sourceSize)
    {
        GameObject lightObject = new GameObject(objectName);
        Undo.RegisterCreatedObjectUndo(
            lightObject, "Create " + objectName);

        lightObject.transform.SetParent(parent, false);
        lightObject.transform.localPosition = localPosition;
        lightObject.transform.localRotation = Quaternion.identity;

        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.None;
        light.renderMode = LightRenderMode.Auto;
        light.bounceIntensity = 0f;
        light.areaSize = new Vector2(sourceSize, sourceSize);
    }

    private static GameObject FindSceneObject(
        Scene scene, string objectName)
    {
        foreach (GameObject sceneRoot in scene.GetRootGameObjects())
        {
            foreach (Transform child in
                sceneRoot.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == objectName)
                    return child.gameObject;
            }
        }

        return null;
    }
}
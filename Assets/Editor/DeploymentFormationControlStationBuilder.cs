using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DeploymentFormationControlStationBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    private const string StationName =
        "[STATION] FORMATION CONTROL";

    [MenuItem("Obsidian Protocol/Deployment/BUILD FORMATION CONTROL STATION")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        GameObject station = GameObject.Find(StationName);

        if (station == null)
        {
            Debug.LogError(
                "FORMATION CONTROL: [STATION] FORMATION CONTROL not found.");
            return;
        }

        RemoveChild(station.transform, "[STRUCTURE] CONSOLE");
        RemoveChild(station.transform, "[DISPLAY] HOLOGRAPHIC SCREEN");
        RemoveChild(station.transform, "[LIGHT] STATUS STRIP");

        Material body = MakeMaterial(
            "FORMATION_CONTROL_BODY",
            new Color(0.025f, 0.045f, 0.055f),
            0f);

        Material trim = MakeMaterial(
            "FORMATION_CONTROL_TRIM",
            new Color(0.02f, 0.25f, 0.32f),
            1.5f);

        Material holo = MakeMaterial(
            "FORMATION_CONTROL_HOLOGRAM",
            new Color(0.02f, 0.55f, 0.9f),
            2.5f);

        // ============================================================
        // EXACT COMMAND STRUCTURE FOOTPRINT
        // ============================================================

        GameObject console = CreateCube(
            "[STRUCTURE] CONSOLE",
            station.transform,
            new Vector3(-9f, 1.2f, 7f),
            new Vector3(14f, 2.4f, 6f),
            body);

        GameObject display = CreateCube(
            "[DISPLAY] HOLOGRAPHIC SCREEN",
            station.transform,
            new Vector3(-9f, 3.1f, 6.1f),
            new Vector3(13f, 2.7f, 0.25f),
            trim);

        GameObject status = CreateCube(
            "[LIGHT] STATUS STRIP",
            station.transform,
            new Vector3(-9f, 0.15f, 7f),
            new Vector3(13f, 0.08f, 5.5f),
            body);

        // ============================================================
        // HOLOGRAPHIC SCREEN
        // CHILD OF THE DISPLAY HOUSING.
        // THEREFORE IT CANNOT FLOAT AWAY FROM THE STATION.
        // ============================================================

        GameObject screenSurface = CreateCube(
            "[HOLO] SCREEN SURFACE",
            display.transform,
            new Vector3(0f, -0.15f, -0.55f),
            new Vector3(11f, 2.0f, 0.025f),
            holo);

        CreateCube(
            "[HOLO] TOP FRAME",
            display.transform,
            new Vector3(0f, 0.95f, -0.57f),
            new Vector3(11.3f, 0.05f, 0.04f),
            trim);

        CreateCube(
            "[HOLO] BOTTOM FRAME",
            display.transform,
            new Vector3(0f, -1.15f, -0.57f),
            new Vector3(11.3f, 0.05f, 0.04f),
            trim);

        CreateCube(
            "[HOLO] LEFT FRAME",
            display.transform,
            new Vector3(-5.55f, -0.1f, -0.57f),
            new Vector3(0.05f, 2.1f, 0.04f),
            trim);

        CreateCube(
            "[HOLO] RIGHT FRAME",
            display.transform,
            new Vector3(5.55f, -0.1f, -0.57f),
            new Vector3(0.05f, 2.1f, 0.04f),
            trim);

        // Formation command nodes.
        CreateCube(
            "[HOLO] COMMAND CORE",
            screenSurface.transform,
            new Vector3(0f, 0.2f, -0.03f),
            new Vector3(0.35f, 0.06f, 0.35f),
            holo);

        CreateCube(
            "[HOLO] UNIT LEFT",
            screenSurface.transform,
            new Vector3(-1.8f, -0.25f, -0.03f),
            new Vector3(0.25f, 0.06f, 0.25f),
            holo);

        CreateCube(
            "[HOLO] UNIT RIGHT",
            screenSurface.transform,
            new Vector3(1.8f, -0.25f, -0.03f),
            new Vector3(0.25f, 0.06f, 0.25f),
            holo);

        CreateCube(
            "[HOLO] UNIT LOWER LEFT",
            screenSurface.transform,
            new Vector3(-2.8f, -0.75f, -0.03f),
            new Vector3(0.2f, 0.06f, 0.2f),
            holo);

        CreateCube(
            "[HOLO] UNIT LOWER RIGHT",
            screenSurface.transform,
            new Vector3(2.8f, -0.75f, -0.03f),
            new Vector3(0.2f, 0.06f, 0.2f),
            holo);

        // ============================================================
        // CONSOLE DETAIL
        // CHILD OF THE ACTUAL CONSOLE.
        // ============================================================

        CreateCube(
            "[CONSOLE] FRONT PANEL",
            console.transform,
            new Vector3(0f, 0f, -3.02f),
            new Vector3(11f, 1.4f, 0.08f),
            trim);

        CreateCube(
            "[CONSOLE] CONTROL SURFACE",
            console.transform,
            new Vector3(0f, 1.05f, -2.5f),
            new Vector3(9f, 0.08f, 1.3f),
            body);

        // ============================================================
        // STATUS STRIP
        // CHILDREN ARE ATTACHED TO THE EXACT STATUS OBJECT.
        // ============================================================

        CreateCube(
            "[STATUS] CENTER LIGHT",
            status.transform,
            new Vector3(0f, 0.08f, -0.15f),
            new Vector3(2.5f, 0.04f, 0.08f),
            holo);

        CreateCube(
            "[STATUS] LEFT LIGHT",
            status.transform,
            new Vector3(-4f, 0.08f, -0.15f),
            new Vector3(2f, 0.04f, 0.08f),
            holo);

        CreateCube(
            "[STATUS] RIGHT LIGHT",
            status.transform,
            new Vector3(4f, 0.08f, -0.15f),
            new Vector3(2f, 0.04f, 0.08f),
            holo);

        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "FORMATION CONTROL: rebuilt using exact Command Structure footprint.");
    }

    private static void RemoveChild(
        Transform parent,
        string childName)
    {
        Transform child = parent.Find(childName);

        if (child != null)
            Object.DestroyImmediate(child.gameObject);
    }

    private static GameObject CreateCube(
        string name,
        Transform parent,
        Vector3 localPosition,
        Vector3 localScale,
        Material material)
    {
        GameObject go =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube);

        go.name = name;

        go.transform.SetParent(
            parent,
            false);

        go.transform.localPosition =
            localPosition;

        go.transform.localRotation =
            Quaternion.identity;

        go.transform.localScale =
            localScale;

        Renderer renderer =
            go.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial = material;

        return go;
    }

    private static Material MakeMaterial(
        string name,
        Color color,
        float emission)
    {
        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        Material material =
            new Material(shader);

        material.name = name;

        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);

        if (material.HasProperty("_Color"))
            material.SetColor("_Color", color);

        if (emission > 0f &&
            material.HasProperty("_EmissionColor"))
        {
            material.EnableKeyword("_EMISSION");

            material.SetColor(
                "_EmissionColor",
                color * emission);
        }

        return material;
    }
}

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class FormationTwoSectionHolographicDisplayBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/BUILD ONE TWO-SECTION HOLOGRAPHIC DISPLAY")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        GameObject station =
            GameObject.Find("[STATION] FORMATION CONTROL");

        if (station == null)
        {
            Debug.LogError(
                "FORMATION DISPLAY: [STATION] FORMATION CONTROL not found.");
            return;
        }

        Transform old =
            station.transform.Find("[DISPLAY] HOLOGRAPHIC SCREEN");

        if (old != null)
            Object.DestroyImmediate(old.gameObject);

        Material holo = MakeMaterial(
            "FORMATION_HOLOGRAM",
            new Color(0.02f, 0.55f, 0.9f),
            2.5f);

        Material border = MakeMaterial(
            "FORMATION_HOLOGRAM_BORDER",
            new Color(0.02f, 0.25f, 0.4f),
            2.0f);

        GameObject display =
            new GameObject("[DISPLAY] HOLOGRAPHIC SCREEN");

        display.transform.SetParent(
            station.transform,
            false);

        display.transform.localPosition =
            Vector3.zero;

        display.transform.localRotation =
            Quaternion.identity;

        display.transform.localScale =
            Vector3.one;

        // ============================================================
        // LEFT OPERATOR DISPLAY
        // 5.8 x 2.4
        // ============================================================

        CreateCube(
            "[SCREEN] LEFT OPERATOR",
            display.transform,
            new Vector3(-3.1f, 0f, 0f),
            new Vector3(5.8f, 2.4f, 0.02f),
            holo);

        // ============================================================
        // RIGHT OPERATOR DISPLAY
        // 5.8 x 2.4
        // ============================================================

        CreateCube(
            "[SCREEN] RIGHT OPERATOR",
            display.transform,
            new Vector3(3.1f, 0f, 0f),
            new Vector3(5.8f, 2.4f, 0.02f),
            holo);

        // ============================================================
        // OUTER + CENTER BORDERS
        // ============================================================

        CreateCube(
            "[BORDER] TOP",
            display.transform,
            new Vector3(0f, 1.22f, -0.02f),
            new Vector3(12.4f, 0.06f, 0.06f),
            border);

        CreateCube(
            "[BORDER] BOTTOM",
            display.transform,
            new Vector3(0f, -1.22f, -0.02f),
            new Vector3(12.4f, 0.06f, 0.06f),
            border);

        CreateCube(
            "[BORDER] LEFT",
            display.transform,
            new Vector3(-6.2f, 0f, -0.02f),
            new Vector3(0.06f, 2.5f, 0.06f),
            border);

        CreateCube(
            "[BORDER] CENTER DIVIDER",
            display.transform,
            new Vector3(0f, 0f, -0.04f),
            new Vector3(0.40f, 2.50f, 0.06f),
            border);

        CreateCube(
            "[BORDER] RIGHT",
            display.transform,
            new Vector3(6.2f, 0f, -0.02f),
            new Vector3(0.06f, 2.5f, 0.06f),
            border);

        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "FORMATION DISPLAY: one two-section holographic display built.");
    }

    private static GameObject CreateCube(
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale,
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
            position;

        go.transform.localRotation =
            Quaternion.identity;

        go.transform.localScale =
            scale;

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

        if (material.HasProperty("_EmissionColor"))
        {
            material.EnableKeyword("_EMISSION");

            material.SetColor(
                "_EmissionColor",
                color * emission);
        }

        return material;
    }
}

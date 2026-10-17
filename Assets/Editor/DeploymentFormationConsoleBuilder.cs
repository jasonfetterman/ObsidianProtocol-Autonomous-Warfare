using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DeploymentFormationConsoleBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/BUILD FORMATION CONSOLE SAME SIZE AS COMMAND")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        GameObject formation =
            GameObject.Find("[STATION] FORMATION CONTROL");

        GameObject command =
            GameObject.Find("[STATION] COMMAND STRUCTURE");

        if (formation == null)
        {
            Debug.LogError(
                "FORMATION CONSOLE: [STATION] FORMATION CONTROL not found.");
            return;
        }

        if (command == null)
        {
            Debug.LogError(
                "FORMATION CONSOLE: [STATION] COMMAND STRUCTURE not found.");
            return;
        }

        Transform commandConsole =
            command.transform.Find("[STRUCTURE] CONSOLE");

        if (commandConsole == null)
        {
            Debug.LogError(
                "FORMATION CONSOLE: Command Structure console not found.");
            return;
        }

        Transform old =
            formation.transform.Find("[STRUCTURE] CONSOLE");

        if (old != null)
            Object.DestroyImmediate(old.gameObject);

        GameObject console =
            Object.Instantiate(commandConsole.gameObject);

        console.name =
            "[STRUCTURE] CONSOLE";

        console.transform.SetParent(
            formation.transform,
            false);

        // EXACT SAME LOCAL TRANSFORM AS COMMAND STRUCTURE.
        console.transform.localPosition =
            commandConsole.localPosition;

        console.transform.localRotation =
            commandConsole.localRotation;

        console.transform.localScale =
            commandConsole.localScale;

        // Remove Command-specific detail children if any.
        for (int i = console.transform.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(
                console.transform.GetChild(i).gameObject);
        }

        // Futuristic surface detail, kept INSIDE the exact console footprint.
        Material accent = MakeMaterial(
            "FORMATION_CONSOLE_ACCENT",
            new Color(0.02f, 0.45f, 0.65f),
            1.5f);

        CreateDetail(
            console.transform,
            "[DETAIL] FRONT LIGHT",
            new Vector3(0f, -0.15f, -0.501f),
            new Vector3(8f, 0.035f, 0.02f),
            accent);

        CreateDetail(
            console.transform,
            "[DETAIL] CENTRAL LIGHT",
            new Vector3(0f, 0.35f, -0.502f),
            new Vector3(2.5f, 0.025f, 0.02f),
            accent);

        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "FORMATION CONSOLE: exact Command Structure console size copied.");
    }

    private static void CreateDetail(
        Transform parent,
        string name,
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

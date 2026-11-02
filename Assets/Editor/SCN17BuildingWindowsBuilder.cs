using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class SCN17BuildingWindowsBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";
    private const string RootName = "SCN17 BUILDING WINDOWS UPGRADE";
    private const float WallBottom = -0.5f;
    private const float WallTop = 16.5f;
    private const float WindowBottom = 9.5f;
    private const float WindowTop = 12.5f;
    private const float WindowWidth = 8f;
    private const float Frame = 0.22f;

    [MenuItem("Obsidian Protocol/SCN17/Build Building Windows")]
    public static void Build()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
        {
            EditorUtility.DisplayDialog("SCN17 Windows", "Open SCN17_PERSISTENT_MILITARY first. No changes were made.", "OK");
            return;
        }

        GameObject architecture = GameObject.Find("FACILITY ARCHITECTURE");
        if (!architecture)
        {
            EditorUtility.DisplayDialog("SCN17 Windows", "FACILITY ARCHITECTURE was not found. No changes were made.", "OK");
            return;
        }

        GameObject north = FindChild(architecture.transform, "NORTH WALL");
        GameObject east = FindChild(architecture.transform, "EAST WALL");
        GameObject west = FindChild(architecture.transform, "WEST WALL");

        if (!north || !east || !west)
        {
            EditorUtility.DisplayDialog("SCN17 Windows", "One or more original walls were not found. No changes were made.", "OK");
            return;
        }

        GameObject oldRoot = FindChild(architecture.transform, RootName);
        if (oldRoot) Undo.DestroyObjectImmediate(oldRoot);

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build SCN17 Building Windows");
        root.transform.SetParent(architecture.transform, false);

        Material wallMaterial = GetWallMaterial(north, east, west);
        Material glassMaterial = CreateGlassMaterial();

        BuildNorth(root.transform, north, wallMaterial, glassMaterial);
        BuildSide(root.transform, east, wallMaterial, glassMaterial, true);
        BuildSide(root.transform, west, wallMaterial, glassMaterial, false);

        north.SetActive(false);
        east.SetActive(false);
        west.SetActive(false);

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        Selection.activeGameObject = root;

        EditorUtility.DisplayDialog("SCN17 Windows",
            "Built actual window openings with framed tinted glass on the north, east, and west walls. Original walls are preserved but inactive. South entrance and HUD were not changed.",
            "OK");
    }

    private static GameObject FindChild(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            if (child.name == name) return child.gameObject;
        return null;
    }

    private static Material GetWallMaterial(params GameObject[] walls)
    {
        foreach (GameObject wall in walls)
        {
            Renderer r = wall.GetComponent<Renderer>();
            if (r && r.sharedMaterial) return r.sharedMaterial;
        }
        return null;
    }

    private static Material CreateGlassMaterial()
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (!shader) shader = Shader.Find("Standard");
        Material m = new Material(shader);
        m.name = "SCN17 Tinted Window Glass";
        Color tint = new Color(0.12f, 0.24f, 0.28f, 0.22f);

        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", tint);
        if (m.HasProperty("_Color")) m.SetColor("_Color", tint);
        if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", 0.05f);
        if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", 0.35f);
        if (m.HasProperty("_Glossiness")) m.SetFloat("_Glossiness", 0.35f);
        if (m.HasProperty("_EmissionColor"))
        {
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", new Color(0.01f, 0.18f, 0.25f, 1f));
        }
        return m;
    }

    private static void BuildNorth(Transform root, GameObject original, Material wallMat, Material glassMat)
    {
        float[] centers = { -27f, -9f, 9f, 27f };
        float[] cuts = new float[centers.Length * 2 + 2];
        cuts[0] = -41f;
        for (int i = 0; i < centers.Length; i++)
        {
            cuts[i * 2 + 1] = centers[i] - WindowWidth / 2f;
            cuts[i * 2 + 2] = centers[i] + WindowWidth / 2f;
        }
        cuts[cuts.Length - 1] = 41f;

        Vector3 wallPos = original.transform.localPosition;
        Vector3 wallScale = original.transform.localScale;
        float z = wallPos.z;

        CreateWallBox(root, "NORTH LOWER WALL", new Vector3(0, (WallBottom + WindowBottom) / 2f, z),
            new Vector3(82, WindowBottom - WallBottom, 1), wallMat);
        CreateWallBox(root, "NORTH UPPER WALL", new Vector3(0, (WindowTop + WallTop) / 2f, z),
            new Vector3(82, WallTop - WindowTop, 1), wallMat);

        for (int i = 0; i <= centers.Length; i++)
        {
            float left = (i == 0) ? -41f : centers[i - 1] + WindowWidth / 2f;
            float right = (i == centers.Length) ? 41f : centers[i] - WindowWidth / 2f;
            if (right > left)
                CreateWallBox(root, "NORTH PIER " + (i + 1),
                    new Vector3((left + right) / 2f, (WindowBottom + WindowTop) / 2f, z),
                    new Vector3(right - left, WindowTop - WindowBottom, 1), wallMat);
        }

        foreach (float x in centers)
            CreateWindow(root, "NORTH WINDOW", x, (WindowBottom + WindowTop) / 2f, z + 0.04f,
                WindowWidth, WindowTop - WindowBottom, false, glassMat);
    }

    private static void BuildSide(Transform root, GameObject original, Material wallMat, Material glassMat, bool isEast)
    {
        float[] centers = { -13f, 6f, 25f };
        float wallX = original.transform.localPosition.x;
        float wallZ = original.transform.localPosition.z;
        float faceX = wallX + (isEast ? -0.04f : 0.04f);

        CreateWallBox(root, (isEast ? "EAST" : "WEST") + " LOWER WALL",
            new Vector3(wallX, (WallBottom + WindowBottom) / 2f, wallZ),
            new Vector3(1, WindowBottom - WallBottom, 64), wallMat);
        CreateWallBox(root, (isEast ? "EAST" : "WEST") + " UPPER WALL",
            new Vector3(wallX, (WindowTop + WallTop) / 2f, wallZ),
            new Vector3(1, WallTop - WindowTop, 64), wallMat);

        for (int i = 0; i <= centers.Length; i++)
        {
            float left = (i == 0) ? -26f : centers[i - 1] + WindowWidth / 2f;
            float right = (i == centers.Length) ? 38f : centers[i] - WindowWidth / 2f;
            if (right > left)
                CreateWallBox(root, (isEast ? "EAST" : "WEST") + " PIER " + (i + 1),
                    new Vector3(wallX, (WindowBottom + WindowTop) / 2f, (left + right) / 2f),
                    new Vector3(1, WindowTop - WindowBottom, right - left), wallMat);
        }

        foreach (float z in centers)
            CreateWindow(root, (isEast ? "EAST" : "WEST") + " WINDOW",
                faceX, (WindowBottom + WindowTop) / 2f, z,
                WindowWidth, WindowTop - WindowBottom, true, glassMat);
    }

    private static void CreateWallBox(Transform root, string name, Vector3 position, Vector3 scale, Material material)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(root, false);
        go.transform.localPosition = position;
        go.transform.localScale = scale;
        if (material) go.GetComponent<Renderer>().sharedMaterial = material;
    }

    private static void CreateWindow(Transform root, string prefix, float x, float y, float z,
        float width, float height, bool sideWall, Material glassMat)
    {
        Vector3 glassScale = sideWall
            ? new Vector3(0.10f, height - Frame * 2f, width - Frame * 2f)
            : new Vector3(width - Frame * 2f, height - Frame * 2f, 0.10f);

        GameObject glass = GameObject.CreatePrimitive(PrimitiveType.Cube);
        glass.name = prefix + " TINTED GLASS";
        glass.transform.SetParent(root, false);
        glass.transform.localPosition = new Vector3(x, y, z);
        glass.transform.localScale = glassScale;
        Object.DestroyImmediate(glass.GetComponent<Collider>());
        glass.GetComponent<Renderer>().sharedMaterial = glassMat;

        Vector3 barScale;
        Vector3 barPos;

        if (!sideWall)
        {
            barScale = new Vector3(width, Frame, 0.18f);
            barPos = new Vector3(x, y - height / 2f + Frame / 2f, z);
            CreateFrameBar(root, prefix + " SILL", barPos, barScale);
            barPos.y = y + height / 2f - Frame / 2f;
            CreateFrameBar(root, prefix + " HEADER", barPos, barScale);

            barScale = new Vector3(Frame, height, 0.18f);
            barPos = new Vector3(x - width / 2f + Frame / 2f, y, z);
            CreateFrameBar(root, prefix + " LEFT JAMB", barPos, barScale);
            barPos.x = x + width / 2f - Frame / 2f;
            CreateFrameBar(root, prefix + " RIGHT JAMB", barPos, barScale);
        }
        else
        {
            barScale = new Vector3(0.18f, Frame, width);
            barPos = new Vector3(x, y - height / 2f + Frame / 2f, z);
            CreateFrameBar(root, prefix + " SILL", barPos, barScale);
            barPos.y = y + height / 2f - Frame / 2f;
            CreateFrameBar(root, prefix + " HEADER", barPos, barScale);

            barScale = new Vector3(0.18f, height, Frame);
            barPos = new Vector3(x, y, z - width / 2f + Frame / 2f);
            CreateFrameBar(root, prefix + " LEFT JAMB", barPos, barScale);
            barPos.z = z + width / 2f - Frame / 2f;
            CreateFrameBar(root, prefix + " RIGHT JAMB", barPos, barScale);
        }
    }

    private static void CreateFrameBar(Transform root, string name, Vector3 position, Vector3 scale)
    {
        GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bar.name = name;
        bar.transform.SetParent(root, false);
        bar.transform.localPosition = position;
        bar.transform.localScale = scale;
        bar.GetComponent<Renderer>().sharedMaterial = CreateFrameMaterial();
    }

    private static Material cachedFrameMaterial;

    private static Material CreateFrameMaterial()
    {
        if (cachedFrameMaterial) return cachedFrameMaterial;
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (!shader) shader = Shader.Find("Standard");
        cachedFrameMaterial = new Material(shader);
        cachedFrameMaterial.name = "SCN17 Window Frame Metal";
        Color c = new Color(0.035f, 0.12f, 0.16f, 1f);
        if (cachedFrameMaterial.HasProperty("_BaseColor")) cachedFrameMaterial.SetColor("_BaseColor", c);
        if (cachedFrameMaterial.HasProperty("_Color")) cachedFrameMaterial.SetColor("_Color", c);
        if (cachedFrameMaterial.HasProperty("_Metallic")) cachedFrameMaterial.SetFloat("_Metallic", 0.8f);
        if (cachedFrameMaterial.HasProperty("_Smoothness")) cachedFrameMaterial.SetFloat("_Smoothness", 0.72f);
        return cachedFrameMaterial;
    }
}


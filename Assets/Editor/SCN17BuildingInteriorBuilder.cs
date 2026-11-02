using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SCN17BuildingInteriorBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";
    private const string RootName = "SCN17 INTERIOR DETAIL UPGRADE";

    [MenuItem("Obsidian Protocol/SCN17/Build Interior Details")]
    public static void Build()
    {
        if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path != ScenePath)
        {
            EditorUtility.DisplayDialog("SCN17 Interior", "Open SCN17_PERSISTENT_MILITARY first. No changes made.", "OK");
            return;
        }

        GameObject architecture = GameObject.Find("FACILITY ARCHITECTURE");
        if (!architecture)
        {
            EditorUtility.DisplayDialog("SCN17 Interior", "FACILITY ARCHITECTURE not found. No changes made.", "OK");
            return;
        }

        Transform old = architecture.transform.Find(RootName);
        if (old) Undo.DestroyObjectImmediate(old.gameObject);

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build SCN17 Interior Details");
        root.transform.SetParent(architecture.transform, false);

        Material metal = MakeMaterial("SCN17 Dark Brushed Metal", new Color(0.055f, 0.075f, 0.09f), 0.82f, 0.68f, false);
        Material cyan = MakeMaterial("SCN17 Cyan Accent", new Color(0.015f, 0.55f, 0.72f), 0.35f, 0.8f, true);
        Material panel = MakeMaterial("SCN17 Floor Panel", new Color(0.09f, 0.12f, 0.14f), 0.65f, 0.55f, false);

        // Inset floor panels: decorative only, with no colliders.
        int count = 0;
        for (int x = -36; x <= 36; x += 6)
        {
            for (int z = -20; z <= 32; z += 6)
            {
                CreateBox(root.transform, "FLOOR PANEL " + (++count),
                    new Vector3(x, 0.012f, z), new Vector3(5.65f, 0.025f, 5.65f), panel, false);
            }
        }

        // Perimeter accent strips sit just above the floor surface.
        CreateBox(root.transform, "NORTH FLOOR EDGE LIGHT",
            new Vector3(0, 0.035f, 36.8f), new Vector3(78f, 0.035f, 0.12f), cyan, false);
        CreateBox(root.transform, "EAST FLOOR EDGE LIGHT",
            new Vector3(39.8f, 0.035f, 6f), new Vector3(0.12f, 0.035f, 60f), cyan, false);
        CreateBox(root.transform, "WEST FLOOR EDGE LIGHT",
            new Vector3(-39.8f, 0.035f, 6f), new Vector3(0.12f, 0.035f, 60f), cyan, false);

        // Metal collars and narrow cyan bands around existing structural columns.
        Transform[] all = architecture.GetComponentsInChildren<Transform>(true);
        int columnIndex = 0;
        foreach (Transform item in all)
        {
            if (item.name != "STRUCTURAL COLUMN" || item.IsChildOf(root.transform))
                continue;

            Vector3 p = item.localPosition;
            Vector3 s = item.localScale;
            float width = Mathf.Max(1.3f, s.x * 1.18f);
            float depth = Mathf.Max(1.3f, s.z * 1.18f);
            float height = Mathf.Max(0.5f, s.y);

            CreateBox(root.transform, "COLUMN METAL BASE " + (++columnIndex),
                new Vector3(p.x, 0.12f, p.z), new Vector3(width + 0.35f, 0.24f, depth + 0.35f), metal, false);
            CreateBox(root.transform, "COLUMN CYAN BAND " + columnIndex,
                new Vector3(p.x, 1.1f, p.z), new Vector3(width + 0.08f, 0.08f, depth + 0.08f), cyan, false);
            CreateBox(root.transform, "COLUMN TOP COLLAR " + columnIndex,
                new Vector3(p.x, Mathf.Min(15.7f, height + p.y - 0.5f), p.z),
                new Vector3(width + 0.2f, 0.22f, depth + 0.2f), metal, false);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Selection.activeGameObject = root;
        EditorUtility.DisplayDialog("SCN17 Interior", "Interior floor panels, cyan perimeter accents, and column trim added. Existing walls, windows, entrance, HUD, and original floor remain unchanged.", "OK");
    }

    private static Material MakeMaterial(string name, Color color, float metallic, float smoothness, bool emissive)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (!shader) shader = Shader.Find("Standard");
        Material material = new Material(shader);
        material.name = name;

        if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
        if (material.HasProperty("_Color")) material.SetColor("_Color", color);
        if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", metallic);
        if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", smoothness);
        if (material.HasProperty("_Glossiness")) material.SetFloat("_Glossiness", smoothness);

        if (emissive && material.HasProperty("_EmissionColor"))
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * 1.8f);
        }

        return material;
    }

    private static void CreateBox(Transform parent, string name, Vector3 position, Vector3 scale, Material material, bool collider)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = position;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = material;
        if (!collider)
            Object.DestroyImmediate(go.GetComponent<Collider>());
    }
}

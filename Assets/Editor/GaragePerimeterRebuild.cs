
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public static class GaragePerimeterRebuild
{
    private const string RootName = "GARAGE_PERIMETER_OFFICES";

    [MenuItem("Obsidian Protocol/Garage/REBUILD PERIMETER OFFICES")]
    public static void Rebuild()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.path.Replace("\\", "/").EndsWith(
            "SCN04_GARAGE/SCN04_GARAGE.unity"))
        {
            EditorUtility.DisplayDialog("Garage",
                "Open SCN04_GARAGE first.", "OK");
            return;
        }

        GameObject floor = GameObject.Find("GARAGE_FLOOR");

        if (floor == null)
        {
            Debug.LogError("GARAGE_FLOOR not found.");
            return;
        }

        Renderer[] renderers =
            floor.GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            Debug.LogError("GARAGE_FLOOR has no renderers.");
            return;
        }

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);

        float width = bounds.size.x;
        float depth = bounds.size.z;

        if (width < 20 || depth < 20)
        {
            Debug.LogError("Garage floor bounds too small: " +
                width + " x " + depth);
            return;
        }

        string backup = scene.path + ".before_perimeter_rebuild.bak";

        if (!File.Exists(backup))
            File.Copy(scene.path, backup);

        GameObject old = GameObject.Find(RootName);

        if (old != null)
            Undo.DestroyObjectImmediate(old);

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Garage Offices");

        Material wall = MakeMaterial("Office Graphite",
            new Color(0.075f, 0.10f, 0.13f));

        Material trim = MakeMaterial("Office Steel",
            new Color(0.22f, 0.29f, 0.34f));

        Material glass = MakeMaterial("Office Glass",
            new Color(0.10f, 0.55f, 0.68f));

        Material desk = MakeMaterial("Office Workstations",
            new Color(0.035f, 0.045f, 0.055f));

        Material display = MakeMaterial("Office Displays",
            new Color(0.02f, 0.42f, 0.62f));

        float roomDepth = Mathf.Clamp(
            Mathf.Min(width, depth) * 0.16f, 4f, 7f);

        float height = 4f;
        float floorY = bounds.min.y + 0.05f;

        float cx = bounds.center.x;
        float cz = bounds.center.z;

        float north = bounds.max.z - roomDepth * 0.5f;
        float south = bounds.min.z + roomDepth * 0.5f;
        float east = bounds.max.x - roomDepth * 0.5f;
        float west = bounds.min.x + roomDepth * 0.5f;

        // NORTH: COMMAND AND INTELLIGENCE

        BuildRoom(root.transform, "COMMAND",
            new Vector3(cx - width * 0.22f, floorY, north),
            true, width * 0.20f, roomDepth, height, -1,
            wall, trim, glass, desk, display);

        BuildRoom(root.transform, "INTELLIGENCE",
            new Vector3(cx + width * 0.22f, floorY, north),
            true, width * 0.20f, roomDepth, height, -1,
            wall, trim, glass, desk, display);

        // EAST: RESEARCH

        BuildRoom(root.transform, "RESEARCH",
            new Vector3(east, floorY, cz),
            false, depth * 0.25f, roomDepth, height, -1,
            wall, trim, glass, desk, display);

        // SOUTH: DEPLOYMENT

        BuildRoom(root.transform, "DEPLOYMENT",
            new Vector3(cx, floorY, south),
            true, width * 0.34f, roomDepth, height, 1,
            wall, trim, glass, desk, display);

        // WEST: MAINTENANCE AND LOGISTICS

        BuildRoom(root.transform, "MAINTENANCE",
            new Vector3(west, floorY, cz - depth * 0.22f),
            false, depth * 0.18f, roomDepth, height, 1,
            wall, trim, glass, desk, display);

        BuildRoom(root.transform, "LOGISTICS",
            new Vector3(west, floorY, cz + depth * 0.22f),
            false, depth * 0.18f, roomDepth, height, 1,
            wall, trim, glass, desk, display);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("GARAGE REBUILD COMPLETE. Six offices created. Center preserved.");
        EditorUtility.DisplayDialog("Garage Rebuild",
            "Six perimeter offices created. Central hangar remains open.\n\nBackup:\n" +
            backup, "OK");
    }

    private static Material MakeMaterial(string name, Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        Material mat = new Material(shader);
        mat.name = name;
        mat.color = color;
        return mat;
    }

    private static void BuildRoom(
        Transform parent, string label, Vector3 center,
        bool alongX, float length, float depth, float height,
        int inward, Material wall, Material trim,
        Material glass, Material desk, Material display)
    {
        GameObject room = new GameObject(label + "_OFFICE");
        room.transform.SetParent(parent);
        room.transform.position = center;

        float hl = length * 0.5f;
        float hd = depth * 0.5f;

        Cube(room.transform, "Office Floor",
            new Vector3(0, 0.08f, 0),
            new Vector3(length, 0.16f, depth), trim);

        Cube(room.transform, "Rear Wall",
            alongX
                ? new Vector3(0, height * 0.5f, -inward * hd)
                : new Vector3(-inward * hd, height * 0.5f, 0),
            alongX
                ? new Vector3(length, height, 0.18f)
                : new Vector3(0.18f, height, length),
            wall);

        if (alongX)
        {
            Cube(room.transform, "Side Wall Left",
                new Vector3(-hl, height * 0.5f, 0),
                new Vector3(0.18f, height, depth), wall);

            Cube(room.transform, "Side Wall Right",
                new Vector3(hl, height * 0.5f, 0),
                new Vector3(0.18f, height, depth), wall);

            float front = inward * hd;
            float panel = (length - 1.2f) * 0.5f;

            Cube(room.transform, "Glass Left",
                new Vector3(-0.3f - panel * 0.5f,
                    height * 0.5f, front),
                new Vector3(panel, height * 0.78f, 0.10f), glass);

            Cube(room.transform, "Glass Right",
                new Vector3(0.3f + panel * 0.5f,
                    height * 0.5f, front),
                new Vector3(panel, height * 0.78f, 0.10f), glass);

            Cube(room.transform, "Header",
                new Vector3(0, height * 0.9f, front),
                new Vector3(length, 0.2f, 0.18f), trim);
        }
        else
        {
            Cube(room.transform, "Side Wall Left",
                new Vector3(0, height * 0.5f, -hl),
                new Vector3(depth, height, 0.18f), wall);

            Cube(room.transform, "Side Wall Right",
                new Vector3(0, height * 0.5f, hl),
                new Vector3(depth, height, 0.18f), wall);

            float front = inward * hd;
            float panel = (length - 1.2f) * 0.5f;

            Cube(room.transform, "Glass Left",
                new Vector3(front, height * 0.5f,
                    -0.3f - panel * 0.5f),
                new Vector3(0.10f, height * 0.78f, panel), glass);

            Cube(room.transform, "Glass Right",
                new Vector3(front, height * 0.5f,
                    0.3f + panel * 0.5f),
                new Vector3(0.10f, height * 0.78f, panel), glass);

            Cube(room.transform, "Header",
                new Vector3(front, height * 0.9f, 0),
                new Vector3(0.18f, 0.2f, length), trim);
        }

        Cube(room.transform, "Office Roof",
            new Vector3(0, height, 0),
            new Vector3(alongX ? length : depth,
                0.2f, alongX ? depth : length), trim);

        Vector3 deskPosition = alongX
            ? new Vector3(0, 1f, -inward * (hd - 0.8f))
            : new Vector3(-inward * (hd - 0.8f), 1f, 0);

        Cube(room.transform, "Workstation",
            deskPosition,
            alongX
                ? new Vector3(length * 0.4f, 0.15f, 0.7f)
                : new Vector3(0.7f, 0.15f, length * 0.4f),
            desk);

        Vector3 screenPosition = deskPosition +
            Vector3.up * 0.55f;

        Cube(room.transform, "Operations Display",
            screenPosition,
            alongX
                ? new Vector3(length * 0.22f, 0.65f, 0.08f)
                : new Vector3(0.08f, 0.65f, length * 0.22f),
            display);

        GameObject sign = new GameObject("Department Sign");
        sign.transform.SetParent(room.transform, false);

        sign.transform.localPosition = alongX
            ? new Vector3(0, height + 0.4f, inward * (hd + 0.1f))
            : new Vector3(inward * (hd + 0.1f), height + 0.4f, 0);

        TextMesh text = sign.AddComponent<TextMesh>();
        text.text = label;
        text.fontSize = 64;
        text.characterSize = 0.22f;
        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.color = Color.cyan;
    }

    private static void Cube(
        Transform parent, string name,
        Vector3 position, Vector3 scale, Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = position;
        obj.transform.localScale = scale;

        obj.GetComponent<Renderer>().sharedMaterial = material;

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
            Object.DestroyImmediate(col);
    }
}

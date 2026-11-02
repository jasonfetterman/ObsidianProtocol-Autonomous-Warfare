using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreScreenInterfaceBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    private static readonly string[] ScreenNames =
    {
        "AIR UNITS wall screen",
        "GROUND UNITS wall screen",
        "SEA UNITS wall screen",
        "COMMANDERS wall screen",
        "EXPERIMENTAL UNITS wall screen",
        "SATELLITES wall screen"
    };

    private static readonly string[] Categories =
    {
        "AIRFRAME PROCUREMENT",
        "GROUND SYSTEMS",
        "NAVAL SYSTEMS",
        "COMMAND AUTHORITY",
        "EXPERIMENTAL DIVISION",
        "ORBITAL SYSTEMS"
    };

    private static readonly string[] Subtitles =
    {
        "AIR ASSET REGISTRY",
        "ARMORED ASSET REGISTRY",
        "MARITIME ASSET REGISTRY",
        "COMMANDER REGISTRY",
        "PROTOTYPE REGISTRY",
        "SATELLITE REGISTRY"
    };

    private static readonly Color[] Accent =
    {
        new Color(0.00f, 0.88f, 1.00f),
        new Color(1.00f, 0.48f, 0.08f),
        new Color(0.00f, 1.00f, 0.58f),
        new Color(1.00f, 0.82f, 0.20f),
        new Color(1.00f, 0.16f, 0.43f),
        new Color(0.62f, 0.42f, 1.00f)
    };

    [MenuItem("Obsidian Protocol/Store/Build Screen Interfaces")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (EditorApplication.isPlaying || scene.path != ScenePath)
        {
            Debug.LogError("Open SCN16_STORE and stop Play mode first. No changes made.");
            return;
        }

        GameObject[] screens = new GameObject[ScreenNames.Length];

        for (int i = 0; i < ScreenNames.Length; i++)
        {
            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.scene == scene && go.name == ScreenNames[i])
                {
                    screens[i] = go;
                    break;
                }
            }

            if (screens[i] == null)
            {
                Debug.LogError("Screen not found: " + ScreenNames[i] + ". No changes made.");
                return;
            }

            if (screens[i].transform.Find("PROCUREMENT UI") != null)
            {
                Debug.LogError("Interface already exists on " + ScreenNames[i] +
                               ". No changes made to avoid duplicates.");
                return;
            }
        }

        string backup = ScenePath + ".bak_screen_interfaces_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(ScenePath, backup, false);

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Build Store Screen Interfaces");

        for (int i = 0; i < screens.Length; i++)
            BuildOne(screens[i].transform, i);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(undoGroup);

        Debug.Log("Built six Store procurement interfaces. Backup: " + backup);
    }

    private static void BuildOne(Transform screen, int index)
    {
        GameObject root = new GameObject("PROCUREMENT UI");
        Undo.RegisterCreatedObjectUndo(root, "Create procurement interface");
        root.transform.SetParent(screen, false);
        root.transform.localPosition = new Vector3(0f, 0f, -0.56f);
        root.transform.localRotation = Quaternion.identity;
        root.transform.localScale = Vector3.one;

        float w = 4.0f;
        float h = 2.8f;
        Color accent = Accent[index];
        Color dark = new Color(0.012f, 0.025f, 0.045f, 1f);
        Color panel = new Color(0.025f, 0.065f, 0.09f, 1f);

        Quad(root.transform, "Terminal background", Vector3.zero,
             new Vector3(w, h, 1f), dark);

        Quad(root.transform, "Top accent rail",
             new Vector3(0f, h * 0.44f, -0.02f),
             new Vector3(w, 0.035f, 1f), accent);

        Text(root.transform, "System title", Categories[index],
             new Vector3(-w * 0.46f, h * 0.36f, -0.04f),
             0.19f, accent, TextAnchor.MiddleLeft, FontStyle.Bold);

        Text(root.transform, "System subtitle", Subtitles[index],
             new Vector3(-w * 0.46f, h * 0.27f, -0.04f),
             0.105f, Color.white, TextAnchor.MiddleLeft, FontStyle.Normal);

        Text(root.transform, "System status", "SYSTEM ONLINE  //  INVENTORY LINK PENDING",
             new Vector3(w * 0.46f, h * 0.36f, -0.04f),
             0.075f, accent, TextAnchor.MiddleRight, FontStyle.Normal);

        float left = -1.28f;
        float top = 0.34f;
        float cardW = 0.79f;
        float cardH = 0.82f;
        float gapX = 0.08f;
        float gapY = 0.12f;

        for (int slot = 0; slot < 3; slot++)
        {
            float x = left + slot * (cardW + gapX);
            float y = top;

            Quad(root.transform, "Asset slot " + (slot + 1),
                 new Vector3(x, y, -0.025f),
                 new Vector3(cardW, cardH, 1f), panel);

            Quad(root.transform, "Asset slot accent " + (slot + 1),
                 new Vector3(x, y + cardH * 0.43f, -0.045f),
                 new Vector3(cardW, 0.025f, 1f), accent);

            Text(root.transform, "Slot code " + (slot + 1),
                 "ASSET 0" + (slot + 1),
                 new Vector3(x, y + 0.23f, -0.06f),
                 0.09f, accent, TextAnchor.MiddleCenter, FontStyle.Bold);

            Text(root.transform, "Slot state " + (slot + 1),
                 "AWAITING DATA",
                 new Vector3(x, y - 0.22f, -0.06f),
                 0.07f, Color.white, TextAnchor.MiddleCenter, FontStyle.Normal);
        }

        float lowerY = -0.66f;
        Quad(root.transform, "Selection details panel",
             new Vector3(0f, lowerY, -0.025f),
             new Vector3(3.72f, 0.46f, 1f), panel);

        Text(root.transform, "Selection heading", "SELECTION DETAILS",
             new Vector3(-1.75f, lowerY + 0.08f, -0.06f),
             0.09f, accent, TextAnchor.MiddleLeft, FontStyle.Bold);

        Text(root.transform, "Selection status", "NO ASSET SELECTED",
             new Vector3(1.70f, lowerY - 0.08f, -0.06f),
             0.085f, Color.white, TextAnchor.MiddleRight, FontStyle.Normal);

        Text(root.transform, "Footer", "OBSIDIAN PROTOCOL  /  PROCUREMENT NETWORK",
             new Vector3(-1.83f, -1.19f, -0.04f),
             0.075f, accent, TextAnchor.MiddleLeft, FontStyle.Normal);

        Text(root.transform, "Page indicator", "PAGE 01  /  --",
             new Vector3(1.83f, -1.19f, -0.04f),
             0.075f, Color.white, TextAnchor.MiddleRight, FontStyle.Normal);
    }

    private static void Quad(Transform parent, string name, Vector3 pos,
                             Vector3 scale, Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Undo.RegisterCreatedObjectUndo(go, "Create terminal graphic");
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = scale;

        Collider collider = go.GetComponent<Collider>();
        if (collider != null)
            Undo.DestroyObjectImmediate(collider);

        Shader shader = Shader.Find("Unlit/Color");
        if (shader == null)
            shader = Shader.Find("Sprites/Default");

        if (shader != null)
        {
            Material mat = new Material(shader);
            mat.color = color;
            mat.name = "Runtime generated " + name;
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        }
    }

    private static void Text(Transform parent, string name, string value,
                             Vector3 pos, float size, Color color,
                             TextAnchor anchor, FontStyle style)
    {
        GameObject go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, "Create terminal text");
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        TextMesh mesh = go.AddComponent<TextMesh>();
        mesh.text = value;
        mesh.characterSize = size;
        mesh.fontSize = 48;
        mesh.anchor = anchor;
        mesh.alignment = TextAlignment.Center;
        mesh.fontStyle = style;
        mesh.color = color;
    }
}

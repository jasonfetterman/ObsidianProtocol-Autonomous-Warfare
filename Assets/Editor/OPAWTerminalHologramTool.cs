using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class OPAWTerminalHologramTool
{
    private const string RootName = "OPAW_TERMINAL_HOLOGRAMS";
    private const string MaterialFolder = "Assets/Materials";
    private const string MaterialPath = "Assets/Materials/OPAW_Terminal_HologramGlow.mat";

    [MenuItem("Obsidian Protocol/Garage/Build Terminal Holograms")]
    private static void Build()
    {
        Scene scene = EditorSceneManager.GetActiveScene();
        if (scene.name != "SCN04_GARAGE" || string.IsNullOrEmpty(scene.path))
        {
            Debug.LogError("[TerminalHolograms] Open and save SCN04_GARAGE first. No changes made.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            Debug.LogWarning("[TerminalHolograms] Cancelled. No changes made.");
            return;
        }

        GameObject existing = GameObject.Find(RootName);
        if (existing != null)
        {
            if (!EditorUtility.DisplayDialog("Rebuild terminal holograms",
                "The hologram root already exists. Replace only that generated root?",
                "Replace generated holograms", "Cancel"))
                return;
        }

        string backupPath = scene.path + ".terminal-holograms-backup";
        if (File.Exists(backupPath))
            backupPath = scene.path + ".terminal-holograms-backup-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        File.Copy(scene.path, backupPath);

        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            Debug.LogError("[TerminalHolograms] URP shader not found. Backup: " + backupPath);
            return;
        }

        if (!AssetDatabase.IsValidFolder(MaterialFolder))
            AssetDatabase.CreateFolder("Assets", "Materials");

        Material glow = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
        if (glow == null)
        {
            glow = new Material(shader);
            glow.name = "OPAW_Terminal_HologramGlow";
            AssetDatabase.CreateAsset(glow, MaterialPath);
        }
        Color cyan = new Color(0.02f, 0.78f, 1f, 1f);
        if (glow.HasProperty("_BaseColor")) glow.SetColor("_BaseColor", cyan);
        if (glow.HasProperty("_Color")) glow.SetColor("_Color", cyan);
        if (glow.HasProperty("_EmissionColor")) glow.SetColor("_EmissionColor", cyan * 1.5f);
        if (glow.HasProperty("_Surface")) glow.SetFloat("_Surface", 0f);
        glow.EnableKeyword("_EMISSION");
        EditorUtility.SetDirty(glow);
        AssetDatabase.SaveAssets();

        if (existing != null) Undo.DestroyObjectImmediate(existing);

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Terminal Holograms");
        int count = 0;

        foreach (GameObject sceneRoot in scene.GetRootGameObjects())
        {
            foreach (Transform terminal in sceneRoot.GetComponentsInChildren<Transform>(true))
            {
                if (!terminal.name.StartsWith("TERMINAL_")) continue;

                Transform display = null;
                foreach (Transform child in terminal.GetComponentsInChildren<Transform>(true))
                {
                    if (child != terminal && child.name == "DISPLAY")
                    {
                        display = child;
                        break;
                    }
                }
                if (display == null) continue;

                GameObject holo = new GameObject("HOLO_" + terminal.name);
                Undo.RegisterCreatedObjectUndo(holo, "Create Terminal Hologram");
                holo.transform.SetParent(root.transform, true);
                holo.transform.position = display.transform.position + display.transform.forward * -0.09f;
                holo.transform.rotation = display.transform.rotation;

                Vector3 scale = display.lossyScale;
                float w = Mathf.Clamp(scale.x * 0.72f, 0.7f, 1.8f);
                float h = Mathf.Clamp(scale.y * 0.72f, 0.55f, 1.25f);

                CreateLine(holo.transform, "Frame", glow, new Vector3[] {
                    new Vector3(-w/2,-h/2,0), new Vector3(w/2,-h/2,0),
                    new Vector3(w/2,h/2,0), new Vector3(-w/2,h/2,0),
                    new Vector3(-w/2,-h/2,0)
                }, 0.008f);

                string title = terminal.name.Replace("TERMINAL_", "").Replace("_", " ");
                float headerSize = Mathf.Clamp(w * 0.055f, 0.025f, 0.07f) * Mathf.Clamp(16f / Mathf.Max(title.Length, 12), 0.55f, 1f);
        CreateText(holo.transform, "Header", glow, title, new Vector3(-w * 0.43f, h * 0.36f, -0.01f), headerSize);

                CreateLine(holo.transform, "HeaderRule", glow, new Vector3[] {
                    new Vector3(-w*0.43f,h*0.25f,-0.01f), new Vector3(w*0.43f,h*0.25f,-0.01f)
                }, 0.006f);

                string type = terminal.name;
                if (type.Contains("FLEET") || type.Contains("FORCE"))
                {
                    CreateCircle(holo.transform, "UnitRadar", glow, new Vector3(0, -h*0.02f, -0.015f), Mathf.Min(w,h)*0.2f);
                    CreateLine(holo.transform, "UnitAxis", glow, new Vector3[] {
                        new Vector3(-w*0.24f,-h*0.03f,-0.02f), new Vector3(w*0.24f,-h*0.03f,-0.02f),
                        new Vector3(0,-h*0.03f,-0.02f), new Vector3(0,h*0.16f,-0.02f)
                    }, 0.007f);
                }
                else if (type.Contains("LOGISTICS") || type.Contains("DEPLOYMENT") || type.Contains("OPERATION"))
                {
                    CreateLine(holo.transform, "Route", glow, new Vector3[] {
                        new Vector3(-w*0.32f,-h*0.05f,-0.02f), new Vector3(-w*0.12f,h*0.06f,-0.02f),
                        new Vector3(w*0.08f,-h*0.02f,-0.02f), new Vector3(w*0.30f,h*0.12f,-0.02f)
                    }, 0.009f);
                    CreateNode(holo.transform, glow, new Vector3(-w*0.32f,-h*0.05f,-0.025f));
                    CreateNode(holo.transform, glow, new Vector3(w*0.30f,h*0.12f,-0.025f));
                }
                else if (type.Contains("AI_COMMAND") || type.Contains("COMMAND_INTENT") || type.Contains("DOCTRINE") || type.Contains("RULES"))
                {
                    Vector3 a = new Vector3(-w*0.24f,-h*0.02f,-0.02f);
                    Vector3 b = new Vector3(0,h*0.10f,-0.02f);
                    Vector3 c = new Vector3(w*0.24f,-h*0.02f,-0.02f);
                    CreateLine(holo.transform, "Network", glow, new Vector3[] { a,b,c,a }, 0.008f);
                    CreateNode(holo.transform, glow, a);
                    CreateNode(holo.transform, glow, b);
                    CreateNode(holo.transform, glow, c);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float y = h * (0.13f - i * 0.09f);
                        float length = w * (0.18f + 0.08f * ((i % 3) + 1));
                        CreateLine(holo.transform, "DataBar_" + i, glow, new Vector3[] {
                            new Vector3(-w*0.30f,y,-0.02f), new Vector3(-w*0.30f+length,y,-0.02f)
                        }, 0.012f);
                    }
                }

                CreateText(holo.transform, "Status", glow, "SYSTEM ONLINE - LIVE DATA",
                    new Vector3(-w * 0.42f, -h * 0.39f, -0.02f), Mathf.Clamp(w * 0.032f, 0.018f, 0.04f));

                count++;
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        if (EditorSceneManager.SaveScene(scene))
            Debug.Log("[TerminalHolograms] Created " + count + " station-matched hologram overlays. Backup: " + backupPath);
        else
            Debug.LogError("[TerminalHolograms] Save failed. Backup remains: " + backupPath);
    }

    private static void CreateLine(Transform parent, string name, Material material, Vector3[] points, float width)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        LineRenderer line = go.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.positionCount = points.Length;
        line.SetPositions(points);
        line.startWidth = width;
        line.endWidth = width;
        line.material = material;
        line.numCapVertices = 2;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
    }

    private static void CreateNode(Transform parent, Material material, Vector3 position)
    {
        GameObject node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        node.name = "SignalNode";
        node.transform.SetParent(parent, false);
        node.transform.localPosition = position;
        node.transform.localScale = Vector3.one * 0.045f;
        var collider = node.GetComponent<Collider>();
        if (collider) UnityEngine.Object.DestroyImmediate(collider);
        var renderer = node.GetComponent<Renderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
    }

    private static void CreateCircle(Transform parent, string name, Material material, Vector3 center, float radius)
    {
        Vector3[] points = new Vector3[49];
        for (int i = 0; i < points.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / (points.Length - 1);
            points[i] = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }
        CreateLine(parent, name, material, points, 0.007f);
    }

    private static void CreateText(Transform parent, string name, Material material, string value, Vector3 position, float size)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = position;
        TextMesh text = go.AddComponent<TextMesh>();
        Font safeFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (safeFont == null)
            safeFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        if (safeFont != null)
            text.font = safeFont;
        text.text = value;
        text.fontSize = 18;
        text.characterSize = size;
        text.anchor = TextAnchor.MiddleLeft;
        text.alignment = TextAlignment.Left;
        text.color = new Color(0.15f, 0.9f, 1f, 1f);
        MeshRenderer renderer = go.GetComponent<MeshRenderer>();
        // Keep the font-generated material so TextMesh glyphs render correctly.
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
    }
}







using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class GarageHolographicSignBuilder
{
    private const string RootName = "GARAGE_HOLOGRAPHIC_SIGNAGE";

    [MenuItem("Obsidian Protocol/Garage/Replace Physical Labels With Holograms")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() ||
            !scene.path.Replace("\\", "/").EndsWith("/SCN04_GARAGE.unity"))
        {
            EditorUtility.DisplayDialog(
                "Garage Signage",
                "Open SCN04_GARAGE before running this tool.",
                "OK");
            return;
        }

        List<TextMesh> labels = new List<TextMesh>(
            Object.FindObjectsOfType<TextMesh>(true));

        labels.RemoveAll(t =>
            t == null ||
            !t.gameObject.scene.Equals(scene) ||
            !t.gameObject.name.StartsWith("LABEL_") ||
            string.IsNullOrWhiteSpace(t.text));

        if (labels.Count == 0)
        {
            EditorUtility.DisplayDialog(
                "Garage Signage",
                "No physical LABEL_ TextMesh objects were found. Nothing changed.",
                "OK");
            return;
        }

        GameObject oldRoot = GameObject.Find(RootName);
        if (oldRoot != null)
            Undo.DestroyObjectImmediate(oldRoot);

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Create Garage Holograms");

        Material cyan = MakeMaterial("Hologram Cyan", new Color(0.02f, 0.75f, 1f), 2.4f);
        Material blue = MakeMaterial("Hologram Blue", new Color(0.02f, 0.22f, 0.8f), 1.5f);
        Material amber = MakeMaterial("Hologram Amber", new Color(1f, 0.42f, 0.04f), 2.0f);
        Material glass = MakeMaterial("Hologram Glass", new Color(0.015f, 0.12f, 0.19f), 0.65f);

        int built = 0;

        foreach (TextMesh label in labels)
        {
            if (label == null) continue;

            string title = label.text.Trim().ToUpperInvariant();
            Transform source = label.transform;

            GameObject sign = new GameObject("HOLO_SIGN_" + Clean(title));
            Undo.RegisterCreatedObjectUndo(sign, "Create Holographic Sign");
            sign.transform.SetParent(root.transform);
            sign.transform.SetPositionAndRotation(
                source.position,
                source.rotation);

            float width = Mathf.Clamp(title.Length * 0.105f + 0.75f, 1.35f, 3.4f);
            float height = 0.78f;

            Part(sign.transform, "Dark Glass Panel",
                new Vector3(0, 0, 0), new Vector3(width, height, 0.035f), glass);

            // Outer frame and bright corner rails.
            Part(sign.transform, "Top Cyan Rail",
                new Vector3(0, height / 2, 0.035f),
                new Vector3(width, 0.025f, 0.025f), cyan);
            Part(sign.transform, "Bottom Cyan Rail",
                new Vector3(0, -height / 2, 0.035f),
                new Vector3(width, 0.018f, 0.025f), cyan);
            Part(sign.transform, "Left Blue Rail",
                new Vector3(-width / 2, 0, 0.035f),
                new Vector3(0.025f, height, 0.025f), blue);
            Part(sign.transform, "Right Blue Rail",
                new Vector3(width / 2, 0, 0.035f),
                new Vector3(0.025f, height, 0.025f), blue);

            // Corner markers and technical status lights.
            for (int i = 0; i < 4; i++)
            {
                float x = (i < 2 ? -1 : 1) * (width / 2 - 0.09f);
                float y = (i % 2 == 0 ? -1 : 1) * (height / 2 - 0.09f);
                Part(sign.transform, "Corner Indicator",
                    new Vector3(x, y, 0.055f),
                    new Vector3(0.065f, 0.065f, 0.018f),
                    i == 0 ? amber : cyan);
            }

            // Animated-looking technical bars, fixed in place.
            Part(sign.transform, "Data Line",
                new Vector3(0, -0.22f, 0.055f),
                new Vector3(width * 0.58f, 0.012f, 0.012f), blue);
            Part(sign.transform, "Active Data Segment",
                new Vector3(-width * 0.20f, -0.22f, 0.067f),
                new Vector3(width * 0.16f, 0.025f, 0.014f), cyan);

            GameObject heading = new GameObject("Hologram Heading");
            heading.transform.SetParent(sign.transform, false);
            heading.transform.localPosition = new Vector3(0, 0.08f, 0.075f);

            TextMesh text = heading.AddComponent<TextMesh>();
            text.text = title;
            text.fontSize = 64;
            text.characterSize = 0.045f;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.color = Color.white;

            Renderer textRenderer = heading.GetComponent<Renderer>();
            if (textRenderer != null)
                textRenderer.sharedMaterial = cyan;

            GameObject sub = new GameObject("Hologram System Code");
            sub.transform.SetParent(sign.transform, false);
            sub.transform.localPosition = new Vector3(0, -0.08f, 0.075f);

            TextMesh code = sub.AddComponent<TextMesh>();
            code.text = "OPAW  //  SYSTEM ONLINE  //  " +
                        (built + 1).ToString("D3");
            code.fontSize = 48;
            code.characterSize = 0.018f;
            code.anchor = TextAnchor.MiddleCenter;
            code.alignment = TextAlignment.Center;
            code.color = new Color(0.15f, 0.85f, 1f);

            Renderer codeRenderer = sub.GetComponent<Renderer>();
            if (codeRenderer != null)
                codeRenderer.sharedMaterial = cyan;

            // Remove only the old physical label text component.
            // Preserve the object if it contains anything else.
            GameObject oldLabel = label.gameObject;
            Component[] components = oldLabel.GetComponents<Component>();
            bool safeToRemoveObject = components.Length == 2;

            if (safeToRemoveObject)
                Undo.DestroyObjectImmediate(oldLabel);
            else
                Undo.DestroyObjectImmediate(label);

            built++;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        EditorUtility.DisplayDialog(
            "Garage Holograms Created",
            "Created " + built + " holographic signs in SCN04_GARAGE.\n\n" +
            "The HUD and walking-mode objects were not targeted.",
            "OK");

        Debug.Log("GARAGE HOLOGRAMS: Created " + built + " signs in " + scene.path);
    }

    private static GameObject Part(
        Transform parent, string name, Vector3 position,
        Vector3 scale, Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = position;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = scale;

        Collider collider = obj.GetComponent<Collider>();
        if (collider != null) Object.DestroyImmediate(collider);

        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.sharedMaterial = material;
        return obj;
    }

    private static Material MakeMaterial(string name, Color color, float intensity)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null) shader = Shader.Find("Unlit/Color");
        if (shader == null) shader = Shader.Find("Standard");

        Material material = new Material(shader);
        material.name = name;

        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);
        if (material.HasProperty("_Color"))
            material.SetColor("_Color", color);
        if (material.HasProperty("_EmissionColor"))
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * intensity);
        }

        return material;
    }

    private static string Clean(string value)
    {
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            value = value.Replace(c, '_');

        value = value.Replace(' ', '_');
        return value.Length > 32 ? value.Substring(0, 32) : value;
    }
}

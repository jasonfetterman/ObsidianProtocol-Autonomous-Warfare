using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class GarageHologramTextRepair
{
    [MenuItem("Obsidian Protocol/Garage/Repair Hologram Text")]
    public static void Repair()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() ||
            !scene.path.Replace("\\", "/").EndsWith("/SCN04_GARAGE.unity"))
        {
            EditorUtility.DisplayDialog(
                "Hologram Text Repair",
                "Open SCN04_GARAGE first.", "OK");
            return;
        }

        GameObject root = GameObject.Find("GARAGE_HOLOGRAPHIC_SIGNAGE");
        if (root == null)
        {
            EditorUtility.DisplayDialog(
                "Hologram Text Repair",
                "GARAGE_HOLOGRAPHIC_SIGNAGE was not found. Nothing changed.",
                "OK");
            return;
        }

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
        {
            EditorUtility.DisplayDialog(
                "Hologram Text Repair",
                "Unity's built-in Arial font could not be loaded. Nothing changed.",
                "OK");
            return;
        }

        int repaired = 0;

        foreach (TextMesh text in root.GetComponentsInChildren<TextMesh>(true))
        {
            if (text == null) continue;

            Undo.RecordObject(text, "Repair Hologram Text");

            text.font = font;
            text.richText = false;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;

            bool heading = text.gameObject.name == "Hologram Heading";

            if (heading)
            {
                text.fontSize = 64;
                text.characterSize = 0.045f;
                text.color = new Color(0.35f, 0.95f, 1f, 1f);
                text.transform.localPosition = new Vector3(0f, 0.08f, 0.095f);
            }
            else
            {
                text.fontSize = 48;
                text.characterSize = 0.018f;
                text.color = new Color(0.1f, 0.8f, 1f, 1f);
                text.transform.localPosition = new Vector3(0f, -0.08f, 0.095f);
            }

            Renderer renderer = text.GetComponent<Renderer>();
            if (renderer != null)
            {
                Undo.RecordObject(renderer, "Restore Font Material");
                renderer.sharedMaterial = font.material;
                renderer.enabled = true;
                EditorUtility.SetDirty(renderer);
            }

            EditorUtility.SetDirty(text);
            repaired++;
        }

        if (repaired == 0)
        {
            EditorUtility.DisplayDialog(
                "Hologram Text Repair",
                "No TextMesh components were found under the signage root.",
                "OK");
            return;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("HOLOGRAM TEXT REPAIR: Repaired " + repaired + " text components.");
        EditorUtility.DisplayDialog(
            "Hologram Text Repair",
            "Repaired " + repaired + " text components and saved SCN04_GARAGE.",
            "OK");
    }
}


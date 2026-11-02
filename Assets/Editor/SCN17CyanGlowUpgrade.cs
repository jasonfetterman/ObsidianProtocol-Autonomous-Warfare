using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

public static class SCN17CyanGlowUpgrade
{
    const string ScenePath =
        "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";

    [MenuItem("Obsidian Protocol/SCN17/Make All Cyan Glow")]
    static void Apply()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
        {
            Debug.LogError("[SCN17 GLOW] Open SCN17_PERSISTENT_MILITARY first.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        int lights = 0, materials = 0;

        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (!go.scene.IsValid() || go.scene.path != ScenePath)
                continue;

            foreach (Light l in go.GetComponents<Light>())
            {
                if (l == null) continue;

                Color c = l.color;
                bool cyan = c.g > c.r * 1.35f && c.b > c.r * 1.35f
                         && c.g > 0.2f && c.b > 0.25f;

                if (!cyan) continue;

                Undo.RecordObject(l, "Make SCN17 Cyan Lights Glow");
                l.intensity = Mathf.Max(l.intensity, 3.5f);
                l.color = new Color(0.08f, 0.72f, 1f, 1f);
                l.range = Mathf.Max(l.range, 8f);
                EditorUtility.SetDirty(l);
                lights++;
            }

            foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
            {
                if (r == null) continue;

                foreach (Material m in r.sharedMaterials)
                {
                    if (m == null) continue;

                    Color baseColor = Color.black;
                    bool hasColor = false;

                    if (m.HasProperty("_BaseColor"))
                    {
                        baseColor = m.GetColor("_BaseColor");
                        hasColor = true;
                    }
                    else if (m.HasProperty("_Color"))
                    {
                        baseColor = m.GetColor("_Color");
                        hasColor = true;
                    }

                    if (!hasColor ||
                        baseColor.g < baseColor.r * 1.35f ||
                        baseColor.b < baseColor.r * 1.35f ||
                        baseColor.g < 0.2f || baseColor.b < 0.25f)
                        continue;

                    Undo.RecordObject(m, "Make SCN17 Cyan Materials Glow");

                    Color glow = new Color(0.05f, 0.65f, 1f, baseColor.a);

                    if (m.HasProperty("_BaseColor"))
                        m.SetColor("_BaseColor", glow);
                    if (m.HasProperty("_Color"))
                        m.SetColor("_Color", glow);

                    if (m.HasProperty("_EmissionColor"))
                    {
                        m.EnableKeyword("_EMISSION");
                        m.SetColor("_EmissionColor",
                            new Color(0.02f, 1.4f, 2.5f, 1f));
                    }

                    EditorUtility.SetDirty(m);
                    materials++;
                }
            }
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log($"[SCN17 GLOW] Updated {lights} cyan lights and {materials} cyan material references. No object positions changed.");
    }
}

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using ObsidianProtocol.Garage;

public static class OPAWGarageTextEffectsTool
{
    [MenuItem("Obsidian Protocol/Garage/Apply Safe Text Effects")]
    private static void Apply()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name != "SCN04_GARAGE")
        {
            EditorUtility.DisplayDialog(
                "Garage Text Effects",
                "Open SCN04_GARAGE first. No changes were made.",
                "OK");
            return;
        }

        string scenePath = scene.path;
        if (string.IsNullOrEmpty(scenePath) || !File.Exists(scenePath))
        {
            EditorUtility.DisplayDialog(
                "Garage Text Effects",
                "Save SCN04_GARAGE before applying effects. No changes were made.",
                "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Apply Garage Text Effects",
            "A backup of the scene will be created first. This tool will add a subtle cyan outline to text and slowly spin objects named TITLE or HEADER. It will not rebuild the scene or remove Image components.",
            "Back Up and Apply",
            "Cancel"))
            return;

        string backupPath = scenePath + ".text-effects-backup";
        if (File.Exists(backupPath))
            backupPath = scenePath + ".text-effects-backup-" +
                         DateTime.Now.ToString("yyyyMMdd-HHmmss");

        File.Copy(scenePath, backupPath, false);

        int outlined = 0;
        int spinning = 0;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Graphic[] graphics = root.GetComponentsInChildren<Graphic>(true);

            foreach (Graphic graphic in graphics)
            {
                if (graphic is not Text && graphic is not TMP_Text)
                    continue;

                Outline outline = graphic.GetComponent<Outline>();
                if (outline == null)
                    outline = Undo.AddComponent<Outline>(graphic.gameObject);

                outline.effectColor = new Color(0.0f, 0.85f, 1.0f, 0.8f);
                outline.effectDistance = new Vector2(1.5f, 1.5f);
                outline.useGraphicAlpha = true;
                outlined++;
            }

            Transform[] transforms = root.GetComponentsInChildren<Transform>(true);

            foreach (Transform item in transforms)
            {
                if (item.GetComponent<Text>() == null &&
                    item.GetComponent<TMP_Text>() == null)
                    continue;

                if (item.GetComponent<GarageTextSpinEffect>() == null)
                {
                    Undo.AddComponent<GarageTextSpinEffect>(item.gameObject);
                    spinning++;
                }
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[GarageTextEffects] Applied outline to " + outlined +
            " text graphics; added spin to " + spinning +
            " title/header objects. Scene backup: " + backupPath);

        EditorUtility.DisplayDialog(
            "Garage Text Effects",
            "Applied cyan text outlines: " + outlined +
            "\nTitle/header objects set to spin: " + spinning +
            "\nScene backup:\n" + backupPath +
            "\n\nSquare backgrounds were left untouched to protect buttons and HUD panels.",
            "OK");
    }
}


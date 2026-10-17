using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildBattlefieldHUDLayout
{
    [MenuItem("Obsidian Protocol/Battlefield/FIX HUD PANEL LAYOUT")]
    public static void Build()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject hud =
            GameObject.Find("[HUD] BATTLEFIELD HUD");

        if (hud == null)
        {
            Debug.LogError("[BATTLEFIELD LAYOUT] HUD NOT FOUND.");
            return;
        }

        SetPanel(
            hud,
            "[PANEL] TOP BAR",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -34f),
            new Vector2(1840f, 68f));

        SetPanel(
            hud,
            "[PANEL] LEFT COMMAND PANEL",
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(190f, 0f),
            new Vector2(340f, 650f));

        SetPanel(
            hud,
            "[PANEL] CENTER TACTICAL PANEL",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 0f),
            new Vector2(760f, 650f));

        SetPanel(
            hud,
            "[PANEL] RIGHT INFORMATION PANEL",
            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.5f),
            new Vector2(-190f, 0f),
            new Vector2(340f, 650f));

        SetPanel(
            hud,
            "[PANEL] BOTTOM COMMAND BAR",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 60f),
            new Vector2(1100f, 100f));

        SetPanel(
            hud,
            "[DISPLAY] BATTLEFIELD STATUS",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 145f),
            new Vector2(700f, 50f));

        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[BATTLEFIELD LAYOUT] PANEL LAYOUT FIXED.");
    }

    private static void SetPanel(
        GameObject hud,
        string objectName,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 position,
        Vector2 size)
    {
        Transform[] children =
            hud.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name != objectName)
                continue;

            RectTransform rt =
                child.GetComponent<RectTransform>();

            if (rt == null)
            {
                Debug.LogError(
                    "[BATTLEFIELD LAYOUT] NO RECTTRANSFORM: " +
                    objectName);
                return;
            }

            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = position;
            rt.sizeDelta = size;

            Debug.Log(
                "[BATTLEFIELD LAYOUT] " +
                objectName +
                " → POS=" +
                position +
                " SIZE=" +
                size);
            
            return;
        }

        Debug.LogError(
            "[BATTLEFIELD LAYOUT] OBJECT NOT FOUND: " +
            objectName);
    }
}

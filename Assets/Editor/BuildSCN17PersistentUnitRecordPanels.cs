using System;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ObsidianProtocol.Game.PersistentMilitary;

public static class BuildSCN17PersistentUnitRecordPanels
{
    private const string ScenePath =
        "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";
    private const string HudName = "PERSISTENT MILITARY HUD";
    private const string RecordName = "PERSISTENT UNIT RECORD";
    private const string StackName = "UNIT RECORD 8 PANEL STACK";

    private static readonly string[] PanelNames =
    {
        "UNIT HISTORY",
        "VETERAN UNITS",
        "DAMAGE HISTORY",
        "REPAIRS",
        "LOSSES",
        "EQUIPMENT HISTORY",
        "OPERATIONAL HISTORY",
        "MILITARY DEVELOPMENT"
    };

    [MenuItem("Obsidian Protocol/SCN17/BUILD EXACT 8 UNIT RECORD PANELS")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            Debug.LogError("[SCN17 RECORD] Open exactly:\n" + ScenePath);
            return;
        }

        GameObject hud = GameObject.Find(HudName);
        if (hud == null)
        {
            Debug.LogError("[SCN17 RECORD] HUD not found: " + HudName);
            return;
        }

        Transform record = FindDeepChild(hud.transform, RecordName);

        if (record == null)
        {
            GameObject recordObject = new GameObject(
                RecordName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

            record = recordObject.transform;
            record.SetParent(hud.transform, false);

            RectTransform recordRect = recordObject.GetComponent<RectTransform>();
            recordRect.anchorMin = new Vector2(0.5f, 0.5f);
            recordRect.anchorMax = new Vector2(0.5f, 0.5f);
            recordRect.pivot = new Vector2(0.5f, 0.5f);
            recordRect.sizeDelta = new Vector2(820f, 620f);
            recordRect.anchoredPosition = Vector2.zero;
            recordRect.localScale = Vector3.one;

            Image recordImage = recordObject.GetComponent<Image>();
            recordImage.color = new Color(0.008f, 0.014f, 0.022f, 0.985f);
            recordImage.raycastTarget = false;

            Debug.Log("[SCN17 RECORD] Created missing record container.");
        }

        Transform oldStack = record.Find(StackName);
        if (oldStack != null)
            UnityEngine.Object.DestroyImmediate(oldStack.gameObject);

        GameObject stackObject = new GameObject(StackName, typeof(RectTransform));
        stackObject.transform.SetParent(record, false);

        RectTransform stackRect = stackObject.GetComponent<RectTransform>();
        stackRect.anchorMin = Vector2.zero;
        stackRect.anchorMax = Vector2.one;
        stackRect.offsetMin = Vector2.zero;
        stackRect.offsetMax = Vector2.zero;
        stackRect.localScale = Vector3.one;

        for (int i = 0; i < PanelNames.Length; i++)
            CreatePanel(stackObject.transform, PanelNames[i], i == 0);

        SCN17PersistentUnitRecordPanels controller =
            hud.GetComponent<SCN17PersistentUnitRecordPanels>();

        if (controller == null)
            controller = hud.AddComponent<SCN17PersistentUnitRecordPanels>();

        EditorUtility.SetDirty(hud);
        EditorUtility.SetDirty(record.gameObject);
        EditorUtility.SetDirty(stackObject);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[SCN17 RECORD] BUILD COMPLETE. Eight panels created.");
    }

    private static void CreatePanel(Transform parent, string panelName, bool active)
    {
        GameObject panel = new GameObject(
            "UNIT RECORD PANEL - " + panelName,
            typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

        panel.transform.SetParent(parent, false);
        panel.SetActive(active);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;

        Image background = panel.GetComponent<Image>();
        background.color = new Color(0.008f, 0.014f, 0.022f, 0.985f);
        background.raycastTarget = false;

        CreateText(panel.transform, "TITLE", panelName, 34f,
            new Vector2(0f, 0.82f), new Vector2(1f, 0.98f),
            TextAlignmentOptions.Center);

        CreateText(panel.transform, "BODY", "", 22f,
            new Vector2(0.07f, 0.08f), new Vector2(0.93f, 0.78f),
            TextAlignmentOptions.TopLeft);
    }

    private static void CreateText(
        Transform parent, string objectName, string text, float fontSize,
        Vector2 anchorMin, Vector2 anchorMax, TextAlignmentOptions alignment)
    {
        GameObject go = new GameObject(
            objectName, typeof(RectTransform), typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));

        go.transform.SetParent(parent, false);

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        TMP_Text tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.alignment = alignment;
        tmp.raycastTarget = false;
        tmp.textWrappingMode = TextWrappingModes.Normal;
    }

    private static Transform FindDeepChild(Transform root, string targetName)
    {
        if (root.name == targetName)
            return root;

        for (int i = 0; i < root.childCount; i++)
        {
            Transform result = FindDeepChild(root.GetChild(i), targetName);
            if (result != null)
                return result;
        }

        return null;
    }
}

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ObsidianProtocol.Game.Deployment;

public static class BuildBattlefieldReinforcementSelector
{
    private const string ScenePath =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";

    [MenuItem("Obsidian Protocol/Battlefield/BUILD REINFORCEMENT SELECTOR")]
    public static void Build()
    {
        if (!File.Exists(ScenePath))
        {
            Debug.LogError("Battlefield scene not found: " + ScenePath);
            return;
        }

        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        GameObject overlay = GameObject.Find("BattleHUD_Overlay");
        GameObject deploymentObject = GameObject.Find("[SYSTEM] BATTLEFIELD DEPLOYMENT");

        if (overlay == null || deploymentObject == null)
        {
            Debug.LogError("Required battlefield UI or deployment system object was not found.");
            return;
        }

        if (deploymentObject.GetComponent<BattlefieldReinforcementService>() == null)
        {
            Debug.LogError("BattlefieldReinforcementService is missing.");
            return;
        }

        if (deploymentObject.GetComponent<BattlefieldReinforcementSelector>() != null ||
            overlay.transform.Find("REINFORCEMENT SELECTOR") != null)
        {
            Debug.LogWarning("Reinforcement selector already exists. No changes made.");
            return;
        }

        string recoveryFolder = "Assets/_Recovery/SceneBackups";
        Directory.CreateDirectory(recoveryFolder);

        string backupPath = Path.Combine(
            recoveryFolder,
            "SCN03_BATTLEFIELD_AAA_before_reinforcement_selector_" +
            System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".unity");

        File.Copy(ScenePath, backupPath, true);

        GameObject entry = CreateUIObject(
            "CALL IN", overlay.transform, new Vector2(0f, 1f),
            new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(105f, -82f), new Vector2(190f, 48f),
            new Color(0.05f, 0.24f, 0.32f, 0.96f));

        Button openButton = entry.AddComponent<Button>();
        openButton.targetGraphic = entry.GetComponent<Image>();
        AddButtonText(entry.transform, "CALL IN", 18);

        GameObject panel = CreateUIObject(
            "REINFORCEMENT SELECTOR", overlay.transform,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(720f, 490f),
            new Color(0.025f, 0.055f, 0.075f, 0.98f));

        Text title = CreateText(
            "TITLE", panel.transform, "REINFORCEMENT DEPLOYMENT",
            24, TextAnchor.MiddleLeft);

        SetRect(title.rectTransform,
            new Vector2(0f, 1f), new Vector2(1f, 1f),
            new Vector2(0.5f, 1f), new Vector2(24f, -12f),
            new Vector2(-90f, 48f));

        Text points = CreateText(
            "AVAILABLE POINTS", panel.transform,
            "REINFORCEMENT POINTS", 16, TextAnchor.MiddleLeft);

        SetRect(points.rectTransform,
            new Vector2(0f, 1f), new Vector2(1f, 1f),
            new Vector2(0.5f, 1f), new Vector2(24f, -65f),
            new Vector2(-48f, 32f));

        Text status = CreateText(
            "STATUS", panel.transform,
            "INITIALIZING DATABASE...", 14, TextAnchor.MiddleLeft);

        SetRect(status.rectTransform,
            new Vector2(0f, 1f), new Vector2(1f, 1f),
            new Vector2(0.5f, 1f), new Vector2(24f, -100f),
            new Vector2(-48f, 28f));

        GameObject closeObject = CreateUIObject(
            "CLOSE", panel.transform,
            new Vector2(1f, 1f), new Vector2(1f, 1f),
            new Vector2(1f, 1f), new Vector2(-42f, -36f),
            new Vector2(52f, 42f),
            new Color(0.35f, 0.09f, 0.09f, 1f));

        Button closeButton = closeObject.AddComponent<Button>();
        closeButton.targetGraphic = closeObject.GetComponent<Image>();
        AddButtonText(closeObject.transform, "X", 20);

        GameObject scrollObject = CreateUIObject(
            "UNIT LIST", panel.transform,
            new Vector2(0f, 0f), new Vector2(1f, 1f),
            new Vector2(0.5f, 0.5f), new Vector2(0f, -12f),
            new Vector2(-40f, -160f),
            new Color(0.015f, 0.03f, 0.045f, 0.9f));

        ScrollRect scroll = scrollObject.AddComponent<ScrollRect>();
        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.movementType = ScrollRect.MovementType.Clamped;

        GameObject viewport = CreateUIObject(
            "VIEWPORT", scrollObject.transform,
            Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero, Color.white);

        viewport.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.015f);
        viewport.AddComponent<Mask>().showMaskGraphic = false;

        GameObject content = CreateUIObject(
            "CONTENT", viewport.transform,
            new Vector2(0f, 1f), new Vector2(1f, 1f),
            new Vector2(0.5f, 1f), Vector2.zero,
            new Vector2(0f, 0f), Color.clear);

        VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(6, 6, 6, 6);
        layout.spacing = 7;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        scroll.viewport = viewport.GetComponent<RectTransform>();
        scroll.content = content.GetComponent<RectTransform>();

        BattlefieldReinforcementSelector selector =
            deploymentObject.AddComponent<BattlefieldReinforcementSelector>();

        SerializedObject serializedSelector = new SerializedObject(selector);
        serializedSelector.FindProperty("panel").objectReferenceValue = panel;
        serializedSelector.FindProperty("openButton").objectReferenceValue = openButton;
        serializedSelector.FindProperty("closeButton").objectReferenceValue = closeButton;
        serializedSelector.FindProperty("contentRoot").objectReferenceValue = content.transform;
        serializedSelector.FindProperty("pointsLabel").objectReferenceValue = points;
        serializedSelector.FindProperty("statusLabel").objectReferenceValue = status;
        serializedSelector.ApplyModifiedPropertiesWithoutUndo();

        panel.SetActive(false);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[BATTLEFIELD REINFORCEMENT] SELECTOR UI BUILT. Existing Reinforce intent button preserved. Backup: " + backupPath);
    }

    private static GameObject CreateUIObject(
        string name, Transform parent,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
        Vector2 position, Vector2 size, Color color)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        SetRect(rect, anchorMin, anchorMax, pivot, position, size);

        Image image = obj.GetComponent<Image>();
        image.color = color;

        return obj;
    }

    private static void SetRect(
        RectTransform rect, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 pivot, Vector2 position, Vector2 size)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    private static Text CreateText(
        string name, Transform parent, string value,
        int fontSize, TextAnchor alignment)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        obj.transform.SetParent(parent, false);

        Text text = obj.GetComponent<Text>();
        text.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
        text.fontSize = fontSize;
        text.color = new Color(0.72f, 0.91f, 0.96f, 1f);
        text.alignment = alignment;
        text.text = value;
        text.raycastTarget = false;

        return text;
    }

    private static void AddButtonText(Transform parent, string value, int fontSize)
    {
        Text text = CreateText("LABEL", parent, value, fontSize, TextAnchor.MiddleCenter);
        RectTransform rect = text.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}

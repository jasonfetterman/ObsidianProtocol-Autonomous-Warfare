using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Command;

public static class BuildBattlefieldCommandIntentControls
{
    private const string ScenePath =
        "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

    [MenuItem("Obsidian Protocol/Battlefield/BUILD COMMAND INTENT CONTROLS")]
    public static void Build()
    {
        EditorSceneManager.OpenScene(ScenePath);

        GameObject hud =
            GameObject.Find("[HUD] BATTLEFIELD HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[COMMAND INTENT] BATTLEFIELD HUD NOT FOUND.");
            return;
        }

        Transform window =
            FindChildRecursive(
                hud.transform,
                "[WINDOW] COMMAND INTENT");

        if (window == null)
        {
            Debug.LogError(
                "[COMMAND INTENT] WINDOW NOT FOUND.");
            return;
        }

        CommandIntentManager intentManager =
            Object.FindFirstObjectByType<CommandIntentManager>();

        if (intentManager == null)
        {
            Debug.LogError(
                "[COMMAND INTENT] COMMAND INTENT MANAGER NOT FOUND.");
            return;
        }

        BattlefieldHUDController hudController =
            hud.GetComponent<BattlefieldHUDController>();

        RectTransform windowRect =
            window.GetComponent<RectTransform>();

        if (windowRect == null)
        {
            Debug.LogError(
                "[COMMAND INTENT] WINDOW HAS NO RECT TRANSFORM.");
            return;
        }

        windowRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        windowRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        windowRect.pivot =
            new Vector2(0.5f, 0.5f);

        windowRect.anchoredPosition =
            Vector2.zero;

        windowRect.sizeDelta =
            new Vector2(680f, 620f);

        Image windowImage =
            window.GetComponent<Image>();

        if (windowImage != null)
        {
            windowImage.color =
                new Color(
                    0.015f,
                    0.025f,
                    0.035f,
                    0.97f);
        }

        RemoveOldControls(window);

        Transform title =
            FindChildRecursive(
                window,
                "COMMAND INTENT");

        if (title != null)
        {
            Text titleText =
                title.GetComponent<Text>();

            RectTransform titleRect =
                title.GetComponent<RectTransform>();

            if (titleText != null)
            {
                titleText.font =
                    GetRuntimeFont();

                titleText.fontSize = 26;
                titleText.fontStyle =
                    FontStyle.Bold;

                titleText.color =
                    Color.white;

                titleText.alignment =
                    TextAnchor.MiddleCenter;
            }

            if (titleRect != null)
            {
                titleRect.anchorMin =
                    new Vector2(0.5f, 1f);

                titleRect.anchorMax =
                    new Vector2(0.5f, 1f);

                titleRect.pivot =
                    new Vector2(0.5f, 1f);

                titleRect.anchoredPosition =
                    new Vector2(0f, -24f);

                titleRect.sizeDelta =
                    new Vector2(600f, 44f);
            }
        }

        Transform subtitle =
            FindChildRecursive(
                window,
                "DEFINE OBJECTIVE / PRIORITY / ROE / AUTONOMY");

        if (subtitle != null)
        {
            Text subtitleText =
                subtitle.GetComponent<Text>();

            RectTransform subtitleRect =
                subtitle.GetComponent<RectTransform>();

            if (subtitleText != null)
            {
                subtitleText.font =
                    GetRuntimeFont();

                subtitleText.fontSize = 15;
                subtitleText.fontStyle =
                    FontStyle.Normal;

                subtitleText.color =
                    new Color(
                        0.72f,
                        0.78f,
                        0.84f,
                        1f);

                subtitleText.alignment =
                    TextAnchor.MiddleCenter;

                subtitleText.horizontalOverflow =
                    HorizontalWrapMode.Wrap;

                subtitleText.verticalOverflow =
                    VerticalWrapMode.Overflow;
            }

            if (subtitleRect != null)
            {
                subtitleRect.anchorMin =
                    new Vector2(0.5f, 1f);

                subtitleRect.anchorMax =
                    new Vector2(0.5f, 1f);

                subtitleRect.pivot =
                    new Vector2(0.5f, 1f);

                subtitleRect.anchoredPosition =
                    new Vector2(0f, -70f);

                subtitleRect.sizeDelta =
                    new Vector2(580f, 48f);
            }
        }

        GameObject controls =
            new GameObject(
                "[COMMAND CONTROLS]",
                typeof(RectTransform),
                typeof(GridLayoutGroup));

        controls.transform.SetParent(
            window,
            false);

        RectTransform controlsRect =
            controls.GetComponent<RectTransform>();

        controlsRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        controlsRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        controlsRect.pivot =
            new Vector2(0.5f, 0.5f);

        controlsRect.anchoredPosition =
            new Vector2(0f, -35f);

        controlsRect.sizeDelta =
            new Vector2(570f, 455f);

        GridLayoutGroup grid =
            controls.GetComponent<GridLayoutGroup>();

        grid.cellSize =
            new Vector2(270f, 54f);

        grid.spacing =
            new Vector2(14f, 12f);

        grid.padding =
            new RectOffset(4, 4, 4, 4);

        grid.startCorner =
            GridLayoutGroup.Corner.UpperLeft;

        grid.startAxis =
            GridLayoutGroup.Axis.Horizontal;

        grid.childAlignment =
            TextAnchor.UpperCenter;

        grid.constraint =
            GridLayoutGroup.Constraint.FixedColumnCount;

        grid.constraintCount = 2;

        IntentType[] intents =
        {
            IntentType.Move,
            IntentType.Attack,
            IntentType.Defend,
            IntentType.Recon,
            IntentType.Flank,
            IntentType.Suppress,
            IntentType.Breach,
            IntentType.Pursue,
            IntentType.Retreat,
            IntentType.Reinforce
        };

        foreach (IntentType intent in intents)
        {
            CreateIntentButton(
                controls.transform,
                intent,
                intentManager);
        }

        GameObject close =
            CreateCloseButton(
                controls.transform,
                hudController);

        close.GetComponent<LayoutElement>()
            .preferredHeight = 54f;

        RectTransform closeRect =
            close.GetComponent<RectTransform>();

        closeRect.sizeDelta =
            new Vector2(270f, 54f);

        window.gameObject.SetActive(false);

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "[COMMAND INTENT] COMMAND WINDOW REBUILT.");

        Debug.Log(
            "[COMMAND INTENT] WINDOW SIZE = 680 x 620.");

        Debug.Log(
            "[COMMAND INTENT] GRID = 2 COLUMNS.");

        Debug.Log(
            "[COMMAND INTENT] CONTROLS = 10 INTENTS + CLOSE.");

        Debug.Log(
            "[COMMAND INTENT] WINDOW START STATE = INACTIVE.");
    }

    private static void RemoveOldControls(
        Transform window)
    {
        Transform oldControls =
            FindChildRecursive(
                window,
                "[COMMAND CONTROLS]");

        if (oldControls != null)
        {
            Object.DestroyImmediate(
                oldControls.gameObject);
        }
    }

    private static void CreateIntentButton(
        Transform parent,
        IntentType intent,
        CommandIntentManager manager)
    {
        GameObject buttonObject =
            new GameObject(
                "[BUTTON] INTENT " +
                intent.ToString().ToUpperInvariant(),
                typeof(RectTransform),
                typeof(Image),
                typeof(Button),
                typeof(LayoutElement),
                typeof(IntentButton));

        buttonObject.transform.SetParent(
            parent,
            false);

        Image image =
            buttonObject.GetComponent<Image>();

        image.color =
            new Color(
                0.06f,
                0.10f,
                0.14f,
                1f);

        LayoutElement layoutElement =
            buttonObject.GetComponent<LayoutElement>();

        layoutElement.preferredHeight =
            54f;

        layoutElement.minHeight =
            54f;

        Button button =
            buttonObject.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.06f,
                0.10f,
                0.14f,
                1f);

        colors.highlightedColor =
            new Color(
                0.12f,
                0.20f,
                0.27f,
                1f);

        colors.pressedColor =
            new Color(
                0.18f,
                0.28f,
                0.36f,
                1f);

        colors.selectedColor =
            colors.highlightedColor;

        colors.disabledColor =
            new Color(
                0.04f,
                0.05f,
                0.06f,
                1f);

        colors.colorMultiplier = 1f;
        colors.fadeDuration = 0.08f;

        button.colors = colors;

        IntentButton intentButton =
            buttonObject.GetComponent<IntentButton>();

        SerializedObject serializedIntentButton =
            new SerializedObject(intentButton);

        serializedIntentButton
            .FindProperty("intentType")
            .enumValueIndex =
                (int)intent;

        serializedIntentButton
            .FindProperty("intentManager")
            .objectReferenceValue =
                manager;

        serializedIntentButton
            .ApplyModifiedPropertiesWithoutUndo();

        GameObject labelObject =
            new GameObject(
                "[LABEL] " +
                intent.ToString().ToUpperInvariant(),
                typeof(RectTransform),
                typeof(Text));

        labelObject.transform.SetParent(
            buttonObject.transform,
            false);

        RectTransform labelRect =
            labelObject.GetComponent<RectTransform>();

        labelRect.anchorMin =
            Vector2.zero;

        labelRect.anchorMax =
            Vector2.one;

        labelRect.offsetMin =
            new Vector2(8f, 2f);

        labelRect.offsetMax =
            new Vector2(-8f, -2f);

        Text label =
            labelObject.GetComponent<Text>();

        label.font =
            GetRuntimeFont();

        label.text =
            intent.ToString().ToUpperInvariant();

        label.alignment =
            TextAnchor.MiddleCenter;

        label.fontSize =
            18;

        label.fontStyle =
            FontStyle.Bold;

        label.color =
            Color.white;

        label.raycastTarget =
            false;

        button.targetGraphic =
            image;
    }

    private static GameObject CreateCloseButton(
        Transform parent,
        BattlefieldHUDController controller)
    {
        GameObject buttonObject =
            new GameObject(
                "[BUTTON] CLOSE COMMAND INTENT",
                typeof(RectTransform),
                typeof(Image),
                typeof(Button),
                typeof(LayoutElement));

        buttonObject.transform.SetParent(
            parent,
            false);

        Image image =
            buttonObject.GetComponent<Image>();

        image.color =
            new Color(
                0.12f,
                0.07f,
                0.07f,
                1f);

        LayoutElement layoutElement =
            buttonObject.GetComponent<LayoutElement>();

        layoutElement.preferredHeight =
            54f;

        layoutElement.minHeight =
            54f;

        Button button =
            buttonObject.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.12f,
                0.07f,
                0.07f,
                1f);

        colors.highlightedColor =
            new Color(
                0.24f,
                0.12f,
                0.12f,
                1f);

        colors.pressedColor =
            new Color(
                0.32f,
                0.16f,
                0.16f,
                1f);

        button.colors = colors;

        GameObject labelObject =
            new GameObject(
                "[LABEL] CLOSE",
                typeof(RectTransform),
                typeof(Text));

        labelObject.transform.SetParent(
            buttonObject.transform,
            false);

        RectTransform labelRect =
            labelObject.GetComponent<RectTransform>();

        labelRect.anchorMin =
            Vector2.zero;

        labelRect.anchorMax =
            Vector2.one;

        labelRect.offsetMin =
            new Vector2(8f, 2f);

        labelRect.offsetMax =
            new Vector2(-8f, -2f);

        Text label =
            labelObject.GetComponent<Text>();

        label.font =
            GetRuntimeFont();

        label.text =
            "CLOSE";

        label.alignment =
            TextAnchor.MiddleCenter;

        label.fontSize =
            18;

        label.fontStyle =
            FontStyle.Bold;

        label.color =
            Color.white;

        label.raycastTarget =
            false;

        button.targetGraphic =
            image;

        if (controller != null)
        {
            button.onClick.RemoveListener(
                controller.CloseCommandIntent);

            button.onClick.AddListener(
                controller.CloseCommandIntent);
        }

        return buttonObject;
    }

    private static Font GetRuntimeFont()
    {
        return Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf");
    }

    private static Transform FindChildRecursive(
        Transform parent,
        string objectName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == objectName)
                return child;

            Transform result =
                FindChildRecursive(
                    child,
                    objectName);

            if (result != null)
                return result;
        }

        return null;
    }
}

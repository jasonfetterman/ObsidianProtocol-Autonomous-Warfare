using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace MiniRTS
{
    /// <summary>
    /// Runtime-built controls reference with an introductory auto-dismiss window.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class HelpOverlayUI : MonoBehaviour
    {
        private const float IntroDuration = 10f;
        private const float FadeDuration = 0.75f;
        private const int CanvasSortingOrder = 90;

        private static readonly HelpSection[] LeftSections =
        {
            new HelpSection(
                "SELECTION",
                new HelpEntry("Left-click", "Select a unit or building"),
                new HelpEntry("Left-drag", "Box-select units"),
                new HelpEntry("Shift + left-click", "Add or remove one unit"),
                new HelpEntry("Shift + left-drag", "Add units to selection")),
            new HelpSection(
                "CAMERA",
                new HelpEntry("WASD / Arrow keys / edge", "Pan camera"),
                new HelpEntry("Mouse wheel", "Zoom"),
                new HelpEntry("Minimap left-click / drag", "Focus camera")),
            new HelpSection(
                "ORDERS",
                new HelpEntry("Right-click ground", "Move / set production rally"),
                new HelpEntry("Right-click resource", "Gather with workers"),
                new HelpEntry("Right-click enemy", "Attack"),
                new HelpEntry("A, then left-click", "Attack-move"),
                new HelpEntry("Right-click while targeting", "Cancel attack-move"),
                new HelpEntry("H", "Stop / hold position"))
        };

        private static readonly HelpSection[] RightSections =
        {
            new HelpSection(
                "BUILDING — WORKER SELECTED",
                new HelpEntry("D", "Build Supply Depot"),
                new HelpEntry("B", "Build Barracks"),
                new HelpEntry("F", "Build Factory"),
                new HelpEntry("R", "Build Refinery"),
                new HelpEntry("Left-click", "Place building"),
                new HelpEntry("Right-click", "Cancel placement")),
            new HelpSection(
                "PRODUCTION",
                new HelpEntry("HQ: S", "Train Worker"),
                new HelpEntry("Barracks: M", "Train Marine"),
                new HelpEntry("Factory: T", "Train Tank")),
            new HelpSection(
                "CONTROL GROUPS",
                new HelpEntry("Ctrl + 1–5", "Assign selected units / building"),
                new HelpEntry("1–5", "Recall group")),
            new HelpSection(
                "OTHER",
                new HelpEntry("F1 / ?", "Toggle this help"),
                new HelpEntry("Cmd + Q", "Quit (macOS)"))
        };

        private GameObject panelRoot;
        private CanvasGroup panelCanvasGroup;
        private bool showingIntro;
        private bool fading;
        private float autoHideTime;
        private float fadeStartTime;
        private float fadeStartAlpha;
        private int acceptDismissInputAfterFrame;

        public bool IsVisible =>
            panelRoot != null && panelRoot.activeSelf;

        public void Initialize()
        {
            EnsureEventSystem();
            BuildUi();
            Show(true);
        }

        public void Toggle()
        {
            if (IsVisible)
            {
                HideImmediately();
            }
            else
            {
                Show(false);
            }
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null &&
                (keyboard.f1Key.wasPressedThisFrame ||
                 keyboard.slashKey.wasPressedThisFrame))
            {
                Toggle();
                return;
            }

            if (!IsVisible)
            {
                return;
            }

            if (showingIntro &&
                Time.frameCount > acceptDismissInputAfterFrame &&
                AnyDismissInputPressed(keyboard, Mouse.current))
            {
                BeginFade();
            }

            if (showingIntro &&
                !fading &&
                Time.unscaledTime >= autoHideTime)
            {
                BeginFade();
            }

            if (!fading)
            {
                return;
            }

            float progress = FadeDuration > 0f
                ? (Time.unscaledTime - fadeStartTime) / FadeDuration
                : 1f;
            if (progress >= 1f)
            {
                HideImmediately();
                return;
            }

            panelCanvasGroup.alpha =
                Mathf.Lerp(fadeStartAlpha, 0f, Mathf.Clamp01(progress));
        }

        private void Show(bool introductory)
        {
            panelRoot.SetActive(true);
            panelCanvasGroup.alpha = 1f;
            panelCanvasGroup.blocksRaycasts = true;
            panelCanvasGroup.interactable = true;
            showingIntro = introductory;
            fading = false;
            acceptDismissInputAfterFrame = Time.frameCount + 1;
            autoHideTime = Time.unscaledTime + IntroDuration;
        }

        private void BeginFade()
        {
            if (fading)
            {
                return;
            }

            showingIntro = false;
            fading = true;
            fadeStartTime = Time.unscaledTime;
            fadeStartAlpha = panelCanvasGroup.alpha;
        }

        private void HideImmediately()
        {
            showingIntro = false;
            fading = false;
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.blocksRaycasts = false;
            panelCanvasGroup.interactable = false;
            panelRoot.SetActive(false);
        }

        private void BuildUi()
        {
            GameObject canvasObject = new GameObject(
                "HelpCanvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = CanvasSortingOrder;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            panelRoot = new GameObject(
                "ControlsPanel",
                typeof(RectTransform),
                typeof(Image),
                typeof(CanvasGroup));
            panelRoot.transform.SetParent(canvasObject.transform, false);
            RectTransform panel = panelRoot.GetComponent<RectTransform>();
            panel.anchorMin = new Vector2(0.5f, 0.5f);
            panel.anchorMax = new Vector2(0.5f, 0.5f);
            panel.pivot = new Vector2(0.5f, 0.5f);
            panel.anchoredPosition = new Vector2(0f, 8f);
            panel.sizeDelta = new Vector2(1100f, 810f);

            Image panelImage = panelRoot.GetComponent<Image>();
            panelImage.color = new Color(0.018f, 0.032f, 0.052f, 0.96f);
            panelImage.raycastTarget = true;
            panelCanvasGroup = panelRoot.GetComponent<CanvasGroup>();

            GameObject accent = CreateImage(
                "TopAccent",
                panelRoot.transform,
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(0.5f, 1f),
                Vector2.zero,
                new Vector2(0f, 4f),
                new Color(0.12f, 0.72f, 1f, 0.95f));
            accent.GetComponent<Image>().raycastTarget = false;

            Text title = CreateText(
                "Title",
                panelRoot.transform,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -22f),
                new Vector2(760f, 46f),
                30,
                TextAnchor.MiddleCenter,
                Color.white);
            title.text = "CONTROLS";
            title.fontStyle = FontStyle.Bold;

            Text subtitle = CreateText(
                "Subtitle",
                panelRoot.transform,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -67f),
                new Vector2(900f, 30f),
                17,
                TextAnchor.MiddleCenter,
                new Color(0.65f, 0.76f, 0.86f));
            subtitle.text = "The battle continues while this reference is open";

            CreateColumn(
                panelRoot.transform,
                "LeftColumn",
                new Vector2(34f, -112f),
                500f,
                LeftSections);
            CreateColumn(
                panelRoot.transform,
                "RightColumn",
                new Vector2(566f, -112f),
                500f,
                RightSections);

            CreateImage(
                "ColumnDivider",
                panelRoot.transform,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -112f),
                new Vector2(1f, 650f),
                new Color(0.22f, 0.34f, 0.46f, 0.7f))
                .GetComponent<Image>().raycastTarget = false;

            Text hint = CreateText(
                "HelpHint",
                canvasObject.transform,
                new Vector2(1f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 1f),
                new Vector2(-18f, -16f),
                new Vector2(190f, 32f),
                18,
                TextAnchor.UpperRight,
                new Color(0.74f, 0.86f, 0.96f));
            hint.text = "F1 — Help";
            hint.fontStyle = FontStyle.Bold;

            CreateHelpButton(canvasObject.transform);
        }

        private void CreateHelpButton(Transform parent)
        {
            GameObject buttonObject = new GameObject(
                "HelpButton",
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = new Vector2(-470f, 18f);
            rect.sizeDelta = new Vector2(48f, 48f);

            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.08f, 0.16f, 0.25f, 0.98f);
            Button button = buttonObject.GetComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(1.16f, 1.16f, 1.16f);
            colors.pressedColor = new Color(0.65f, 0.78f, 0.9f);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;
            button.navigation = new Navigation
            {
                mode = Navigation.Mode.None
            };
            button.onClick.AddListener(Toggle);

            Text label = CreateText(
                "Label",
                buttonObject.transform,
                Vector2.zero,
                Vector2.one,
                new Vector2(0.5f, 0.5f),
                Vector2.zero,
                Vector2.zero,
                25,
                TextAnchor.MiddleCenter,
                Color.white);
            label.text = "?";
            label.fontStyle = FontStyle.Bold;
        }

        private static void CreateColumn(
            Transform parent,
            string objectName,
            Vector2 anchoredPosition,
            float width,
            HelpSection[] sections)
        {
            GameObject columnObject = new GameObject(
                objectName,
                typeof(RectTransform));
            columnObject.transform.SetParent(parent, false);
            RectTransform column = columnObject.GetComponent<RectTransform>();
            column.anchorMin = new Vector2(0f, 1f);
            column.anchorMax = new Vector2(0f, 1f);
            column.pivot = new Vector2(0f, 1f);
            column.anchoredPosition = anchoredPosition;
            column.sizeDelta = new Vector2(width, 650f);

            float y = 0f;
            for (int sectionIndex = 0;
                 sectionIndex < sections.Length;
                 sectionIndex++)
            {
                HelpSection section = sections[sectionIndex];
                Text header = CreateTopLeftText(
                    $"{section.Title}Header",
                    column,
                    new Vector2(0f, y),
                    new Vector2(width, 29f),
                    18,
                    new Color(0.16f, 0.75f, 1f));
                header.text = section.Title;
                header.fontStyle = FontStyle.Bold;
                y -= 34f;

                for (int entryIndex = 0;
                     entryIndex < section.Entries.Length;
                     entryIndex++)
                {
                    HelpEntry entry = section.Entries[entryIndex];
                    Text key = CreateTopLeftText(
                        $"{section.Title}Key{entryIndex}",
                        column,
                        new Vector2(0f, y),
                        new Vector2(210f, 27f),
                        17,
                        new Color(0.9f, 0.95f, 1f));
                    key.text = entry.Key;
                    key.fontStyle = FontStyle.Bold;

                    Text description = CreateTopLeftText(
                        $"{section.Title}Description{entryIndex}",
                        column,
                        new Vector2(214f, y),
                        new Vector2(width - 214f, 27f),
                        17,
                        new Color(0.78f, 0.84f, 0.9f));
                    description.text = entry.Description;
                    y -= 28f;
                }

                y -= 15f;
            }
        }

        private static Text CreateTopLeftText(
            string objectName,
            Transform parent,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            int fontSize,
            Color color)
        {
            return CreateText(
                objectName,
                parent,
                new Vector2(0f, 1f),
                new Vector2(0f, 1f),
                new Vector2(0f, 1f),
                anchoredPosition,
                sizeDelta,
                fontSize,
                TextAnchor.UpperLeft,
                color);
        }

        private static Text CreateText(
            string objectName,
            Transform parent,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 pivot,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            int fontSize,
            TextAnchor alignment,
            Color color)
        {
            GameObject textObject = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Text));
            textObject.transform.SetParent(parent, false);
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;

            Text text = textObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        private static GameObject CreateImage(
            string objectName,
            Transform parent,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 pivot,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Color color)
        {
            GameObject imageObject = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Image));
            imageObject.transform.SetParent(parent, false);
            RectTransform rect = imageObject.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;
            imageObject.GetComponent<Image>().color = color;
            return imageObject;
        }

        private static bool AnyDismissInputPressed(
            Keyboard keyboard,
            Mouse mouse)
        {
            if (keyboard != null && keyboard.anyKey.wasPressedThisFrame)
            {
                return true;
            }

            return mouse != null &&
                   (mouse.leftButton.wasPressedThisFrame ||
                    mouse.rightButton.wasPressedThisFrame ||
                    mouse.middleButton.wasPressedThisFrame ||
                    mouse.forwardButton.wasPressedThisFrame ||
                    mouse.backButton.wasPressedThisFrame);
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null)
            {
                return;
            }

            GameObject eventSystemObject = new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));
            eventSystemObject
                .GetComponent<InputSystemUIInputModule>()
                .AssignDefaultActions();
        }

        private readonly struct HelpEntry
        {
            public HelpEntry(string key, string description)
            {
                Key = key;
                Description = description;
            }

            public string Key { get; }
            public string Description { get; }
        }

        private readonly struct HelpSection
        {
            public HelpSection(string title, params HelpEntry[] entries)
            {
                Title = title;
                Entries = entries;
            }

            public string Title { get; }
            public HelpEntry[] Entries { get; }
        }
    }
}

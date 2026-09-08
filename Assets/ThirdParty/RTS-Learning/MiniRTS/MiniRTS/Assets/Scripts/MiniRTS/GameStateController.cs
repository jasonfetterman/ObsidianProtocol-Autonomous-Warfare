using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MiniRTS
{
    /// <summary>
    /// Watches faction building elimination and owns the paused end-game overlay.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GameStateController : MonoBehaviour
    {
        private const float CheckInterval = 0.25f;

        private GameObject overlayRoot;
        private Text resultText;
        private int playerOwnerId;
        private int enemyOwnerId;
        private float nextCheckTime;
        private bool initialized;

        public GameOutcome Outcome { get; private set; } =
            GameOutcome.InProgress;

        public void Initialize(
            int playerFactionOwnerId,
            int enemyFactionOwnerId)
        {
            if (playerFactionOwnerId == enemyFactionOwnerId)
            {
                throw new ArgumentException(
                    "Player and enemy owner IDs must be different.");
            }

            playerOwnerId = playerFactionOwnerId;
            enemyOwnerId = enemyFactionOwnerId;
            EnsureEventSystem();
            BuildOverlay();
            nextCheckTime = Time.unscaledTime;
            initialized = true;
        }

        public GameOutcome EvaluateNow()
        {
            if (!initialized || Outcome != GameOutcome.InProgress)
            {
                return Outcome;
            }

            int playerBuildings = 0;
            int enemyBuildings = 0;
            System.Collections.Generic.IReadOnlyList<Building> buildings =
                Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Building building = buildings[i];
                if (building == null || !building.IsAlive)
                {
                    continue;
                }

                if (building.OwnerId == playerOwnerId)
                {
                    playerBuildings++;
                }
                else if (building.OwnerId == enemyOwnerId)
                {
                    enemyBuildings++;
                }
            }

            GameOutcome outcome = GameOutcomeLogic.Determine(
                playerBuildings,
                enemyBuildings);
            if (outcome != GameOutcome.InProgress)
            {
                FinishGame(outcome);
            }

            return Outcome;
        }

        private void Update()
        {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null &&
                keyboard.qKey.wasPressedThisFrame &&
                (keyboard.leftCommandKey.isPressed ||
                 keyboard.rightCommandKey.isPressed))
            {
                QuitGame();
                return;
            }
#endif

            if (!initialized ||
                Outcome != GameOutcome.InProgress ||
                Time.unscaledTime < nextCheckTime)
            {
                return;
            }

            nextCheckTime = Time.unscaledTime + CheckInterval;
            EvaluateNow();
        }

        private void OnDestroy()
        {
            if (Outcome != GameOutcome.InProgress)
            {
                Time.timeScale = 1f;
            }
        }

        private void FinishGame(GameOutcome outcome)
        {
            Outcome = outcome;
            resultText.text =
                outcome == GameOutcome.Victory
                    ? "VICTORY"
                    : "DEFEAT";
            resultText.color =
                outcome == GameOutcome.Victory
                    ? new Color(0.28f, 1f, 0.42f)
                    : new Color(1f, 0.22f, 0.18f);
            overlayRoot.SetActive(true);
            Time.timeScale = 0f;
        }

        private void RestartGame()
        {
            Time.timeScale = 1f;
            FogOfWar.ResetStaticState();
            Unit.ResetStaticState();
            Building.ResetStaticState();
            ResourceNode.ResetStaticState();

            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.buildIndex >= 0)
            {
                SceneManager.LoadScene(activeScene.buildIndex);
            }
            else
            {
                SceneManager.LoadScene(activeScene.name);
            }
        }

        private static void QuitGame()
        {
            Time.timeScale = 1f;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void BuildOverlay()
        {
            overlayRoot = new GameObject(
                "GameOverCanvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            overlayRoot.transform.SetParent(transform, false);

            Canvas canvas = overlayRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            CanvasScaler scaler = overlayRoot.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            GameObject backdrop = new GameObject(
                "Backdrop",
                typeof(RectTransform),
                typeof(Image));
            backdrop.transform.SetParent(overlayRoot.transform, false);
            RectTransform backdropRect =
                backdrop.GetComponent<RectTransform>();
            backdropRect.anchorMin = Vector2.zero;
            backdropRect.anchorMax = Vector2.one;
            backdropRect.offsetMin = Vector2.zero;
            backdropRect.offsetMax = Vector2.zero;
            backdrop.GetComponent<Image>().color =
                new Color(0.012f, 0.018f, 0.03f, 0.93f);

            resultText = CreateText(
                "Result",
                backdrop.transform,
                new Vector2(0f, 110f),
                new Vector2(900f, 150f),
                76);

            Button restartButton = CreateButton(
                "RestartButton",
                backdrop.transform,
                new Vector2(0f, -40f),
                "RESTART");
            restartButton.onClick.AddListener(RestartGame);

            Button quitButton = CreateButton(
                "QuitButton",
                backdrop.transform,
                new Vector2(0f, -120f),
#if UNITY_EDITOR
                "STOP PLAY MODE");
#else
                "QUIT GAME");
#endif
            quitButton.onClick.AddListener(QuitGame);

            overlayRoot.SetActive(false);
        }

        private static Text CreateText(
            string objectName,
            Transform parent,
            Vector2 anchoredPosition,
            Vector2 size,
            int fontSize)
        {
            GameObject textObject = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Text));
            textObject.transform.SetParent(parent, false);
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            Text text = textObject.GetComponent<Text>();
            text.font =
                Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            text.raycastTarget = false;
            return text;
        }

        private static Button CreateButton(
            string objectName,
            Transform parent,
            Vector2 anchoredPosition,
            string label)
        {
            GameObject buttonObject = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            RectTransform rect =
                buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = new Vector2(310f, 58f);

            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.08f, 0.19f, 0.31f, 1f);
            Button button = buttonObject.GetComponent<Button>();
            ColorBlock colors = button.colors;
            colors.highlightedColor = new Color(0.14f, 0.36f, 0.57f, 1f);
            colors.pressedColor = new Color(0.05f, 0.12f, 0.2f, 1f);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;

            Text buttonText = CreateText(
                "Label",
                buttonObject.transform,
                Vector2.zero,
                rect.sizeDelta,
                25);
            buttonText.text = label;
            buttonText.color = Color.white;
            return button;
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
    }
}

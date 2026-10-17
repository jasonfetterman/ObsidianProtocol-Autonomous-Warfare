using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace ObsidianProtocol.Game.PersistentMilitary
{
    public class SCN17FullRecordBottomButton : MonoBehaviour
    {
        // IMPORTANT:
        // The SCN17 mouse bridge dispatches buttons by name.
        // Use the existing command name so the normal
        // VIEW FULL RECORD handler is executed.
        private const string ButtonName = "BUTTON - VIEW FULL RECORD";
        private const string BottomBarName = "BOTTOM COMMAND BAR";
        private const string CommandCenterButtonName = "BUTTON - COMMAND CENTER";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            string activeScene = SceneManager.GetActiveScene().name;
            if (activeScene != "Persistent_Military" && activeScene != "SCN17_PERSISTENT_MILITARY")
                return;

            CreateButton();
        }

        private static void CreateButton()
        {
            GameObject bottomBar =
                GameObject.Find(BottomBarName);

            if (bottomBar == null)
            {
                Debug.LogWarning(
                    "[SCN17] BOTTOM COMMAND BAR not found.");
                return;
            }

            // GameObject.Find does not return inactive objects.
            // The original VIEW FULL RECORD button is inside
            // the inactive RECORD DETAIL WINDOW, so this is safe.
            GameObject existing =
                GameObject.Find(ButtonName);

            if (existing != null)
            {
                // If an active button with this name already exists
                // outside the detail window, use it.
                if (existing.transform.IsChildOf(bottomBar.transform))
                {
                    WireOpenAction(existing);
                    PositionButton(existing);
                    existing.SetActive(true);
                    return;
                }
            }

            GameObject buttonObject =
                new GameObject(
                    ButtonName,
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Image),
                    typeof(Button));

            buttonObject.transform.SetParent(
                bottomBar.transform,
                false);

            RectTransform rect =
                buttonObject.GetComponent<RectTransform>();

            rect.sizeDelta =
                new Vector2(210f, 48f);

            Image image =
                buttonObject.GetComponent<Image>();

            image.color =
                new Color(
                    0.012f,
                    0.0864f,
                    0.108f,
                    0.98f);

            image.raycastTarget = true;

            Button button =
                buttonObject.GetComponent<Button>();

            button.interactable = true;
            WireOpenAction(buttonObject);

            GameObject labelObject =
                new GameObject(
                    "LABEL",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(TextMeshProUGUI));

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
                Vector2.zero;

            labelRect.offsetMax =
                Vector2.zero;

            TextMeshProUGUI label =
                labelObject.GetComponent<TextMeshProUGUI>();

            label.text =
                "OPEN FULL RECORD";

            label.alignment =
                TextAlignmentOptions.Center;

            label.fontSize =
                18f;

            label.color =
                Color.white;

            label.raycastTarget =
                false;

            PositionButton(buttonObject);

            Debug.Log(
                "[SCN17] Bottom VIEW FULL RECORD button created.");
        }

        private static void WireOpenAction(GameObject buttonObject)
        {
            Button button = buttonObject.GetComponent<Button>();
            if (button == null)
                return;

            button.onClick.RemoveListener(OpenRecordDetail);
            button.onClick.AddListener(OpenRecordDetail);
        }

        private static void OpenRecordDetail()
        {
            SCN17PersistentMilitaryUI controller =
                Object.FindFirstObjectByType<SCN17PersistentMilitaryUI>();

            if (controller != null)
            {
                controller.OpenRecordDetailWindow();
                return;
            }

            Debug.LogError("[SCN17] Cannot open Record Detail: UI controller not found.");
        }
        private static void PositionButton(
            GameObject buttonObject)
        {
            GameObject commandCenter =
                GameObject.Find(
                    CommandCenterButtonName);

            RectTransform buttonRect =
                buttonObject.GetComponent<RectTransform>();

            if (commandCenter == null)
            {
                buttonRect.anchorMin =
                    new Vector2(1f, 0.5f);

                buttonRect.anchorMax =
                    new Vector2(1f, 0.5f);

                buttonRect.pivot =
                    new Vector2(1f, 0.5f);

                buttonRect.anchoredPosition =
                    new Vector2(-20f, 0f);

                return;
            }

            RectTransform commandRect =
                commandCenter.GetComponent<RectTransform>();

            buttonRect.anchorMin =
                commandRect.anchorMin;

            buttonRect.anchorMax =
                commandRect.anchorMax;

            buttonRect.pivot =
                new Vector2(0.5f, 0.5f);

            buttonRect.anchoredPosition =
                commandRect.anchoredPosition +
                new Vector2(
                    commandRect.rect.width * 0.5f +
                    buttonRect.rect.width * 0.5f +
                    12f,
                    0f);
        }
    }
}


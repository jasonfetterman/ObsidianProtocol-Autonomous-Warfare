using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ObsidianProtocol.UI.Credits
{
    public sealed class CreditsSceneController : MonoBehaviour
    {
        [Header("Navigation")]
        [SerializeField] private string mainMenuScene = "SCN-01  MAIN MENU";

        [Header("Credits Scroll")]
        [SerializeField] private ScrollRect creditsScroll;
        [SerializeField] private float scrollSpeed = 0.025f;

        [Header("Back Button")]
        [SerializeField] private Button backButton;

        private void Awake()
        {
            if (creditsScroll == null)
            {
                GameObject scrollObject = GameObject.Find("CreditsScrollArea");
                if (scrollObject != null)
                    creditsScroll = scrollObject.GetComponent<ScrollRect>();
            }

            if (backButton == null)
            {
                GameObject backObject = GameObject.Find("BACK");
                if (backObject != null)
                    backButton = backObject.GetComponent<Button>();
            }

            if (backButton != null)
            {
                backButton.onClick.RemoveListener(ReturnToMainMenu);
                backButton.onClick.AddListener(ReturnToMainMenu);
            }
            else
            {
                Debug.LogError("[SCN-23] BACK button not found.");
            }

            if (creditsScroll == null)
                Debug.LogError("[SCN-23] CreditsScrollArea ScrollRect not found.");
        }

        private void Start()
        {
            if (creditsScroll != null)
                creditsScroll.verticalNormalizedPosition = 1f;
        }

        private void Update()
        {
            if (creditsScroll == null)
                return;

            creditsScroll.verticalNormalizedPosition = Mathf.Clamp01(
                creditsScroll.verticalNormalizedPosition -
                scrollSpeed * Time.deltaTime);
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}

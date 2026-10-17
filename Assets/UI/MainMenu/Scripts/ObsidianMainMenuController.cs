using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObsidianProtocol.UI
{
    public class ObsidianMainMenuController : MonoBehaviour
    {
        [Header("Resolved destination scenes")]
        [SerializeField] private string continueScene = "Command_Center";
        [SerializeField] private string campaignScene = "Command_Center";
        [SerializeField] private string multiplayerScene = "MultiplayerHUD";
        [SerializeField] private string garageScene = "SCN04_GARAGE";
        [SerializeField] private string storeScene = "SCN16_STORE";
        [SerializeField] private string vrScene = "SCN20_VR_COMMAND";
        [SerializeField] private string settingsScene = "Settings";

        [Header("Popups")]
        [SerializeField] private GameObject exitPopup;
        [SerializeField] private GameObject creditsPopup;

        private void Awake()
        {
            Debug.Log("[DIAGNOSTIC] ObsidianMainMenuController Awake");
            GameObject campaignButton =
                GameObject.Find("CAMPAIGN");

            if (campaignButton == null)
            {
                Debug.LogError(
                    "[Obsidian Protocol] BUTTON_Campaign not found."
                );
                return;
            }

            UnityEngine.UI.Button button =
                campaignButton.GetComponent<UnityEngine.UI.Button>();

            if (button == null)
            {
                Debug.LogError(
                    "[Obsidian Protocol] BUTTON_Campaign has no Button component."
                );
                return;
            }

            button.onClick.RemoveListener(Campaign);
            button.onClick.AddListener(Campaign);
        }

        public void Continue() => LoadScene(continueScene, "CONTINUE");
        public void Campaign()
{
    GameObject centerMenu =
        GameObject.Find("CENTER_COMMAND_MENU");

    if (centerMenu == null)
    {
        Debug.LogError(
            "[Obsidian Protocol] CAMPAIGN failed: PANEL_CenterMenu not found."
        );
        return;
    }

    Transform existing =
        centerMenu.transform.Find("Campaign_Setup_Panel");

    GameObject setupPanel;

    if (existing != null)
    {
        setupPanel = existing.gameObject;
    }
    else
    {
        setupPanel = new GameObject(
            "Campaign_Setup_Panel",
            typeof(RectTransform),
            typeof(UnityEngine.UI.Image)
        );

        UnityEngine.UI.Image panelBackground =
            setupPanel.GetComponent<UnityEngine.UI.Image>();

        panelBackground.color = Color.black;
        panelBackground.raycastTarget = true;

        setupPanel.transform.SetParent(
            centerMenu.transform,
            false
        );

        RectTransform rect =
            setupPanel.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    setupPanel.SetActive(true);

    ObsidianProtocol.UI.CampaignSetupController setup =
        setupPanel.GetComponent<
            ObsidianProtocol.UI.CampaignSetupController>();

    if (setup == null)
    {
        setup =
            setupPanel.AddComponent<
                ObsidianProtocol.UI.CampaignSetupController>();
    }

    setup.Build();

    Debug.Log(
        "[Obsidian Protocol] CAMPAIGN setup opened."
    );
}
        public void Multiplayer() => LoadScene(multiplayerScene, "MULTIPLAYER");
        public void Garage() => LoadScene(garageScene, "GARAGE");
        public void Store() => LoadScene(storeScene, "STORE");
        public void VROperator() => LoadScene(vrScene, "VR OPERATOR");
        public void Settings() => LoadScene(settingsScene, "SETTINGS");

        public void Credits()
        {
            if (creditsPopup != null)
                creditsPopup.SetActive(true);
            else
                Debug.Log("[Obsidian Protocol] CREDITS opened.");
        }

        public void CloseCredits()
        {
            if (creditsPopup != null)
                creditsPopup.SetActive(false);
        }

        public void Exit()
        {
            if (exitPopup != null)
                exitPopup.SetActive(true);
        }

        public void ConfirmExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void CancelExit()
        {
            if (exitPopup != null)
                exitPopup.SetActive(false);
        }

        private void LoadScene(string sceneName, string action)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("[Obsidian Protocol] " + action +
                               " has no destination.");
                return;
            }

            string resolved = ResolveScene(sceneName);

            if (string.IsNullOrEmpty(resolved))
            {
                Debug.LogError("[Obsidian Protocol] Could not resolve " +
                               action + " destination: " + sceneName);
                return;
            }

            Debug.Log("[Obsidian Protocol] " + action +
                      " -> " + resolved);

            SceneManager.LoadScene(resolved);
        }

        private string ResolveScene(string id)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);

                if (string.IsNullOrEmpty(path))
                    continue;

                string file = System.IO.Path.GetFileNameWithoutExtension(path);

                if (file.StartsWith(id, System.StringComparison.OrdinalIgnoreCase))
                    return file;
            }

            return null;
        }
    }
}









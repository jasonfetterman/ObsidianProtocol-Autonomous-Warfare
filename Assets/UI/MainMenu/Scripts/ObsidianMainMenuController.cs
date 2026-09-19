using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObsidianProtocol.UI
{
    public class ObsidianMainMenuController : MonoBehaviour
    {
        [Header("Resolved destination scenes")]
        [SerializeField] private string continueScene = "SCN-02";
        [SerializeField] private string campaignScene = "SCN-02";
        [SerializeField] private string multiplayerScene = "SCN-13";
        [SerializeField] private string garageScene = "SCN-04";
        [SerializeField] private string storeScene = "SCN-16";
        [SerializeField] private string vrScene = "SCN-20";
        [SerializeField] private string settingsScene = "SCN-14";

        [Header("Popups")]
        [SerializeField] private GameObject exitPopup;
        [SerializeField] private GameObject creditsPopup;

        public void Continue() => LoadScene(continueScene, "CONTINUE");
        public void Campaign() => LoadScene(campaignScene, "CAMPAIGN");
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

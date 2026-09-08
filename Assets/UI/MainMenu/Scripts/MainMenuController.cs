using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ObsidianProtocol.UI
{
    public sealed class MainMenuController : MonoBehaviour
    {
        [Header("Obsidian Protocol Main Menu")]
        public Canvas mainCanvas;
        public Button playButton;
        public Button settingsButton;
        public Button quitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(OnPlay);
            settingsButton.onClick.AddListener(OnSettings);
            quitButton.onClick.AddListener(OnQuit);
        }

        private void OnPlay() => SceneManager.LoadScene("Game");
        private void OnSettings() => SceneManager.LoadScene("Settings");
        private void OnQuit() => Application.Quit();
    }
}

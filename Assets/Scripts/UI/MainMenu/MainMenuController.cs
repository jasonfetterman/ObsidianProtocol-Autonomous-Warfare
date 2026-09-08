using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class MainMenuController : MonoBehaviour
{
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlay);
        settingsButton.onClick.AddListener(OnSettings);
        quitButton.onClick.AddListener(OnQuit);
    }

    private void OnPlay()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    private void OnQuit()
    {
        Application.Quit();
    }
}

using UnityEngine;
using ObsidianProtocol.Game.Core;

namespace ObsidianProtocol.Game.UI
{
    public sealed class MainMenuContinueAction : MonoBehaviour
    {
        public void ResumeGameplay()
        {
            Time.timeScale = 1f;

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.TryChangeState(GameState.Gameplay);
            }

            GameObject menu = GameObject.Find("Main Menu HUD");

            if (menu != null)
            {
                menu.SetActive(false);
            }
        }
    }
}
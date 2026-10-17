using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinanceCloseNavigation : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Handle the scene that is already loaded.
        TryInstall(SceneManager.GetActiveScene());
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryInstall(scene);
    }

    private static void TryInstall(Scene scene)
    {
        if (!scene.IsValid())
            return;

        if (!scene.name.Contains("Finance"))
            return;

        GameObject buttonObject = FindButtonClose(scene);

        if (buttonObject == null)
        {
            Debug.LogError(
                "[FINANCE] BUTTON_CLOSE exists in the Finance HUD hierarchy but could not be located at runtime."
            );
            return;
        }

        Button button = buttonObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError(
                "[FINANCE] BUTTON_CLOSE HAS NO BUTTON COMPONENT."
            );
            return;
        }

        button.onClick.RemoveListener(ReturnToMainMenu);
        button.onClick.AddListener(ReturnToMainMenu);

        Debug.Log(
            "[FINANCE] CLOSE navigation connected -> MAIN MENU (index 0)."
        );
    }

    private static GameObject FindButtonClose(Scene scene)
    {
        GameObject[] roots = scene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            Transform[] transforms =
                root.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in transforms)
            {
                if (t.name == "BUTTON_CLOSE")
                    return t.gameObject;
            }
        }

        return null;
    }

    private static void ReturnToMainMenu()
    {
        Debug.Log(
            "[FINANCE] CLOSE pressed -> loading MAIN MENU index 0."
        );

        SceneManager.LoadScene(0);
    }
}

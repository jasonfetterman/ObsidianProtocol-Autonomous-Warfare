using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerMatchSettingsOpenCloseFix : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!SceneContains("MATCH_SETTINGS_TERMINAL"))
            return;

        GameObject obj =
            new GameObject("MULTIPLAYER_MATCH_SETTINGS_OPEN_CLOSE_FIX");

        DontDestroyOnLoad(obj);
        obj.AddComponent<MultiplayerMatchSettingsOpenCloseFix>();
    }

    private IEnumerator Start()
    {
        yield return null;
        yield return null;

        WireMatchSettings();
    }

    private void WireMatchSettings()
    {
        MultiplayerLobbyRuntime runtime =
            Object.FindFirstObjectByType<MultiplayerLobbyRuntime>();

        if (runtime == null)
        {
            Debug.LogError("[MULTIPLAYER] Runtime not found.");
            return;
        }

        GameObject terminal =
            FindSceneObject("MATCH_SETTINGS_TERMINAL");

        if (terminal == null)
        {
            Debug.LogError(
                "[MULTIPLAYER] MATCH_SETTINGS_TERMINAL NOT FOUND."
            );
        }
        else
        {
            LobbyInteractable interactable =
                terminal.GetComponent<LobbyInteractable>();

            if (interactable == null)
                interactable = terminal.AddComponent<LobbyInteractable>();

            interactable.interactionType =
                LobbyInteractable.InteractionType.MatchSettings;

            interactable.displayName = "MATCH SETTINGS";

            Debug.Log(
                "[MULTIPLAYER] MATCH SETTINGS TERMINAL WIRED."
            );
        }

        Canvas canvas =
            Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("[MULTIPLAYER] CANVAS NOT FOUND.");
            return;
        }

        Transform panel =
            FindChild(canvas.transform, "MATCH_SETTINGS_PANEL");

        if (panel == null)
        {
            Debug.LogError(
                "[MULTIPLAYER] MATCH_SETTINGS_PANEL NOT FOUND."
            );
            return;
        }

        Button closeButton =
            FindButton(panel, "CLOSE_BUTTON");

        if (closeButton == null)
        {
            Debug.LogError(
                "[MULTIPLAYER] MATCH SETTINGS CLOSE BUTTON NOT FOUND."
            );
            return;
        }

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(
            () => panel.gameObject.SetActive(false)
        );

        panel.gameObject.SetActive(false);

        Debug.Log(
            "[MULTIPLAYER] MATCH SETTINGS OPEN/CLOSE READY."
        );
    }

    private static bool SceneContains(string objectName)
    {
        GameObject[] objects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        foreach (GameObject obj in objects)
        {
            if (obj != null && obj.name == objectName)
                return true;
        }

        return false;
    }

    private static Button FindButton(
        Transform root,
        string name)
    {
        Transform t = FindChild(root, name);

        if (t == null)
            return null;

        return t.GetComponent<Button>();
    }

    private static Transform FindChild(
        Transform root,
        string name)
    {
        Transform[] children =
            root.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == name)
                return child;
        }

        return null;
    }

    private static GameObject FindSceneObject(
        string name)
    {
        GameObject[] objects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        foreach (GameObject obj in objects)
        {
            if (obj != null && obj.name == name)
                return obj;
        }

        return null;
    }
}

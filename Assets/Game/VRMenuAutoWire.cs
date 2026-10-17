using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class VRMenuAutoWire
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Wire()
    {
        if (SceneManager.GetActiveScene().name.Contains("SCN20_VR_COMMAND"))
            return;

        foreach (var button in Object.FindObjectsByType<Button>(FindObjectsSortMode.None))
        {
            string text = button.gameObject.name;

            var tmp = button.GetComponentInChildren<TMP_Text>(true);
            if (tmp != null)
                text += " " + tmp.text;

            var legacy = button.GetComponentInChildren<Text>(true);
            if (legacy != null)
                text += " " + legacy.text;

            if (text.ToLowerInvariant().Contains("vr") ||
                text.ToLowerInvariant().Contains("virtual reality") ||
                text.ToLowerInvariant().Contains("operator"))
            {
                button.onClick.RemoveListener(OpenVR);
                button.onClick.AddListener(OpenVR);
                Debug.Log("[VR] Main Menu VR button wired: " + button.gameObject.name);
            }
        }
    }

    static void OpenVR()
    {
        SceneManager.LoadScene("SCN20_VR_COMMAND");
    }
}


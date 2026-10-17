using UnityEngine;
using UnityEngine.UI;

public class MultiplayerTeamButtonFix : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Fix()
    {
        MultiplayerLobbyRuntime runtime =
            Object.FindFirstObjectByType<MultiplayerLobbyRuntime>();

        if (runtime == null)
            return;

        Canvas canvas =
            Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
            return;

        Transform panel =
            FindChild(canvas.transform, "TEAM_PANEL");

        if (panel == null)
            return;

        Button inviteButton =
            FindButton(panel, "INVITE_BUTTON");

        Button leaveButton =
            FindButton(panel, "LEAVE_BUTTON");

        if (inviteButton != null)
        {
            RectTransform rect =
                inviteButton.GetComponent<RectTransform>();

            if (rect != null)
            {
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);

                rect.anchoredPosition =
                    new Vector2(0f, -205f);

                rect.sizeDelta =
                    new Vector2(360f, 60f);
            }

            inviteButton.onClick.RemoveAllListeners();
            inviteButton.onClick.AddListener(
                runtime.InviteCommander);

            inviteButton.interactable = true;

            Debug.Log(
                "[MULTIPLAYER] INVITE BUTTON POSITIONED AND WIRED."
            );
        }

        if (leaveButton != null)
        {
            RectTransform rect =
                leaveButton.GetComponent<RectTransform>();

            if (rect != null)
            {
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);

                rect.anchoredPosition =
                    new Vector2(0f, -285f);

                rect.sizeDelta =
                    new Vector2(360f, 60f);
            }

            leaveButton.onClick.RemoveAllListeners();
            leaveButton.onClick.AddListener(
                runtime.LeaveTeam);

            leaveButton.interactable = true;

            leaveButton.transform.SetAsLastSibling();

            Debug.Log(
                "[MULTIPLAYER] LEAVE BUTTON POSITIONED AND WIRED."
            );
        }

        Debug.Log(
            "[MULTIPLAYER] TEAM BUTTON OVERLAP FIX APPLIED."
        );
    }

    private static Button FindButton(
        Transform root,
        string name)
    {
        Transform t =
            FindChild(root, name);

        return t != null
            ? t.GetComponent<Button>()
            : null;
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
}

using UnityEngine;

public class LobbyInteractable : MonoBehaviour
{
    public string displayName = "INTERACTION";

    public enum InteractionType
    {
        Queue,
        CreateLobby,
        Ready,
        Team,
        Chat,
        Profile,
        MatchSettings,
        Deployment
    }

    public InteractionType interactionType;

    public void Interact()
    {
        if (MultiplayerLobbyRuntime.Instance == null)
            return;

        switch (interactionType)
        {
            case InteractionType.Queue:
                MultiplayerLobbyRuntime.Instance.OpenMatchmaking();
                break;

            case InteractionType.CreateLobby:
                MultiplayerLobbyRuntime.Instance.CreateLobby();
                break;

            case InteractionType.Ready:
                MultiplayerLobbyRuntime.Instance.ToggleReady();
                break;

            case InteractionType.Team:
                MultiplayerLobbyRuntime.Instance.OpenTeam();
                break;

            case InteractionType.Chat:
                MultiplayerLobbyRuntime.Instance.OpenChat();
                break;

            case InteractionType.Profile:
                MultiplayerLobbyRuntime.Instance.OpenProfile();
                break;

            case InteractionType.MatchSettings:
                MultiplayerLobbyRuntime.Instance.OpenMatchSettings();
                break;

            case InteractionType.Deployment:
                MultiplayerLobbyRuntime.Instance.OpenDeployment();
                break;
        }
    }
}




using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MultiplayerLobbyRuntime : MonoBehaviour
{
    public static MultiplayerLobbyRuntime Instance;

    private Canvas canvas;

    private GameObject interactionPanel;
    private Text interactionText;

    private GameObject matchmakingWing;
    private GameObject matchConfiguration;
    private GameObject teamWing;

    private Text statusText;
    private Text lobbyStateText;
    private Text playerCountText;
    private Text queueText;
    private Text chatText;

    private bool cursorUnlocked;
    private bool queued;
    private bool ready;

    private readonly Queue<string> chatHistory =
        new Queue<string>();

    private const int MaxChatMessages = 12;

    private readonly List<string> players = new List<string>
    {
        "COMMANDER-01",
        "VANGUARD",
        "SENTINEL",
        "NIGHTWATCH"
    };

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        if (canvas != null)
        {
            Transform root = canvas.transform;

            statusText = FindText(root, "LOBBY_STATUS_TEXT");
            lobbyStateText = FindText(root, "LOBBY_STATE_TEXT");
            playerCountText = FindText(root, "PLAYER_COUNT_TEXT");
            queueText = FindText(root, "QUEUE_STATUS_TEXT");
            chatText = FindText(root, "CHAT_LOG_TEXT");

            interactionPanel = FindObject(root, "INTERACTION_PANEL");

            if (interactionPanel != null)
            {
                interactionText =
                    FindText(interactionPanel.transform, "INTERACTION_TEXT");

                interactionPanel.SetActive(false);
            }

            matchmakingWing = FindObject(root, "MATCHMAKING_PANEL");

            if (matchmakingWing != null)
            {
                matchmakingWing.SetActive(false);

                Button joinButton =
                    FindSceneObject("JOIN_QUEUE_BUTTON")?.GetComponent<Button>();

                if (joinButton != null)
                {
                    joinButton.onClick.RemoveAllListeners();
                    joinButton.onClick.AddListener(JoinQueue);
                }

                Button closeButton =
                    FindObject(matchmakingWing.transform, "CLOSE_BUTTON")
                        ?.GetComponent<Button>();

                if (closeButton != null)
                {
                    closeButton.onClick.RemoveAllListeners();
                    closeButton.onClick.AddListener(CloseMatchmaking);

                    Debug.Log("[MULTIPLAYER] CLOSE BUTTON WIRED.");
                }
                else
                {
                    Debug.LogError("[MULTIPLAYER] CLOSE_BUTTON NOT FOUND.");
                }
            }

            matchConfiguration = FindSceneObject("MATCH_CONFIGURATION");

            if (matchConfiguration != null)
                matchConfiguration.SetActive(false);

            teamWing = FindSceneObject("TEAM_WING");

            if (teamWing != null)
                teamWing.SetActive(false);

            teamWing = FindSceneObject("TEAM_WING");

            if (teamWing != null)
                teamWing.SetActive(false);
        }

        UpdateHUD();
    }

    void Update()
    {
        HandleInteraction();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }
    }

    private void HandleInteraction()
    {
        Camera cam = Camera.main;

        if (cam == null)
            return;

        Ray ray = new Ray(
            cam.transform.position,
            cam.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 4f))
        {
            LobbyInteractable interactable =
                hit.collider.GetComponent<LobbyInteractable>();

            if (interactable != null)
            {
                ShowInteraction(
                    "[E] " + interactable.displayName);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }

                return;
            }
        }

        HideInteraction();
    }

    private void ShowInteraction(string message)
    {
        if (interactionPanel == null)
            return;

        interactionPanel.SetActive(true);

        if (interactionText != null)
            interactionText.text = message;
    }

    private void HideInteraction()
    {
        if (interactionPanel != null)
            interactionPanel.SetActive(false);
    }

    public void CloseMatchmaking()
    {
        if (matchmakingWing == null)
        {
            Canvas currentCanvas = FindObjectOfType<Canvas>();

            if (currentCanvas != null)
                matchmakingWing =
                    FindObject(currentCanvas.transform, "MATCHMAKING_PANEL");
        }

        if (matchmakingWing != null)
        {
            matchmakingWing.SetActive(false);
            Debug.Log("[MULTIPLAYER] MATCHMAKING_PANEL CLOSED.");
        }
        else
        {
            Debug.LogError("[MULTIPLAYER] MATCHMAKING_PANEL NOT FOUND.");
        }
    }
    public void CreateLobby()
{
    Canvas currentCanvas = FindObjectOfType<Canvas>();

    if (currentCanvas == null)
    {
        Debug.LogError("[MULTIPLAYER] Canvas not found.");
        return;
    }

    GameObject createPanel =
        FindObject(
            currentCanvas.transform,
            "CREATE_MATCH_LOBBY_PANEL"
        );

    if (createPanel == null)
    {
        Debug.LogError(
            "[MULTIPLAYER] CREATE_MATCH_LOBBY_PANEL NOT FOUND."
        );
        return;
    }

    createPanel.SetActive(true);

    Button createButton =
        FindObject(
            createPanel.transform,
            "CREATE_BUTTON"
        )?.GetComponent<Button>();

    if (createButton != null)
    {
        createButton.onClick.RemoveAllListeners();
        createButton.onClick.AddListener(CreatePrivateMatchLobby);

        Debug.Log(
            "[MULTIPLAYER] CREATE MATCH BUTTON WIRED."
        );
    }
    else
    {
        Debug.LogError(
            "[MULTIPLAYER] CREATE_BUTTON NOT FOUND."
        );
    }

    Button closeButton =
        FindObject(
            createPanel.transform,
            "CLOSE_BUTTON"
        )?.GetComponent<Button>();

    if (closeButton != null)
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseCreateLobby);
    }

    Debug.Log(
        "[MULTIPLAYER] CREATE MATCH LOBBY PANEL OPENED."
    );
}

public void CreatePrivateMatchLobby()
{
    GameObject lobbyStatusObject =
    FindSceneObject("LOBBY_STATUS");

Text lobbyStatus =
    lobbyStatusObject != null
        ? lobbyStatusObject.GetComponent<Text>()
        : null;

if (lobbyStatus != null)
    lobbyStatus.text = "PRIVATE MATCH LOBBY ACTIVE";

    if (lobbyStateText != null)
        lobbyStateText.text =
            "MATCH LOBBY // COMMANDER-01 // LEADER";

    AddChat(
        "LOBBY",
        "Private match lobby created. Commander-01 is lobby leader."
    );

    Canvas currentCanvas = FindObjectOfType<Canvas>();

    if (currentCanvas != null)
    {
        GameObject createPanel =
            FindObject(
                currentCanvas.transform,
                "CREATE_MATCH_LOBBY_PANEL"
            );

        if (createPanel != null)
            createPanel.SetActive(false);
    }

    Debug.Log(
        "[MULTIPLAYER] PRIVATE MATCH LOBBY CREATED."
    );

    UpdateHUD();
}

public void CloseCreateLobby()
{
    Canvas currentCanvas = FindObjectOfType<Canvas>();

    if (currentCanvas == null)
        return;

    GameObject createPanel =
        FindObject(
            currentCanvas.transform,
            "CREATE_MATCH_LOBBY_PANEL"
        );

    if (createPanel != null)
    {
        createPanel.SetActive(false);

        Debug.Log(
            "[MULTIPLAYER] CREATE MATCH LOBBY PANEL CLOSED."
        );
    }
}

    public void JoinQueue()
    {
        queued = true;

        if (queueText != null)
            queueText.text = "QUEUE STATUS // SEARCHING";

        if (statusText != null)
            statusText.text = "SEARCHING FOR COMMANDERS";

        AddChat("MATCHMAKER", "Commander entered matchmaking queue.");

        UpdateHUD();
    }

    public void CancelQueue()
    {
        queued = false;

        if (queueText != null)
            queueText.text = "QUEUE STATUS // READY";

        if (statusText != null)
            statusText.text = "MATCHMAKING CANCELLED";

        AddChat("MATCHMAKER", "Commander left matchmaking queue.");

        UpdateHUD();
    }

    public void ToggleReady()
    {
        ready = !ready;

        string readyStatus =
            ready
                ? "COMMANDER READY"
                : "COMMANDER NOT READY";

        if (statusText != null)
            statusText.text = readyStatus;

        if (lobbyStateText != null)
            lobbyStateText.text = readyStatus;

        AddChat(
            "TEAM",
            ready
                ? "COMMANDER-01 is READY."
                : "COMMANDER-01 is NOT READY.");

        UpdateHUD();

        Debug.Log(
            "[MULTIPLAYER] READY STATE // " +
            readyStatus);
    }

    public void InvitePlayer()
    {
        AddChat(
            "TEAM",
            "Invitation sent to available commander.");
    }

    public void OpenChat()
    {
        chatHistory.Clear();

        AddChat("SYSTEM", "Secure lobby channel established.");
        AddChat("VANGUARD", "Ready when you are.");
        AddChat("SENTINEL", "Fleet checked.");
        AddChat("NIGHTWATCH", "Standing by.");
        AddChat("SYSTEM", "Chat channel opened.");

        if (chatText != null)
        {
            chatText.verticalOverflow = VerticalWrapMode.Truncate;
            chatText.horizontalOverflow = HorizontalWrapMode.Wrap;
        }
    }

    public void OpenProfile()
    {
        Canvas currentCanvas = FindObjectOfType<Canvas>();

        if (currentCanvas == null)
        {
            Debug.LogError("[MULTIPLAYER] Canvas not found.");
            return;
        }

        GameObject profilePanel =
            FindObject(currentCanvas.transform, "PROFILE_PANEL");

        if (profilePanel == null)
        {
            Debug.LogError("[MULTIPLAYER] PROFILE_PANEL NOT FOUND.");
            return;
        }

        profilePanel.SetActive(true);

        Button closeButton =
            FindObject(profilePanel.transform, "CLOSE_BUTTON")
                ?.GetComponent<Button>();

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseProfile);

            Debug.Log("[MULTIPLAYER] PROFILE CLOSE BUTTON WIRED.");
        }
        else
        {
            Debug.LogError("[MULTIPLAYER] PROFILE CLOSE BUTTON NOT FOUND.");
        }

        Debug.Log("[MULTIPLAYER] PROFILE PANEL OPENED.");
    }

    public void CloseProfile()
    {
        Canvas currentCanvas = FindObjectOfType<Canvas>();

        if (currentCanvas == null)
            return;

        GameObject profilePanel =
            FindObject(currentCanvas.transform, "PROFILE_PANEL");

        if (profilePanel != null)
        {
            profilePanel.SetActive(false);
            Debug.Log("[MULTIPLAYER] PROFILE PANEL CLOSED.");
        }
    }

    public void OpenTeam()
{
    Canvas currentCanvas = FindObjectOfType<Canvas>();

    if (currentCanvas == null)
    {
        Debug.LogError("[MULTIPLAYER] Canvas not found.");
        return;
    }

    teamWing =
        FindObject(currentCanvas.transform, "TEAM_PANEL");

    GameObject teamPanel = teamWing;

    if (teamPanel == null)
    {
        Debug.LogError("[MULTIPLAYER] TEAM_PANEL NOT FOUND.");
        return;
    }

    teamPanel.SetActive(true);

    RectTransform rect =
        teamPanel.GetComponent<RectTransform>();

    if (rect != null)
    {
        rect.SetParent(currentCanvas.transform, false);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(900f, 620f);
    }

    teamPanel.transform.SetAsLastSibling();

    Button inviteButton = null;
    Button leaveButton = null;
    Button closeButton = null;

    Transform[] children =
        teamPanel.GetComponentsInChildren<Transform>(true);

    foreach (Transform child in children)
    {
        if (child.name == "INVITE_BUTTON")
            inviteButton = child.GetComponent<Button>();

        if (child.name == "LEAVE_BUTTON")
            leaveButton = child.GetComponent<Button>();

        if (child.name == "CLOSE_BUTTON")
            closeButton = child.GetComponent<Button>();
    }

    if (inviteButton != null)
    {
        inviteButton.onClick.RemoveAllListeners();
        inviteButton.onClick.AddListener(
            InviteCommander
        );

        inviteButton.interactable = true;

        RectTransform inviteRect =
            inviteButton.GetComponent<RectTransform>();

        if (inviteRect != null)
        {
            inviteRect.SetAsLastSibling();
        }

        Image inviteImage =
            inviteButton.GetComponent<Image>();

        if (inviteImage != null)
        {
            inviteImage.raycastTarget = true;
        }

        Debug.Log(
            "[MULTIPLAYER] INVITE COMMANDER BUTTON CONNECTED."
        );
    }
    else
    {
        Debug.LogError(
            "[MULTIPLAYER] INVITE_BUTTON COMPONENT NOT FOUND."
        );
    }

    if (leaveButton != null)
    {
        leaveButton.onClick.RemoveAllListeners();
        leaveButton.onClick.AddListener(
            LeaveTeam
        );

        Debug.Log(
            "[MULTIPLAYER] LEAVE TEAM BUTTON CONNECTED."
        );
    }
    else
    {
        Debug.LogError(
            "[MULTIPLAYER] LEAVE_BUTTON COMPONENT NOT FOUND."
        );
    }

    if (closeButton != null)
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(
            () => CloseTeam()
        );
    }

    Debug.Log(
        "[MULTIPLAYER] TEAM PANEL OPENED."
    );
}

public void InviteCommander()
{
    if (statusText != null)
        statusText.text =
            "COMMANDER INVITATION SENT";

    if (lobbyStateText != null)
        lobbyStateText.text =
            "MATCH LOBBY // INVITATION SENT";

    AddChat(
        "TEAM",
        "Commander invitation sent."
    );

    Debug.Log(
        "[MULTIPLAYER] COMMANDER INVITATION SENT."
    );

    UpdateHUD();
}
public void LeaveTeam()
{
    if (statusText != null)
        statusText.text = "TEAM LEFT";

    if (lobbyStateText != null)
        lobbyStateText.text = "MATCH LOBBY // NO TEAM";

    AddChat(
        "TEAM",
        "Commander-01 left the team."
    );

    UpdateHUD();

    if (teamWing != null)
        teamWing.SetActive(false);

    Debug.Log(
        "[MULTIPLAYER] COMMANDER LEFT TEAM."
    );
}
public void CloseTeam()
    {
        if (teamWing != null)
        {
            teamWing.SetActive(false);
            Debug.Log("[MULTIPLAYER] TEAM PANEL CLOSED.");
        }
    }
    public void OpenMatchSettings()
    {
        Canvas currentCanvas = FindObjectOfType<Canvas>();

        if (currentCanvas == null)
        {
            Debug.LogError("[MULTIPLAYER] Canvas not found.");
            return;
        }

        matchConfiguration =
            FindObject(currentCanvas.transform, "MATCH_SETTINGS_PANEL");

        if (matchConfiguration == null)
        {
            Debug.LogError("[MULTIPLAYER] MATCH_SETTINGS_PANEL NOT FOUND.");
            return;
        }

        matchConfiguration.SetActive(true);

        Button closeButton =
            FindObject(matchConfiguration.transform, "CLOSE_BUTTON")
                ?.GetComponent<Button>();

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseMatchSettings);
        }

        Debug.Log("[MULTIPLAYER] MATCH SETTINGS PANEL OPENED.");
    }

    public void CloseMatchSettings()
    {
        if (matchConfiguration != null)
        {
            matchConfiguration.SetActive(false);
            Debug.Log("[MULTIPLAYER] MATCH SETTINGS PANEL CLOSED.");
        }
    }
    public void OpenMatchmaking()
    {
        if (matchmakingWing == null)
        {
            Canvas currentCanvas = FindObjectOfType<Canvas>();

            if (currentCanvas != null)
                matchmakingWing =
                    FindObject(currentCanvas.transform, "MATCHMAKING_PANEL");
        }

        if (matchmakingWing == null)
        {
            Debug.LogError("[MULTIPLAYER] MATCHMAKING_PANEL NOT FOUND.");
            return;
        }

        matchmakingWing.SetActive(true);

        Debug.Log("[MULTIPLAYER] MATCHMAKING_PANEL OPENED.");
    }
    public void OpenDeployment()
    {
        if (queued)
        {
            if (statusText != null)
                statusText.text =
                    "CANCEL MATCHMAKING BEFORE DEPLOYMENT.";

            return;
        }

        AddChat(
            "SYSTEM",
            "Opening deployment staging.");

        string target = FindDeploymentScene();

        if (!string.IsNullOrEmpty(target))
        {
            SceneManager.LoadScene(target);
        }
        else
        {
            if (statusText != null)
                statusText.text =
                    "DEPLOYMENT SCENE NOT FOUND";
        }
    }

    private string FindDeploymentScene()
    {
        string[] possible =
        {
            "Deployment",
            "SCN-12 DEPLOYMENT"
        };

        foreach (string sceneName in possible)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path =
                    SceneUtility.GetScenePathByBuildIndex(i);

                if (path.EndsWith(
                    "/" + sceneName + ".unity",
                    System.StringComparison.OrdinalIgnoreCase))
                {
                    return sceneName;
                }
            }
        }

        return null;
    }

    private void UpdateHUD()
    {
        if (playerCountText != null)
            playerCountText.text =
                "COMMANDERS // " + players.Count + " / 8";

        if (queueText != null && !queued)
            queueText.text = "QUEUE STATUS // READY";
    }

    private void AddChat(string speaker, string message)
    {
        string line =
            "[" + speaker + "] " + message;

        chatHistory.Enqueue(line);

        while (chatHistory.Count > MaxChatMessages)
            chatHistory.Dequeue();

        RefreshChatLog();
    }

    private void RefreshChatLog()
    {
        if (chatText == null)
            return;

        chatText.text = "CHAT LOG\\n\\n";

        foreach (string line in chatHistory)
            chatText.text += line + "\\n";

        chatText.verticalOverflow = VerticalWrapMode.Truncate;
        chatText.horizontalOverflow = HorizontalWrapMode.Wrap;
    }

    private void ToggleCursor()
    {
        cursorUnlocked = !cursorUnlocked;

        Cursor.lockState =
            cursorUnlocked
                ? CursorLockMode.None
                : CursorLockMode.Locked;

        Cursor.visible = cursorUnlocked;
    }

    private static GameObject FindSceneObject(string name)
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
    private static GameObject FindObject(
        Transform root,
        string name)
    {
        if (root == null)
            return null;

        Transform[] children =
            root.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in children)
        {
            if (t.name == name)
                return t.gameObject;
        }

        return null;
    }

    private static Text FindText(
        Transform root,
        string name)
    {
        GameObject obj =
            FindObject(root, name);

        return obj != null
            ? obj.GetComponent<Text>()
            : null;
    }
}

































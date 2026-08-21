using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AuthenticationUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPagePanel;
    [SerializeField] private GameObject signinPanel;
    [SerializeField] private GameObject createAccountPanel;
    [SerializeField] private GameObject authenticatedPanel;
    [SerializeField] private GameObject settingsPanel;


[Header("Sign In Fields")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    [Header("Create Account Fields")]
    [SerializeField] private TMP_InputField callsignInput;
    [SerializeField] private TMP_InputField createPasswordInput;

    [Header("Status")]
    [SerializeField] private TMP_Text statusText;

    private bool isBusy;

    private void Awake()
    {
        AutoFindReferences();
    }

    private async void Start()
    {
        await WaitForAuthenticationManager();

        if (AuthenticationManager.Instance.IsSignedIn)
        {
            await LoadAndShowAuthenticated();
        }
        else
        {
            ShowMainPagePanel();
        }
    }

    private void AutoFindReferences()
    {
        TMP_InputField[] fields =
            GetComponentsInChildren<TMP_InputField>(true);

        foreach (TMP_InputField field in fields)
        {
            string n = field.name.ToLower();

            if (emailInput == null &&
                (n.Contains("email") || n.Contains("username")))
            {
                emailInput = field;
                continue;
            }

            if (passwordInput == null &&
                n.Contains("password") &&
                !n.Contains("create"))
            {
                passwordInput = field;
                continue;
            }

            if (callsignInput == null &&
                n.Contains("callsign"))
            {
                callsignInput = field;
                continue;
            }

            if (createPasswordInput == null &&
                n.Contains("password") &&
                n.Contains("create"))
            {
                createPasswordInput = field;
            }
        }

        FindPanelReference(ref mainPagePanel, "MainPagePanel");
        FindPanelReference(ref signinPanel, "SigninPanel");
        FindPanelReference(ref createAccountPanel, "CreateAccountPanel");
        FindPanelReference(ref authenticatedPanel, "AuthenticatedPanel");
        FindPanelReference(ref settingsPanel, "SettingsPanel");

        Debug.Log(
            $"AuthenticationUI references: " +
            $"EmailInput={emailInput != null}, " +
            $"PasswordInput={passwordInput != null}, " +
            $"CallsignInput={callsignInput != null}, " +
            $"CreatePasswordInput={createPasswordInput != null}, " +
            $"MainPagePanel={mainPagePanel != null}, " +
            $"SigninPanel={signinPanel != null}, " +
            $"CreateAccountPanel={createAccountPanel != null}, " +
            $"AuthenticatedPanel={authenticatedPanel != null}, " +
            $"SettingsPanel={settingsPanel != null}, " +
            $"StatusText={statusText != null}"
        );
    }

    private void FindPanelReference(
        ref GameObject panel,
        string panelName)
    {
        if (panel != null)
            return;

        Transform t = transform.Find(panelName);

        if (t != null)
            panel = t.gameObject;
    }

    private async Task WaitForAuthenticationManager()
    {
        while (AuthenticationManager.Instance == null)
        {
            await Task.Yield();
        }

        while (Unity.Services.Core.UnityServices.State !=
               Unity.Services.Core.ServicesInitializationState.Initialized)
        {
            await Task.Yield();
        }
    }

    public void ShowMainPagePanel()
    {
        SetPanelState(
            true,
            false,
            false,
            false,
            false
        );
    }

    public void ShowSignInPanel()
    {
        SetPanelState(
            false,
            true,
            false,
            false,
            false
        );
    }

    public void ShowCreateAccountPanel()
    {
        SetPanelState(
            false,
            false,
            true,
            false,
            false
        );
    }

    public void ShowSettingsPanel()
    {
        SetPanelState(
            false,
            false,
            false,
            false,
            true
        );
    }

    public void ShowAuthenticatedPanel()
    {
        SetPanelState(
            false,
            false,
            false,
            true,
            false
        );

        if (statusText != null)
        {
            string username =
                AuthenticationManager.Instance.Username;

            statusText.text =
                $"AUTHENTICATED\n{username}";
        }
    }

    private void SetPanelState(
        bool mainPage,
        bool signin,
        bool createAccount,
        bool authenticated,
        bool settings)
    {
        if (mainPagePanel != null)
            mainPagePanel.SetActive(mainPage);

        if (signinPanel != null)
            signinPanel.SetActive(signin);

        if (createAccountPanel != null)
            createAccountPanel.SetActive(createAccount);

        if (authenticatedPanel != null)
            authenticatedPanel.SetActive(authenticated);

        if (settingsPanel != null)
            settingsPanel.SetActive(settings);
    }

    public async void SignIn()
    {
        if (isBusy)
            return;

        isBusy = true;

        if (emailInput == null || passwordInput == null)
        {
            SetStatus("SIGN IN FIELDS NOT FOUND.");
            isBusy = false;
            return;
        }

        string username = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(username))
        {
            SetStatus("ENTER USERNAME.");
            isBusy = false;
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            SetStatus("ENTER PASSWORD.");
            isBusy = false;
            return;
        }

        SetStatus("SIGNING IN...");

        bool success =
            await AuthenticationManager.Instance.SignIn(
                username,
                password
            );

        if (!success)
        {
            SetStatus("SIGN IN FAILED.");
            isBusy = false;
            return;
        }

        SetStatus("SIGNED IN.");

        ShowAuthenticatedPanel();

        isBusy = false;
    }

    public async void CreateAccount()
    {
        if (isBusy)
            return;

        isBusy = true;

        if (callsignInput == null ||
            createPasswordInput == null)
        {
            SetStatus("CREATE ACCOUNT FIELDS NOT FOUND.");
            isBusy = false;
            return;
        }

        string callsign =
            callsignInput.text.Trim();

        string password =
            createPasswordInput.text;

        if (string.IsNullOrWhiteSpace(callsign))
        {
            SetStatus("ENTER CALLSIGN.");
            isBusy = false;
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            SetStatus("ENTER PASSWORD.");
            isBusy = false;
            return;
        }

        SetStatus("CREATING ACCOUNT...");

        bool success =
            await AuthenticationManager.Instance.CreateAccount(
                callsign,
                password
            );

        if (!success)
        {
            SetStatus("ACCOUNT CREATION FAILED.");
            isBusy = false;
            return;
        }

        ShowAuthenticatedPanel();

        isBusy = false;
    }

    public void Logout()
    {
        if (AuthenticationManager.Instance != null)
        {
            AuthenticationManager.Instance.SignOut();
        }

        ShowMainPagePanel();
    }

    private async Task LoadAndShowAuthenticated()
    {
        ShowAuthenticatedPanel();
        await Task.CompletedTask;
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }


}

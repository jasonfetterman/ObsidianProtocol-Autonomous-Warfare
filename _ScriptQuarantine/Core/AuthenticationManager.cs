using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance { get; private set; }


public bool IsSignedIn
    {
        get
        {
            return AuthenticationService.Instance != null &&
                   AuthenticationService.Instance.IsSignedIn;
        }
    }

    public string PlayerId
    {
        get
        {
            return IsSignedIn
                ? AuthenticationService.Instance.PlayerId
                : "";
        }
    }

    public string Username { get; private set; } = "";

    public event Action AuthenticationStateChanged;

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        await InitializeUnityServices();
    }

    private async Task InitializeUnityServices()
    {
        try
        {
            if (UnityServices.State ==
                ServicesInitializationState.Uninitialized)
            {
                await UnityServices.InitializeAsync();

                Debug.Log("Unity Services initialized.");
            }

            AuthenticationService.Instance.SignedIn -= OnSignedIn;
            AuthenticationService.Instance.SignedIn += OnSignedIn;

            AuthenticationService.Instance.SignedOut -= OnSignedOut;
            AuthenticationService.Instance.SignedOut += OnSignedOut;

            Debug.Log("Authentication service ready.");
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Unity Services initialization failed: {exception}"
            );
        }
    }

    public async Task<bool> CreateAccount(
        string username,
        string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            Debug.LogError("CreateAccount failed: username is empty.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            Debug.LogError("CreateAccount failed: password is empty.");
            return false;
        }

        try
        {
            Debug.Log(
                $"Creating account for username: {username}"
            );

            await AuthenticationService.Instance
                .SignUpWithUsernamePasswordAsync(
                    username,
                    password
                );

            Username = username;

            Debug.Log(
                $"Account created successfully. Username: {Username}"
            );

            return true;
        }
        catch (AuthenticationException exception)
        {
            Debug.LogError(
                $"Account creation authentication failed: {exception}"
            );

            return false;
        }
        catch (RequestFailedException exception)
        {
            Debug.LogError(
                $"Account creation request failed: {exception}"
            );

            return false;
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Account creation error: {exception}"
            );

            return false;
        }
    }

    public async Task<bool> SignIn(
        string username,
        string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            Debug.LogError("SignIn failed: username is empty.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            Debug.LogError("SignIn failed: password is empty.");
            return false;
        }

        try
        {
            if (!IsSignedIn)
            {
                Debug.Log(
                    $"Signing in username: {username}"
                );

                await AuthenticationService.Instance
                    .SignInWithUsernamePasswordAsync(
                        username,
                        password
                    );
            }

            Username = username;

            Debug.Log(
                $"Signed in successfully. Username: {Username}"
            );

            Debug.Log(
                $"Player ID: {PlayerId}"
            );

            return true;
        }
        catch (AuthenticationException exception)
        {
            Debug.LogError(
                $"Sign in authentication failed: {exception}"
            );

            return false;
        }
        catch (RequestFailedException exception)
        {
            Debug.LogError(
                $"Sign in request failed: {exception}"
            );

            return false;
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Sign in error: {exception}"
            );

            return false;
        }
    }

    public void SignOut()
    {
        if (!IsSignedIn)
        {
            Username = "";
            return;
        }

        try
        {
            AuthenticationService.Instance.SignOut();

            Username = "";

            Debug.Log("Player signed out.");
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Sign out error: {exception}"
            );
        }
    }

    private void OnSignedIn()
    {
        if (AuthenticationService.Instance != null)
        {
            Username =
                AuthenticationService.Instance.PlayerName;
        }

        Debug.Log(
            $"Authentication event: Player signed in. Username: {Username}"
        );

        AuthenticationStateChanged?.Invoke();
    }

    private void OnSignedOut()
    {
        Username = "";

        Debug.Log(
            "Authentication event: Player signed out."
        );

        AuthenticationStateChanged?.Invoke();
    }

    private void OnDestroy()
    {
        if (AuthenticationService.Instance == null)
            return;

        AuthenticationService.Instance.SignedIn -= OnSignedIn;
        AuthenticationService.Instance.SignedOut -= OnSignedOut;
    }


}

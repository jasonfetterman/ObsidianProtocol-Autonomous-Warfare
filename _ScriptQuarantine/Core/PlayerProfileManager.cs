using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager Instance { get; private set; }

    public string Callsign { get; private set; } = "";

    public event Action ProfileLoaded;

    private const string CallsignKey = "callsign";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async Task<bool> LoadProfile()
    {
        if (AuthenticationManager.Instance == null ||
            !AuthenticationManager.Instance.IsSignedIn)
        {
            Debug.LogWarning("Cannot load profile: player is not authenticated.");
            return false;
        }

        try
        {
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(
                new HashSet<string> { CallsignKey }
            );

            if (data.TryGetValue(CallsignKey, out var savedItem))
            {
                Callsign = savedItem.Value.GetAs<string>();

                Debug.Log($"Profile loaded. Callsign: {Callsign}");

                // Fix the old test data that accidentally stored the Player ID.
                if (Callsign == AuthenticationManager.Instance.PlayerId)
                {
                    Callsign = AuthenticationManager.Instance.Username;

                    await SaveProfile(Callsign);

                    Debug.Log(
                        $"Profile corrected. Callsign: {Callsign}"
                    );
                }
            }
            else
            {
                Callsign = AuthenticationManager.Instance.Username;

                if (!string.IsNullOrWhiteSpace(Callsign))
                {
                    await SaveProfile(Callsign);

                    Debug.Log(
                        $"Initial profile created. Callsign: {Callsign}"
                    );
                }
                else
                {
                    Debug.LogWarning(
                        "No authenticated username available for profile."
                    );
                }
            }

            ProfileLoaded?.Invoke();

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Failed to load player profile: {exception}"
            );

            return false;
        }
    }

    public async Task<bool> SaveProfile(string callsign)
    {
        if (AuthenticationManager.Instance == null ||
            !AuthenticationManager.Instance.IsSignedIn)
        {
            Debug.LogWarning(
                "Cannot save profile: player is not authenticated."
            );

            return false;
        }

        if (string.IsNullOrWhiteSpace(callsign))
        {
            Debug.LogWarning("Cannot save an empty callsign.");
            return false;
        }

        try
        {
            Callsign = callsign.Trim();

            var data = new Dictionary<string, object>
            {
                { CallsignKey, Callsign }
            };

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);

            Debug.Log($"Profile saved. Callsign: {Callsign}");

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Failed to save player profile: {exception}"
            );

            return false;
        }
    }
}
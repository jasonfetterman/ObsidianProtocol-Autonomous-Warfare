using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CommanderProfileUpload : MonoBehaviour
{
    [SerializeField] private Image profileImage;

    private Button uploadButton;

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, "commander_profile.png");

    private void Awake()
    {
        uploadButton = GetComponent<Button>();

        if (uploadButton != null)
            uploadButton.onClick.AddListener(UploadPhoto);

        LoadSavedPhoto();
    }

    private void UploadPhoto()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        try
        {
            string script =
                "Add-Type -AssemblyName System.Windows.Forms; " +
                "$d = New-Object System.Windows.Forms.OpenFileDialog; " +
                "$d.Title = 'Select Commander Profile Photo'; " +
                "$d.Filter = 'Image Files|*.png;*.jpg;*.jpeg'; " +
                "$d.Multiselect = $false; " +
                "if ($d.ShowDialog() -eq 'OK') " +
                "{ [Console]::Write($d.FileName) }";

            string encoded = Convert.ToBase64String(
                Encoding.Unicode.GetBytes(script));

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-NoProfile -STA -EncodedCommand " + encoded,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string path = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                path = path.Trim();

                if (!string.IsNullOrEmpty(path))
                    LoadPhoto(path);
            }
        }
        catch (Exception exception)
        {
            UnityEngine.Debug.LogError(
                "[COMMANDER PROFILE] Upload failed: " + exception.Message);
        }
#else
        UnityEngine.Debug.LogWarning(
            "Commander photo upload requires Windows.");
#endif
    }

    private void LoadPhoto(string path)
    {
        if (!File.Exists(path))
            return;

        byte[] imageData = File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(2, 2);

        if (!texture.LoadImage(imageData))
        {
            Destroy(texture);
            return;
        }

        File.WriteAllBytes(SavePath, texture.EncodeToPNG());

        DisplayPhoto(texture);
    }

    private void LoadSavedPhoto()
    {
        if (!File.Exists(SavePath))
            return;

        byte[] imageData = File.ReadAllBytes(SavePath);

        Texture2D texture = new Texture2D(2, 2);

        if (!texture.LoadImage(imageData))
        {
            Destroy(texture);
            return;
        }

        DisplayPhoto(texture);
    }

    private void DisplayPhoto(Texture2D texture)
    {
        if (profileImage == null)
        {
            Destroy(texture);
            return;
        }

        if (profileImage.sprite != null)
            Destroy(profileImage.sprite);

        profileImage.sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));

        profileImage.preserveAspect = true;
    }
}
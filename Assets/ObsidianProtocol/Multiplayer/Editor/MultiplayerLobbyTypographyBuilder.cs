using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class MultiplayerLobbyTypographyBuilder
{
    private const string SceneName = "MultiplayerHUD";

    [MenuItem("Obsidian Protocol/Multiplayer/AAA TYPOGRAPHY - FIX ALL HUD TEXT")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid())
        {
            Debug.LogError("[MULTIPLAYER TYPOGRAPHY] No valid scene is open.");
            return;
        }

        int changed = 0;

        Canvas[] canvases = Object.FindObjectsByType<Canvas>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (Canvas canvas in canvases)
        {
            Text[] texts = canvas.GetComponentsInChildren<Text>(
                true);

            foreach (Text text in texts)
            {
                if (text == null)
                    continue;

                ApplyTypography(text);
                changed++;
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[MULTIPLAYER TYPOGRAPHY] AAA typography applied to " +
            changed +
            " UI text elements.");
    }

    private static void ApplyTypography(Text text)
    {
        string n = text.gameObject.name.ToUpperInvariant();

        int size = 26;

        // =========================================================
        // MAJOR TITLES / PRIMARY HUD HEADERS
        // =========================================================

        if (ContainsAny(n,
            "TITLE",
            "HEADER",
            "HUD_TOP",
            "MULTIPLAYER",
            "LOBBY_STATUS",
            "LOBBY_STATE"))
        {
            size = 38;
        }

        // =========================================================
        // SECTION / PANEL HEADERS
        // =========================================================

        else if (ContainsAny(n,
            "SECTION",
            "PANEL_HEADER",
            "MATCHMAKING",
            "MATCH_CONFIGURATION",
            "TEAM_FORMATION",
            "DEPLOYMENT_WING",
            "COMMANDER_PROFILES",
            "PARTY_MANAGEMENT",
            "COMMAND_CONFIGURATION",
            "COMMUNICATION_WING",
            "TEAM_WING",
            "CHAT_PANEL"))
        {
            size = 32;
        }

        // =========================================================
        // BUTTONS / INTERACTION
        // =========================================================

        else if (ContainsAny(n,
            "BUTTON",
            "TERMINAL",
            "READY",
            "QUEUE",
            "CREATE",
            "JOIN",
            "CANCEL",
            "CONFIRM",
            "BACK",
            "OPEN",
            "CLOSE",
            "INVITE",
            "SELECT",
            "DEPLOY"))
        {
            size = 21;
        }

        // =========================================================
        // IMPORTANT STATUS / READOUTS
        // =========================================================

        else if (ContainsAny(n,
            "STATUS",
            "PLAYER_COUNT",
            "DEPLOYMENT",
            "BUDGET",
            "STATE",
            "RATING",
            "COUNT",
            "LOG"))
        {
            size = 21;
        }

        // =========================================================
        // CHAT / NORMAL INFORMATION
        // =========================================================

        else if (ContainsAny(n,
            "CHAT",
            "DESCRIPTION",
            "INFO",
            "DETAIL",
            "MESSAGE",
            "TEXT"))
        {
            size = 21;
        }

        // =========================================================
        // WORLD-SCALE / SECONDARY LABELS
        // =========================================================

        else if (ContainsAny(n,
            "LABEL",
            "NAME",
            "SUBTITLE",
            "SECONDARY",
            "CAPTION"))
        {
            size = 21;
        }

        text.fontSize = size;

        // Strong readable presentation.
        text.resizeTextForBestFit = false;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        // Slightly increased line spacing for command-console readability.
        text.lineSpacing = 1.08f;

        // Preserve existing alignment unless it is unset.
        if (text.alignment == TextAnchor.UpperLeft)
        {
            // Keep existing left alignment.
        }

        // Improve readability against dark futuristic HUD panels.
        Color c = text.color;

        if (c.a < 0.95f)
        {
            c.a = 1f;
            text.color = c;
        }

        // Make important UI text visually stronger without changing
        // the actual font asset.
        if (size >= 36)
            text.fontStyle = FontStyle.Bold;
        else if (size >= 30)
            text.fontStyle = FontStyle.Bold;
        else
            text.fontStyle = FontStyle.Normal;

        EditorUtility.SetDirty(text);
    }

    private static bool ContainsAny(
        string value,
        params string[] terms)
    {
        foreach (string term in terms)
        {
            if (value.Contains(term))
                return true;
        }

        return false;
    }
}


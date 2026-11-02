using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMatchSettingsController : MonoBehaviour
{
    private Button rankedButton;
    private Button rulesetButton;
    private Button commanderButton;

    private Text rankedLabel;
    private Text rulesetLabel;
    private Text commanderLabel;

    private bool ranked = true;

    private readonly string[] rulesets =
    {
        "STANDARD",
        "LIMITED",
        "SPECIAL"
    };

    private int rulesetIndex = 0;

    private readonly int[] commanderCounts =
    {
        2,
        4,
        6,
        8
    };

    private int commanderIndex = 1;

    private void Awake()
    {
        WireButtons();
        RefreshDisplay();
    }

    private void OnEnable()
    {
        WireButtons();
        RefreshDisplay();
    }

    private void WireButtons()
    {
        Transform ranked =
            transform.Find("RANKED_BUTTON");

        Transform ruleset =
            transform.Find("RULESET_BUTTON");

        Transform commanders =
            transform.Find("COMMANDER_COUNT_BUTTON");

        if (ranked != null)
        {
            rankedButton =
                ranked.GetComponent<Button>();

            rankedLabel =
                ranked.GetComponentInChildren<Text>();
        }

        if (ruleset != null)
        {
            rulesetButton =
                ruleset.GetComponent<Button>();

            rulesetLabel =
                ruleset.GetComponentInChildren<Text>();
        }

        if (commanders != null)
        {
            commanderButton =
                commanders.GetComponent<Button>();

            commanderLabel =
                commanders.GetComponentInChildren<Text>();
        }

        if (rankedButton != null)
        {
            rankedButton.onClick.RemoveListener(
                ToggleRanked);

            rankedButton.onClick.AddListener(
                ToggleRanked);
        }

        if (rulesetButton != null)
        {
            rulesetButton.onClick.RemoveListener(
                CycleRuleset);

            rulesetButton.onClick.AddListener(
                CycleRuleset);
        }

        if (commanderButton != null)
        {
            commanderButton.onClick.RemoveListener(
                CycleCommanders);

            commanderButton.onClick.AddListener(
                CycleCommanders);
        }
    }

    private void ToggleRanked()
    {
        ranked = !ranked;
        RefreshDisplay();

        Debug.Log(
            "[MULTIPLAYER] MATCH SETTINGS RANKED = " +
            ranked);
    }

    private void CycleRuleset()
    {
        rulesetIndex++;

        if (rulesetIndex >= rulesets.Length)
            rulesetIndex = 0;

        RefreshDisplay();

        Debug.Log(
            "[MULTIPLAYER] MATCH SETTINGS RULESET = " +
            rulesets[rulesetIndex]);
    }

    private void CycleCommanders()
    {
        commanderIndex++;

        if (commanderIndex >= commanderCounts.Length)
            commanderIndex = 0;

        RefreshDisplay();

        Debug.Log(
            "[MULTIPLAYER] MATCH SETTINGS COMMANDERS = " +
            commanderCounts[commanderIndex]);
    }

    private void RefreshDisplay()
    {
        if (rankedLabel != null)
        {
            rankedLabel.text =
                ranked
                ? "RANKED OPERATIONS // RANKED"
                : "RANKED OPERATIONS // UNRANKED";
        }

        if (rulesetLabel != null)
        {
            rulesetLabel.text =
                "RULESET // " +
                rulesets[rulesetIndex];
        }

        if (commanderLabel != null)
        {
            commanderLabel.text =
                "COMMANDERS // " +
                commanderCounts[commanderIndex];
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MultiplayerTeamPanelController : MonoBehaviour
{
    private Button inviteButton;
    private Button leaveButton;
    private Button closeButton;

    private Text slot1;
    private Text slot2;
    private Text slot3;
    private Text slot4;
    private Text slot5;
    private Text slot6;
    private Text slot7;
    private Text slot8;
    private Text capacityText;

    private int memberCount = 1;

    private void Awake()
    {
        Wire();
        Refresh();
    }

    private void OnEnable()
    {
        Wire();
        Refresh();
    }

    private void Wire()
    {
        inviteButton = FindButton("INVITE_BUTTON");
        leaveButton = FindButton("LEAVE_BUTTON");
        closeButton = FindButton("CLOSE_BUTTON");

        slot1 = FindText("TEAM_SLOT_1");
        slot2 = FindText("TEAM_SLOT_2");
        slot3 = FindText("TEAM_SLOT_3");
        slot4 = FindText("TEAM_SLOT_4");
        slot5 = FindText("TEAM_SLOT_5");
        slot6 = FindText("TEAM_SLOT_6");
        slot7 = FindText("TEAM_SLOT_7");
        slot8 = FindText("TEAM_SLOT_8");

        capacityText = FindText("TEAM_CAPACITY");

        if (inviteButton != null)
        {
            inviteButton.onClick.RemoveListener(
                InviteCommander);

            inviteButton.onClick.AddListener(
                InviteCommander);
        }

        if (leaveButton != null)
        {
            leaveButton.onClick.RemoveListener(
                LeaveTeam);

            leaveButton.onClick.AddListener(
                LeaveTeam);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(
                ClosePanel);

            closeButton.onClick.AddListener(
                ClosePanel);
        }
    }

    private Button FindButton(string objectName)
    {
        Transform t = transform.Find(objectName);

        if (t == null)
            return null;

        return t.GetComponent<Button>();
    }

    private Text FindText(string objectName)
    {
        Transform t = transform.Find(objectName);

        if (t == null)
            return null;

        return t.GetComponent<Text>();
    }

    private void InviteCommander()
    {
        if (memberCount >= 8)
        {
            Debug.Log(
                "[MULTIPLAYER] TEAM FULL.");

            return;
        }

        memberCount++;

        Refresh();

        Debug.Log(
            "[MULTIPLAYER] COMMANDER INVITED. TEAM SIZE = " +
            memberCount);
    }

    private void LeaveTeam()
    {
        if (memberCount <= 1)
        {
            Debug.Log(
                "[MULTIPLAYER] COMMANDER-01 IS TEAM LEADER. " +
                "NO ADDITIONAL MEMBERS TO REMOVE.");

            return;
        }

        memberCount--;

        Refresh();

        Debug.Log(
            "[MULTIPLAYER] COMMANDER REMOVED. TEAM SIZE = " +
            memberCount);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        SetSlot(
            slot1,
            1,
            memberCount >= 1
                ? "COMMANDER-01"
                : null);

        SetSlot(
            slot2,
            2,
            memberCount >= 2
                ? "COMMANDER-02"
                : null);

        SetSlot(
            slot3,
            3,
            memberCount >= 3
                ? "COMMANDER-03"
                : null);

        SetSlot(
            slot4,
            4,
            memberCount >= 4
                ? "COMMANDER-04"
                : null);

        SetSlot(
            slot5,
            5,
            memberCount >= 5
                ? "COMMANDER-05"
                : null);

        SetSlot(
            slot6,
            6,
            memberCount >= 6
                ? "COMMANDER-06"
                : null);

        SetSlot(
            slot7,
            7,
            memberCount >= 7
                ? "COMMANDER-07"
                : null);

        SetSlot(
            slot8,
            8,
            memberCount >= 8
                ? "COMMANDER-08"
                : null);

        if (capacityText != null)
        {
            capacityText.text =
                "TEAM SIZE // " +
                memberCount +
                " / 8";
        }
    }

    private void SetSlot(
        Text target,
        int slot,
        string commander)
    {
        if (target == null)
            return;

        if (commander == null)
        {
            target.text =
                "SLOT " +
                slot.ToString("00") +
                " // OPEN";
        }
        else
        {
            target.text =
                "SLOT " +
                slot.ToString("00") +
                " // " +
                commander;
        }
    }
}

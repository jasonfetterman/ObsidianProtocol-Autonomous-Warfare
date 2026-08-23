using UnityEngine;
using UnityEngine.UI;

public class HUDCommandIntentAvailability : MonoBehaviour
{
    [SerializeField] private Text intentText;

    public int AvailableCommands { get; private set; }
    public int MaximumCommands { get; private set; }

    public void SetAvailability(int available, int maximum)
    {
        AvailableCommands = Mathf.Max(0, available);
        MaximumCommands = Mathf.Max(0, maximum);

        Refresh();
    }

    public void SetAvailable(int available)
    {
        AvailableCommands = Mathf.Max(0, available);
        Refresh();
    }

    public void SetMaximum(int maximum)
    {
        MaximumCommands = Mathf.Max(0, maximum);
        Refresh();
    }

    private void Refresh()
    {
        if (intentText != null)
        {
            intentText.text =
                $"INTENT {AvailableCommands}/{MaximumCommands}";
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class HUDPopulationForceIndicator : MonoBehaviour
{
    [SerializeField] private Text forceText;

    public int CurrentForce { get; private set; }
    public int MaximumForce { get; private set; }

    public void SetForce(int current, int maximum)
    {
        CurrentForce = Mathf.Max(0, current);
        MaximumForce = Mathf.Max(0, maximum);

        Refresh();
    }

    public void SetCurrentForce(int current)
    {
        CurrentForce = Mathf.Max(0, current);
        Refresh();
    }

    public void SetMaximumForce(int maximum)
    {
        MaximumForce = Mathf.Max(0, maximum);
        Refresh();
    }

    private void Refresh()
    {
        if (forceText != null)
            forceText.text = $"FORCE {CurrentForce}/{MaximumForce}";
    }
}
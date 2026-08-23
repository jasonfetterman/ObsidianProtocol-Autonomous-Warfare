using UnityEngine;
using UnityEngine.UI;

public class HUDUnitAmmunition : MonoBehaviour
{
    [SerializeField] private Text ammunitionText;

    public int CurrentAmmunition { get; private set; }
    public int MaximumAmmunition { get; private set; }

    public void SetAmmunition(int current, int maximum)
    {
        MaximumAmmunition = Mathf.Max(0, maximum);
        CurrentAmmunition = Mathf.Clamp(current, 0, MaximumAmmunition);

        Refresh();
    }

    private void Refresh()
    {
        if (ammunitionText != null)
            ammunitionText.text = $"{CurrentAmmunition}/{MaximumAmmunition}";
    }
}

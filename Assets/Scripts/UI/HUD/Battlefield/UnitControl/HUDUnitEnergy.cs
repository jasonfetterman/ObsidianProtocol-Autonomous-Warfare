using UnityEngine;
using UnityEngine.UI;

public class HUDUnitEnergy : MonoBehaviour
{
    [SerializeField] private Slider energyBar;
    [SerializeField] private Text energyText;

    public float CurrentEnergy { get; private set; }
    public float MaximumEnergy { get; private set; }

    public void SetEnergy(float current, float maximum)
    {
        MaximumEnergy = Mathf.Max(0f, maximum);
        CurrentEnergy = Mathf.Clamp(current, 0f, MaximumEnergy);

        Refresh();
    }

    private void Refresh()
    {
        if (energyBar != null)
        {
            energyBar.maxValue = MaximumEnergy;
            energyBar.value = CurrentEnergy;
        }

        if (energyText != null)
            energyText.text = $"{CurrentEnergy:0}/{MaximumEnergy:0}";
    }
}

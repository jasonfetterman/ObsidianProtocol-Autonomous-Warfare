using UnityEngine;
using UnityEngine.UI;

public class HUDUnitFuel : MonoBehaviour
{
    [SerializeField] private Slider fuelBar;
    [SerializeField] private Text fuelText;

    public float CurrentFuel { get; private set; }
    public float MaximumFuel { get; private set; }

    public void SetFuel(float current, float maximum)
    {
        MaximumFuel = Mathf.Max(0f, maximum);
        CurrentFuel = Mathf.Clamp(current, 0f, MaximumFuel);

        Refresh();
    }

    private void Refresh()
    {
        if (fuelBar != null)
        {
            fuelBar.maxValue = MaximumFuel;
            fuelBar.value = CurrentFuel;
        }

        if (fuelText != null)
            fuelText.text = $"{CurrentFuel:0}/{MaximumFuel:0}";
    }
}

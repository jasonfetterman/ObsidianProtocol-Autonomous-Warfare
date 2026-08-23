using UnityEngine;
using UnityEngine.UI;

public class HUDUnitArmor : MonoBehaviour
{
    [SerializeField] private Slider armorBar;
    [SerializeField] private Text armorText;

    public float CurrentArmor { get; private set; }
    public float MaximumArmor { get; private set; }

    public void SetArmor(float current, float maximum)
    {
        MaximumArmor = Mathf.Max(0f, maximum);
        CurrentArmor = Mathf.Clamp(current, 0f, MaximumArmor);

        Refresh();
    }

    private void Refresh()
    {
        if (armorBar != null)
        {
            armorBar.maxValue = MaximumArmor;
            armorBar.value = CurrentArmor;
        }

        if (armorText != null)
            armorText.text = $"{CurrentArmor:0}/{MaximumArmor:0}";
    }
}

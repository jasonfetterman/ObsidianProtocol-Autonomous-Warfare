using UnityEngine;
using UnityEngine.UI;

public class HUDUnitHealth : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Text healthText;

    public float CurrentHealth { get; private set; }
    public float MaximumHealth { get; private set; }

    public void SetHealth(float current, float maximum)
    {
        MaximumHealth = Mathf.Max(0f, maximum);
        CurrentHealth = Mathf.Clamp(current, 0f, MaximumHealth);

        Refresh();
    }

    public void SetCurrentHealth(float current)
    {
        CurrentHealth = Mathf.Clamp(current, 0f, MaximumHealth);
        Refresh();
    }

    private void Refresh()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = MaximumHealth;
            healthBar.value = CurrentHealth;
        }

        if (healthText != null)
            healthText.text = $"{CurrentHealth:0}/{MaximumHealth:0}";
    }
}

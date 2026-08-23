using UnityEngine;
using UnityEngine.UI;

public class HUDUnitHealthBars : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

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

    public void SetMaximumHealth(float maximum)
    {
        MaximumHealth = Mathf.Max(0f, maximum);
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaximumHealth);
        Refresh();
    }

    private void Refresh()
    {
        if (healthBar == null)
            return;

        healthBar.maxValue = MaximumHealth;
        healthBar.value = CurrentHealth;
    }
}
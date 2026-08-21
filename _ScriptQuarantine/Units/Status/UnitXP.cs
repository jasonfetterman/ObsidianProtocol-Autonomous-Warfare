using UnityEngine;

public class UnitXP : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField] private float experience = 0f;
    [SerializeField] private int level = 1;

    [Header("Combat Scaling")]
    [SerializeField] private float damageMultiplierPerLevel = 0.05f;

    public float Experience => experience;
    public int Level => level;

    public float GetDamageMultiplier()
    {
        return 1f + ((level - 1) * damageMultiplierPerLevel);
    }

    public void AddXP(float amount)
    {
        if (amount <= 0f)
            return;

        experience += amount;
        UpdateLevel();
    }

    public void SetXP(float amount)
    {
        experience = Mathf.Max(0f, amount);
        UpdateLevel();
    }

    private void UpdateLevel()
    {
        level = Mathf.Max(1, Mathf.FloorToInt(experience / 100f) + 1);
    }
}
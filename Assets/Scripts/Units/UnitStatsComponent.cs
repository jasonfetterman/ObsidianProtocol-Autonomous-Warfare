using UnityEngine;
using ObsidianProtocol.Game.Units;

public class UnitStatsComponent : MonoBehaviour
{
    [SerializeField] private string unitId = "unnamed-unit";

    [Header("Base Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float armor = 0f;
    [SerializeField] private float mobility = 5f;
    [SerializeField] private float sensorRange = 20f;
    [SerializeField] private float detection = 10f;
    [SerializeField] private float accuracy = 0.7f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float commandCapacity = 0f;
    [SerializeField] private float energyCapacity = 100f;

    [SerializeField] private float currentHealth;

    public UnitStatistics Stats { get; private set; }
    public float CurrentHealth => currentHealth;

    public float MaxHealth => maxHealth;
    private void Awake()
    {
        Stats = new UnitStatistics(unitId);
        Stats.Configure(
            maxHealth,
            armor,
            mobility,
            sensorRange,
            detection,
            accuracy,
            damage,
            fireRate,
            commandCapacity,
            energyCapacity);

        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0f, currentHealth - amount);
        Debug.Log($"{gameObject.name} took {amount} damage. Health: {currentHealth}/{maxHealth}");
    }
}
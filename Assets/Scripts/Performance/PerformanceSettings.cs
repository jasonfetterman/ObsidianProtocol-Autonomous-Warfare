using UnityEngine;

[CreateAssetMenu(fileName = "PerformanceSettings", menuName = "RTS/Performance Settings")]
public class PerformanceSettings : ScriptableObject
{
    public float aiTickRate = 0.2f;
    public float projectileSpeedMultiplier = 1f;
    public float lodDistance = 25f;
    public float cullingDistance = 60f;
}

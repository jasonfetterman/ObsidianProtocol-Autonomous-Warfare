using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Common damageable surface implemented by units and buildings.
    /// </summary>
    public interface ICombatTarget
    {
        int OwnerId { get; }
        int HitPoints { get; }
        int MaxHitPoints { get; }
        bool IsAlive { get; }
        bool IsDamaged { get; }
        bool IsSelected { get; }
        Vector3 CombatTargetPosition { get; }
        Vector3 HealthBarWorldPosition { get; }

        float DistanceTo(Vector3 worldPosition);
        void TakeDamage(int damage);
    }
}

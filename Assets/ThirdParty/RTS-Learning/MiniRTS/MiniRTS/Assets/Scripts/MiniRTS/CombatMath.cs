using System;

namespace MiniRTS
{
    /// <summary>
    /// Deterministic combat arithmetic kept separate from scene objects for testing.
    /// </summary>
    public static class CombatMath
    {
        public static int ApplyDamage(int currentHitPoints, int damage)
        {
            if (currentHitPoints < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentHitPoints));
            }

            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }

            return Math.Max(0, currentHitPoints - damage);
        }

        public static float HitPointFraction(int currentHitPoints, int maximumHitPoints)
        {
            if (maximumHitPoints <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumHitPoints));
            }

            float fraction = (float)currentHitPoints / maximumHitPoints;
            return Math.Max(0f, Math.Min(1f, fraction));
        }

        public static bool IsCooldownReady(float currentTime, float nextAttackTime)
        {
            return currentTime >= nextAttackTime;
        }

        public static float ScheduleNextAttack(float currentTime, float cooldownSeconds)
        {
            if (cooldownSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(cooldownSeconds));
            }

            return currentTime + cooldownSeconds;
        }
    }
}

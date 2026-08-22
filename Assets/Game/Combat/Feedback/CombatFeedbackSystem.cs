using System;
using UnityEngine;

namespace ObsidianProtocol.Game.Combat.Feedback
{
    public sealed class CombatFeedbackSystem : MonoBehaviour
    {
        public event Action<float> DamageReceived;
        public event Action<Vector3> ImpactReceived;
        public event Action CombatDestroyed;

        public void ReportDamage(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            DamageReceived?.Invoke(amount);
        }

        public void ReportImpact(Vector3 position)
        {
            ImpactReceived?.Invoke(position);
        }

        public void ReportDestroyed()
        {
            CombatDestroyed?.Invoke();
        }
    }
}

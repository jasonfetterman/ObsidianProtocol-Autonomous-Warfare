using Assets.Scripts.AI;
using Assets.Scripts.Squad;
using UnityEngine;

namespace Assets.Scripts.Squad
{
    public class SquadTactics
    {
        private SquadAI squad;
        private SquadMemory memory;
        private SquadController controller;

        public float idleRecenterDistance = 5f;
        public float fallbackDistance = 8f;

        public SquadTactics()
        {
            squad = ServiceLocator.Get<SquadAI>();
            memory = ServiceLocator.Get<SquadMemory>();
            controller = ServiceLocator.Get<SquadController>();
        }

        public void Tick()
        {
            if (squad == null || squad.members.Count == 0)
                return;

            HandleIdleRecenter();
            HandleFallback();
        }

        private void HandleIdleRecenter()
        {
            if (memory.TimeSinceEnemySeen > 3f &&
                memory.LastMoveTarget.HasValue == false &&
                memory.LastAttackTarget == null)
            {
                Vector3 center = squad.SquadCenter;
                controller.SetMoveTarget(center);
            }
        }

        private void HandleFallback()
        {
            if (memory.LastEnemyPosition.HasValue)
            {
                Vector3 enemyPos = memory.LastEnemyPosition.Value;
                Vector3 center = squad.SquadCenter;

                float dist = Vector3.Distance(center, enemyPos);
                if (dist > fallbackDistance)
                {
                    Vector3 dir = (center - enemyPos).normalized;
                    Vector3 fallback = center + dir * 2f;

                    controller.SetMoveTarget(fallback);
                }
            }
        }
    }
}

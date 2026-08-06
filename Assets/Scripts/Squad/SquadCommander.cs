using Assets.Scripts.AI;      // SquadController
using Assets.Scripts.Squad;   // SquadIntent, SquadAI
using UnityEngine;

namespace Assets.Scripts.Squad
{
    public class SquadCommander
    {
        private SquadController controller;
        private SquadIntent intent;

        public SquadCommander()
        {
            controller = ServiceLocator.Get<SquadController>();
            intent = ServiceLocator.Get<SquadIntent>();
        }

        // Called by UI or input system
        public void IssueMoveCommand(Vector3 target)
        {
            intent.SetMoveIntent(target);
            controller.SetMoveTarget(target);
        }

        public void IssueAttackCommand(GameObject target)
        {
            if (target == null)
                return;

            intent.SetAttackIntent(target);
            controller.Attack(target);
        }

        public void IssueFormationCommand(SquadAI.FormationType type)
        {
            intent.SetFormationIntent(type);
            controller.SetFormation(type);
        }

        public void ClearFormation()
        {
            intent.ClearFormationIntent();
            controller.ClearFormation();
        }
    }
}

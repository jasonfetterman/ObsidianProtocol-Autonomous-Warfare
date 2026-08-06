using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Squad;   // SquadMember, SquadAI

namespace Assets.Scripts.AI
{
    public class SquadController : MonoBehaviour
    {
        public void IssueMoveCommand(IReadOnlyList<SquadMember> members, Vector3 point)
        {
            // movement logic
        }

        public void IssueAttackCommand(IReadOnlyList<SquadMember> members)
        {
            // attack logic
        }

        public void IssueStopCommand(IReadOnlyList<SquadMember> members)
        {
            // stop logic
        }

        public void SetMoveTarget(Vector3 point)
        {
            // internal movement logic
        }

        public void Attack(GameObject target)
        {
            // internal attack logic
        }

        public void ClearFormation()
        {
            // internal formation clear
        }

        public void SetFormation(SquadAI.FormationType type)
        {
            // internal formation logic
        }
    }
}

using System;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.IntentOrders
{
    public enum IntentOrderType
    {
        None,
        Advance,
        Attack,
        Defend,
        Hold,
        Recon,
        Capture,
        Reinforce,
        Retreat,
        Support
    }

    [Serializable]
    public sealed class IntentOrder
    {
        public IntentOrderType Type;
        public Vector3 ObjectivePosition;
        public GameObject Target;
        public float Priority = 1f;

        public IntentOrder(
            IntentOrderType type,
            Vector3 objectivePosition)
        {
            Type = type;
            ObjectivePosition = objectivePosition;
        }

        public IntentOrder(
            IntentOrderType type,
            Vector3 objectivePosition,
            GameObject target)
        {
            Type = type;
            ObjectivePosition = objectivePosition;
            Target = target;
        }
    }

    public sealed class IntentOrderSystem
    {
        public event Action<IntentOrder> IntentIssued;

        public IntentOrder CurrentIntent { get; private set; }

        public bool HasIntent =>
            CurrentIntent != null &&
            CurrentIntent.Type != IntentOrderType.None;

        public void IssueIntent(IntentOrder intent)
        {
            if (intent == null)
            {
                return;
            }

            CurrentIntent = intent;
            IntentIssued?.Invoke(intent);
        }

        public void ClearIntent()
        {
            CurrentIntent = null;
        }
    }
}

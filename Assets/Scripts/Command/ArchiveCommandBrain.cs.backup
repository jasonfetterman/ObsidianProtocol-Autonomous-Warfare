using System;
using System.Collections.Generic;
using UnityEngine;
using ObsidianProtocol.Game.Command;

namespace ObsidianProtocol.Game.Command.Autonomy
{
    /// <summary>
    /// ARCHIVE autonomous command brain.
    ///
    /// Receives player intent and is responsible for turning that intent
    /// into a complete operational plan.
    /// </summary>
    [RequireComponent(typeof(CommandUnit))]
    public sealed class ArchiveCommandBrain : MonoBehaviour
    {
        private CommandUnit commandUnit;

        private readonly List<ArchiveOperationalOrder> activeOrders =
            new List<ArchiveOperationalOrder>();

        public bool Operational { get; private set; }

        public IntentType LastPlayerIntent { get; private set; }

        public IReadOnlyList<ArchiveOperationalOrder> ActiveOrders =>
            activeOrders;

        private void Awake()
        {
            commandUnit = GetComponent<CommandUnit>();

            Operational = false;

            Debug.Log(
                "[ARCHIVE] Brain initialized: " +
                gameObject.name);
        }

        private void Start()
        {
            Activate();
        }

        public void Activate()
        {
            Operational = true;

            Debug.Log(
                "[ARCHIVE] ONLINE: " +
                gameObject.name);
        }

        public void Deactivate()
        {
            Operational = false;

            Debug.Log(
                "[ARCHIVE] OFFLINE: " +
                gameObject.name);
        }

        /// <summary>
        /// Receives the player's high-level intent.
        /// ARCHIVE decides what operational actions are required.
        /// </summary>
        public void ReceiveIntent(IntentType intent)
        {
            if (!Operational)
            {
                Debug.LogWarning(
                    "[ARCHIVE] Intent rejected because ARCHIVE is offline.");

                return;
            }

            LastPlayerIntent = intent;

            Debug.Log(
                "[ARCHIVE] PLAYER INTENT RECEIVED: " +
                intent);

            AnalyzeIntent(intent);
        }

        private void AnalyzeIntent(IntentType intent)
        {
            activeOrders.Clear();

            Debug.Log(
                "[ARCHIVE] Analyzing intent: " +
                intent);

            switch (intent)
            {
                case IntentType.Move:
                    BuildMovePlan();
                    break;

                case IntentType.Attack:
                    BuildAttackPlan();
                    break;

                case IntentType.Defend:
                    BuildDefendPlan();
                    break;

                case IntentType.Recon:
                    BuildReconPlan();
                    break;

                case IntentType.Flank:
                    BuildFlankPlan();
                    break;

                case IntentType.Suppress:
                    BuildSuppressPlan();
                    break;

                case IntentType.Breach:
                    BuildBreachPlan();
                    break;

                case IntentType.Pursue:
                    BuildPursuitPlan();
                    break;

                case IntentType.Retreat:
                    BuildRetreatPlan();
                    break;

                case IntentType.Reinforce:
                    BuildReinforcementPlan();
                    break;

                default:
                    Debug.LogWarning(
                        "[ARCHIVE] No planning doctrine exists for: " +
                        intent);
                    break;
            }

            PublishOperationalPlan();
        }

        private void BuildMovePlan()
        {
            Debug.Log("[ARCHIVE] Building MOVEMENT plan.");

            AddOrder(
                ArchiveOrderType.Advance,
                "ARCHIVE movement plan");
        }

        private void BuildAttackPlan()
        {
            Debug.Log("[ARCHIVE] Building ATTACK plan.");

            AddOrder(
                ArchiveOrderType.Attack,
                "ARCHIVE attack plan");
        }

        private void BuildDefendPlan()
        {
            Debug.Log("[ARCHIVE] Building DEFENSE plan.");

            AddOrder(
                ArchiveOrderType.Defend,
                "ARCHIVE defensive plan");
        }

        private void BuildReconPlan()
        {
            Debug.Log("[ARCHIVE] Building RECONNAISSANCE plan.");

            AddOrder(
                ArchiveOrderType.Recon,
                "ARCHIVE reconnaissance plan");
        }

        private void BuildFlankPlan()
        {
            Debug.Log("[ARCHIVE] Building FLANK plan.");

            AddOrder(
                ArchiveOrderType.Flank,
                "ARCHIVE flanking plan");
        }

        private void BuildSuppressPlan()
        {
            Debug.Log("[ARCHIVE] Building SUPPRESSION plan.");

            AddOrder(
                ArchiveOrderType.Suppress,
                "ARCHIVE suppression plan");
        }

        private void BuildBreachPlan()
        {
            Debug.Log("[ARCHIVE] Building BREACH plan.");

            AddOrder(
                ArchiveOrderType.Breach,
                "ARCHIVE breach plan");
        }

        private void BuildPursuitPlan()
        {
            Debug.Log("[ARCHIVE] Building PURSUIT plan.");

            AddOrder(
                ArchiveOrderType.Pursue,
                "ARCHIVE pursuit plan");
        }

        private void BuildRetreatPlan()
        {
            Debug.Log("[ARCHIVE] Building RETREAT plan.");

            AddOrder(
                ArchiveOrderType.Retreat,
                "ARCHIVE withdrawal plan");
        }

        private void BuildReinforcementPlan()
        {
            Debug.Log("[ARCHIVE] Building REINFORCEMENT plan.");

            AddOrder(
                ArchiveOrderType.Reinforce,
                "ARCHIVE reinforcement plan");
        }

        private void AddOrder(
            ArchiveOrderType type,
            string reason)
        {
            ArchiveOperationalOrder order =
                new ArchiveOperationalOrder(
                    Guid.NewGuid().ToString(),
                    type,
                    reason,
                    transform.position);

            activeOrders.Add(order);
        }

        private void PublishOperationalPlan()
        {
            Debug.Log(
                "[ARCHIVE] OPERATIONAL PLAN CREATED: " +
                activeOrders.Count +
                " order(s).");

            foreach (ArchiveOperationalOrder order in activeOrders)
            {
                Debug.Log(
                    "[ARCHIVE] ORDER: " +
                    order.Type +
                    " | " +
                    order.Reason);
            }
        }
    }

    public enum ArchiveOrderType
    {
        Advance,
        Attack,
        Defend,
        Recon,
        Flank,
        Suppress,
        Breach,
        Pursue,
        Retreat,
        Reinforce
    }

    public sealed class ArchiveOperationalOrder
    {
        public string OrderId { get; }

        public ArchiveOrderType Type { get; }

        public string Reason { get; }

        public Vector3 ObjectivePosition { get; }

        public DateTime CreatedAt { get; }

        public ArchiveOperationalOrder(
            string orderId,
            ArchiveOrderType type,
            string reason,
            Vector3 objectivePosition)
        {
            OrderId = orderId;
            Type = type;
            Reason = reason;
            ObjectivePosition = objectivePosition;
            CreatedAt = DateTime.UtcNow;
        }
    }
}

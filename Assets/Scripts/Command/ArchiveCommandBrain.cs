
using System;
using System.Collections.Generic;
using UnityEngine;
using ObsidianProtocol.Game.Command;

namespace ObsidianProtocol.Game.Command.Autonomy
{
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

            Debug.Log("[ARCHIVE] Brain initialized: " + gameObject.name);
        }

        private void Start()
        {
            Activate();
        }

        public void Activate()
        {
            Operational = true;
            Debug.Log("[ARCHIVE] ONLINE: " + gameObject.name);
        }

        public void Deactivate()
        {
            Operational = false;
            Debug.Log("[ARCHIVE] OFFLINE: " + gameObject.name);
        }

        public void ReceiveIntent(IntentType intent)
        {
            if (!Operational)
            {
                Debug.LogWarning("[ARCHIVE] Intent rejected: ARCHIVE offline.");
                return;
            }

            LastPlayerIntent = intent;

            Debug.Log("[ARCHIVE] PLAYER INTENT RECEIVED: " + intent);

            AnalyzeIntent(intent);
        }

        private void AnalyzeIntent(IntentType intent)
        {
            activeOrders.Clear();

            ArchiveOrderType orderType;

            switch (intent)
            {
                case IntentType.Move:
                    orderType = ArchiveOrderType.Advance;
                    break;

                case IntentType.Attack:
                    orderType = ArchiveOrderType.Attack;
                    break;

                case IntentType.Defend:
                    orderType = ArchiveOrderType.Defend;
                    break;

                case IntentType.Hold:
                    orderType = ArchiveOrderType.Hold;
                    break;

                case IntentType.Capture:
                    orderType = ArchiveOrderType.Capture;
                    break;

                case IntentType.Recon:
                    orderType = ArchiveOrderType.Recon;
                    break;

                case IntentType.Reinforce:
                    orderType = ArchiveOrderType.Reinforce;
                    break;

                case IntentType.Retreat:
                    orderType = ArchiveOrderType.Retreat;
                    break;

                // Retained legacy intents.
                case IntentType.Flank:
                    orderType = ArchiveOrderType.Flank;
                    break;

                case IntentType.Suppress:
                    orderType = ArchiveOrderType.Suppress;
                    break;

                case IntentType.Breach:
                    orderType = ArchiveOrderType.Breach;
                    break;

                case IntentType.Pursue:
                    orderType = ArchiveOrderType.Pursue;
                    break;

                default:
                    Debug.LogWarning(
                        "[ARCHIVE] Unsupported intent: " + intent);
                    return;
            }

            AddOrder(orderType, "ARCHIVE " + intent + " operational plan");
            PublishOperationalPlan();
        }

        private void AddOrder(ArchiveOrderType type, string reason)
        {
            ArchiveOperationalOrder order =
                new ArchiveOperationalOrder(
                    Guid.NewGuid().ToString(),
                    type,
                    reason,
                    transform.position);

            activeOrders.Add(order);
            DispatchOrderToSubordinates(order);
        }

        private void DispatchOrderToSubordinates(
            ArchiveOperationalOrder order)
        {
            if (commandUnit == null)
                return;

            foreach (SelectableUnit unit in commandUnit.CommandedUnits)
            {
                if (unit == null)
                    continue;

                UnitAutonomy autonomy = unit.GetComponent<UnitAutonomy>();

                if (autonomy == null)
                {
                    Debug.LogWarning(
                        "[ARCHIVE] No UnitAutonomy on " + unit.gameObject.name);
                    continue;
                }

                UnitAutonomy.AutonomousState state;

                switch (order.Type)
                {
                    case ArchiveOrderType.Advance:
                    case ArchiveOrderType.Pursue:
                    case ArchiveOrderType.Flank:
                    case ArchiveOrderType.Breach:
                        state = UnitAutonomy.AutonomousState.Pursue;
                        break;

                    case ArchiveOrderType.Attack:
                    case ArchiveOrderType.Suppress:
                        state = UnitAutonomy.AutonomousState.Engage;
                        break;

                    case ArchiveOrderType.Defend:
                    case ArchiveOrderType.Hold:
                        state = UnitAutonomy.AutonomousState.Idle;
                        break;

                    case ArchiveOrderType.Recon:
                    case ArchiveOrderType.Capture:
                    case ArchiveOrderType.Reinforce:
                        state = UnitAutonomy.AutonomousState.Investigate;
                        break;

                    case ArchiveOrderType.Retreat:
                        state = UnitAutonomy.AutonomousState.Retreat;
                        break;

                    default:
                        state = UnitAutonomy.AutonomousState.Patrol;
                        break;
                }

                autonomy.SetAutonomousIntent(
                    state,
                    order.ObjectivePosition);

                Debug.Log(
                    "[ARCHIVE] ORDER DISPATCHED: " +
                    order.Type + " -> " + unit.gameObject.name);
            }
        }

        private void PublishOperationalPlan()
        {
            Debug.Log(
                "[ARCHIVE] OPERATIONAL PLAN CREATED: " +
                activeOrders.Count + " order(s).");

            foreach (ArchiveOperationalOrder order in activeOrders)
            {
                Debug.Log(
                    "[ARCHIVE] ORDER: " +
                    order.Type + " | " + order.Reason);
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
        Reinforce,
        Hold,
        Capture
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

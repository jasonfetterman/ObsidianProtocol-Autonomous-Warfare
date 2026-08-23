using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class ModerationAction
    {
        public string ActionId { get; }

        public string PlayerId { get; }

        public string ActionType { get; }

        public string Reason { get; }

        public DateTime CreatedAtUtc { get; }

        public bool Active { get; private set; }

        public ModerationAction(
            string actionId,
            string playerId,
            string actionType,
            string reason)
        {
            ActionId =
                actionId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            ActionType =
                actionType ?? string.Empty;

            Reason =
                reason ?? string.Empty;

            CreatedAtUtc =
                DateTime.UtcNow;

            Active = true;
        }

        public bool Revoke()
        {
            if (!Active)
            {
                return false;
            }

            Active = false;

            return true;
        }
    }

    public sealed class ModerationTools
    {
        private readonly Dictionary<
            string,
            ModerationAction> actions =
            new Dictionary<
                string,
                ModerationAction>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ActionCount =>
            actions.Count;

        public int ActiveActionCount
        {
            get
            {
                int count = 0;

                foreach (ModerationAction action
                         in actions.Values)
                {
                    if (action.Active)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            actions.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateAction(
            string actionId,
            string playerId,
            string actionType,
            string reason)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(actionId) ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(actionType) ||
                string.IsNullOrWhiteSpace(reason))
            {
                return false;
            }

            string id =
                actionId.Trim();

            if (actions.ContainsKey(id))
            {
                return false;
            }

            actions.Add(
                id,
                new ModerationAction(
                    id,
                    playerId.Trim(),
                    actionType.Trim(),
                    reason.Trim()));

            return true;
        }

        public bool RevokeAction(
            string actionId)
        {
            ModerationAction action =
                GetAction(actionId);

            return action != null &&
                   action.Revoke();
        }

        public ModerationAction GetAction(
            string actionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(actionId))
            {
                return null;
            }

            actions.TryGetValue(
                actionId.Trim(),
                out ModerationAction action);

            return action;
        }

        public IReadOnlyCollection<
            ModerationAction>
            GetActions()
        {
            return actions.Values;
        }

        public void Reset()
        {
            actions.Clear();
            Initialized = false;
        }
    }
}

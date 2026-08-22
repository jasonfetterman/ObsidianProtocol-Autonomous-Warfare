using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public enum AutonomousActivityType
    {
        Moving,
        Advancing,
        Attacking,
        Defending,
        Flanking,
        Suppressing,
        Breaching,
        Pursuing,
        Retreating,
        Reinforcing,
        Scouting,
        EstablishingPosition,
        Repairing,
        AwaitingOrders
    }

    public sealed class AutonomousActivity
    {
        public string ActivityId { get; }
        public string UnitId { get; }
        public string SquadId { get; }

        public AutonomousActivityType Type { get; private set; }

        public string Description { get; private set; }

        public float Progress { get; private set; }

        public bool Active { get; private set; }

        public AutonomousActivity(
            string activityId,
            string unitId,
            string squadId,
            AutonomousActivityType type,
            string description)
        {
            ActivityId = activityId ?? string.Empty;
            UnitId = unitId ?? string.Empty;
            SquadId = squadId ?? string.Empty;

            Type = type;

            Description =
                description ?? string.Empty;

            Progress = 0f;
            Active = true;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(ActivityId);

        public void SetType(
            AutonomousActivityType type)
        {
            Type = type;
        }

        public void SetDescription(
            string description)
        {
            Description =
                description ?? string.Empty;
        }

        public void SetProgress(
            float progress)
        {
            Progress =
                Math.Max(
                    0f,
                    Math.Min(
                        1f,
                        progress));
        }

        public void Activate()
        {
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }
    }

    public sealed class AutonomousActivityFeedback
    {
        private readonly Dictionary<
            string,
            AutonomousActivity> activities =
            new Dictionary<
                string,
                AutonomousActivity>(
                StringComparer.OrdinalIgnoreCase);

        public bool Visible { get; private set; }

        public AutonomousActivityFeedback()
        {
            Visible = true;
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public bool Register(
            AutonomousActivity activity)
        {
            if (activity == null ||
                !activity.Valid ||
                activities.ContainsKey(
                    activity.ActivityId))
            {
                return false;
            }

            activities.Add(
                activity.ActivityId,
                activity);

            return true;
        }

        public bool Remove(
            string activityId)
        {
            if (string.IsNullOrWhiteSpace(
                    activityId))
            {
                return false;
            }

            return activities.Remove(
                activityId);
        }

        public bool TryGet(
            string activityId,
            out AutonomousActivity activity)
        {
            return activities.TryGetValue(
                activityId,
                out activity);
        }

        public bool SetProgress(
            string activityId,
            float progress)
        {
            if (!activities.TryGetValue(
                    activityId,
                    out AutonomousActivity activity))
            {
                return false;
            }

            activity.SetProgress(progress);
            return true;
        }

        public IReadOnlyCollection<
            AutonomousActivity>
            GetActivities()
        {
            return activities.Values;
        }

        public void Clear()
        {
            activities.Clear();
        }

        public void Reset()
        {
            Visible = true;
            activities.Clear();
        }
    }
}

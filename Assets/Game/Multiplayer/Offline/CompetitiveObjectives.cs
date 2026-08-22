using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public enum CompetitiveObjectiveOwner
    {
        Neutral,
        Player1,
        Player2
    }

    public enum CompetitiveObjectiveState
    {
        Inactive,
        Active,
        Contested,
        Captured,
        Completed
    }

    public sealed class CompetitiveObjective
    {
        public string ObjectiveId { get; }

        public string Description { get; }

        public CompetitiveObjectiveOwner Owner
        {
            get;
            private set;
        }

        public CompetitiveObjectiveState State
        {
            get;
            private set;
        }

        public CompetitiveObjective(
            string objectiveId,
            string description)
        {
            ObjectiveId = objectiveId ?? string.Empty;
            Description = description ?? string.Empty;
            Owner = CompetitiveObjectiveOwner.Neutral;
            State = CompetitiveObjectiveState.Inactive;
        }

        public bool Activate()
        {
            if (State != CompetitiveObjectiveState.Inactive)
            {
                return false;
            }

            State = CompetitiveObjectiveState.Active;

            return true;
        }

        public bool SetContested()
        {
            if (State != CompetitiveObjectiveState.Active &&
                State != CompetitiveObjectiveState.Captured)
            {
                return false;
            }

            State = CompetitiveObjectiveState.Contested;

            return true;
        }

        public bool Capture(
            CompetitiveObjectiveOwner player)
        {
            if (player == CompetitiveObjectiveOwner.Neutral)
            {
                return false;
            }

            if (State != CompetitiveObjectiveState.Active &&
                State != CompetitiveObjectiveState.Contested &&
                State != CompetitiveObjectiveState.Captured)
            {
                return false;
            }

            Owner = player;
            State = CompetitiveObjectiveState.Captured;

            return true;
        }

        public bool Complete()
        {
            if (State != CompetitiveObjectiveState.Captured)
            {
                return false;
            }

            State = CompetitiveObjectiveState.Completed;

            return true;
        }

        public bool IsOwnedBy(
            CompetitiveObjectiveOwner player)
        {
            return Owner == player;
        }
    }

    public sealed class CompetitiveObjectives
    {
        private readonly Dictionary<
            string,
            CompetitiveObjective> objectives =
            new Dictionary<
                string,
                CompetitiveObjective>(
                StringComparer.OrdinalIgnoreCase);

        private SharedWorldState worldState;

        public bool Initialized { get; private set; }

        public int ObjectiveCount =>
            objectives.Count;

        public int Player1ObjectiveCount
        {
            get
            {
                int count = 0;

                foreach (CompetitiveObjective objective
                    in objectives.Values)
                {
                    if (objective.Owner ==
                        CompetitiveObjectiveOwner.Player1)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public int Player2ObjectiveCount
        {
            get
            {
                int count = 0;

                foreach (CompetitiveObjective objective
                    in objectives.Values)
                {
                    if (objective.Owner ==
                        CompetitiveObjectiveOwner.Player2)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize(
            SharedWorldState sharedWorldState)
        {
            if (Initialized ||
                sharedWorldState == null ||
                !sharedWorldState.Initialized)
            {
                return false;
            }

            worldState = sharedWorldState;
            objectives.Clear();
            Initialized = true;

            return true;
        }

        public bool AddObjective(
            string objectiveId,
            string description)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId))
            {
                return false;
            }

            string id = objectiveId.Trim();

            if (objectives.ContainsKey(id))
            {
                return false;
            }

            CompetitiveObjective objective =
                new CompetitiveObjective(
                    id,
                    description);

            objectives.Add(id, objective);

            worldState.RegisterObjective(id);

            return true;
        }

        public bool ActivateObjective(
            string objectiveId)
        {
            CompetitiveObjective objective =
                GetObjective(objectiveId);

            return objective != null &&
                   objective.Activate();
        }

        public bool ContestObjective(
            string objectiveId)
        {
            CompetitiveObjective objective =
                GetObjective(objectiveId);

            return objective != null &&
                   objective.SetContested();
        }

        public bool CaptureObjective(
            string objectiveId,
            CompetitiveObjectiveOwner player)
        {
            CompetitiveObjective objective =
                GetObjective(objectiveId);

            return objective != null &&
                   objective.Capture(player);
        }

        public bool CompleteObjective(
            string objectiveId)
        {
            CompetitiveObjective objective =
                GetObjective(objectiveId);

            return objective != null &&
                   objective.Complete();
        }

        public CompetitiveObjective GetObjective(
            string objectiveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId))
            {
                return null;
            }

            objectives.TryGetValue(
                objectiveId.Trim(),
                out CompetitiveObjective objective);

            return objective;
        }

        public IReadOnlyCollection<
            CompetitiveObjective>
            GetObjectives()
        {
            return objectives.Values;
        }

        public bool Player1HasMajority()
        {
            return Player1ObjectiveCount >
                   Player2ObjectiveCount;
        }

        public bool Player2HasMajority()
        {
            return Player2ObjectiveCount >
                   Player1ObjectiveCount;
        }

        public void Reset()
        {
            objectives.Clear();
            worldState = null;
            Initialized = false;
        }
    }
}

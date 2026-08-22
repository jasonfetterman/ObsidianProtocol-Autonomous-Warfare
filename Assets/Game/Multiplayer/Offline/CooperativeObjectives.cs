using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public enum CooperativeObjectiveState
    {
        Inactive,
        Active,
        Completed,
        Failed
    }

    public sealed class CooperativeObjective
    {
        public string ObjectiveId { get; }

        public string Description { get; }

        public CooperativeObjectiveState State { get; private set; }

        public CooperativeObjective(
            string objectiveId,
            string description)
        {
            ObjectiveId =
                objectiveId ?? string.Empty;

            Description =
                description ?? string.Empty;

            State =
                CooperativeObjectiveState.Inactive;
        }

        public bool Activate()
        {
            if (State !=
                CooperativeObjectiveState.Inactive)
            {
                return false;
            }

            State =
                CooperativeObjectiveState.Active;

            return true;
        }

        public bool Complete()
        {
            if (State !=
                CooperativeObjectiveState.Active)
            {
                return false;
            }

            State =
                CooperativeObjectiveState.Completed;

            return true;
        }

        public bool Fail()
        {
            if (State !=
                CooperativeObjectiveState.Active)
            {
                return false;
            }

            State =
                CooperativeObjectiveState.Failed;

            return true;
        }
    }

    public sealed class CooperativeObjectives
    {
        private readonly Dictionary<string, CooperativeObjective>
            objectives =
                new Dictionary<string, CooperativeObjective>(
                    StringComparer.OrdinalIgnoreCase);

        private SharedWorldState worldState;

        public bool Initialized { get; private set; }

        public int ObjectiveCount =>
            objectives.Count;

        public int ActiveObjectiveCount
        {
            get
            {
                int count = 0;

                foreach (CooperativeObjective objective
                    in objectives.Values)
                {
                    if (objective.State ==
                        CooperativeObjectiveState.Active)
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

            worldState =
                sharedWorldState;

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

            string id =
                objectiveId.Trim();

            if (objectives.ContainsKey(id))
            {
                return false;
            }

            CooperativeObjective objective =
                new CooperativeObjective(
                    id,
                    description);

            objectives.Add(
                id,
                objective);

            worldState.RegisterObjective(id);

            return true;
        }

        public bool ActivateObjective(
            string objectiveId)
        {
            CooperativeObjective objective =
                GetObjective(objectiveId);

            return objective != null &&
                   objective.Activate();
        }

        public bool CompleteObjective(
            string objectiveId)
        {
            CooperativeObjective objective =
                GetObjective(objectiveId);

            return objective != null &&
                   objective.Complete();
        }

        public bool FailObjective(
            string objectiveId)
        {
            CooperativeObjective objective =
                GetObjective(objectiveId);

            return objective != null &&
                   objective.Fail();
        }

        public CooperativeObjective
            GetObjective(
                string objectiveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId))
            {
                return null;
            }

            objectives.TryGetValue(
                objectiveId.Trim(),
                out CooperativeObjective objective);

            return objective;
        }

        public IReadOnlyCollection<CooperativeObjective>
            GetObjectives()
        {
            return objectives.Values;
        }

        public bool AreAllObjectivesComplete()
        {
            if (!Initialized ||
                objectives.Count == 0)
            {
                return false;
            }

            foreach (CooperativeObjective objective
                in objectives.Values)
            {
                if (objective.State !=
                    CooperativeObjectiveState.Completed)
                {
                    return false;
                }
            }

            return true;
        }

        public void Reset()
        {
            objectives.Clear();
            worldState = null;
            Initialized = false;
        }
    }
}

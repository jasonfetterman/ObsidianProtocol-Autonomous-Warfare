using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignObjective
    {
        public string ObjectiveId { get; }

        public string Description { get; }

        public bool Required { get; }

        public bool Completed { get; private set; }

        public CampaignObjective(
            string objectiveId,
            string description,
            bool required)
        {
            ObjectiveId =
                objectiveId ?? string.Empty;

            Description =
                description ?? string.Empty;

            Required =
                required;

            Completed = false;
        }

        public bool Complete()
        {
            if (Completed)
            {
                return false;
            }

            Completed = true;

            return true;
        }

        public bool Reset()
        {
            if (!Completed)
            {
                return false;
            }

            Completed = false;

            return true;
        }
    }

    public sealed class CampaignObjectives
    {
        private readonly Dictionary<
            string,
            CampaignObjective> objectives =
            new Dictionary<
                string,
                CampaignObjective>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ObjectiveCount =>
            objectives.Count;

        public int CompletedObjectiveCount
        {
            get
            {
                int count = 0;

                foreach (CampaignObjective objective
                         in objectives.Values)
                {
                    if (objective.Completed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool RequiredObjectivesComplete
        {
            get
            {
                foreach (CampaignObjective objective
                         in objectives.Values)
                {
                    if (objective.Required &&
                        !objective.Completed)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            objectives.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterObjective(
            string objectiveId,
            string description,
            bool required)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId) ||
                string.IsNullOrWhiteSpace(description))
            {
                return false;
            }

            string id =
                objectiveId.Trim();

            if (objectives.ContainsKey(id))
            {
                return false;
            }

            objectives.Add(
                id,
                new CampaignObjective(
                    id,
                    description.Trim(),
                    required));

            return true;
        }

        public bool CompleteObjective(
            string objectiveId)
        {
            CampaignObjective objective =
                GetObjective(objectiveId);

            return objective != null &&
                   objective.Complete();
        }

        public CampaignObjective GetObjective(
            string objectiveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId))
            {
                return null;
            }

            objectives.TryGetValue(
                objectiveId.Trim(),
                out CampaignObjective objective);

            return objective;
        }

        public IReadOnlyCollection<
            CampaignObjective>
            GetObjectives()
        {
            return objectives.Values;
        }

        public void Reset()
        {
            objectives.Clear();
            Initialized = false;
        }
    }
}

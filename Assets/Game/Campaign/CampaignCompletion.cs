using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignCompletion
    {
        private readonly Dictionary<
            string,
            bool> requirements =
            new Dictionary<
                string,
                bool>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int RequirementCount =>
            requirements.Count;

        public int CompletedRequirementCount
        {
            get
            {
                int count = 0;

                foreach (bool complete
                         in requirements.Values)
                {
                    if (complete)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Complete
        {
            get
            {
                if (!Initialized ||
                    requirements.Count == 0)
                {
                    return false;
                }

                foreach (bool complete
                         in requirements.Values)
                {
                    if (!complete)
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

            requirements.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterRequirement(
            string requirementId,
            bool completed)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(requirementId))
            {
                return false;
            }

            string id =
                requirementId.Trim();

            if (requirements.ContainsKey(id))
            {
                return false;
            }

            requirements.Add(
                id,
                completed);

            return true;
        }

        public bool SetRequirementComplete(
            string requirementId,
            bool completed)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(requirementId))
            {
                return false;
            }

            string id =
                requirementId.Trim();

            if (!requirements.ContainsKey(id))
            {
                return false;
            }

            requirements[id] =
                completed;

            return true;
        }

        public bool IsRequirementComplete(
            string requirementId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(requirementId))
            {
                return false;
            }

            return requirements.TryGetValue(
                requirementId.Trim(),
                out bool completed) &&
                   completed;
        }

        public IReadOnlyDictionary<
            string,
            bool>
            GetRequirements()
        {
            return requirements;
        }

        public void Reset()
        {
            requirements.Clear();
            Initialized = false;
        }
    }
}

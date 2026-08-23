using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Release
{
    public sealed class SteamPreparation
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

        public int CompletedCount
        {
            get
            {
                int count = 0;

                foreach (bool completed
                         in requirements.Values)
                {
                    if (completed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool ReadyForSteamSubmission =>
            Initialized &&
            RequirementCount > 0 &&
            CompletedCount == RequirementCount;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            requirements.Clear();

            RegisterRequirement(
                "SteamAppConfiguration");

            RegisterRequirement(
                "SteamBuildConfiguration");

            RegisterRequirement(
                "SteamDepotConfiguration");

            RegisterRequirement(
                "StorePage");

            RegisterRequirement(
                "GameDescription");

            RegisterRequirement(
                "Screenshots");

            RegisterRequirement(
                "Trailer");

            RegisterRequirement(
                "CapsuleArtwork");

            RegisterRequirement(
                "SystemRequirements");

            RegisterRequirement(
                "AgeRating");

            RegisterRequirement(
                "PrivacyPolicy");

            RegisterRequirement(
                "TermsOfService");

            RegisterRequirement(
                "ControllerSupport");

            RegisterRequirement(
                "VRInformation");

            RegisterRequirement(
                "MultiplayerInformation");

            RegisterRequirement(
                "BuildValidation");

            Initialized = true;

            return true;
        }

        public bool RegisterRequirement(
            string requirementId)
        {
            if (string.IsNullOrWhiteSpace(
                    requirementId))
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
                false);

            return true;
        }

        public bool CompleteRequirement(
            string requirementId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(
                    requirementId))
            {
                return false;
            }

            string id =
                requirementId.Trim();

            if (!requirements.ContainsKey(id))
            {
                return false;
            }

            requirements[id] = true;

            return true;
        }

        public bool IsRequirementComplete(
            string requirementId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(
                    requirementId))
            {
                return false;
            }

            requirements.TryGetValue(
                requirementId.Trim(),
                out bool completed);

            return completed;
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

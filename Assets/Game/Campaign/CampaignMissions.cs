using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignMission
    {
        public string MissionId { get; }

        public string MissionName { get; }

        public bool Unlocked { get; private set; }

        public bool Completed { get; private set; }

        public CampaignMission(
            string missionId,
            string missionName)
        {
            MissionId =
                missionId ?? string.Empty;

            MissionName =
                missionName ?? string.Empty;

            Unlocked = false;
            Completed = false;
        }

        public bool Unlock()
        {
            if (Unlocked)
            {
                return false;
            }

            Unlocked = true;

            return true;
        }

        public bool Complete()
        {
            if (!Unlocked ||
                Completed)
            {
                return false;
            }

            Completed = true;

            return true;
        }
    }

    public sealed class CampaignMissions
    {
        private readonly Dictionary<
            string,
            CampaignMission> missions =
            new Dictionary<
                string,
                CampaignMission>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int MissionCount =>
            missions.Count;

        public int CompletedMissionCount
        {
            get
            {
                int count = 0;

                foreach (CampaignMission mission
                         in missions.Values)
                {
                    if (mission.Completed)
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

            missions.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterMission(
            string missionId,
            string missionName)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(missionId) ||
                string.IsNullOrWhiteSpace(missionName))
            {
                return false;
            }

            string id =
                missionId.Trim();

            if (missions.ContainsKey(id))
            {
                return false;
            }

            missions.Add(
                id,
                new CampaignMission(
                    id,
                    missionName.Trim()));

            return true;
        }

        public bool UnlockMission(
            string missionId)
        {
            CampaignMission mission =
                GetMission(missionId);

            return mission != null &&
                   mission.Unlock();
        }

        public bool CompleteMission(
            string missionId)
        {
            CampaignMission mission =
                GetMission(missionId);

            return mission != null &&
                   mission.Complete();
        }

        public CampaignMission GetMission(
            string missionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(missionId))
            {
                return null;
            }

            missions.TryGetValue(
                missionId.Trim(),
                out CampaignMission mission);

            return mission;
        }

        public IReadOnlyCollection<
            CampaignMission>
            GetMissions()
        {
            return missions.Values;
        }

        public void Reset()
        {
            missions.Clear();
            Initialized = false;
        }
    }
}

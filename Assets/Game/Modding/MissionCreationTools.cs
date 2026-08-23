using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class MissionCreationDefinition
    {
        public string MissionId { get; }

        public string MissionName { get; }

        public string MissionType { get; }

        public bool Enabled { get; private set; }

        public MissionCreationDefinition(
            string missionId,
            string missionName,
            string missionType)
        {
            MissionId =
                missionId ?? string.Empty;

            MissionName =
                missionName ?? string.Empty;

            MissionType =
                missionType ?? string.Empty;

            Enabled = true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }
    }

    public sealed class MissionCreationTools
    {
        private readonly Dictionary<
            string,
            MissionCreationDefinition> missions =
            new Dictionary<
                string,
                MissionCreationDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int MissionCount =>
            missions.Count;

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

        public bool CreateMission(
            string missionId,
            string missionName,
            string missionType)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(missionId) ||
                string.IsNullOrWhiteSpace(missionName) ||
                string.IsNullOrWhiteSpace(missionType))
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
                new MissionCreationDefinition(
                    id,
                    missionName.Trim(),
                    missionType.Trim()));

            return true;
        }

        public bool RemoveMission(
            string missionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(missionId))
            {
                return false;
            }

            return missions.Remove(
                missionId.Trim());
        }

        public MissionCreationDefinition GetMission(
            string missionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(missionId))
            {
                return null;
            }

            missions.TryGetValue(
                missionId.Trim(),
                out MissionCreationDefinition mission);

            return mission;
        }

        public IReadOnlyCollection<
            MissionCreationDefinition>
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

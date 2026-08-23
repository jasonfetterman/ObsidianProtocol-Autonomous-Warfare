using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class LODProfile
    {
        public string ProfileId { get; }

        public float[] Distances { get; }

        public int LevelCount =>
            Distances.Length + 1;

        public LODProfile(
            string profileId,
            params float[] distances)
        {
            ProfileId =
                profileId ?? string.Empty;

            Distances =
                distances ?? Array.Empty<float>();

            ValidateDistances();
        }

        public int GetLevel(
            float distance)
        {
            if (distance <= 0f)
            {
                return 0;
            }

            for (int i = 0;
                 i < Distances.Length;
                 i++)
            {
                if (distance < Distances[i])
                {
                    return i;
                }
            }

            return Distances.Length;
        }

        private void ValidateDistances()
        {
            for (int i = 0;
                 i < Distances.Length;
                 i++)
            {
                if (Distances[i] < 0f)
                {
                    Distances[i] = 0f;
                }

                if (i > 0 &&
                    Distances[i] < Distances[i - 1])
                {
                    Distances[i] =
                        Distances[i - 1];
                }
            }
        }
    }

    public sealed class LODSystem
    {
        private readonly Dictionary<
            string,
            LODProfile> profiles =
            new Dictionary<
                string,
                LODProfile>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ProfileCount =>
            profiles.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            profiles.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterProfile(
            string profileId,
            params float[] distances)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId))
            {
                return false;
            }

            string id =
                profileId.Trim();

            if (profiles.ContainsKey(id))
            {
                return false;
            }

            profiles.Add(
                id,
                new LODProfile(
                    id,
                    distances));

            return true;
        }

        public bool RemoveProfile(
            string profileId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId))
            {
                return false;
            }

            return profiles.Remove(
                profileId.Trim());
        }

        public int GetLODLevel(
            string profileId,
            float distance)
        {
            LODProfile profile =
                GetProfile(profileId);

            return profile == null
                ? 0
                : profile.GetLevel(distance);
        }

        public LODProfile GetProfile(
            string profileId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId))
            {
                return null;
            }

            profiles.TryGetValue(
                profileId.Trim(),
                out LODProfile profile);

            return profile;
        }

        public IReadOnlyCollection<LODProfile>
            GetProfiles()
        {
            return profiles.Values;
        }

        public void Reset()
        {
            profiles.Clear();

            Initialized = false;
        }
    }
}

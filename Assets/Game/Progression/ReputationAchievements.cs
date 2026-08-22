using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Progression
{
    public enum ReputationCategory
    {
        Command,
        Fleet,
        Research,
        Logistics,
        Autonomy,
        Combat,
        Campaign,
        Multiplayer
    }

    public sealed class Reputation
    {
        private readonly Dictionary<
            ReputationCategory,
            int> values =
            new Dictionary<
                ReputationCategory,
                int>();

        public Reputation()
        {
            foreach (ReputationCategory category
                in Enum.GetValues(
                    typeof(ReputationCategory)))
            {
                values[category] = 0;
            }
        }

        public int Get(
            ReputationCategory category)
        {
            return values.TryGetValue(
                       category,
                       out int value)
                ? value
                : 0;
        }

        public void Add(
            ReputationCategory category,
            int amount)
        {
            if (amount == 0)
                return;

            int current = Get(category);

            long result =
                (long)current + amount;

            result =
                Math.Max(
                    0L,
                    Math.Min(
                        int.MaxValue,
                        result));

            values[category] =
                (int)result;
        }

        public void Set(
            ReputationCategory category,
            int value)
        {
            values[category] =
                Math.Max(0, value);
        }

        public int Total
        {
            get
            {
                long total = 0;

                foreach (int value
                    in values.Values)
                {
                    total += value;
                }

                return (int)Math.Min(
                    int.MaxValue,
                    total);
            }
        }

        public void Reset()
        {
            foreach (ReputationCategory category
                in Enum.GetValues(
                    typeof(ReputationCategory)))
            {
                values[category] = 0;
            }
        }
    }

    public sealed class Achievement
    {
        public string AchievementId { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public bool Unlocked
        {
            get;
            private set;
        }

        public DateTime? UnlockedAt
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                AchievementId) &&
            !string.IsNullOrWhiteSpace(
                DisplayName);

        public Achievement(
            string achievementId,
            string displayName,
            string description)
        {
            AchievementId =
                achievementId ?? string.Empty;

            DisplayName =
                displayName ?? string.Empty;

            Description =
                description ?? string.Empty;

            Unlocked = false;
            UnlockedAt = null;
        }

        public bool Unlock()
        {
            if (Unlocked)
                return false;

            Unlocked = true;
            UnlockedAt = DateTime.UtcNow;

            return true;
        }

        public void Lock()
        {
            Unlocked = false;
            UnlockedAt = null;
        }
    }

    public sealed class AchievementRegistry
    {
        private readonly Dictionary<
            string,
            Achievement> achievements =
            new Dictionary<
                string,
                Achievement>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            Achievement achievement)
        {
            if (achievement == null ||
                !achievement.Valid ||
                achievements.ContainsKey(
                    achievement.AchievementId))
            {
                return false;
            }

            achievements.Add(
                achievement.AchievementId,
                achievement);

            return true;
        }

        public bool Remove(
            string achievementId)
        {
            if (string.IsNullOrWhiteSpace(
                    achievementId))
            {
                return false;
            }

            return achievements.Remove(
                achievementId);
        }

        public bool TryGet(
            string achievementId,
            out Achievement achievement)
        {
            return achievements.TryGetValue(
                achievementId,
                out achievement);
        }

        public bool Unlock(
            string achievementId)
        {
            if (!achievements.TryGetValue(
                    achievementId,
                    out Achievement achievement))
            {
                return false;
            }

            return achievement.Unlock();
        }

        public bool IsUnlocked(
            string achievementId)
        {
            return achievements.TryGetValue(
                       achievementId,
                       out Achievement achievement) &&
                   achievement.Unlocked;
        }

        public IReadOnlyCollection<
            Achievement>
            GetAchievements()
        {
            return achievements.Values;
        }

        public void Clear()
        {
            achievements.Clear();
        }
    }

    public sealed class ReputationAchievementFramework
    {
        public Reputation Reputation
        {
            get;
        }

        public AchievementRegistry Achievements
        {
            get;
        }

        public ReputationAchievementFramework()
        {
            Reputation =
                new Reputation();

            Achievements =
                new AchievementRegistry();
        }

        public bool RecordAchievement(
            string achievementId,
            ReputationCategory category,
            int reputationReward)
        {
            if (!Achievements.Unlock(
                    achievementId))
            {
                return false;
            }

            Reputation.Add(
                category,
                reputationReward);

            return true;
        }

        public void Reset()
        {
            Reputation.Reset();
            Achievements.Clear();
        }
    }
}

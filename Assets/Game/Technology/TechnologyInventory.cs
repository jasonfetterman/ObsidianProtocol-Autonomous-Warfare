using System.Collections.Generic;

namespace ObsidianProtocol.Game.Technology
{
    public sealed class TechnologyInventory
    {
        private readonly HashSet<string> unlocked = new();

        public bool IsUnlocked(string technologyId)
        {
            return !string.IsNullOrEmpty(technologyId) &&
                   unlocked.Contains(technologyId);
        }

        public bool Unlock(string technologyId)
        {
            if (string.IsNullOrEmpty(technologyId))
            {
                return false;
            }

            return unlocked.Add(technologyId);
        }

        public List<string> GetUnlockedIds()
        {
            return new List<string>(unlocked);
        }

        public void RestoreUnlockedIds(IEnumerable<string> technologyIds)
        {
            unlocked.Clear();

            if (technologyIds == null)
            {
                return;
            }

            foreach (string technologyId in technologyIds)
            {
                if (!string.IsNullOrWhiteSpace(technologyId))
                {
                    unlocked.Add(technologyId);
                }
            }
        }
        public void Lock(string technologyId)
        {
            if (!string.IsNullOrEmpty(technologyId))
            {
                unlocked.Remove(technologyId);
            }
        }
    }
}

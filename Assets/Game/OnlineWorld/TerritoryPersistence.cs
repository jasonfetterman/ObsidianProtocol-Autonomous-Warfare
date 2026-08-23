using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class TerritoryPersistence
    {
        private readonly Dictionary<
            string,
            string> territories =
            new Dictionary<
                string,
                string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TerritoryCount =>
            territories.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            territories.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterTerritory(
            string territoryId,
            string ownerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId) ||
                string.IsNullOrWhiteSpace(ownerId))
            {
                return false;
            }

            string id =
                territoryId.Trim();

            if (territories.ContainsKey(id))
            {
                return false;
            }

            territories.Add(
                id,
                ownerId.Trim());

            return true;
        }

        public bool SetOwner(
            string territoryId,
            string ownerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId) ||
                string.IsNullOrWhiteSpace(ownerId))
            {
                return false;
            }

            string id =
                territoryId.Trim();

            if (!territories.ContainsKey(id))
            {
                return false;
            }

            territories[id] =
                ownerId.Trim();

            return true;
        }

        public string GetOwner(
            string territoryId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId))
            {
                return null;
            }

            territories.TryGetValue(
                territoryId.Trim(),
                out string ownerId);

            return ownerId;
        }

        public bool RemoveTerritory(
            string territoryId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId))
            {
                return false;
            }

            return territories.Remove(
                territoryId.Trim());
        }

        public IReadOnlyDictionary<
            string,
            string>
            GetTerritories()
        {
            return territories;
        }

        public void Reset()
        {
            territories.Clear();
            Initialized = false;
        }
    }
}

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public enum TerritoryOwner
    {
        Neutral,
        Player1,
        Player2
    }

    public sealed class PlayerTerritory
    {
        public string TerritoryId { get; }

        public TerritoryOwner Owner { get; private set; }

        public bool Contested { get; private set; }

        public PlayerTerritory(
            string territoryId)
        {
            TerritoryId =
                territoryId ?? string.Empty;

            Owner =
                TerritoryOwner.Neutral;

            Contested = false;
        }

        public bool SetOwner(
            TerritoryOwner owner)
        {
            Owner = owner;
            Contested = false;

            return true;
        }

        public bool SetContested(
            bool contested)
        {
            Contested = contested;

            return true;
        }
    }

    public sealed class PlayerTerritorySystem
    {
        private readonly Dictionary<
            string,
            PlayerTerritory> territories =
            new Dictionary<
                string,
                PlayerTerritory>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TerritoryCount =>
            territories.Count;

        public int Player1TerritoryCount
        {
            get
            {
                int count = 0;

                foreach (PlayerTerritory territory
                    in territories.Values)
                {
                    if (territory.Owner ==
                        TerritoryOwner.Player1)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public int Player2TerritoryCount
        {
            get
            {
                int count = 0;

                foreach (PlayerTerritory territory
                    in territories.Values)
                {
                    if (territory.Owner ==
                        TerritoryOwner.Player2)
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

            territories.Clear();
            Initialized = true;

            return true;
        }

        public bool AddTerritory(
            string territoryId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId))
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
                new PlayerTerritory(id));

            return true;
        }

        public bool ClaimTerritory(
            string territoryId,
            TerritoryOwner owner)
        {
            if (owner ==
                TerritoryOwner.Neutral)
            {
                return false;
            }

            PlayerTerritory territory =
                GetTerritory(territoryId);

            if (territory == null)
            {
                return false;
            }

            return territory.SetOwner(owner);
        }

        public bool ContestTerritory(
            string territoryId)
        {
            PlayerTerritory territory =
                GetTerritory(territoryId);

            if (territory == null)
            {
                return false;
            }

            return territory.SetContested(true);
        }

        public PlayerTerritory GetTerritory(
            string territoryId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId))
            {
                return null;
            }

            territories.TryGetValue(
                territoryId.Trim(),
                out PlayerTerritory territory);

            return territory;
        }

        public IReadOnlyCollection<
            PlayerTerritory>
            GetTerritories()
        {
            return territories.Values;
        }

        public void Reset()
        {
            territories.Clear();
            Initialized = false;
        }
    }
}

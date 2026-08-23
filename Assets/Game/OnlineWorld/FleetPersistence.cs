using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class FleetPersistence
    {
        private readonly Dictionary<
            string,
            HashSet<string>> fleets =
            new Dictionary<
                string,
                HashSet<string>>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int FleetCount =>
            fleets.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            fleets.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterFleet(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            string id =
                playerId.Trim();

            if (fleets.ContainsKey(id))
            {
                return false;
            }

            fleets.Add(
                id,
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase));

            return true;
        }

        public bool AddUnit(
            string playerId,
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            if (!fleets.TryGetValue(
                    playerId.Trim(),
                    out HashSet<string> fleet))
            {
                return false;
            }

            return fleet.Add(
                unitId.Trim());
        }

        public bool RemoveUnit(
            string playerId,
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            if (!fleets.TryGetValue(
                    playerId.Trim(),
                    out HashSet<string> fleet))
            {
                return false;
            }

            return fleet.Remove(
                unitId.Trim());
        }

        public bool ContainsUnit(
            string playerId,
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return fleets.TryGetValue(
                playerId.Trim(),
                out HashSet<string> fleet) &&
                   fleet.Contains(
                       unitId.Trim());
        }

        public IReadOnlyCollection<string>
            GetFleet(
                string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return Array.Empty<string>();
            }

            if (!fleets.TryGetValue(
                    playerId.Trim(),
                    out HashSet<string> fleet))
            {
                return Array.Empty<string>();
            }

            return fleet;
        }

        public bool RemoveFleet(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            return fleets.Remove(
                playerId.Trim());
        }

        public void Reset()
        {
            fleets.Clear();
            Initialized = false;
        }
    }
}

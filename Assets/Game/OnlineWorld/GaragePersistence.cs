using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class GaragePersistence
    {
        private readonly Dictionary<
            string,
            HashSet<string>> garages =
            new Dictionary<
                string,
                HashSet<string>>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int GarageCount =>
            garages.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            garages.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterGarage(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            string id =
                playerId.Trim();

            if (garages.ContainsKey(id))
            {
                return false;
            }

            garages.Add(
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

            if (!garages.TryGetValue(
                    playerId.Trim(),
                    out HashSet<string> garage))
            {
                return false;
            }

            return garage.Add(
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

            if (!garages.TryGetValue(
                    playerId.Trim(),
                    out HashSet<string> garage))
            {
                return false;
            }

            return garage.Remove(
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

            return garages.TryGetValue(
                playerId.Trim(),
                out HashSet<string> garage) &&
                   garage.Contains(
                       unitId.Trim());
        }

        public IReadOnlyCollection<string>
            GetGarage(
                string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return Array.Empty<string>();
            }

            if (!garages.TryGetValue(
                    playerId.Trim(),
                    out HashSet<string> garage))
            {
                return Array.Empty<string>();
            }

            return garage;
        }

        public bool RemoveGarage(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            return garages.Remove(
                playerId.Trim());
        }

        public void Reset()
        {
            garages.Clear();
            Initialized = false;
        }
    }
}

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public sealed class OnlineSquadState
    {
        private readonly HashSet<string> unitIds =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public string SquadId { get; }

        public string OwnerPlayerId { get; private set; }

        public string SquadName { get; private set; }

        public bool Active { get; private set; }

        public long LastUpdateTick { get; private set; }

        public int UnitCount =>
            unitIds.Count;

        public OnlineSquadState(
            string squadId,
            string ownerPlayerId)
        {
            SquadId =
                squadId ?? string.Empty;

            OwnerPlayerId =
                ownerPlayerId ?? string.Empty;
        }

        public bool Update(
            string ownerPlayerId,
            string squadName,
            bool active,
            long tick)
        {
            if (string.IsNullOrWhiteSpace(SquadId))
            {
                return false;
            }

            OwnerPlayerId =
                ownerPlayerId ?? string.Empty;

            SquadName =
                squadName ?? string.Empty;

            Active = active;
            LastUpdateTick = tick;

            return true;
        }

        public bool AddUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return unitIds.Add(
                unitId.Trim());
        }

        public bool RemoveUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return unitIds.Remove(
                unitId.Trim());
        }

        public IReadOnlyCollection<string>
            GetUnitIds()
        {
            return unitIds;
        }
    }

    public sealed class OnlineSquadSynchronization
    {
        private readonly Dictionary<string, OnlineSquadState> squads =
            new Dictionary<string, OnlineSquadState>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SquadCount =>
            squads.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            squads.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterSquad(
            string squadId,
            string ownerPlayerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(squadId))
            {
                return false;
            }

            string id =
                squadId.Trim();

            if (squads.ContainsKey(id))
            {
                return false;
            }

            squads.Add(
                id,
                new OnlineSquadState(
                    id,
                    ownerPlayerId));

            return true;
        }

        public bool SynchronizeSquad(
            string squadId,
            string ownerPlayerId,
            string squadName,
            bool active,
            long tick)
        {
            OnlineSquadState squad =
                GetSquad(squadId);

            return squad != null &&
                   squad.Update(
                       ownerPlayerId,
                       squadName,
                       active,
                       tick);
        }

        public bool AddUnitToSquad(
            string squadId,
            string unitId)
        {
            OnlineSquadState squad =
                GetSquad(squadId);

            return squad != null &&
                   squad.AddUnit(unitId);
        }

        public bool RemoveUnitFromSquad(
            string squadId,
            string unitId)
        {
            OnlineSquadState squad =
                GetSquad(squadId);

            return squad != null &&
                   squad.RemoveUnit(unitId);
        }

        public OnlineSquadState GetSquad(
            string squadId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(squadId))
            {
                return null;
            }

            squads.TryGetValue(
                squadId.Trim(),
                out OnlineSquadState squad);

            return squad;
        }

        public bool RemoveSquad(
            string squadId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(squadId))
            {
                return false;
            }

            return squads.Remove(
                squadId.Trim());
        }

        public IReadOnlyCollection<OnlineSquadState>
            GetSquads()
        {
            return squads.Values;
        }

        public void Reset()
        {
            squads.Clear();
            Initialized = false;
        }
    }
}

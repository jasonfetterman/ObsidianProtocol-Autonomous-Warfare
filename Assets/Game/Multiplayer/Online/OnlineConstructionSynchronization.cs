using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineConstructionState
    {
        Planned,
        Building,
        Completed,
        Cancelled
    }

    public sealed class OnlineConstructionStateData
    {
        public string ConstructionId { get; }

        public string OwnerPlayerId { get; private set; }

        public string BuildingId { get; private set; }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public float Progress { get; private set; }

        public OnlineConstructionState State { get; private set; }

        public long LastUpdateTick { get; private set; }

        public OnlineConstructionStateData(
            string constructionId,
            string ownerPlayerId,
            string buildingId)
        {
            ConstructionId =
                constructionId ?? string.Empty;

            OwnerPlayerId =
                ownerPlayerId ?? string.Empty;

            BuildingId =
                buildingId ?? string.Empty;

            State =
                OnlineConstructionState.Planned;
        }

        public bool Update(
            string ownerPlayerId,
            string buildingId,
            float x,
            float y,
            float z,
            float progress,
            OnlineConstructionState state,
            long tick)
        {
            if (string.IsNullOrWhiteSpace(ConstructionId) ||
                string.IsNullOrWhiteSpace(BuildingId))
            {
                return false;
            }

            OwnerPlayerId =
                ownerPlayerId ?? string.Empty;

            BuildingId =
                buildingId ?? string.Empty;

            X = x;
            Y = y;
            Z = z;

            Progress =
                Math.Max(
                    0f,
                    Math.Min(1f, progress));

            State = state;
            LastUpdateTick = tick;

            return true;
        }
    }

    public sealed class OnlineConstructionSynchronization
    {
        private readonly Dictionary<
            string,
            OnlineConstructionStateData> constructions =
            new Dictionary<
                string,
                OnlineConstructionStateData>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ConstructionCount =>
            constructions.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            constructions.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterConstruction(
            string constructionId,
            string ownerPlayerId,
            string buildingId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(constructionId) ||
                string.IsNullOrWhiteSpace(buildingId))
            {
                return false;
            }

            string id =
                constructionId.Trim();

            if (constructions.ContainsKey(id))
            {
                return false;
            }

            constructions.Add(
                id,
                new OnlineConstructionStateData(
                    id,
                    ownerPlayerId,
                    buildingId));

            return true;
        }

        public bool SynchronizeConstruction(
            string constructionId,
            string ownerPlayerId,
            string buildingId,
            float x,
            float y,
            float z,
            float progress,
            OnlineConstructionState state,
            long tick)
        {
            OnlineConstructionStateData construction =
                GetConstruction(constructionId);

            return construction != null &&
                   construction.Update(
                       ownerPlayerId,
                       buildingId,
                       x,
                       y,
                       z,
                       progress,
                       state,
                       tick);
        }

        public OnlineConstructionStateData
            GetConstruction(
                string constructionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(constructionId))
            {
                return null;
            }

            constructions.TryGetValue(
                constructionId.Trim(),
                out OnlineConstructionStateData construction);

            return construction;
        }

        public bool RemoveConstruction(
            string constructionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(constructionId))
            {
                return false;
            }

            return constructions.Remove(
                constructionId.Trim());
        }

        public IReadOnlyCollection<
            OnlineConstructionStateData>
            GetConstructions()
        {
            return constructions.Values;
        }

        public void Reset()
        {
            constructions.Clear();
            Initialized = false;
        }
    }
}

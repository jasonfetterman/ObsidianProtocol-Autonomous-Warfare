using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum PersistentConstructionState
    {
        Planned,
        Materializing,
        Constructing,
        Operational,
        Suspended,
        Destroyed
    }

    public sealed class PersistentConstructionRecord
    {
        public string ConstructionId { get; }

        public string OwnerId { get; }

        public string RegionId { get; }

        public string StructureId { get; }

        public PersistentConstructionState State { get; private set; }

        public float Progress { get; private set; }

        public long LastUpdateTick { get; private set; }

        public PersistentConstructionRecord(
            string constructionId,
            string ownerId,
            string regionId,
            string structureId)
        {
            ConstructionId =
                constructionId ?? string.Empty;

            OwnerId =
                ownerId ?? string.Empty;

            RegionId =
                regionId ?? string.Empty;

            StructureId =
                structureId ?? string.Empty;

            State =
                PersistentConstructionState.Planned;

            Progress = 0f;
            LastUpdateTick = 0;
        }

        public bool SetState(
            PersistentConstructionState state,
            long updateTick)
        {
            if (updateTick < LastUpdateTick)
            {
                return false;
            }

            if (State ==
                    PersistentConstructionState.Destroyed &&
                state !=
                    PersistentConstructionState.Destroyed)
            {
                return false;
            }

            State = state;
            LastUpdateTick = updateTick;

            return true;
        }

        public bool SetProgress(
            float progress,
            long updateTick)
        {
            if (progress < 0f ||
                progress > 100f ||
                updateTick < LastUpdateTick ||
                State ==
                    PersistentConstructionState.Destroyed)
            {
                return false;
            }

            Progress = progress;
            LastUpdateTick = updateTick;

            if (Progress >= 100f)
            {
                State =
                    PersistentConstructionState.Operational;
            }
            else if (Progress > 0f)
            {
                State =
                    PersistentConstructionState.Constructing;
            }

            return true;
        }
    }

    public sealed class PersistentConstruction
    {
        private readonly Dictionary<
            string,
            PersistentConstructionRecord> projects =
            new Dictionary<
                string,
                PersistentConstructionRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ProjectCount =>
            projects.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            projects.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterProject(
            string constructionId,
            string ownerId,
            string regionId,
            string structureId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(constructionId) ||
                string.IsNullOrWhiteSpace(ownerId) ||
                string.IsNullOrWhiteSpace(regionId) ||
                string.IsNullOrWhiteSpace(structureId))
            {
                return false;
            }

            string id =
                constructionId.Trim();

            if (projects.ContainsKey(id))
            {
                return false;
            }

            projects.Add(
                id,
                new PersistentConstructionRecord(
                    id,
                    ownerId.Trim(),
                    regionId.Trim(),
                    structureId.Trim()));

            return true;
        }

        public bool SetState(
            string constructionId,
            PersistentConstructionState state,
            long updateTick)
        {
            PersistentConstructionRecord project =
                GetProject(constructionId);

            return project != null &&
                   project.SetState(
                       state,
                       updateTick);
        }

        public bool SetProgress(
            string constructionId,
            float progress,
            long updateTick)
        {
            PersistentConstructionRecord project =
                GetProject(constructionId);

            return project != null &&
                   project.SetProgress(
                       progress,
                       updateTick);
        }

        public PersistentConstructionRecord GetProject(
            string constructionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(constructionId))
            {
                return null;
            }

            projects.TryGetValue(
                constructionId.Trim(),
                out PersistentConstructionRecord project);

            return project;
        }

        public IReadOnlyCollection<
            PersistentConstructionRecord>
            GetProjects()
        {
            return projects.Values;
        }

        public void Reset()
        {
            projects.Clear();
            Initialized = false;
        }
    }
}

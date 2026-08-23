using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum StrategicObjectiveType
    {
        CaptureRegion,
        HoldRegion,
        SecureResource,
        EstablishBase,
        DestroyTarget,
        Escort,
        Reconnaissance
    }

    public enum StrategicObjectiveState
    {
        Available,
        Active,
        Completed,
        Failed,
        Cancelled
    }

    public sealed class StrategicObjectiveRecord
    {
        public string ObjectiveId { get; }

        public StrategicObjectiveType Type { get; }

        public string RegionId { get; }

        public string TargetId { get; }

        public StrategicObjectiveState State { get; private set; }

        public float Progress { get; private set; }

        public StrategicObjectiveRecord(
            string objectiveId,
            StrategicObjectiveType type,
            string regionId,
            string targetId)
        {
            ObjectiveId =
                objectiveId ?? string.Empty;

            Type = type;

            RegionId =
                regionId ?? string.Empty;

            TargetId =
                targetId ?? string.Empty;

            State =
                StrategicObjectiveState.Available;

            Progress = 0f;
        }

        public bool Activate()
        {
            if (State !=
                StrategicObjectiveState.Available)
            {
                return false;
            }

            State =
                StrategicObjectiveState.Active;

            return true;
        }

        public bool SetProgress(
            float progress)
        {
            if (State !=
                StrategicObjectiveState.Active ||
                progress < 0f ||
                progress > 100f)
            {
                return false;
            }

            Progress = progress;

            if (Progress >= 100f)
            {
                State =
                    StrategicObjectiveState.Completed;
            }

            return true;
        }

        public bool Fail()
        {
            if (State !=
                StrategicObjectiveState.Active)
            {
                return false;
            }

            State =
                StrategicObjectiveState.Failed;

            return true;
        }

        public bool Cancel()
        {
            if (State ==
                StrategicObjectiveState.Completed ||
                State ==
                    StrategicObjectiveState.Failed)
            {
                return false;
            }

            State =
                StrategicObjectiveState.Cancelled;

            return true;
        }
    }

    public sealed class StrategicObjectives
    {
        private readonly Dictionary<
            string,
            StrategicObjectiveRecord> objectives =
            new Dictionary<
                string,
                StrategicObjectiveRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ObjectiveCount =>
            objectives.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            objectives.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterObjective(
            string objectiveId,
            StrategicObjectiveType type,
            string regionId,
            string targetId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId))
            {
                return false;
            }

            string id =
                objectiveId.Trim();

            if (objectives.ContainsKey(id))
            {
                return false;
            }

            objectives.Add(
                id,
                new StrategicObjectiveRecord(
                    id,
                    type,
                    regionId,
                    targetId));

            return true;
        }

        public bool Activate(
            string objectiveId)
        {
            StrategicObjectiveRecord record =
                GetObjective(objectiveId);

            return record != null &&
                   record.Activate();
        }

        public bool SetProgress(
            string objectiveId,
            float progress)
        {
            StrategicObjectiveRecord record =
                GetObjective(objectiveId);

            return record != null &&
                   record.SetProgress(progress);
        }

        public bool Fail(
            string objectiveId)
        {
            StrategicObjectiveRecord record =
                GetObjective(objectiveId);

            return record != null &&
                   record.Fail();
        }

        public bool Cancel(
            string objectiveId)
        {
            StrategicObjectiveRecord record =
                GetObjective(objectiveId);

            return record != null &&
                   record.Cancel();
        }

        public StrategicObjectiveRecord GetObjective(
            string objectiveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId))
            {
                return null;
            }

            objectives.TryGetValue(
                objectiveId.Trim(),
                out StrategicObjectiveRecord record);

            return record;
        }

        public IReadOnlyCollection<
            StrategicObjectiveRecord>
            GetObjectives()
        {
            return objectives.Values;
        }

        public void Reset()
        {
            objectives.Clear();
            Initialized = false;
        }
    }
}

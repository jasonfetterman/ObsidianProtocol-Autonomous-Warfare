using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class ObjectiveMarker
    {
        public string ObjectiveId { get; }
        public string Label { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }
        public bool Active { get; private set; }
        public bool Completed { get; private set; }

        public ObjectiveMarker(
            string objectiveId,
            string label,
            float positionX,
            float positionY,
            float positionZ)
        {
            ObjectiveId = objectiveId ?? string.Empty;
            Label = label ?? string.Empty;

            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;

            Active = true;
            Completed = false;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(ObjectiveId);

        public void Activate()
        {
            if (!Completed)
                Active = true;
        }

        public void Deactivate()
        {
            if (!Completed)
                Active = false;
        }

        public void Complete()
        {
            Active = false;
            Completed = true;
        }

        public void Reset()
        {
            Active = true;
            Completed = false;
        }
    }

    public sealed class ObjectiveMarkerSystem
    {
        private readonly Dictionary<string, ObjectiveMarker> objectives =
            new Dictionary<string, ObjectiveMarker>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(ObjectiveMarker objective)
        {
            if (objective == null ||
                !objective.Valid ||
                objectives.ContainsKey(objective.ObjectiveId))
            {
                return false;
            }

            objectives.Add(
                objective.ObjectiveId,
                objective);

            return true;
        }

        public bool Remove(string objectiveId)
        {
            if (string.IsNullOrWhiteSpace(objectiveId))
                return false;

            return objectives.Remove(objectiveId);
        }

        public bool TryGet(
            string objectiveId,
            out ObjectiveMarker objective)
        {
            return objectives.TryGetValue(
                objectiveId,
                out objective);
        }

        public bool Activate(string objectiveId)
        {
            if (!objectives.TryGetValue(
                    objectiveId,
                    out ObjectiveMarker objective))
            {
                return false;
            }

            objective.Activate();
            return true;
        }

        public bool Deactivate(string objectiveId)
        {
            if (!objectives.TryGetValue(
                    objectiveId,
                    out ObjectiveMarker objective))
            {
                return false;
            }

            objective.Deactivate();
            return true;
        }

        public bool Complete(string objectiveId)
        {
            if (!objectives.TryGetValue(
                    objectiveId,
                    out ObjectiveMarker objective))
            {
                return false;
            }

            objective.Complete();
            return true;
        }

        public IReadOnlyCollection<ObjectiveMarker>
            GetObjectives()
        {
            return objectives.Values;
        }

        public void Clear()
        {
            objectives.Clear();
        }
    }
}

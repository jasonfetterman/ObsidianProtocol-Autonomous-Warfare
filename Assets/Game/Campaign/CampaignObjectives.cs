
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignObjective
    {
        public string ObjectiveId { get; }
        public string Description { get; }
        public bool Required { get; }
        public bool Completed { get; private set; }

        public UnitMovement AssignedUnit { get; private set; }
        public Vector3 Destination { get; private set; }
        public float ArrivalDistance { get; private set; }
        public bool IsMoveObjective { get; private set; }

        public CampaignObjective(
            string objectiveId,
            string description,
            bool required)
        {
            ObjectiveId = objectiveId ?? string.Empty;
            Description = description ?? string.Empty;
            Required = required;
            Completed = false;
            ArrivalDistance = 2f;
        }

        public void SetMoveTarget(
            UnitMovement unit,
            Vector3 destination,
            float arrivalDistance)
        {
            AssignedUnit = unit;
            Destination = destination;
            ArrivalDistance = Mathf.Max(0.1f, arrivalDistance);
            IsMoveObjective = unit != null;
        }

        public bool Complete()
        {
            if (Completed)
                return false;

            Completed = true;
            return true;
        }

        public bool Reset()
        {
            if (!Completed)
                return false;

            Completed = false;
            return true;
        }
    }

    public sealed class CampaignObjectives
    {
        private readonly Dictionary<string, CampaignObjective> objectives =
            new Dictionary<string, CampaignObjective>(
                StringComparer.OrdinalIgnoreCase);

        public static CampaignObjectives ActiveInstance { get; private set; }

        public static event Action ObjectivesChanged;

        public bool Initialized { get; private set; }

        public int ObjectiveCount => objectives.Count;

        public int CompletedObjectiveCount
        {
            get
            {
                int count = 0;

                foreach (CampaignObjective objective in objectives.Values)
                    if (objective.Completed)
                        count++;

                return count;
            }
        }

        public bool RequiredObjectivesComplete
        {
            get
            {
                foreach (CampaignObjective objective in objectives.Values)
                    if (objective.Required && !objective.Completed)
                        return false;

                return true;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
                return false;

            objectives.Clear();
            Initialized = true;
            ActiveInstance = this;

            ObjectivesChanged?.Invoke();
            return true;
        }

        public bool RegisterObjective(
            string objectiveId,
            string description,
            bool required)
        {
            if (!CanRegister(objectiveId, description))
                return false;

            string id = objectiveId.Trim();

            objectives.Add(
                id,
                new CampaignObjective(id, description.Trim(), required));

            ObjectivesChanged?.Invoke();
            return true;
        }

        public bool RegisterMoveObjective(
            string objectiveId,
            string description,
            bool required,
            UnitMovement unit,
            Vector3 destination,
            float arrivalDistance = 2f)
        {
            if (!CanRegister(objectiveId, description) || unit == null)
                return false;

            string id = objectiveId.Trim();

            CampaignObjective objective =
                new CampaignObjective(id, description.Trim(), required);

            objective.SetMoveTarget(unit, destination, arrivalDistance);
            objectives.Add(id, objective);

            ObjectivesChanged?.Invoke();
            return true;
        }

        private bool CanRegister(string objectiveId, string description)
        {
            return Initialized &&
                   !string.IsNullOrWhiteSpace(objectiveId) &&
                   !string.IsNullOrWhiteSpace(description) &&
                   !objectives.ContainsKey(objectiveId.Trim());
        }

        public void CheckMoveObjectives(UnitMovement unit)
        {
            if (!Initialized || unit == null)
                return;

            foreach (CampaignObjective objective in objectives.Values)
            {
                if (objective.Completed ||
                    !objective.IsMoveObjective ||
                    objective.AssignedUnit != unit)
                    continue;

                if (Vector3.Distance(
                    unit.transform.position,
                    objective.Destination) <= objective.ArrivalDistance)
                {
                    objective.Complete();
                    ObjectivesChanged?.Invoke();
                }
            }
        }

        public bool CompleteObjective(string objectiveId)
        {
            CampaignObjective objective = GetObjective(objectiveId);

            if (objective == null || !objective.Complete())
                return false;

            ObjectivesChanged?.Invoke();
            return true;
        }

        public CampaignObjective GetObjective(string objectiveId)
        {
            if (!Initialized || string.IsNullOrWhiteSpace(objectiveId))
                return null;

            objectives.TryGetValue(
                objectiveId.Trim(),
                out CampaignObjective objective);

            return objective;
        }

        public IReadOnlyCollection<CampaignObjective> GetObjectives()
        {
            return objectives.Values;
        }

        public void Reset()
        {
            objectives.Clear();
            Initialized = false;

            if (ActiveInstance == this)
                ActiveInstance = null;

            ObjectivesChanged?.Invoke();
        }
    }
}

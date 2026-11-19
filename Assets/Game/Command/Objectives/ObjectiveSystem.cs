
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Objectives
{
    public enum ObjectiveType
    {
        None,
        Move,
        Attack,
        Defend,
        Hold,
        Capture,
        Recon,
        Reinforce,
        Retreat
    }

    [Serializable]
    public sealed class Objective
    {
        public int Id;
        public ObjectiveType Type;
        public Vector3 Position;
        public GameObject Target;
        public UnitMovement AssignedUnit;
        public string Description;
        public float ArrivalDistance;
        public bool Completed;

        public Objective(
            int id,
            ObjectiveType type,
            Vector3 position)
        {
            Id = id;
            Type = type;
            Position = position;
            Description = string.Empty;
            ArrivalDistance = 2f;
        }
    }

    public sealed class ObjectiveSystem
    {
        private readonly List<Objective> objectives =
            new List<Objective>();

        private int nextObjectiveId = 1;

        public static ObjectiveSystem ActiveInstance { get; private set; }

        public static event Action ObjectivesChanged;

        public IReadOnlyList<Objective> Objectives =>
            objectives;

        public ObjectiveSystem()
        {
            ActiveInstance = this;
        }

        public Objective CreateObjective(
            ObjectiveType type,
            Vector3 position)
        {
            return CreateObjective(
                type,
                position,
                string.Empty,
                null);
        }

        public Objective CreateObjective(
            ObjectiveType type,
            Vector3 position,
            string description,
            UnitMovement assignedUnit,
            float arrivalDistance = 2f)
        {
            Objective objective = new Objective(
                nextObjectiveId++,
                type,
                position);

            objective.Description = description ?? string.Empty;
            objective.AssignedUnit = assignedUnit;
            objective.ArrivalDistance = Mathf.Max(0.1f, arrivalDistance);

            objectives.Add(objective);

            ObjectivesChanged?.Invoke();

            return objective;
        }

        public bool CompleteObjective(int objectiveId)
        {
            Objective objective = FindObjective(objectiveId);

            if (objective == null || objective.Completed)
                return false;

            objective.Completed = true;

            ObjectivesChanged?.Invoke();

            return true;
        }

        public void CheckMoveObjectives(UnitMovement unit)
        {
            if (unit == null)
                return;

            foreach (Objective objective in objectives)
            {
                if (objective.Completed ||
                    objective.Type != ObjectiveType.Move ||
                    objective.AssignedUnit != unit)
                    continue;

                if (Vector3.Distance(
                    unit.transform.position,
                    objective.Position) <= objective.ArrivalDistance)
                {
                    objective.Completed = true;
                    ObjectivesChanged?.Invoke();
                }
            }
        }

        public Objective FindObjective(int objectiveId)
        {
            for (int i = 0; i < objectives.Count; i++)
            {
                if (objectives[i].Id == objectiveId)
                    return objectives[i];
            }

            return null;
        }

        public void RemoveObjective(int objectiveId)
        {
            Objective objective = FindObjective(objectiveId);

            if (objective != null)
            {
                objectives.Remove(objective);
                ObjectivesChanged?.Invoke();
            }
        }

        public void Clear()
        {
            objectives.Clear();
            nextObjectiveId = 1;

            ObjectivesChanged?.Invoke();
        }
    }
}

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
        public bool Completed;

        public Objective(
            int id,
            ObjectiveType type,
            Vector3 position)
        {
            Id = id;
            Type = type;
            Position = position;
        }
    }

    public sealed class ObjectiveSystem
    {
        private readonly List<Objective> objectives =
            new List<Objective>();

        private int nextObjectiveId = 1;

        public IReadOnlyList<Objective> Objectives =>
            objectives;

        public Objective CreateObjective(
            ObjectiveType type,
            Vector3 position)
        {
            Objective objective = new Objective(
                nextObjectiveId++,
                type,
                position);

            objectives.Add(objective);

            return objective;
        }

        public bool CompleteObjective(int objectiveId)
        {
            Objective objective = FindObjective(objectiveId);

            if (objective == null)
            {
                return false;
            }

            objective.Completed = true;
            return true;
        }

        public Objective FindObjective(int objectiveId)
        {
            for (int i = 0; i < objectives.Count; i++)
            {
                if (objectives[i].Id == objectiveId)
                {
                    return objectives[i];
                }
            }

            return null;
        }

        public void RemoveObjective(int objectiveId)
        {
            Objective objective = FindObjective(objectiveId);

            if (objective != null)
            {
                objectives.Remove(objective);
            }
        }

        public void Clear()
        {
            objectives.Clear();
            nextObjectiveId = 1;
        }
    }
}

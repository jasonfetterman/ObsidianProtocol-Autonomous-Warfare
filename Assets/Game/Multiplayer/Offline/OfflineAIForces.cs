using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public enum OfflineAIForceState
    {
        Inactive,
        Active,
        Defeated
    }

    public sealed class OfflineAIForce
    {
        private readonly HashSet<string> unitIds =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public string ForceId { get; }

        public OfflineAIForceState State { get; private set; }

        public int UnitCount =>
            unitIds.Count;

        public OfflineAIForce(
            string forceId)
        {
            ForceId =
                forceId ?? string.Empty;

            State =
                OfflineAIForceState.Inactive;
        }

        public bool Activate()
        {
            if (State != OfflineAIForceState.Inactive)
            {
                return false;
            }

            State = OfflineAIForceState.Active;
            return true;
        }

        public bool AddUnit(
            string unitId)
        {
            if (State != OfflineAIForceState.Active ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return unitIds.Add(unitId.Trim());
        }

        public bool RemoveUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            bool removed =
                unitIds.Remove(unitId.Trim());

            if (removed &&
                unitIds.Count == 0)
            {
                State =
                    OfflineAIForceState.Defeated;
            }

            return removed;
        }

        public bool Defeat()
        {
            if (State != OfflineAIForceState.Active)
            {
                return false;
            }

            State =
                OfflineAIForceState.Defeated;

            return true;
        }

        public IReadOnlyCollection<string>
            GetUnitIds()
        {
            return unitIds;
        }
    }

    public sealed class OfflineAIForces
    {
        private readonly Dictionary<
            string,
            OfflineAIForce> forces =
            new Dictionary<
                string,
                OfflineAIForce>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ForceCount =>
            forces.Count;

        public int ActiveForceCount
        {
            get
            {
                int count = 0;

                foreach (OfflineAIForce force
                    in forces.Values)
                {
                    if (force.State ==
                        OfflineAIForceState.Active)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            forces.Clear();
            Initialized = true;

            return true;
        }

        public bool AddForce(
            string forceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(forceId))
            {
                return false;
            }

            string id =
                forceId.Trim();

            if (forces.ContainsKey(id))
            {
                return false;
            }

            forces.Add(
                id,
                new OfflineAIForce(id));

            return true;
        }

        public bool ActivateForce(
            string forceId)
        {
            OfflineAIForce force =
                GetForce(forceId);

            return force != null &&
                   force.Activate();
        }

        public OfflineAIForce GetForce(
            string forceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(forceId))
            {
                return null;
            }

            forces.TryGetValue(
                forceId.Trim(),
                out OfflineAIForce force);

            return force;
        }

        public IReadOnlyCollection<
            OfflineAIForce>
            GetForces()
        {
            return forces.Values;
        }

        public void Reset()
        {
            forces.Clear();
            Initialized = false;
        }
    }
}

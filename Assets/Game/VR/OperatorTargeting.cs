using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public sealed class OperatorTarget
    {
        public string TargetId { get; }

        public float X { get; }

        public float Y { get; }

        public float Z { get; }

        public float Range { get; }

        public bool Hostile { get; }

        public OperatorTarget(
            string targetId,
            float x,
            float y,
            float z,
            float range,
            bool hostile)
        {
            TargetId =
                targetId ?? string.Empty;

            X = x;
            Y = y;
            Z = z;
            Range = range;
            Hostile = hostile;
        }
    }

    public sealed class OperatorTargeting
    {
        private readonly Dictionary<
            string,
            OperatorTarget> targets =
            new Dictionary<
                string,
                OperatorTarget>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Active { get; private set; }

        public string UnitId { get; private set; }

        public string CurrentTargetId { get; private set; }

        public int TargetCount =>
            targets.Count;

        public bool Initialize(
            string unitId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            UnitId =
                unitId.Trim();

            CurrentTargetId =
                string.Empty;

            targets.Clear();

            Active = false;
            Initialized = true;

            return true;
        }

        public bool Activate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = true;

            return true;
        }

        public bool Deactivate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = false;
            ClearTarget();

            return true;
        }

        public bool RegisterTarget(
            OperatorTarget target)
        {
            if (!Initialized ||
                target == null ||
                string.IsNullOrWhiteSpace(
                    target.TargetId))
            {
                return false;
            }

            return targets.TryAdd(
                target.TargetId.Trim(),
                target);
        }

        public bool SelectTarget(
            string targetId)
        {
            if (!Initialized ||
                !Active ||
                string.IsNullOrWhiteSpace(targetId))
            {
                return false;
            }

            string id =
                targetId.Trim();

            if (!targets.ContainsKey(id))
            {
                return false;
            }

            CurrentTargetId = id;

            return true;
        }

        public bool ClearTarget()
        {
            if (!Initialized)
            {
                return false;
            }

            CurrentTargetId =
                string.Empty;

            return true;
        }

        public OperatorTarget GetCurrentTarget()
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(
                    CurrentTargetId))
            {
                return null;
            }

            targets.TryGetValue(
                CurrentTargetId,
                out OperatorTarget target);

            return target;
        }

        public OperatorTarget GetTarget(
            string targetId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(targetId))
            {
                return null;
            }

            targets.TryGetValue(
                targetId.Trim(),
                out OperatorTarget target);

            return target;
        }

        public IReadOnlyCollection<OperatorTarget>
            GetTargets()
        {
            return targets.Values;
        }

        public void Reset()
        {
            targets.Clear();

            Initialized = false;
            Active = false;

            UnitId =
                string.Empty;

            CurrentTargetId =
                string.Empty;
        }
    }
}

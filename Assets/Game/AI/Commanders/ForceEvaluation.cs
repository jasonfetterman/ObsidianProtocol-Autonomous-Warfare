using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum ForceCondition
    {
        Unknown,
        Overwhelming,
        Strong,
        Balanced,
        Weak,
        Critical
    }

    public sealed class ForceGroup
    {
        public string GroupId { get; }

        public int UnitCount
        {
            get;
            private set;
        }

        public int OperationalUnitCount
        {
            get;
            private set;
        }

        public float CombatStrength
        {
            get;
            private set;
        }

        public float Readiness
        {
            get;
            private set;
        }

        public float Mobility
        {
            get;
            private set;
        }

        public float Survivability
        {
            get;
            private set;
        }

        public float ReconCapability
        {
            get;
            private set;
        }

        public ForceCondition Condition
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(GroupId) &&
            UnitCount >= 0 &&
            OperationalUnitCount >= 0;

        public ForceGroup(
            string groupId)
        {
            GroupId =
                groupId ?? string.Empty;

            UnitCount = 0;
            OperationalUnitCount = 0;

            CombatStrength = 0.0f;
            Readiness = 0.0f;
            Mobility = 0.0f;
            Survivability = 0.0f;
            ReconCapability = 0.0f;

            Condition =
                ForceCondition.Unknown;
        }

        public void SetUnitCounts(
            int total,
            int operational)
        {
            UnitCount =
                Math.Max(0, total);

            OperationalUnitCount =
                Math.Max(
                    0,
                    Math.Min(
                        UnitCount,
                        operational));

            Recalculate();
        }

        public void SetCapabilities(
            float combatStrength,
            float readiness,
            float mobility,
            float survivability,
            float reconCapability)
        {
            CombatStrength =
                Clamp01(combatStrength);

            Readiness =
                Clamp01(readiness);

            Mobility =
                Clamp01(mobility);

            Survivability =
                Clamp01(survivability);

            ReconCapability =
                Clamp01(reconCapability);

            Recalculate();
        }

        public float OperationalRatio
        {
            get
            {
                if (UnitCount <= 0)
                    return 0.0f;

                return Clamp01(
                    (float)OperationalUnitCount /
                    UnitCount);
            }
        }

        public float OverallReadiness
        {
            get
            {
                return Clamp01(
                    (Readiness +
                     OperationalRatio) /
                    2.0f);
            }
        }

        private void Recalculate()
        {
            float overall =
                (CombatStrength +
                 Readiness +
                 Survivability) /
                3.0f;

            if (OperationalUnitCount <= 0)
            {
                Condition =
                    ForceCondition.Critical;
            }
            else if (overall >= 0.85f)
            {
                Condition =
                    ForceCondition.Overwhelming;
            }
            else if (overall >= 0.65f)
            {
                Condition =
                    ForceCondition.Strong;
            }
            else if (overall >= 0.40f)
            {
                Condition =
                    ForceCondition.Balanced;
            }
            else if (overall >= 0.20f)
            {
                Condition =
                    ForceCondition.Weak;
            }
            else
            {
                Condition =
                    ForceCondition.Critical;
            }
        }

        private static float Clamp01(
            float value)
        {
            return Math.Max(
                0.0f,
                Math.Min(
                    1.0f,
                    value));
        }
    }

    public sealed class ForceEvaluation
    {
        private readonly Dictionary<
            string,
            ForceGroup> friendlyForces =
            new Dictionary<
                string,
                ForceGroup>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<
            string,
            ForceGroup> enemyForces =
            new Dictionary<
                string,
                ForceGroup>(
                StringComparer.OrdinalIgnoreCase);

        public bool Valid =>
            friendlyForces.Count > 0 ||
            enemyForces.Count > 0;

        public bool RegisterFriendly(
            ForceGroup group)
        {
            if (group == null ||
                !group.Valid ||
                friendlyForces.ContainsKey(
                    group.GroupId))
            {
                return false;
            }

            friendlyForces.Add(
                group.GroupId,
                group);

            return true;
        }

        public bool RegisterEnemy(
            ForceGroup group)
        {
            if (group == null ||
                !group.Valid ||
                enemyForces.ContainsKey(
                    group.GroupId))
            {
                return false;
            }

            enemyForces.Add(
                group.GroupId,
                group);

            return true;
        }

        public bool RemoveFriendly(
            string groupId)
        {
            if (string.IsNullOrWhiteSpace(
                    groupId))
            {
                return false;
            }

            return friendlyForces.Remove(
                groupId);
        }

        public bool RemoveEnemy(
            string groupId)
        {
            if (string.IsNullOrWhiteSpace(
                    groupId))
            {
                return false;
            }

            return enemyForces.Remove(
                groupId);
        }

        public float FriendlyStrength
        {
            get
            {
                return CalculateStrength(
                    friendlyForces);
            }
        }

        public float EnemyStrength
        {
            get
            {
                return CalculateStrength(
                    enemyForces);
            }
        }

        public float ForceAdvantage
        {
            get
            {
                return FriendlyStrength -
                       EnemyStrength;
            }
        }

        public int FriendlyUnitCount
        {
            get
            {
                return CalculateUnitCount(
                    friendlyForces);
            }
        }

        public int EnemyUnitCount
        {
            get
            {
                return CalculateUnitCount(
                    enemyForces);
            }
        }

        public int FriendlyOperationalUnits
        {
            get
            {
                return CalculateOperationalCount(
                    friendlyForces);
            }
        }

        public int EnemyOperationalUnits
        {
            get
            {
                return CalculateOperationalCount(
                    enemyForces);
            }
        }

        public ForceCondition OverallFriendlyCondition
        {
            get
            {
                return CalculateCondition(
                    friendlyForces);
            }
        }

        public ForceCondition OverallEnemyCondition
        {
            get
            {
                return CalculateCondition(
                    enemyForces);
            }
        }

        public IReadOnlyCollection<
            ForceGroup>
            GetFriendlyForces()
        {
            return friendlyForces.Values;
        }

        public IReadOnlyCollection<
            ForceGroup>
            GetEnemyForces()
        {
            return enemyForces.Values;
        }

        public void Clear()
        {
            friendlyForces.Clear();
            enemyForces.Clear();
        }

        private static float CalculateStrength(
            Dictionary<
                string,
                ForceGroup> forces)
        {
            if (forces.Count == 0)
                return 0.0f;

            float total = 0.0f;

            foreach (ForceGroup group
                in forces.Values)
            {
                total +=
                    group.CombatStrength *
                    group.Readiness;
            }

            return total /
                   forces.Count;
        }

        private static int CalculateUnitCount(
            Dictionary<
                string,
                ForceGroup> forces)
        {
            int total = 0;

            foreach (ForceGroup group
                in forces.Values)
            {
                if (int.MaxValue - total <
                    group.UnitCount)
                {
                    return int.MaxValue;
                }

                total +=
                    group.UnitCount;
            }

            return total;
        }

        private static int CalculateOperationalCount(
            Dictionary<
                string,
                ForceGroup> forces)
        {
            int total = 0;

            foreach (ForceGroup group
                in forces.Values)
            {
                if (int.MaxValue - total <
                    group.OperationalUnitCount)
                {
                    return int.MaxValue;
                }

                total +=
                    group.OperationalUnitCount;
            }

            return total;
        }

        private static ForceCondition CalculateCondition(
            Dictionary<
                string,
                ForceGroup> forces)
        {
            if (forces.Count == 0)
                return ForceCondition.Unknown;

            float strength =
                CalculateStrength(forces);

            if (strength >= 0.85f)
                return ForceCondition.Overwhelming;

            if (strength >= 0.65f)
                return ForceCondition.Strong;

            if (strength >= 0.40f)
                return ForceCondition.Balanced;

            if (strength >= 0.20f)
                return ForceCondition.Weak;

            return ForceCondition.Critical;
        }
    }
}

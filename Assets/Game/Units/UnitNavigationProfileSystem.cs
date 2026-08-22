using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public enum NavigationDomain
    {
        Ground,
        Air,
        Sea,
        Amphibious
    }

    public sealed class UnitNavigationProfile
    {
        public string UnitId { get; }

        public NavigationDomain Domain { get; private set; }

        public float MaxSpeed { get; private set; }
        public float Acceleration { get; private set; }
        public float TurnRate { get; private set; }
        public float StoppingDistance { get; private set; }
        public float ObstacleClearance { get; private set; }

        public bool CanReverse { get; private set; }
        public bool CanTraverseObstacles { get; private set; }

        public UnitNavigationProfile(string unitId)
        {
            UnitId = unitId ?? string.Empty;
            Domain = NavigationDomain.Ground;
        }

        public void Configure(
            NavigationDomain domain,
            float maxSpeed,
            float acceleration,
            float turnRate,
            float stoppingDistance,
            float obstacleClearance,
            bool canReverse,
            bool canTraverseObstacles)
        {
            Domain = domain;
            MaxSpeed = Math.Max(0f, maxSpeed);
            Acceleration = Math.Max(0f, acceleration);
            TurnRate = Math.Max(0f, turnRate);
            StoppingDistance = Math.Max(0f, stoppingDistance);
            ObstacleClearance = Math.Max(0f, obstacleClearance);
            CanReverse = canReverse;
            CanTraverseObstacles = canTraverseObstacles;
        }
    }

    public sealed class UnitNavigationProfileSystem
    {
        private readonly Dictionary<string, UnitNavigationProfile> profiles =
            new Dictionary<string, UnitNavigationProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!profiles.ContainsKey(unitId))
            {
                profiles.Add(
                    unitId,
                    new UnitNavigationProfile(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            NavigationDomain domain,
            float maxSpeed,
            float acceleration,
            float turnRate,
            float stoppingDistance,
            float obstacleClearance,
            bool canReverse,
            bool canTraverseObstacles)
        {
            RegisterUnit(unitId);

            profiles[unitId].Configure(
                domain,
                maxSpeed,
                acceleration,
                turnRate,
                stoppingDistance,
                obstacleClearance,
                canReverse,
                canTraverseObstacles);
        }

        public bool TryGetProfile(
            string unitId,
            out UnitNavigationProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public void RemoveUnit(string unitId)
        {
            profiles.Remove(unitId);
        }

        public void Clear()
        {
            profiles.Clear();
        }
    }
}

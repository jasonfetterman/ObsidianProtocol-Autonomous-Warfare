using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public sealed class AirNavigationState
    {
        public string UnitId { get; }

        public float CurrentX { get; private set; }
        public float CurrentY { get; private set; }
        public float CurrentZ { get; private set; }

        public float TargetX { get; private set; }
        public float TargetY { get; private set; }
        public float TargetZ { get; private set; }

        public float NavigationSpeed { get; private set; }

        public bool HasDestination { get; private set; }
        public bool NavigationEnabled { get; private set; }

        public AirNavigationState(string unitId)
        {
            UnitId = unitId ?? string.Empty;
            NavigationEnabled = true;
            HasDestination = false;
        }

        public void ConfigureSpeed(float speed)
        {
            NavigationSpeed =
                Math.Max(0f, speed);
        }

        public void SetPosition(
            float x,
            float y,
            float z)
        {
            CurrentX = x;
            CurrentY = y;
            CurrentZ = z;
        }

        public void SetDestination(
            float x,
            float y,
            float z)
        {
            TargetX = x;
            TargetY = y;
            TargetZ = z;

            HasDestination = true;
        }

        public void ClearDestination()
        {
            HasDestination = false;
        }

        public void SetEnabled(bool enabled)
        {
            NavigationEnabled = enabled;

            if (!enabled)
            {
                HasDestination = false;
            }
        }

        public void Update(float deltaTime)
        {
            if (!NavigationEnabled ||
                !HasDestination)
            {
                return;
            }

            float delta =
                Math.Max(0f, deltaTime);

            float dx =
                TargetX - CurrentX;

            float dy =
                TargetY - CurrentY;

            float dz =
                TargetZ - CurrentZ;

            float distance =
                (float)Math.Sqrt(
                    dx * dx +
                    dy * dy +
                    dz * dz);

            if (distance <= 0.01f)
            {
                CurrentX = TargetX;
                CurrentY = TargetY;
                CurrentZ = TargetZ;

                HasDestination = false;
                return;
            }

            float travel =
                NavigationSpeed * delta;

            if (travel >= distance)
            {
                CurrentX = TargetX;
                CurrentY = TargetY;
                CurrentZ = TargetZ;

                HasDestination = false;
                return;
            }

            float ratio =
                travel / distance;

            CurrentX += dx * ratio;
            CurrentY += dy * ratio;
            CurrentZ += dz * ratio;
        }
    }

    public sealed class AirNavigationSystem
    {
        private readonly Dictionary<string, AirNavigationState> states =
            new Dictionary<string, AirNavigationState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterDrone(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!states.ContainsKey(unitId))
            {
                states.Add(
                    unitId,
                    new AirNavigationState(unitId));
            }
        }

        public void ConfigureSpeed(
            string unitId,
            float speed)
        {
            RegisterDrone(unitId);

            states[unitId].ConfigureSpeed(speed);
        }

        public void SetPosition(
            string unitId,
            float x,
            float y,
            float z)
        {
            RegisterDrone(unitId);

            states[unitId].SetPosition(
                x,
                y,
                z);
        }

        public void SetDestination(
            string unitId,
            float x,
            float y,
            float z)
        {
            RegisterDrone(unitId);

            states[unitId].SetDestination(
                x,
                y,
                z);
        }

        public void UpdateDrone(
            string unitId,
            float deltaTime)
        {
            if (!states.TryGetValue(
                    unitId,
                    out AirNavigationState state))
            {
                return;
            }

            state.Update(deltaTime);
        }

        public bool TryGetState(
            string unitId,
            out AirNavigationState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void RemoveDrone(string unitId)
        {
            states.Remove(unitId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}

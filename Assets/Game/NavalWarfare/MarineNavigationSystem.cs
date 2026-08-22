using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum MarineNavigationMode
    {
        Manual,
        Assisted,
        Autonomous
    }

    public sealed class MarineNavigationState
    {
        public string UnitId { get; }

        public MarineNavigationMode Mode { get; private set; }

        public float CurrentX { get; private set; }
        public float CurrentY { get; private set; }
        public float CurrentZ { get; private set; }

        public float TargetX { get; private set; }
        public float TargetY { get; private set; }
        public float TargetZ { get; private set; }

        public float MaximumSpeed { get; private set; }

        public bool HasDestination { get; private set; }
        public bool NavigationEnabled { get; private set; }

        public MarineNavigationState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            Mode =
                MarineNavigationMode.Autonomous;

            NavigationEnabled = true;
            HasDestination = false;
        }

        public void ConfigureSpeed(
            float maximumSpeed)
        {
            MaximumSpeed =
                Math.Max(
                    0f,
                    maximumSpeed);
        }

        public void SetMode(
            MarineNavigationMode mode)
        {
            Mode = mode;
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

        public void SetEnabled(
            bool enabled)
        {
            NavigationEnabled = enabled;

            if (!enabled)
            {
                HasDestination = false;
            }
        }

        public void Update(
            float deltaTime)
        {
            if (!NavigationEnabled ||
                !HasDestination)
            {
                return;
            }

            float delta =
                Math.Max(
                    0f,
                    deltaTime);

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
                MaximumSpeed * delta;

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

    public sealed class MarineNavigationSystem
    {
        private readonly Dictionary<string, MarineNavigationState> states =
            new Dictionary<string, MarineNavigationState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!states.ContainsKey(unitId))
            {
                states.Add(
                    unitId,
                    new MarineNavigationState(unitId));
            }
        }

        public void ConfigureSpeed(
            string unitId,
            float maximumSpeed)
        {
            RegisterUnit(unitId);

            states[unitId].ConfigureSpeed(
                maximumSpeed);
        }

        public void SetMode(
            string unitId,
            MarineNavigationMode mode)
        {
            RegisterUnit(unitId);

            states[unitId].SetMode(mode);
        }

        public void SetPosition(
            string unitId,
            float x,
            float y,
            float z)
        {
            RegisterUnit(unitId);

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
            RegisterUnit(unitId);

            states[unitId].SetDestination(
                x,
                y,
                z);
        }

        public void UpdateUnit(
            string unitId,
            float deltaTime)
        {
            if (states.TryGetValue(
                    unitId,
                    out MarineNavigationState state))
            {
                state.Update(deltaTime);
            }
        }

        public bool TryGetState(
            string unitId,
            out MarineNavigationState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void SetEnabled(
            string unitId,
            bool enabled)
        {
            RegisterUnit(unitId);

            states[unitId].SetEnabled(enabled);
        }

        public void RemoveUnit(
            string unitId)
        {
            states.Remove(unitId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}

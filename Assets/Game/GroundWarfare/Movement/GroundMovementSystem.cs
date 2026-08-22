using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public sealed class GroundMovementState
    {
        public string UnitId { get; }

        public float CurrentX { get; private set; }
        public float CurrentY { get; private set; }
        public float CurrentZ { get; private set; }

        public float TargetX { get; private set; }
        public float TargetY { get; private set; }
        public float TargetZ { get; private set; }

        public float MovementSpeed { get; private set; }

        public bool HasDestination { get; private set; }
        public bool MovementEnabled { get; private set; }

        public GroundMovementState(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            MovementEnabled = true;
            HasDestination = false;
        }

        public void ConfigureSpeed(float speed)
        {
            MovementSpeed =
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
            MovementEnabled = enabled;

            if (!enabled)
            {
                HasDestination = false;
            }
        }

        public void Update(float deltaTime)
        {
            if (!MovementEnabled ||
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
                MovementSpeed * delta;

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

    public sealed class GroundMovementSystem
    {
        private readonly Dictionary<string, GroundMovementState> states =
            new Dictionary<string, GroundMovementState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterVehicle(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!states.ContainsKey(unitId))
            {
                states.Add(
                    unitId,
                    new GroundMovementState(unitId));
            }
        }

        public void ConfigureSpeed(
            string unitId,
            float speed)
        {
            RegisterVehicle(unitId);

            states[unitId].ConfigureSpeed(speed);
        }

        public void SetPosition(
            string unitId,
            float x,
            float y,
            float z)
        {
            RegisterVehicle(unitId);

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
            RegisterVehicle(unitId);

            states[unitId].SetDestination(
                x,
                y,
                z);
        }

        public void UpdateVehicle(
            string unitId,
            float deltaTime)
        {
            if (states.TryGetValue(
                    unitId,
                    out GroundMovementState state))
            {
                state.Update(deltaTime);
            }
        }

        public void SetEnabled(
            string unitId,
            bool enabled)
        {
            RegisterVehicle(unitId);

            states[unitId].SetEnabled(enabled);
        }

        public bool TryGetState(
            string unitId,
            out GroundMovementState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void RemoveVehicle(string unitId)
        {
            states.Remove(unitId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public sealed class SurfaceMovementState
    {
        public string UnitId { get; }

        public float ForwardSpeed { get; private set; }
        public float TurnRate { get; private set; }

        public float MaximumSpeed { get; private set; }
        public float MaximumTurnRate { get; private set; }

        public bool MovementEnabled { get; private set; }
        public bool PropulsionActive { get; private set; }

        public SurfaceMovementState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            MovementEnabled = true;
            PropulsionActive = false;
        }

        public void Configure(
            float maximumSpeed,
            float maximumTurnRate)
        {
            MaximumSpeed =
                Math.Max(
                    0f,
                    maximumSpeed);

            MaximumTurnRate =
                Math.Max(
                    0f,
                    maximumTurnRate);
        }

        public void StartPropulsion()
        {
            PropulsionActive = true;
        }

        public void StopPropulsion()
        {
            PropulsionActive = false;
            StopMovement();
        }

        public void SetMovementEnabled(
            bool enabled)
        {
            MovementEnabled = enabled;

            if (!enabled)
            {
                StopMovement();
            }
        }

        public void SetMovementInput(
            float forwardInput,
            float turnInput)
        {
            if (!MovementEnabled ||
                !PropulsionActive)
            {
                return;
            }

            ForwardSpeed =
                Math.Clamp(
                    forwardInput,
                    -1f,
                    1f) *
                MaximumSpeed;

            TurnRate =
                Math.Clamp(
                    turnInput,
                    -1f,
                    1f) *
                MaximumTurnRate;
        }

        public void StopMovement()
        {
            ForwardSpeed = 0f;
            TurnRate = 0f;
        }
    }

    public sealed class SurfaceMovementSystem
    {
        private readonly Dictionary<string, SurfaceMovementState> states =
            new Dictionary<string, SurfaceMovementState>(
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
                    new SurfaceMovementState(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float maximumSpeed,
            float maximumTurnRate)
        {
            RegisterUnit(unitId);

            states[unitId].Configure(
                maximumSpeed,
                maximumTurnRate);
        }

        public void StartPropulsion(
            string unitId)
        {
            RegisterUnit(unitId);

            states[unitId].StartPropulsion();
        }

        public void StopPropulsion(
            string unitId)
        {
            if (states.TryGetValue(
                    unitId,
                    out SurfaceMovementState state))
            {
                state.StopPropulsion();
            }
        }

        public void SetMovementInput(
            string unitId,
            float forwardInput,
            float turnInput)
        {
            if (states.TryGetValue(
                    unitId,
                    out SurfaceMovementState state))
            {
                state.SetMovementInput(
                    forwardInput,
                    turnInput);
            }
        }

        public void SetMovementEnabled(
            string unitId,
            bool enabled)
        {
            RegisterUnit(unitId);

            states[unitId].SetMovementEnabled(
                enabled);
        }

        public void StopUnit(
            string unitId)
        {
            if (states.TryGetValue(
                    unitId,
                    out SurfaceMovementState state))
            {
                state.StopMovement();
            }
        }

        public bool TryGetState(
            string unitId,
            out SurfaceMovementState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
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

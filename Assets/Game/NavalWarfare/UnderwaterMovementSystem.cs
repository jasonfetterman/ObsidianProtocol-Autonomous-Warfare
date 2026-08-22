using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public sealed class UnderwaterMovementState
    {
        public string UnitId { get; }

        public float ForwardSpeed { get; private set; }
        public float VerticalSpeed { get; private set; }
        public float TurnRate { get; private set; }

        public float MaximumSpeed { get; private set; }
        public float MaximumVerticalSpeed { get; private set; }
        public float MaximumTurnRate { get; private set; }

        public float Depth { get; private set; }
        public float MinimumDepth { get; private set; }
        public float MaximumDepth { get; private set; }

        public bool MovementEnabled { get; private set; }
        public bool PropulsionActive { get; private set; }

        public UnderwaterMovementState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            MovementEnabled = true;
            PropulsionActive = false;
        }

        public void Configure(
            float maximumSpeed,
            float maximumVerticalSpeed,
            float maximumTurnRate,
            float minimumDepth,
            float maximumDepth)
        {
            MaximumSpeed =
                Math.Max(
                    0f,
                    maximumSpeed);

            MaximumVerticalSpeed =
                Math.Max(
                    0f,
                    maximumVerticalSpeed);

            MaximumTurnRate =
                Math.Max(
                    0f,
                    maximumTurnRate);

            MinimumDepth =
                Math.Max(
                    0f,
                    minimumDepth);

            MaximumDepth =
                Math.Max(
                    MinimumDepth,
                    maximumDepth);

            Depth =
                Math.Clamp(
                    Depth,
                    MinimumDepth,
                    MaximumDepth);
        }

        public void SetDepth(
            float depth)
        {
            Depth =
                Math.Clamp(
                    depth,
                    MinimumDepth,
                    MaximumDepth);
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
            float verticalInput,
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

            VerticalSpeed =
                Math.Clamp(
                    verticalInput,
                    -1f,
                    1f) *
                MaximumVerticalSpeed;

            TurnRate =
                Math.Clamp(
                    turnInput,
                    -1f,
                    1f) *
                MaximumTurnRate;
        }

        public void UpdateDepth(
            float deltaTime)
        {
            if (!MovementEnabled ||
                !PropulsionActive)
            {
                return;
            }

            Depth +=
                VerticalSpeed *
                Math.Max(
                    0f,
                    deltaTime);

            Depth =
                Math.Clamp(
                    Depth,
                    MinimumDepth,
                    MaximumDepth);

            if (Depth <= MinimumDepth ||
                Depth >= MaximumDepth)
            {
                VerticalSpeed = 0f;
            }
        }

        public void StopMovement()
        {
            ForwardSpeed = 0f;
            VerticalSpeed = 0f;
            TurnRate = 0f;
        }
    }

    public sealed class UnderwaterMovementSystem
    {
        private readonly Dictionary<string, UnderwaterMovementState> states =
            new Dictionary<string, UnderwaterMovementState>(
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
                    new UnderwaterMovementState(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float maximumSpeed,
            float maximumVerticalSpeed,
            float maximumTurnRate,
            float minimumDepth,
            float maximumDepth)
        {
            RegisterUnit(unitId);

            states[unitId].Configure(
                maximumSpeed,
                maximumVerticalSpeed,
                maximumTurnRate,
                minimumDepth,
                maximumDepth);
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
                    out UnderwaterMovementState state))
            {
                state.StopPropulsion();
            }
        }

        public void SetDepth(
            string unitId,
            float depth)
        {
            RegisterUnit(unitId);

            states[unitId].SetDepth(depth);
        }

        public void SetMovementInput(
            string unitId,
            float forwardInput,
            float verticalInput,
            float turnInput)
        {
            if (states.TryGetValue(
                    unitId,
                    out UnderwaterMovementState state))
            {
                state.SetMovementInput(
                    forwardInput,
                    verticalInput,
                    turnInput);
            }
        }

        public void UpdateUnit(
            string unitId,
            float deltaTime)
        {
            if (states.TryGetValue(
                    unitId,
                    out UnderwaterMovementState state))
            {
                state.UpdateDepth(deltaTime);
            }
        }

        public bool TryGetState(
            string unitId,
            out UnderwaterMovementState state)
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

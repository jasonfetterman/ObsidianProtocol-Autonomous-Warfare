using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public sealed class GroundVehicleState
    {
        public string UnitId { get; }

        public float ForwardSpeed { get; private set; }
        public float TurnRate { get; private set; }

        public float MaximumSpeed { get; private set; }
        public float MaximumTurnRate { get; private set; }

        public bool MovementEnabled { get; private set; }
        public bool EngineRunning { get; private set; }

        public GroundVehicleState(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            MovementEnabled = false;
            EngineRunning = false;
        }

        public void Configure(
            float maximumSpeed,
            float maximumTurnRate)
        {
            MaximumSpeed =
                Math.Max(0f, maximumSpeed);

            MaximumTurnRate =
                Math.Max(0f, maximumTurnRate);
        }

        public void StartEngine()
        {
            EngineRunning = true;
        }

        public void StopEngine()
        {
            EngineRunning = false;
            StopMovement();
        }

        public void EnableMovement()
        {
            MovementEnabled = true;
        }

        public void DisableMovement()
        {
            MovementEnabled = false;
            StopMovement();
        }

        public void SetMovementInput(
            float forwardInput,
            float turnInput)
        {
            if (!MovementEnabled ||
                !EngineRunning)
            {
                return;
            }

            ForwardSpeed =
                Math.Clamp(
                    forwardInput,
                    -1f,
                    1f) * MaximumSpeed;

            TurnRate =
                Math.Clamp(
                    turnInput,
                    -1f,
                    1f) * MaximumTurnRate;
        }

        public void StopMovement()
        {
            ForwardSpeed = 0f;
            TurnRate = 0f;
        }
    }

    public sealed class GroundVehicleController
    {
        private readonly Dictionary<string, GroundVehicleState> vehicles =
            new Dictionary<string, GroundVehicleState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterVehicle(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!vehicles.ContainsKey(unitId))
            {
                vehicles.Add(
                    unitId,
                    new GroundVehicleState(unitId));
            }
        }

        public void ConfigureVehicle(
            string unitId,
            float maximumSpeed,
            float maximumTurnRate)
        {
            RegisterVehicle(unitId);

            vehicles[unitId].Configure(
                maximumSpeed,
                maximumTurnRate);
        }

        public void StartEngine(string unitId)
        {
            RegisterVehicle(unitId);

            vehicles[unitId].StartEngine();
        }

        public void StopEngine(string unitId)
        {
            if (vehicles.TryGetValue(
                    unitId,
                    out GroundVehicleState vehicle))
            {
                vehicle.StopEngine();
            }
        }

        public void SetMovementEnabled(
            string unitId,
            bool enabled)
        {
            RegisterVehicle(unitId);

            if (enabled)
            {
                vehicles[unitId].EnableMovement();
            }
            else
            {
                vehicles[unitId].DisableMovement();
            }
        }

        public void SetMovementInput(
            string unitId,
            float forwardInput,
            float turnInput)
        {
            if (vehicles.TryGetValue(
                    unitId,
                    out GroundVehicleState vehicle))
            {
                vehicle.SetMovementInput(
                    forwardInput,
                    turnInput);
            }
        }

        public void StopVehicle(string unitId)
        {
            if (vehicles.TryGetValue(
                    unitId,
                    out GroundVehicleState vehicle))
            {
                vehicle.StopMovement();
            }
        }

        public bool TryGetVehicle(
            string unitId,
            out GroundVehicleState vehicle)
        {
            return vehicles.TryGetValue(
                unitId,
                out vehicle);
        }

        public void RemoveVehicle(string unitId)
        {
            vehicles.Remove(unitId);
        }

        public void Clear()
        {
            vehicles.Clear();
        }
    }
}

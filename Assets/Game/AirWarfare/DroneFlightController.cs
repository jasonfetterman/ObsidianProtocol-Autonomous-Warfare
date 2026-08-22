using System;

namespace ObsidianProtocol.Game.AirWarfare
{
    public sealed class DroneFlightController
    {
        public string UnitId { get; }

        public float ForwardSpeed { get; private set; }
        public float VerticalSpeed { get; private set; }
        public float TurnRate { get; private set; }

        public float MaximumSpeed { get; private set; }
        public float MaximumVerticalSpeed { get; private set; }
        public float MaximumTurnRate { get; private set; }

        public bool FlightEnabled { get; private set; }
        public bool Stabilized { get; private set; }

        public DroneFlightController(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            ForwardSpeed = 0f;
            VerticalSpeed = 0f;
            TurnRate = 0f;

            MaximumSpeed = 0f;
            MaximumVerticalSpeed = 0f;
            MaximumTurnRate = 0f;

            FlightEnabled = false;
            Stabilized = false;
        }

        public void Configure(
            float maximumSpeed,
            float maximumVerticalSpeed,
            float maximumTurnRate)
        {
            MaximumSpeed =
                Math.Max(0f, maximumSpeed);

            MaximumVerticalSpeed =
                Math.Max(0f, maximumVerticalSpeed);

            MaximumTurnRate =
                Math.Max(0f, maximumTurnRate);
        }

        public void EnableFlight()
        {
            FlightEnabled = true;
        }

        public void DisableFlight()
        {
            FlightEnabled = false;

            ForwardSpeed = 0f;
            VerticalSpeed = 0f;
            TurnRate = 0f;
        }

        public void SetFlightInput(
            float forwardInput,
            float verticalInput,
            float turnInput)
        {
            if (!FlightEnabled)
            {
                return;
            }

            ForwardSpeed =
                Math.Clamp(
                    forwardInput,
                    -1f,
                    1f) * MaximumSpeed;

            VerticalSpeed =
                Math.Clamp(
                    verticalInput,
                    -1f,
                    1f) * MaximumVerticalSpeed;

            TurnRate =
                Math.Clamp(
                    turnInput,
                    -1f,
                    1f) * MaximumTurnRate;
        }

        public void SetStabilized(bool stabilized)
        {
            Stabilized = stabilized;
        }

        public void Stop()
        {
            ForwardSpeed = 0f;
            VerticalSpeed = 0f;
            TurnRate = 0f;
        }
    }
}

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public sealed class HeavyTransportState
    {
        public string UnitId { get; }

        public float CargoCapacity { get; private set; }
        public float CurrentCargo { get; private set; }

        public bool Loading { get; private set; }
        public bool Unloading { get; private set; }

        public HeavyTransportState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;
        }

        public void Configure(
            float cargoCapacity)
        {
            CargoCapacity =
                Math.Max(
                    0f,
                    cargoCapacity);

            CurrentCargo =
                Math.Min(
                    CurrentCargo,
                    CargoCapacity);
        }

        public bool Load(
            float amount)
        {
            if (amount <= 0f ||
                CurrentCargo + amount > CargoCapacity)
            {
                return false;
            }

            Loading = true;
            Unloading = false;

            CurrentCargo += amount;

            Loading = false;

            return true;
        }

        public bool Unload(
            float amount)
        {
            if (amount <= 0f ||
                amount > CurrentCargo)
            {
                return false;
            }

            Unloading = true;
            Loading = false;

            CurrentCargo -= amount;

            Unloading = false;

            return true;
        }

        public void ClearCargo()
        {
            CurrentCargo = 0f;
            Loading = false;
            Unloading = false;
        }

        public float GetAvailableCapacity()
        {
            return Math.Max(
                0f,
                CargoCapacity - CurrentCargo);
        }
    }

    public sealed class HeavyTransportSystem
    {
        private readonly Dictionary<string, HeavyTransportState> transports =
            new Dictionary<string, HeavyTransportState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterTransport(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!transports.ContainsKey(unitId))
            {
                transports.Add(
                    unitId,
                    new HeavyTransportState(unitId));
            }
        }

        public void ConfigureTransport(
            string unitId,
            float cargoCapacity)
        {
            RegisterTransport(unitId);

            transports[unitId].Configure(
                cargoCapacity);
        }

        public bool Load(
            string unitId,
            float amount)
        {
            return transports.TryGetValue(
                       unitId,
                       out HeavyTransportState transport) &&
                   transport.Load(amount);
        }

        public bool Unload(
            string unitId,
            float amount)
        {
            return transports.TryGetValue(
                       unitId,
                       out HeavyTransportState transport) &&
                   transport.Unload(amount);
        }

        public float GetCargo(
            string unitId)
        {
            return transports.TryGetValue(
                       unitId,
                       out HeavyTransportState transport)
                ? transport.CurrentCargo
                : 0f;
        }

        public float GetAvailableCapacity(
            string unitId)
        {
            return transports.TryGetValue(
                       unitId,
                       out HeavyTransportState transport)
                ? transport.GetAvailableCapacity()
                : 0f;
        }

        public bool TryGetTransport(
            string unitId,
            out HeavyTransportState transport)
        {
            return transports.TryGetValue(
                unitId,
                out transport);
        }

        public void RemoveTransport(
            string unitId)
        {
            transports.Remove(unitId);
        }

        public void Clear()
        {
            transports.Clear();
        }
    }
}

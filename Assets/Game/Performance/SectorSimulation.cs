using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public enum SectorSimulationMode
    {
        Full,
        Reduced,
        Background,
        Suspended
    }

    public sealed class SimulationSector
    {
        public string SectorId { get; }

        public SectorSimulationMode Mode { get; private set; }

        public int UnitCount { get; private set; }

        public float SimulationInterval { get; private set; }

        public float TimeUntilSimulation { get; private set; }

        public bool Active =>
            Mode != SectorSimulationMode.Suspended;

        public SimulationSector(
            string sectorId,
            SectorSimulationMode mode,
            int unitCount,
            float simulationInterval)
        {
            SectorId =
                sectorId ?? string.Empty;

            Mode =
                mode;

            UnitCount =
                Math.Max(
                    0,
                    unitCount);

            SimulationInterval =
                Math.Max(
                    0.001f,
                    simulationInterval);

            TimeUntilSimulation = 0f;
        }

        public bool SetMode(
            SectorSimulationMode mode)
        {
            Mode =
                mode;

            return true;
        }

        public bool SetUnitCount(
            int count)
        {
            if (count < 0)
            {
                return false;
            }

            UnitCount =
                count;

            return true;
        }

        public bool SetSimulationInterval(
            float interval)
        {
            if (interval <= 0f)
            {
                return false;
            }

            SimulationInterval =
                interval;

            TimeUntilSimulation =
                Math.Min(
                    TimeUntilSimulation,
                    SimulationInterval);

            return true;
        }

        public bool ShouldSimulate(
            float deltaTime)
        {
            if (!Active ||
                deltaTime < 0f)
            {
                return false;
            }

            if (Mode ==
                SectorSimulationMode.Full)
            {
                return true;
            }

            TimeUntilSimulation -=
                deltaTime;

            if (TimeUntilSimulation > 0f)
            {
                return false;
            }

            TimeUntilSimulation =
                SimulationInterval;

            return true;
        }
    }

    public sealed class SectorSimulation
    {
        private readonly Dictionary<
            string,
            SimulationSector> sectors =
            new Dictionary<
                string,
                SimulationSector>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SectorCount =>
            sectors.Count;

        public int ActiveSectorCount
        {
            get
            {
                int count = 0;

                foreach (SimulationSector sector
                         in sectors.Values)
                {
                    if (sector.Active)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            sectors.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterSector(
            string sectorId,
            SectorSimulationMode mode,
            int unitCount,
            float simulationInterval)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sectorId) ||
                unitCount < 0 ||
                simulationInterval <= 0f)
            {
                return false;
            }

            string id =
                sectorId.Trim();

            if (sectors.ContainsKey(id))
            {
                return false;
            }

            sectors.Add(
                id,
                new SimulationSector(
                    id,
                    mode,
                    unitCount,
                    simulationInterval));

            return true;
        }

        public bool RemoveSector(
            string sectorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sectorId))
            {
                return false;
            }

            return sectors.Remove(
                sectorId.Trim());
        }

        public bool SetMode(
            string sectorId,
            SectorSimulationMode mode)
        {
            SimulationSector sector =
                GetSector(sectorId);

            return sector != null &&
                   sector.SetMode(mode);
        }

        public bool SetUnitCount(
            string sectorId,
            int count)
        {
            SimulationSector sector =
                GetSector(sectorId);

            return sector != null &&
                   sector.SetUnitCount(count);
        }

        public bool SetSimulationInterval(
            string sectorId,
            float interval)
        {
            SimulationSector sector =
                GetSector(sectorId);

            return sector != null &&
                   sector.SetSimulationInterval(interval);
        }

        public bool ShouldSimulate(
            string sectorId,
            float deltaTime)
        {
            SimulationSector sector =
                GetSector(sectorId);

            return sector != null &&
                   sector.ShouldSimulate(deltaTime);
        }

        public int UpdateAll(
            float deltaTime)
        {
            if (!Initialized ||
                deltaTime < 0f)
            {
                return 0;
            }

            int simulationCount = 0;

            foreach (SimulationSector sector
                     in sectors.Values)
            {
                if (sector.ShouldSimulate(deltaTime))
                {
                    simulationCount++;
                }
            }

            return simulationCount;
        }

        public SimulationSector GetSector(
            string sectorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sectorId))
            {
                return null;
            }

            sectors.TryGetValue(
                sectorId.Trim(),
                out SimulationSector sector);

            return sector;
        }

        public IReadOnlyCollection<
            SimulationSector>
            GetSectors()
        {
            return sectors.Values;
        }

        public void Reset()
        {
            sectors.Clear();

            Initialized = false;
        }
    }
}

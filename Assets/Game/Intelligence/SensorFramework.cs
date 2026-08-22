using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public enum SensorType
    {
        Visual,
        Audio,
        Thermal,
        Radar,
        Lidar,
        MultiSensor
    }

    public enum SensorStatus
    {
        Offline,
        Standby,
        Active,
        Damaged
    }

    public sealed class SensorDefinition
    {
        public string SensorId;
        public SensorType Type;
        public float Range;
        public float Accuracy;
        public SensorStatus Status;

        public SensorDefinition(
            string sensorId,
            SensorType type,
            float range,
            float accuracy)
        {
            SensorId = sensorId;
            Type = type;
            Range = Math.Max(0f, range);
            Accuracy = Math.Clamp(accuracy, 0f, 1f);
            Status = SensorStatus.Standby;
        }
    }

    public sealed class SensorFramework
    {
        private readonly Dictionary<string, SensorDefinition> sensors =
            new Dictionary<string, SensorDefinition>();

        public void RegisterSensor(
            string sensorId,
            SensorType type,
            float range,
            float accuracy)
        {
            if (string.IsNullOrWhiteSpace(sensorId))
            {
                return;
            }

            sensors[sensorId] =
                new SensorDefinition(
                    sensorId,
                    type,
                    range,
                    accuracy);
        }

        public bool ActivateSensor(string sensorId)
        {
            if (!sensors.TryGetValue(
                    sensorId,
                    out SensorDefinition sensor))
            {
                return false;
            }

            sensor.Status = SensorStatus.Active;
            return true;
        }

        public bool DeactivateSensor(string sensorId)
        {
            if (!sensors.TryGetValue(
                    sensorId,
                    out SensorDefinition sensor))
            {
                return false;
            }

            sensor.Status = SensorStatus.Standby;
            return true;
        }

        public bool SetDamaged(string sensorId)
        {
            if (!sensors.TryGetValue(
                    sensorId,
                    out SensorDefinition sensor))
            {
                return false;
            }

            sensor.Status = SensorStatus.Damaged;
            return true;
        }

        public bool TryGetSensor(
            string sensorId,
            out SensorDefinition sensor)
        {
            return sensors.TryGetValue(
                sensorId,
                out sensor);
        }

        public IReadOnlyCollection<SensorDefinition> GetSensors()
        {
            return sensors.Values;
        }

        public void RemoveSensor(string sensorId)
        {
            sensors.Remove(sensorId);
        }

        public void Clear()
        {
            sensors.Clear();
        }
    }
}

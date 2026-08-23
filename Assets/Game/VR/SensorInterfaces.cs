using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public enum OperatorSensorType
    {
        None,
        Camera,
        Thermal,
        NightVision,
        Lidar,
        Radar,
        Sonar,
        Targeting,
        Navigation,
        Tactical
    }

    public sealed class OperatorSensor
    {
        public string SensorId { get; }

        public OperatorSensorType Type { get; }

        public bool Active { get; private set; }

        public float Zoom { get; private set; }

        public OperatorSensor(
            string sensorId,
            OperatorSensorType type)
        {
            SensorId =
                sensorId ?? string.Empty;

            Type = type;

            Active = false;
            Zoom = 1f;
        }

        public bool SetActive(
            bool active)
        {
            Active = active;

            return true;
        }

        public bool SetZoom(
            float zoom)
        {
            if (zoom < 1f ||
                zoom > 20f)
            {
                return false;
            }

            Zoom = zoom;

            return true;
        }
    }

    public sealed class SensorInterfaces
    {
        private readonly Dictionary<
            string,
            OperatorSensor> sensors =
            new Dictionary<
                string,
                OperatorSensor>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public string UnitId { get; private set; }

        public int SensorCount =>
            sensors.Count;

        public bool Initialize(
            string unitId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            UnitId =
                unitId.Trim();

            sensors.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterSensor(
            string sensorId,
            OperatorSensorType type)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sensorId) ||
                type == OperatorSensorType.None)
            {
                return false;
            }

            string id =
                sensorId.Trim();

            if (sensors.ContainsKey(id))
            {
                return false;
            }

            sensors.Add(
                id,
                new OperatorSensor(
                    id,
                    type));

            return true;
        }

        public bool SetSensorActive(
            string sensorId,
            bool active)
        {
            OperatorSensor sensor =
                GetSensor(sensorId);

            return sensor != null &&
                   sensor.SetActive(active);
        }

        public bool SetSensorZoom(
            string sensorId,
            float zoom)
        {
            OperatorSensor sensor =
                GetSensor(sensorId);

            return sensor != null &&
                   sensor.SetZoom(zoom);
        }

        public OperatorSensor GetSensor(
            string sensorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sensorId))
            {
                return null;
            }

            sensors.TryGetValue(
                sensorId.Trim(),
                out OperatorSensor sensor);

            return sensor;
        }

        public IReadOnlyCollection<OperatorSensor>
            GetSensors()
        {
            return sensors.Values;
        }

        public void Reset()
        {
            sensors.Clear();

            Initialized = false;

            UnitId =
                string.Empty;
        }
    }
}

using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public enum CockpitControlType
    {
        None,
        Throttle,
        Steering,
        Weapon,
        Sensor,
        Communication,
        Navigation,
        Emergency
    }

    public sealed class CockpitControl
    {
        public string ControlId { get; }

        public CockpitControlType Type { get; }

        public float Value { get; private set; }

        public bool Active { get; private set; }

        public CockpitControl(
            string controlId,
            CockpitControlType type)
        {
            ControlId =
                controlId ?? string.Empty;

            Type = type;

            Value = 0f;
            Active = false;
        }

        public bool SetValue(
            float value)
        {
            if (value < -1f ||
                value > 1f)
            {
                return false;
            }

            Value = value;
            Active = true;

            return true;
        }

        public void Reset()
        {
            Value = 0f;
            Active = false;
        }
    }

    public sealed class VRCockpit
    {
        private readonly Dictionary<
            string,
            CockpitControl> controls =
            new Dictionary<
                string,
                CockpitControl>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Active { get; private set; }

        public string UnitId { get; private set; }

        public int ControlCount =>
            controls.Count;

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

            controls.Clear();

            Active = false;
            Initialized = true;

            return true;
        }

        public bool RegisterControl(
            string controlId,
            CockpitControlType type)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(controlId) ||
                type == CockpitControlType.None)
            {
                return false;
            }

            string id =
                controlId.Trim();

            if (controls.ContainsKey(id))
            {
                return false;
            }

            controls.Add(
                id,
                new CockpitControl(
                    id,
                    type));

            return true;
        }

        public bool Activate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = true;

            return true;
        }

        public bool Deactivate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = false;

            foreach (CockpitControl control
                     in controls.Values)
            {
                control.Reset();
            }

            return true;
        }

        public bool SetControlValue(
            string controlId,
            float value)
        {
            if (!Initialized ||
                !Active)
            {
                return false;
            }

            CockpitControl control =
                GetControl(controlId);

            return control != null &&
                   control.SetValue(value);
        }

        public CockpitControl GetControl(
            string controlId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(controlId))
            {
                return null;
            }

            controls.TryGetValue(
                controlId.Trim(),
                out CockpitControl control);

            return control;
        }

        public IReadOnlyCollection<CockpitControl>
            GetControls()
        {
            return controls.Values;
        }

        public void Reset()
        {
            controls.Clear();

            Initialized = false;
            Active = false;

            UnitId =
                string.Empty;
        }
    }
}

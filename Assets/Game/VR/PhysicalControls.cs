using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public enum PhysicalControlType
    {
        None,
        Button,
        Toggle,
        Lever,
        Switch,
        Dial,
        Joystick,
        Pedal,
        Handle
    }

    public sealed class PhysicalControl
    {
        public string ControlId { get; }

        public PhysicalControlType Type { get; }

        public float Value { get; private set; }

        public bool Pressed { get; private set; }

        public PhysicalControl(
            string controlId,
            PhysicalControlType type)
        {
            ControlId =
                controlId ?? string.Empty;

            Type = type;

            Value = 0f;
            Pressed = false;
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

            return true;
        }

        public bool SetPressed(
            bool pressed)
        {
            Pressed = pressed;

            return true;
        }

        public void Reset()
        {
            Value = 0f;
            Pressed = false;
        }
    }

    public sealed class PhysicalControls
    {
        private readonly Dictionary<
            string,
            PhysicalControl> controls =
            new Dictionary<
                string,
                PhysicalControl>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

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

            Initialized = true;

            return true;
        }

        public bool RegisterControl(
            string controlId,
            PhysicalControlType type)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(controlId) ||
                type == PhysicalControlType.None)
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
                new PhysicalControl(
                    id,
                    type));

            return true;
        }

        public bool SetValue(
            string controlId,
            float value)
        {
            PhysicalControl control =
                GetControl(controlId);

            return control != null &&
                   control.SetValue(value);
        }

        public bool SetPressed(
            string controlId,
            bool pressed)
        {
            PhysicalControl control =
                GetControl(controlId);

            return control != null &&
                   control.SetPressed(pressed);
        }

        public PhysicalControl GetControl(
            string controlId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(controlId))
            {
                return null;
            }

            controls.TryGetValue(
                controlId.Trim(),
                out PhysicalControl control);

            return control;
        }

        public IReadOnlyCollection<PhysicalControl>
            GetControls()
        {
            return controls.Values;
        }

        public void Reset()
        {
            controls.Clear();

            Initialized = false;

            UnitId =
                string.Empty;
        }
    }
}
